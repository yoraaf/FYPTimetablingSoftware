using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    public class NewMember<T> {
        public static ManualResetEvent DoneEvt;
        private int N;
        public static int counter;
        public static bool ShouldWait = true;
        public DNA<T> Member { get; private set; }
        //public static List<DNA<T>> Population { get; set; } // T is SolutionGene
        //public static int Elitism;
        //public static Random random;
        public static GeneticAlgorithm<T> ga;
        public NewMember(int n) {
            N = n;
            //DoneEvt = doneEvt;
        }

        public void GenerateNewMember(object threadContext) {
            while (ShouldWait) {
                Thread.Sleep(1);
            }
            if (N < ga.Elitism && N < ga.Population.Count) { //elitism makes it so that the top (5) make it into the next generation 
                Member = ga.Population[N];
            } else if (N < ga.Population.Count) {
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);

                child.Mutate(ga.MutationRate);
                /*Debug.WriteLine("Parent 1: "+ ArrayToString(parent1.Genes));
				Debug.WriteLine("Parent 2: "+ ArrayToString(parent2.Genes));
				Debug.WriteLine("Child:    "+ ArrayToString(child.Genes));*/
                Member = child;
            } else {
                Member= new DNA<T>(ga.dnaSize, ga.random, ga.getRandomGene, ga.fitnessFunction, shouldInitGenes: true);
            }
            Console.WriteLine("[ThreadPool] Thread " + N + " completed, left: "+counter);
            if (Interlocked.Decrement(ref counter) == 0) { 
                DoneEvt.Set();
            };
        }

        private static DNA<T> ChooseParent() {
            //tournament style selection
            double randomNumber = GetFitnessSum() * ga.random.NextDouble();
            int tournamentSize = (Int32)Math.Floor(ga.Population.Count * 0.2);
            DNA<T>[] tournamentMembers = new DNA<T>[tournamentSize];
            for (int i = 0; i < tournamentSize; i++) {
                DNA<T> x;
                do {
                    //add random person from population to the tournament and make sure they aren't in already
                    x = ga.Population[ga.random.Next(0, ga.Population.Count)];
                } while (tournamentMembers.Contains(x));
                tournamentMembers[i] = x;
            }
            //sort tournament members my fitness and return the fittest one
            Array.Sort(tournamentMembers, CompareDNA);
            //Console.WriteLine("top ")
            return tournamentMembers[0];
        }
        private static double GetFitnessSum() {
            double output = 0;
            for (int i = 0; i < ga.Population.Count; i++) {
                output += ga.Population[i].Fitness;
            }
            return output;
        }
        private static int CompareDNA(DNA<T> a, DNA<T> b) {
            //had to switch the >< around cz low = better
            if (a.Fitness < b.Fitness) {
                return -1;
            } else if (a.Fitness > b.Fitness) {
                return 1;
            } else {
                return 0;
            }
        }
    }
}
