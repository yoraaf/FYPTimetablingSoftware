using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace FYPTimetablingSoftware {
	public class GeneticAlgorithm<T> {
		public List<DNA<T>> Population { get; private set; } // T is SolutionGene
		public int Generation { get; private set; }
		public float BestFitness { get; private set; }
		public T[] BestGenes { get; private set; }
		public DNA<T> BestDNA { get; private set; }

		public int Elitism;
		public float MutationRate;
		public static object randLock = new object();
		public static object fitLock = new object();
		private List<DNA<T>> newPopulation;
		private static Random random;
		private float fitnessSum;
		private int dnaSize;
		private Func<Klas, T> getRandomGene;
		private Func<int, float> fitnessFunction;
		private Action updateAlgorithm;
		private readonly Klas[] KlasArr;
		private DNA<T>[] NewGenerationArr;
		private int threadPoolCounter = 0;
		public static ManualResetEvent DoneEvt;

		public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<Klas, T> getRandomGene, Func<int, float> fitnessFunction, Action updateAlgorithm,
			int elitism, float mutationRate = 0.01f) {
			Generation = 1;
			Elitism = elitism;
			MutationRate = mutationRate;
			Population = new List<DNA<T>>(populationSize);
			newPopulation = new List<DNA<T>>(populationSize);
			GeneticAlgorithm<T>.random = random;
			this.dnaSize = dnaSize;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			this.updateAlgorithm = updateAlgorithm;
			BestGenes = new T[dnaSize];
			NewGenerationArr = new DNA<T>[populationSize];
			KlasArr = XMLParser.GetKlasList(); //The parser must be ran before the algorithm starts

			//When the genetic algorithm is created the initial population is generated as follows:
			for (int i = 0; i < populationSize; i++) {
				Population.Add(new DNA<T>(i, dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("-------------------------------------");
			Console.ResetColor();
		}
		public static string ArrayToString(T[] arr) {
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

			Thread t = new Thread(new ThreadStart(ThreadProc));
			t.Start();

			for (int i = 0; i < Population.Count/2; i++) {
				if (i < Elitism && i < Population.Count/2) { //elitism makes it so that the top (5) make it into the next generation 
					NewGenerationArr[i] = Population[i];
				} else if (i < Population.Count) {
					DNA<T> parent1 = ChooseParent();
					DNA<T> parent2 = ChooseParent();

					DNA<T> child = parent1.Crossover(parent2, i);

					child.Mutate(MutationRate);
					NewGenerationArr[i] = child;
				} 
			}
			Debug.WriteLine("Main thread done");
			t.Join();

			Population = NewGenerationArr.ToList(); 

			Generation++;
		}

		private void ThreadProc() {
			for (int j = Population.Count/2; j < Population.Count; j++) {
				if (j < Elitism && j < Population.Count) { //elitism makes it so that the top (5) make it into the next generation 
					NewGenerationArr[j] = Population[j];
				} else if (j < Population.Count) {
					DNA<T> parent1 = ChooseParent();
					DNA<T> parent2 = ChooseParent();

					DNA<T> child = parent1.Crossover(parent2, j);

					child.Mutate(MutationRate);
					NewGenerationArr[j] = child;
				}
			}
			Debug.WriteLine("second thread done");
		}

		private int CompareDNA(DNA<T> a, DNA<T> b) {
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

			//ThreadPool.SetMaxThreads(25, 25); This limit doesn't seem to actually do anything 

			for (int i = 0; i < Population.Count; i++) {
				ThreadPool.QueueUserWorkItem(FitnessThreadPoolMethod, i);
			}
			Debug.WriteLine("Added all events");
			//t.Join();
			DoneEvt.WaitOne();
			//Debug.WriteLine("done waiting");

			BestFitness = BestDNA.Fitness;
			BestDNA.Genes.CopyTo(BestGenes, 0);
			//BestDNA = best;
		}

		private void FitnessThreadPoolMethod(object number) {
			int n = (int)number;
			var fit = Population[n].CalculateFitness(n);
			//Debug.WriteLine("[" + n + "] Fitness: " + fit);
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

		private void fitnessThread() {
			for (int i = Population.Count/2; i < Population.Count; i++) {
				var fit = Population[i].CalculateFitness(i);
				lock (fitLock) {
					fitnessSum += fit;
					if (Population[i].Fitness < BestDNA.Fitness) {
						BestDNA = Population[i];
					}
				}
			}
			Debug.WriteLine("Fitness second thread done");
		}

		private double GetFitnessSum() {
			double output = 0;
			for(int i = 0; i < Population.Count; i++) {
				output += Population[i].Fitness;
            }
			return output;
        }

		private DNA<T> ChooseParent() {
			//tournament style selection
			double randomNumber = GetFitnessSum() * LockedRandomDouble();
			int tournamentSize =  (Int32)Math.Floor(Population.Count * 0.2);
			DNA<T>[] tournamentMembers = new DNA<T>[tournamentSize];
			for (int i = 0; i < tournamentSize; i++) {
				DNA<T> x;
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
			//sort tournament members my fitness and return the fittest one
			Array.Sort(tournamentMembers, CompareDNA); 
			//Console.WriteLine("top ")
			return tournamentMembers[0];
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

		private DNA<T> ChooseParent_old() {
			double r = random.NextDouble();
			double randomNumber = r * fitnessSum * 0.9;
			double[] fitnessArr = new double[Population.Count];
			double[] cumulativeArr = new double[Population.Count];
			for (int i = 0; i < Population.Count; i++) {
				if (randomNumber < Population[i].Fitness) {
					return Population[i];
				}
				fitnessArr[i] = Population[i].Fitness;
				if (i == 0) {
					cumulativeArr[i] = Population[i].Fitness;
				} else {
					cumulativeArr[i] = Population[i].Fitness + cumulativeArr[i - 1];
				}
				randomNumber -= Population[i].Fitness;
			}
			Console.Error.WriteLine("Something went wrong");
			return null;
		}
	}
}