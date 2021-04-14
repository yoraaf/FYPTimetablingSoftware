using System;
using System.Collections.Generic;
using System.Text;

namespace FYPTimetablingSoftware {
	public class DNA<T> {
		public T[] Genes { get; private set; }
		public float Fitness { get; private set; }

		private Random random;
		private Func<Klas, T> getRandomGene;
		private Func<int, float> fitnessFunction;
		public Klas[] KlasArr { get; private set; }
		public string ConstraintResult = "";
		public Dictionary<string, int> ConstraintViolations = new Dictionary<string, int>() { { "BTB", 0 }, { "BTB_TIME", 0 }, { "CAN_SHARE_ROOM", 0 }, { "DIFF_TIME", 0 }, { "MEET_WITH", 0 }, { "NHB(1.5)", 0 }, { "NHB_GTE(1)", 0 }, { "SAME_DAYS", 0 }, { "SAME_INSTR", 0 }, { "SAME_ROOM", 0 }, { "SAME_START", 0 }, { "SAME_TIME", 0 }, { "SAME_STUDENTS", 0 }, { "SPREAD", 0 } };
		public DNA(int size, Random random, Func<Klas, T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true) {
			Genes = new T[size];
			this.random = random;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			KlasArr = XMLParser.GetKlasList();
			if (shouldInitGenes) {
				for (int i = 0; i < Genes.Length; i++) {
					Genes[i] = getRandomGene(KlasArr[i]); //add cloning here 
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
			for (int i = 0; i < Genes.Length; i++) {
				if (random.NextDouble() < mutationRate) {
					Genes[i] = getRandomGene(KlasArr[i]); 
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