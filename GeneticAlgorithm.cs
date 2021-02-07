using System;
using System.Collections.Generic;

public class GeneticAlgorithm<T> {
	public List<DNA<T>> Population { get; private set; }
	public int Generation { get; private set; }
	public float BestFitness { get; private set; }
	public T[] BestGenes { get; private set; }

	public int Elitism;
	public float MutationRate;

	private List<DNA<T>> newPopulation;
	private Random random;
	private float fitnessSum;
	private int dnaSize;
	private Func<T> getRandomGene;
	private Func<int, float> fitnessFunction;
	private Action updateAlgorithm;

	public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, Action updateAlgorithm,
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

		//When the genetic algorithm is created the initial population is generated as follows:
		for (int i = 0; i < populationSize; i++) {
			Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
		}
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
			if (i < Elitism && i < Population.Count) {
				newPopulation.Add(Population[i]);
			} else if (i < Population.Count || crossoverNewDNA) {
				DNA<T> parent1 = ChooseParent();
				DNA<T> parent2 = ChooseParent();

				DNA<T> child = parent1.Crossover(parent2);

				child.Mutate(MutationRate);

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
		if (a.Fitness > b.Fitness) {
			return -1;
		} else if (a.Fitness < b.Fitness) {
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

	private DNA<T> ChooseParent() {
		double r = random.NextDouble();
		double randomNumber = r * fitnessSum*0.9;
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
