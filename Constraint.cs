using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    class Constraint {
        public Klas[] Classes { get; private set; }
        public int ID { get; private set; }
        public float Pref { get; private set; }
        public Func<float> GetFitness;
        public bool IsHardConstraint { get; private set; }
        public string Type { get; private set; }
        public static BitArray AllFalse = new BitArray(7, false);

        public Constraint(Klas[] classes, string type, float pref, bool isHardConstraint) {
            Classes = classes;
            Type = type;
            Pref = pref;
            IsHardConstraint = isHardConstraint;

            if(type == "DIFF_TIME") {
                GetFitness = DIFF_TIME;
            }
        }

        public float DIFF_TIME() {
            float result = Pref;

            for(int i = 0; i < Classes.Length; i++) {
                Klas c1 = Classes[i];
                int minTime = c1.SolutionTime.Start;
                int maxTime = minTime + c1.SolutionTime.Length;
                for(int j = 0; j < Classes.Length; j++) { //currently this is checkinng some combinations of times twice, could be optimized 
                    Klas c2 = Classes[j];
                    if(i != j) { //make sure you're not comparing the same Klas to itself 
                        BitArray andResult = (BitArray)c1.SolutionTime.Days.Clone();
                        andResult.And(c2.SolutionTime.Days);
                        if(andResult != AllFalse) { //this means that at least one day overlaps, so we must check if times overlap
                            if(c2.SolutionTime.Start >=minTime && c2.SolutionTime.Start <= maxTime) {
                                result = 0;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
