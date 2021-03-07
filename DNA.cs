using System;
using System.Text;

namespace FYPTimetablingSoftware {
	public class DNA<T> {
		public T[] Genes { get; private set; }
		public float Fitness { get; private set; }

		private Random random;
		private Func<Klas, T> getRandomGene;
		private Func<int, float> fitnessFunction;

		public DNA(int size, Random random, Func<Klas, T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true) {
			Genes = new T[size];
			this.random = random;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			Klas[] KlasList = XMLParser.GetKlasList();
			if (shouldInitGenes) {
				for (int i = 0; i < Genes.Length; i++) {
					Genes[i] = getRandomGene(KlasList[i]); //add cloning here 
				}
			}
		}

		public float CalculateFitness(int index) {
			Fitness = fitnessFunction(index);
			return Fitness;
		}

		public DNA<T> Crossover(DNA<T> otherParent) {
			DNA<T> child = new DNA<T>(Genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

			for (int i = 0; i < Genes.Length; i++) {
				child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
			}

			return child;
		}

		public void Mutate(float mutationRate) {
			Klas[] KlasList = XMLParser.GetKlasList();
			for (int i = 0; i < Genes.Length; i++) {
				if (random.NextDouble() < mutationRate) {
					Genes[i] = getRandomGene(KlasList[i]); 
					//this will reassign the gene, giving it new random solution values
				}
			}
		}

		public string GetGenesString() {
			var sb = new StringBuilder();
			foreach (var c in Genes) {
				sb.Append(c);
			}

			return sb.ToString();
		}

	}
}