using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FYPTimetablingSoftware {
	public class GeneticAlgorithm<T> {
		public List<DNA<T>> Population { get; private set; } // T is SolutionGene
		public int Generation { get; private set; }
		public float BestFitness { get; private set; }
		public T[] BestGenes { get; private set; }

		public int Elitism;
		public float MutationRate;

		private List<DNA<T>> newPopulation;
		private Random random;
		private float fitnessSum;
		private int dnaSize;
		private Func<Klas, T> getRandomGene;
		private Func<int, float> fitnessFunction;
		private Action updateAlgorithm;
		private readonly Klas[] KlasArr;

		public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<Klas, T> getRandomGene, Func<int, float> fitnessFunction, Action updateAlgorithm,
			int elitism, float mutationRate = 0.01f) {
			Generation = 1;
			Elitism = elitism;
			MutationRate = mutationRate;
			Population = new List<DNA<T>>(populationSize);
			newPopulation = new List<DNA<T>>(populationSize);
			this.random = random;
			this.dnaSize = dnaSize;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			this.updateAlgorithm = updateAlgorithm;
			BestGenes = new T[dnaSize];
			KlasArr = XMLParser.GetKlasList(); //The parser must be ran before the algorithm starts

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
		public void NewGeneration(int numNewDNA = 0, bool crossoverNewDNA = false) {
			int finalCount = Population.Count + numNewDNA;

			if (finalCount <= 0) {
				return;
			}

			if (Population.Count > 0) {
				CalculateFitness();
				Population.Sort(CompareDNA);
			}
			newPopulation.Clear();

			for (int i = 0; i < Population.Count; i++) {
				if (i < Elitism && i < Population.Count) { //elitism makes it so that the top (5) make it into the next generation 
					newPopulation.Add(Population[i]);
				} else if (i < Population.Count || crossoverNewDNA) {
					DNA<T> parent1 = ChooseParent();
					DNA<T> parent2 = ChooseParent();

					DNA<T> child = parent1.Crossover(parent2);

					child.Mutate(MutationRate);
					Debug.WriteLine("Parent 1: "+ ArrayToString(parent1.Genes));
					Debug.WriteLine("Parent 2: "+ ArrayToString(parent2.Genes));
					Debug.WriteLine("Child:    "+ ArrayToString(child.Genes));
					newPopulation.Add(child);
				} else {
					newPopulation.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
				}
			}

			List<DNA<T>> tmpList = Population;
			Population = newPopulation;
			newPopulation = tmpList;

			Generation++;
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

				if (Population[i].Fitness > best.Fitness) {
					best = Population[i];
				}
			}

			BestFitness = best.Fitness;
			best.Genes.CopyTo(BestGenes, 0);
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