using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FYPTimetablingSoftware {

    public struct RoomSharing {
        public RoomSharing(string pattern, string ffa, string notAvailable, int[,] departments) {
            //departments as a 2d array where the first dimension is each department tag and the second dimension always has a length of 2, first value then ID
            Pattern = pattern;
            FFA = ffa;
            NotAvailable = notAvailable;
            Departments = departments;
        }
        public string Pattern { get; private set; }
        public string FFA { get; private set; }
        public string NotAvailable { get; private set; }
        public int[,] Departments { get; private set; }

    }

    public class KlasTime {
        public KlasTime(string days, int start, int length, int breakTime, double pref) {
            //Days = days;
            Start = start;
            Length = length;
            BreakTime = breakTime;
            Pref = pref;
            DaysString = days;
            Days = new BitArray(days.Select(c => c == '1').ToArray());
            NrOfDays = days.Count(f => f == '1');
        }
        public BitArray Days { get; private set; }
        public int NrOfDays { get; private set; }
        public string DaysString { get; private set; }
        public int Start { get; private set; }
        public int Length { get; private set; }
        public int BreakTime { get; private set; }
        public double Pref{ get; private set; }
        public override string ToString() {
            return "KlasTime: Start:" + Start + " Length: "+Length;
        }

    }

    public class SolutionGene {
        public KlasTime SolutionTime { get; set; }
        public Room SolutionRoom { get; set; }
        public int ID { get; set; }
        public int Violations { get; set; }
        public SolutionGene(int id, Room solutionRoom, KlasTime solutionTime) {
            ID = id;
            SolutionTime = solutionTime;
            SolutionRoom = solutionRoom;
            Violations = 0;
        }
        public override string ToString() {
            return "SolutionGene: "+ID+" <" + SolutionRoom+"> <"+ SolutionTime + ">";
        }
    }

    public class CsvData {
        public int Generation { get; private set; }
        public float Fitness { get; private set; }
        public int TimeTaken { get; private set; }
        public int BTB { get; private set; }
        public int BTB_TIME { get; private set; }
        public int CAN_SHARE_ROOM { get; private set; }
        public int DIFF_TIME { get; private set; }
        public int MEET_WITH { get; private set; }
        public int NHB { get; private set; }
        public int NHB_GTE { get; private set; }
        public int SAME_DAYS { get; private set; }
        public int SAME_INSTR { get; private set; }
        public int SAME_ROOM { get; private set; }
        public int SAME_START { get; private set; }
        public int SAME_TIME { get; private set; }
        public int SAME_STUDENTS { get; private set; }
        public int SPREAD { get; private set; }
        public int ROOM_CONFLICTS { get; private set; }
        public string Properties { get; private set; }
        public string PropValue { get; private set; }
        public Dictionary<string, int> ConstraintViolations { get; private set; }

        public CsvData(int gen, float fit, Dictionary<string, int> violations, int timeTaken) {
            Generation = gen;
            Fitness = fit;
            ConstraintViolations = violations;
            TimeTaken = timeTaken;

            var thisType = typeof(CsvData);
            foreach(var entry in violations) {
                if (entry.Key == "NHB(1.5)") {
                    PropertyInfo propInfo = thisType.GetProperty("NHB");
                    propInfo.SetValue(this, entry.Value);
                } else if (entry.Key == "NHB_GTE(1)") {
                    PropertyInfo propInfo = thisType.GetProperty("NHB_GTE");
                    propInfo.SetValue(this, entry.Value);
                } else if (entry.Key == "CAN_SHARE_ROOM") {
                    //this one does nothing 
                } else {
                    PropertyInfo propInfo = thisType.GetProperty(entry.Key);
                    propInfo.SetValue(this, entry.Value);
                }
            }

            switch (gen) {
                case 2:
                    Properties = "Tournament_R";
                    PropValue = "" + Program.TournamentRatio;
                    break;
                case 3:
                    Properties = "Crossover";
                    PropValue = "" + Program.CrossoverMethod;
                    break;
                case 4:
                    Properties = "Selection";
                    PropValue = "" + Program.SelectionMethod;
                    break;
                case 5:
                    Properties = "Mutation";
                    PropValue = "" + Program.MutationRate;
                    break;
                case 6:
                    Properties = "Elitism";
                    PropValue = "" + Program.Elitism;
                    break;
                case 7:
                    Properties = "Population";
                    PropValue = "" + Program.PopulationSize;
                    break;
            }
        }
    }

    /*public struct Room {
        public Room(int id, bool constraint, int cap, int[] loc, double pref, RoomSharing sharing) {
            ID = id;
            Constraint = constraint;
            Cap = cap;
            Loc = loc;
            Pref = pref;
            Sharing = sharing;

        }


        public int ID { get; }
        public bool Constraint { get; }
        public int Cap { get; }
        public int[] Loc { get; }
        public double Pref { get; }
        public RoomSharing Sharing { get; }

    }*/

}