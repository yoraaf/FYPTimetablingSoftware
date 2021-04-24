using System;
using System.Collections.Generic;
using System.Text;

namespace FYPTimetablingSoftware {
	public class DNA {
		public SolutionGene[] Genes { get; private set; }
		public float Fitness { get; private set; }

		private Random random;
		private Func<Klas, SolutionGene> getRandomGene;
		private Func<int, float> fitnessFunction;
		public Klas[] KlasArr { get; private set; }
		public int ID { get; private set; }
		public string ConstraintResult = "";
		public Dictionary<string, int> ConstraintViolations = new Dictionary<string, int>() { { "BTB", 0 }, { "BTB_TIME", 0 }, { "CAN_SHARE_ROOM", 0 }, { "DIFF_TIME", 0 }, { "MEET_WITH", 0 }, { "NHB(1.5)", 0 }, { "NHB_GTE(1)", 0 }, { "SAME_DAYS", 0 }, { "SAME_INSTR", 0 }, { "SAME_ROOM", 0 }, { "SAME_START", 0 }, { "SAME_TIME", 0 }, { "SAME_STUDENTS", 0 }, { "SPREAD", 0 }, { "ROOM_CONFLICTS", 0 } };
		public int TotalViolations { get; set; }
		public DNA(int id, int size, Random random, Func<Klas, SolutionGene> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true) {
			Genes = new SolutionGene[size];
			TotalViolations = 0;
			this.random = random;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;
			KlasArr = XMLParser.GetKlasList();
			ID = id;
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

		public DNA Crossover(DNA otherParent, int id) {
			DNA child = new DNA(id, Genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);
			for (int i = 0; i < Genes.Length; i++) {
				child.Genes[i] = LockedRandomDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
			}
			return child;
		}

		/*
		public DNA<T> CrossoverViolation(DNA<T> otherParent, int id) {
			DNA<T> child = new DNA<T>(id, Genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

			for (int i = 0; i < Genes.Length; i++) {
				int totalV = Genes[i].Violations + otherParent.Genes[i].Violations;
				int randomInt = GeneticAlgorithm<T>.LockedRandomInt(0,totalV);
				if (Genes[i].Violations < otherParent.Genes[i].Violations) {
					if (randomInt <= otherParent.Genes[i].Violations) {
						//if its lower than the one with the most violations, pick the lowest violations
						child.Genes[i] = Genes[i];
					} else {
						//if its higher than most violations, pick high
						child.Genes[i] = otherParent.Genes[i];
					}
				} else {
					if (randomInt <= otherParent.Genes[i].Violations) {
						//if its lower than the one with the most violations, pick the lowest violations
						child.Genes[i] = otherParent.Genes[i];
					} else {
						//if its higher than most violations, pick high
						child.Genes[i] = Genes[i];
					}
				}
			}
			return child;
		}*/

		public void Mutate(float mutationRate) {
			for (int i = 0; i < Genes.Length; i++) {
				if (LockedRandomDouble() < mutationRate) {
					Genes[i] = getRandomGene(KlasArr[i]); 
					//this will reassign the gene, giving it new random solution values
				}
			}
		}
		private double LockedRandomDouble() {
			double result = -1;
			lock (GeneticAlgorithm.randLock) {
				result = random.NextDouble();
			}
			return result;
		}
		public string GetGenesString() {
			var sb = new StringBuilder();
			foreach (var c in Genes) {
				sb.Append(c);
			}

			return sb.ToString();
		}
		public override string ToString() {
			return "[DNA] ID: "+ID;
		}
	}
}