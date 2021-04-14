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

		private List<DNA<T>> newPopulation;
		public Random random;
		private float fitnessSum;
		public int dnaSize;
		public Func<Klas, T> getRandomGene;
		public Func<int, float> fitnessFunction;
		private Action updateAlgorithm;
		private readonly Klas[] KlasArr;
		private ManualResetEvent[] DoneEvents;

		public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<Klas, T> getRandomGene, Func<int, float> fitnessFunction, Action updateAlgorithm,
			int elitism, float mutationRate = 0.01f) {
			Generation = 1;
			Elitism = elitism;
			MutationRate = mutationRate;
			NewMember<T>.ga = this;
			Population = new List<DNA<T>>(populationSize);
			newPopulation = new List<DNA<T>>(populationSize);
			this.random = random;
			this.dnaSize = dnaSize;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			this.updateAlgorithm = updateAlgorithm;
			BestGenes = new T[dnaSize];
			KlasArr = XMLParser.GetKlasList(); //The parser must be ran before the algorithm starts
			ThreadPool.SetMaxThreads(10, 10);
			//When the genetic algorithm is created the initial population is generated as follows:
			for (int i = 0; i < populationSize; i++) {
				Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
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
			int finalCount = Population.Count;

			if (finalCount <= 0) {
				return;
			}

			if (Population.Count > 0) {
				CalculateFitness();
				Population.Sort(CompareDNA);
			}
			NewMember<T>.ShouldWait = true;
			//int counter = 500;
            NewMember<T>.counter = 500;
			var doneEvent = new ManualResetEvent(false);
			NewMember<T>.DoneEvt = doneEvent;
			newPopulation.Clear();
			DoneEvents = new ManualResetEvent[Population.Count];
			var newMembersArr = new NewMember<T>[Population.Count];
			for(int i = 0; i < Population.Count; i++) {
				newMembersArr[i] = new NewMember<T>(i);
			}
			for (int i = 0; i < Population.Count; i++) {
				ThreadPool.QueueUserWorkItem(newMembersArr[i].GenerateNewMember);
				Console.WriteLine(">>>added thread "+i);
			}
			NewMember<T>.ShouldWait = false;
			doneEvent.WaitOne();
			Console.WriteLine("All members done");

			for(int i = 0; i < Population.Count; i++) {
				newPopulation.Add(newMembersArr[i].Member);
            }
			Debug.WriteLine("new population added");
			List<DNA<T>> tmpList = Population;
			Population = newPopulation;
			newPopulation = tmpList;

			Generation++;
		}

		private void NewMember(Object threadContext) {
			object[] arr = threadContext as object[];
			int memberPosition = (int)arr[0];
			ManualResetEvent doneEvt = (ManualResetEvent)arr[1];
			if (memberPosition < Elitism && memberPosition < Population.Count) { //elitism makes it so that the top (5) make it into the next generation 
				newPopulation.Add(Population[memberPosition]);
			} else if (memberPosition < Population.Count) {
				DNA<T> parent1 = ChooseParent();
				DNA<T> parent2 = ChooseParent();

				DNA<T> child = parent1.Crossover(parent2);

				child.Mutate(MutationRate);
				/*Debug.WriteLine("Parent 1: "+ ArrayToString(parent1.Genes));
				Debug.WriteLine("Parent 2: "+ ArrayToString(parent2.Genes));
				Debug.WriteLine("Child:    "+ ArrayToString(child.Genes));*/
				newPopulation.Insert(memberPosition, child);
			} else {
				newPopulation.Insert(memberPosition, new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
			}
			Console.WriteLine("[ThreadPool] Thread " + memberPosition + " completed");
			doneEvt.Set();
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
			DNA<T> best = Population[0];

			for (int i = 0; i < Population.Count; i++) {
				fitnessSum += Population[i].CalculateFitness(i);

				if (Population[i].Fitness < best.Fitness) {
					best = Population[i];
				}
			}

			BestFitness = best.Fitness;
			best.Genes.CopyTo(BestGenes, 0);
			BestDNA = best;
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
			double randomNumber = GetFitnessSum() * random.NextDouble();
			int tournamentSize =  (Int32)Math.Floor(Population.Count * 0.2);
			DNA<T>[] tournamentMembers = new DNA<T>[tournamentSize];
			for (int i = 0; i < tournamentSize; i++) {
				DNA<T> x;
				do {
					//add random person from population to the tournament and make sure they aren't in already
					x = Population[random.Next(0, Population.Count)];
				} while (tournamentMembers.Contains(x));
				tournamentMembers[i] = x;
			}
			//sort tournament members my fitness and return the fittest one
			Array.Sort(tournamentMembers, CompareDNA); 
			//Console.WriteLine("top ")
			return tournamentMembers[0];
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