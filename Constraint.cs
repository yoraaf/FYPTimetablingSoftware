using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    class Constraint {
        public int[] ClassIDs { get; private set; }
        public int ID { get; private set; }
        public float Pref { get; private set; }
        public Func<List<SolutionGene>, float> FitnessFunction;
        public bool IsHardConstraint { get; private set; }
        public string Type { get; private set; }
        public static BitArray AllFalse = new BitArray(7, false);

        public Constraint(int id, string type, float pref, bool isHardConstraint, int[] classIDs) {
            ID = id;
            ClassIDs = classIDs;
            Type = type;
            Pref = pref;
            IsHardConstraint = isHardConstraint;

            if(type == "DIFF_TIME") {
                FitnessFunction = DIFF_TIME;
            }
        }

        public float GetFitness(SolutionGene[] genes) {
            List<SolutionGene> cGenes = new List<SolutionGene>();
            //int k = 0;
            for (int i = 0; i < genes.Length; i++) {
                if (ClassIDs.Contains(genes[i].ID)) {
                    cGenes.Add(genes[i]);
                    Console.WriteLine(genes[i]);
                    //k++;
                }
            }
            float result = FitnessFunction(cGenes);
            return result;
        }

        public float DIFF_TIME(List<SolutionGene> cGenes) {
            float result = Pref;
            
            int adhered = cGenes.Count;
            //^Some efficiency could be added to this^
            for (int i = cGenes.Count-1; i >= 0; i--) {
                bool violation = false;
                SolutionGene c1 = cGenes[i];
                cGenes.RemoveAt(i);
                int minTime = c1.SolutionTime.Start;
                int maxTime = minTime + c1.SolutionTime.Length;
                for(int j = 0; j < cGenes.Count; j++) { //currently this is checkinng some combinations of times twice, could be optimized 
                    SolutionGene c2 = cGenes[j];
                    if(i != j) { //make sure you're not comparing the same Klas to itself 
                        BitArray andResult = (BitArray)c1.SolutionTime.Days.Clone();
                        andResult.And(c2.SolutionTime.Days);
                        if(andResult != AllFalse) { //this means that at least one day overlaps, so we must check if times overlap
                            if(c2.SolutionTime.Start >=minTime && c2.SolutionTime.Start <= maxTime && !violation) {
                                //This checks if its whithin the times and if there's already been a violation
                                violation = true;
                                adhered--;
                                //result = 0;
                            }
                        }
                    }
                }

            }
            float temp = (float)(adhered) / (float)(ClassIDs.Length);
            result = temp * Pref;
            if (result > 0) {
                Console.WriteLine("uhhhh");
            }
            return result;
        }
    }
}
