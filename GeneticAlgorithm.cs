using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace FYPTimetablingSoftware {
	public class GeneticAlgorithm {
		public List<DNA> Population { get; private set; }
		public int Generation { get; private set; }
		public float BestFitness { get; private set; }
		public SolutionGene[] BestGenes { get; private set; }
		public DNA BestDNA { get; private set; }

		public int Elitism;
		public float MutationRate;
		public static object randLock = new object();
		public static object fitLock = new object();
		private List<DNA> newPopulation;
		private static Random random;
		private float fitnessSum;
		private int dnaSize;
		private Func<Klas, SolutionGene> getRandomGene;
		private Func<int, float> fitnessFunction;
		private Action updateAlgorithm;
		private readonly Klas[] KlasArr;
		private DNA[] NewGenerationArr;
		private int threadPoolCounter = 0;
		public static ManualResetEvent DoneEvt;
		public Func<DNA> ChooseParentFunc;

		public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<Klas, SolutionGene> getRandomGene, Func<int, float> fitnessFunction, Action updateAlgorithm,
			int elitism, float mutationRate = 0.01f) {
			Generation = 1;
			Elitism = elitism;
			MutationRate = mutationRate;
			Population = new List<DNA>(populationSize);
			newPopulation = new List<DNA>(populationSize);
			GeneticAlgorithm.random = random;
			this.dnaSize = dnaSize;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			this.updateAlgorithm = updateAlgorithm;
			BestGenes = new SolutionGene[dnaSize];
			NewGenerationArr = new DNA[populationSize];
			KlasArr = XMLParser.GetKlasList(); //The parser must be ran before the algorithm starts

            switch (Program.SelectionMethod) {
				case "Tournament":
					ChooseParentFunc = Tournament;
					break;
				case "RankBased":
					ChooseParentFunc = RankBased;
					break;
				case "Random":
					ChooseParentFunc = RandomSelection;
					break;
			}


			//When the genetic algorithm is created the initial population is generated as follows:
			for (int i = 0; i < populationSize; i++) {
				Population.Add(new DNA(i, dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
			}
		}
		public static string ArrayToString(SolutionGene[] arr) {
			string output = "";
			for(int i = 0; i < arr.Length; i++) {
				output += arr[i].ToString() +"\r\n";
            }
			return output;
        }
		public void NewGeneration() {
			if (Population.Count <= 0) {
				return;
			}

			if (Population.Count > 0) {
				CalculateFitness();
				Population.Sort(CompareDNA);
			}
			newPopulation.Clear();
			Array.Clear(NewGenerationArr, 0, NewGenerationArr.Length);

			if (Program.SelectionMethod == "SexBased") {

				List<DNA> tempPop = new List<DNA>();
				DNA[] sex1 = new DNA[Population.Count / 2];
				DNA[] sex2 = new DNA[Population.Count / 2];
				foreach (var member in Population) {
					tempPop.Add(member);
				}
				tempPop.Shuffle(); //Since the population is by default ordered with best fitness first, it needs to be randomised before assigning gender

				for (int i = 0; i < tempPop.Count / 2; i++) {
					sex1[i] = tempPop[i];
				}
				for (int i = 0; i < tempPop.Count / 2; i++) {
					sex2[i] = tempPop[i + 250];
				}


				for (int i = 0; i < sex1.Length; i++) {
					DNA parent1 = sex1[i];
					DNA parent2 = STournament(sex2);
					DNA[] children = new DNA[2];
					children = parent1.CrossoverUniform(parent2, newPopulation.Count);
					newPopulation.AddRange(children);
				}
				if (Elitism > 0) {
					for (int i = 0; i < Elitism; i++) {
						newPopulation[i] = Population[i]; //top 5 of the new generation are replaced by the old 
					}
				}
				for (int i = 0; i < newPopulation.Count; i++) {
					Population[i] = newPopulation[i];

				}
				Generation++;

			} else {
				for (int i = 0; i < Population.Count; i++) {
					if (i < Elitism) { //elitism makes it so that the top (5) make it into the next generation 
						NewGenerationArr[i] = Population[i];
					} else if (i < Population.Count) {
						//DNA parent1 = ChooseParent();
						DNA parent1 = ChooseParentFunc();
						//DNA parent2 = ChooseParent();
						DNA parent2 = ChooseParentFunc();

						DNA child;
						if (Program.CrossoverMethod == "Discrete") {
							child = parent1.Crossover(parent2, i);
						} else if (Program.CrossoverMethod == "Violation") {
							child = parent1.CrossoverViolation(parent2, i);
						} else {
							throw new InvalidOperationException("Program.CrossoverMethod not defined properly");
						}

						child.Mutate(MutationRate);
						NewGenerationArr[i] = child;
					}
				}

				Population = NewGenerationArr.ToList();

				Generation++;
			}
		}

		private int CompareDNA(DNA a, DNA b) {
			//had to switch the >< around cz low = better
			if (a.Fitness < b.Fitness) {
				return -1;
			} else if (a.Fitness > b.Fitness) {
				return 1;
			} else {
				return 0;
			}
		}

		private void CalculateFitness() {
			fitnessSum = 0;
			BestDNA = Population[0];
			threadPoolCounter = Population.Count;
			DoneEvt = new ManualResetEvent(false);

			for (int i = 0; i < Population.Count; i++) {
				ThreadPool.QueueUserWorkItem(FitnessThreadPoolMethod, i);
			}

			DoneEvt.WaitOne(); //wait till all events are done 
			BestFitness = BestDNA.Fitness;
			BestDNA.Genes.CopyTo(BestGenes, 0);

		}

		private void FitnessThreadPoolMethod(object number) {
			int n = (int)number;
			var fit = Population[n].CalculateFitness(n);

			lock (fitLock) {
				fitnessSum += fit;
				if (Population[n].Fitness < BestDNA.Fitness) {
					BestDNA = Population[n];
				}
			}

			if (Interlocked.Decrement(ref threadPoolCounter) == 0) {
				DoneEvt.Set();
			};
		}

		private DNA ChooseParent() {
			int tournamentSize = (Int32)Math.Floor(Population.Count * Program.TournamentRatio);
			DNA[] tournamentMembers = new DNA[tournamentSize];
			for (int i = 0; i < tournamentSize; i++) {
				DNA x;
				int testCounter = 0;
				List<int> logIDList = new List<int>();
				do {
					//add random person from population to the tournament and make sure they aren't in already
					testCounter++;
					if (testCounter > 500) {
						Debug.WriteLine("broken");
					}
					int id = LockedRandomInt(0, Population.Count);
					logIDList.Add(id);
					x = Population[id];
				} while (tournamentMembers.Contains(x));
				tournamentMembers[i] = x;
			}
			//sort tournament members by fitness and return the fittest one
			Array.Sort(tournamentMembers, CompareDNA);
			return tournamentMembers[0];
		}

		private DNA Tournament() {
			int tournamentSize = (Int32)Math.Floor(Population.Count * Program.TournamentRatio);
			DNA[] tournamentMembers = new DNA[tournamentSize];
			for (int i = 0; i < tournamentSize; i++) {
				DNA x;
				int testCounter = 0;
				List<int> logIDList = new List<int>();
				do {
					//add random person from population to the tournament and make sure they aren't in already
					testCounter++;
					if (testCounter > 500) {
						Debug.WriteLine("broken");
					}
					int id = LockedRandomInt(0, Population.Count);
					logIDList.Add(id);
					x = Population[id];
				} while (tournamentMembers.Contains(x));
				tournamentMembers[i] = x;
			}
			//sort tournament members by fitness and return the fittest one
			Array.Sort(tournamentMembers, CompareDNA);
			return tournamentMembers[0];
		}

		private DNA STournament(DNA[] contestants) {
			//This method is made for sex based selection
			int tournamentSize = (Int32)Math.Floor(contestants.Length * Program.TournamentRatio);
			DNA[] tournamentMembers = new DNA[tournamentSize];
			for (int i = 0; i < tournamentSize; i++) {
				DNA x;
				int testCounter = 0;
				List<int> logIDList = new List<int>();
				do {
					//add random person from contestants to the tournament and make sure they aren't in already
					testCounter++;
					if (testCounter > 500) {
						Debug.WriteLine("broken");
					}
					int id = LockedRandomInt(0, contestants.Length);
					logIDList.Add(id);
					x = contestants[id];
				} while (tournamentMembers.Contains(x));
				tournamentMembers[i] = x;
			}
			Array.Sort(tournamentMembers, CompareDNA);
			return tournamentMembers[0];
		}

		private DNA RankBased() {
			double fitnessBias = 2.5;
			int index = (int)(Population.Count * (fitnessBias - Math.Sqrt(fitnessBias * fitnessBias - 4.0 * (fitnessBias - 1) * LockedRandomDouble()))/ 2.0 / (fitnessBias - 1));
			/*int index;
			do {
				index = LockedRandomInt(0, Program.PopulationSize) + LockedRandomInt(0, Program.PopulationSize) - Program.PopulationSize-1; //-1 to prevent out of bounds
				//not the most efficient method, but the simplest 
			} while (index < 0);*/
			
			return Population[index];
		}

		private DNA RandomSelection() {
			int index = LockedRandomInt(0, Population.Count-1);
			return Population[index];
		}

		public static int LockedRandomInt(int min, int max) {
            lock (randLock) {
				return random.Next(min, max);
            }
        }
		public static double LockedRandomDouble() {
            lock (randLock) {
				return random.NextDouble();
            }
        }

	}
}