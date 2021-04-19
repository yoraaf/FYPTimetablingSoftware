using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    public class Constraint {
        public int[] ClassIDs { get; private set; }
        public int ID { get; private set; }
        public float Pref { get; private set; }
        public Func<List<SolutionGene>, float> FitnessFunction;
        public bool IsHardConstraint { get; private set; }
        public string Type { get; private set; }
        public static BitArray AllFalse = new BitArray(7, false);
        public float BestScore { get; set; } //the score achieved by the best dna

        public static Dictionary<string, int> ConstraintCounts = new Dictionary<string, int>() { { "BTB", 0 }, { "BTB_TIME", 0 }, { "CAN_SHARE_ROOM", 0 }, { "DIFF_TIME", 0 }, { "MEET_WITH", 0 }, { "NHB(1.5)", 0 }, { "NHB_GTE(1)", 0 }, { "SAME_DAYS", 0 }, { "SAME_INSTR", 0 }, { "SAME_ROOM", 0 }, { "SAME_START", 0 }, { "SAME_TIME", 0 }, { "SAME_STUDENTS", 0 }, { "SPREAD", 0 }, { "ROOM_CONFLICTS", 0 } };
        public static Dictionary<string, int> ConstraintWeights = new Dictionary<string, int>() { 
            { "BTB", 0 }, 
            { "BTB_TIME", 0 }, 
            { "CAN_SHARE_ROOM", 0 }, 
            { "DIFF_TIME", 0 }, 
            { "MEET_WITH", 0 }, 
            { "NHB(1.5)", 0 }, 
            { "NHB_GTE(1)", 0 }, 
            { "SAME_DAYS", 0 }, 
            { "SAME_INSTR", 0 }, 
            { "SAME_ROOM", 0 }, 
            { "SAME_START", 0 }, 
            { "SAME_TIME", 0 }, 
            { "SAME_STUDENTS", 0 }, 
            { "SPREAD", 0 }, 
            { "ROOM_CONFLICTS", 0 } 
        };
        public static float RoomConflictWeight = 100000;

        public Constraint(int id, string type, float pref, bool isHardConstraint, int[] classIDs) {
            ID = id;
            ClassIDs = classIDs;
            Type = type;
            Pref = pref;
            IsHardConstraint = isHardConstraint;

            if(type == "DIFF_TIME") {
                FitnessFunction = DIFF_TIME;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "BTB") {
                FitnessFunction = BTB;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "BTB_TIME") {
                FitnessFunction = BTB_TIME;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "CAN_SHARE_ROOM") {

                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "MEET_WITH") {
                FitnessFunction = MEET_WITH;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "NHB(1.5)") {
                FitnessFunction = NHB1_5;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "NHB_GTE(1)") {
                FitnessFunction = NHB_GTE1;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SAME_DAYS") {
                FitnessFunction = SAME_DAYS;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SAME_INSTR") {
                FitnessFunction = SAME_INSTR;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SAME_ROOM") {
                FitnessFunction = SAME_ROOM;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SAME_START") {
                FitnessFunction = SAME_START;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SAME_TIME") {
                FitnessFunction = SAME_TIME;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SAME_STUDENTS") {
                FitnessFunction = SAME_STUDENTS;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "SPREAD") {
                FitnessFunction = SPREAD;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            } else if(type == "ROOM_CONFLICTS") {
                FitnessFunction = ROOM_CONFLICTS;
                ConstraintCounts[type] = ConstraintCounts[type] + 1;
            }
        }
        public override string ToString() {
            return "Constraint: id:" + ID + "; Type:" + Type + "; " + "; IsHardConstraint:" + IsHardConstraint + "; " + "; Pref:" + Pref  + "; ";
        }

        public float GetFitness(SolutionGene[] genes) {
            List<SolutionGene> cGenes = new List<SolutionGene>();
            //int k = 0;
            for (int i = 0; i < genes.Length; i++) {
                if (ClassIDs.Contains(genes[i].ID)) {
                    cGenes.Add(genes[i]);
                    //Console.WriteLine(genes[i]);
                    //k++;
                }
            }
            //float result = FitnessFunction(cGenes);
            float result = (FitnessFunction!=null) ? FitnessFunction(cGenes) : 0;
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
                                if (IsHardConstraint) {
                                    return Pref;
                                }
                                violation = true;
                                adhered--;
                                //result = 0;
                            }
                        }
                    }
                }

            }
            if (IsHardConstraint) {
                return 0;
            }
            float temp = (float)(adhered) / (float)(ClassIDs.Length);
            result = temp * Pref;
            /*if (result > 0) {
                Console.WriteLine("uhhhh");
            }*/
            return result;
        }

        public float SPREAD(List<SolutionGene> cGenes) {
            float result = Pref;

            int adhered = cGenes.Count;
            //^Some efficiency could be added to this^
            for (int i = cGenes.Count - 1; i >= 0; i--) {
                bool violation = false;
                SolutionGene c1 = cGenes[i];
                cGenes.RemoveAt(i);
                int minTime = c1.SolutionTime.Start;
                int maxTime = minTime + c1.SolutionTime.Length;
                for (int j = 0; j < cGenes.Count; j++) { //currently this is checkinng some combinations of times twice, could be optimized 
                    SolutionGene c2 = cGenes[j];
                    if (i != j) { //make sure you're not comparing the same Klas to itself 
                        BitArray andResult = (BitArray)c1.SolutionTime.Days.Clone();
                        andResult.And(c2.SolutionTime.Days);
                        if (andResult != AllFalse) { //this means that at least one day overlaps, so we must check if times overlap
                            if (c2.SolutionTime.Start >= minTime && c2.SolutionTime.Start <= maxTime && !violation) {
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

        /// <summary> SAME_STUDENTS
        /// Given classes are treated as they are attended by the same students, i.e., 
        /// they cannot overlap in time and if they are back-to-back the assigned rooms 
        /// cannot be too far (student limit is used).This constraint can only be required, 
        /// preferred or strongly preferred.
        /// </summary>
        /// <param name="cGenes"></param>
        /// <returns>float of the fitness</returns>
        public float SAME_STUDENTS(List<SolutionGene> cGenes) {
            float result = 0;
            for (int i = cGenes.Count - 1; i >= 0; i--) {
                SolutionGene c1 = cGenes[i];
                cGenes.RemoveAt(i);
                int minTime = c1.SolutionTime.Start;
                int maxTime = minTime + c1.SolutionTime.Length;
                for (int j = 0; j < cGenes.Count; j++) { //currently this is checkinng some combinations of times twice, could be optimized 
                    SolutionGene c2 = cGenes[j];
                    if (i != j) { //make sure you're not comparing the same Klas to itself 
                        BitArray andResult = (BitArray)c1.SolutionTime.Days.Clone();
                        andResult.And(c2.SolutionTime.Days);
                        if (andResult != AllFalse) { //this means that at least one day overlaps, so we must check if times overlap
                            if (c2.SolutionTime.Start >= minTime && c2.SolutionTime.Start <= maxTime) {
                                //This checks if its whithin the times 
                                result = Pref; //this means the constraint has been violated. 
                            } else if (c2.SolutionTime.Start == maxTime ) { //back-to-back
                                int distance = c1.SolutionRoom.CalculateRoomDistance(c2.SolutionRoom);
                                if(c1.SolutionTime.Length >= 18 && distance > 1000) {
                                    //if its a long lesson, the limit is 1000
                                    result = Pref;
                                } else if (distance > 670) {
                                    //otherwise, the limit is 670
                                    result = Pref;
                                }

                            }
                        }
                    }
                }

            }

            return result;
        }

        private float SAME_ROOM(List<SolutionGene> cGenes) {
            int numberOfRoomsShared = 0;
            int nrOfGenes = cGenes.Count;
            SolutionGene[] genesArr = cGenes.ToArray();
            for(int i = 0;i<cGenes.Count;i++) {
                var c1 = cGenes[i];
                cGenes.RemoveAt(i);
                for (int j = 0;j<cGenes.Count;j++) {
                    var c2 = cGenes[j];
                    if (c1.ID != c2.ID) {
                        if (c1.SolutionRoom.ID == c2.SolutionRoom.ID) {
                            numberOfRoomsShared++;
                        } 
                    }
                }
            }
            if (IsHardConstraint) {
                if (numberOfRoomsShared == nrOfGenes - 1) {
                    //if all rooms are shared, that's good
                    return 0;
                } else {
                    //if not all rooms are shared, return pref. (for R thats 40, for P its -40)
                    return Pref;
                }
            } else { //since I know this dataset only has this as a hard constraint, this shouldn't happen.
                return 0;
            }
        }

        private float BTB(List<SolutionGene> cGenes) {
            int numberOfBTB = 0;
            foreach(var g1 in cGenes) {
                foreach(var g2 in cGenes) {
                    if (g1.SolutionTime.Days.Equals(g2.SolutionTime.Days)) {
                        if (g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start || g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start + 1) {
                            //checks if the end of g1 is equal to the start of g2 (or 1 slot later)
                            if (g1.SolutionRoom.ID == g2.SolutionRoom.ID) { //check if its the same room
                                numberOfBTB++;
                            }
                        }
                    }
                }
            }
            
            if(numberOfBTB == cGenes.Count - 1) {
                return IsHardConstraint ? 0 : Pref; //satisfied therefore no penalty 
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }
            
        }

        private float BTB_TIME(List<SolutionGene> cGenes) {
            int numberOfBTB = 0;
            foreach (var g1 in cGenes) {
                foreach (var g2 in cGenes) {
                    if (g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start || g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start + 1) {
                        //checks if the end of g1 is equal to the start of g2 (or 1 slot later)
                        numberOfBTB++;
                    }
                }
            }

            if (numberOfBTB == cGenes.Count - 1) {
                return IsHardConstraint ? 0 : Pref; //satisfies therefore no penalty 
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }

        }

        private float SAME_TIME(List<SolutionGene> cGenes) {
            int numberOfSameTime = 0;
            cGenes.Sort((a, b) => {
                if (a.SolutionTime.Length > b.SolutionTime.Length) {
                    return -1;
                } else if (a.SolutionTime.Length < b.SolutionTime.Length) {
                    return 1;
                } else {
                    return 0;
                }
            });
            
            for(int i = 0; i < cGenes.Count; i++) {
                if (i != cGenes.Count - 1) {
                    var t1 = cGenes[i].SolutionTime;
                    var t2 = cGenes[i + 1].SolutionTime;
                    if(t1.Start <= t2.Start && t1.Start+t1.Length >= t2.Start + t2.Length) {
                        numberOfSameTime++;
                    }
                }
            }
            if(numberOfSameTime == cGenes.Count - 1) {
                return IsHardConstraint ? 0 : Pref;
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }
        }

        private float SAME_START(List<SolutionGene> cGenes) {
            int numberSameStart = 0;
            SolutionGene[] genesArr = cGenes.ToArray();
            for (int i = 0; i < cGenes.Count; i++) {
                var t1 = cGenes[i].SolutionTime;
                cGenes.RemoveAt(i);
                for (int j = 0; j < cGenes.Count; j++) {
                    var t2 = cGenes[j].SolutionTime;
                    if (t1.Start >= t2.Start && t1.Start < t2.Start+6) {
                        //if g1 and g2 are within the same 30min timeslot
                        numberSameStart++;
                    }
                }
            }
            if (numberSameStart == genesArr.Length - 1) {
                return IsHardConstraint ? 0 : Pref;
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }
        }

        private float SAME_DAYS(List<SolutionGene> cGenes) {
            int numberOfSameDays = 0;
            BitArray[] daysArray = new BitArray[cGenes.Count];
            SolutionGene[] genesArr = cGenes.ToArray();
            cGenes.Sort((a, b) => {
                if (a.SolutionTime.NrOfDays > b.SolutionTime.NrOfDays) {
                    return -1;
                } else if (a.SolutionTime.NrOfDays < b.SolutionTime.NrOfDays) {
                    return 1;
                } else {
                    return 0;
                }
            });

            for (int i = 0; i < cGenes.Count; i++) {
                var d1 = cGenes[i].SolutionTime.Days;
                daysArray[i] = d1;
                cGenes.RemoveAt(i);
                for (int j = 0; j < cGenes.Count; j++) {
                    var d2 = cGenes[j].SolutionTime.Days;
                    bool bad = false;
                    for(int k = 0; k < d1.Length; k++) {
                        if (d2[k] && !d1[k]) {
                            //bad, not same day
                            bad = true;
                        }
                    }
                    if (!bad) {
                        numberOfSameDays++;
                    }
                }
            }

            if(numberOfSameDays == genesArr.Length - 1) {
                return IsHardConstraint ? 0 : Pref;
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }
        }

        private float MEET_WITH(List<SolutionGene> cGenes) {
            float score = SAME_DAYS(new List<SolutionGene>(cGenes)) + SAME_ROOM(new List<SolutionGene>(cGenes)) + SAME_TIME(new List<SolutionGene>(cGenes));
            if(score == 0) {
                return 0;
            } else { //this constraint is always R so no need to check
                return Pref;
            }
        }

        private float SAME_INSTR(List<SolutionGene> cGenes) {
            List<SolutionGene> listClone = new List<SolutionGene>(cGenes);
            float score = DIFF_TIME(listClone);
            if(score != 0) {
                return Pref; //this means classes overlap so bad
            }
            //score is guaranteed to be 0 after this point 
            foreach (var g1 in cGenes) {
                foreach (var g2 in cGenes) {
                    if (g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start || g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start + 1) {
                        //checks if the end of g1 is equal to the start of g2 (or 1 slot later)
                        int distance = g1.SolutionRoom.CalculateRoomDistance(g2.SolutionRoom);
                        if(distance >0 && distance <= 50 && score < 1) {
                            score = 1; //set score to 1 unless its already higher
                        } else if (distance > 50 && distance <= 100) {
                            score = 4;
                        } else if(distance > 100){
                            return Pref; //discance > 100 is prohibited
                        }
                    }
                }
            }

            return score;
        }

        private float NHB1_5(List<SolutionGene> cGenes) {
            int numberOfNHB = 0;
            foreach (var g1 in cGenes) {
                foreach (var g2 in cGenes) {
                    if (g1.SolutionTime.Days.Equals(g2.SolutionTime.Days)) {
                        if (g1.SolutionTime.Start + g1.SolutionTime.Length == g2.SolutionTime.Start+18) {
                            //checks if the end of g1 is 1 and a half h ours later than g2 start (18 units)
                            numberOfNHB++;
                        }
                    }
                }
            }

            if (numberOfNHB == cGenes.Count - 1) {
                return IsHardConstraint ? 0 : Pref; //satisfied therefore no penalty 
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }
        }

        private float NHB_GTE1(List<SolutionGene> cGenes) {
            int numberOfNHB_GTE = 0;
            foreach (var g1 in cGenes) {
                foreach (var g2 in cGenes) {
                    if (g1.SolutionTime.Days.Equals(g2.SolutionTime.Days)) {
                        if (g1.SolutionTime.Start + g1.SolutionTime.Length >= g2.SolutionTime.Start + 12) {
                            //checks if the end of g1 is 1 and a half h ours later than g2 start (18 units)
                            numberOfNHB_GTE++;
                        }
                    }
                }
            }

            if (numberOfNHB_GTE == cGenes.Count - 1) {
                return IsHardConstraint ? 0 : Pref; //satisfied therefore no penalty 
            } else {
                return IsHardConstraint ? Pref : 0; //violated therefore penalty (unless p then this is a good thing)
            }
        }
        private float ROOM_CONFLICTS(List<SolutionGene> cGenes) {
            //most of this is copy pasted and doesn't work
            int roomConflicts = 0;
            SolutionGene[] genesArr = cGenes.ToArray(); //this is mostly for debugging to save the original array
            Dictionary<SolutionGene, List<SolutionGene>> conflictingGenes = new Dictionary<SolutionGene, List<SolutionGene>>();
            var KlasList = XMLParser.GetKlasList();

            for (int i = 0; i < cGenes.Count; i++) {
                var c1 = cGenes[i];
                cGenes.RemoveAt(i);
                for (int j = 0; j < cGenes.Count; j++) {
                    var c2 = cGenes[j];
                    if (KlasList[c1.ID - 1].Can_Share_Room != null) {
                        if (Array.Exists(KlasList[c1.ID - 1].Can_Share_Room, element => element == c2.ID)) {
                            continue; //if the two classes are allowed to share a room, skip the rest of this itteration 
                        }
                    }
                    if (c1.ID != c2.ID) {
                        var c1Start = c1.SolutionTime.Start;
                        var c2Start = c2.SolutionTime.Start;
                        var c1End = c1Start + c1.SolutionTime.Length;
                        var c2End = c2Start + c2.SolutionTime.Length;
                        if((c1Start<=c2Start && c1End>=c2End) || (c2Start<=c1Start && c2End >= c1End)) {
                            //The above checks if they are taught at or within the same time
                            if (c1.SolutionRoom.ID == c2.SolutionRoom.ID) {
                                //Check if they're in the same room
                                var sameDay = false;
                                foreach(bool b1 in c1.SolutionTime.Days) {
                                    foreach(bool b2 in c2.SolutionTime.Days) {
                                        if(b1 && b2) {
                                            //Check if they have any days in common, if yes break both loops
                                            sameDay = true;
                                            break;
                                        }
                                    }
                                    if (sameDay) { break; }
                                }
                                if (sameDay) {
                                    //if all if statements have passed true, and they share a day,
                                    //increase the conflict counter and add to the conflict list for debugging
                                    roomConflicts++;
                                    if (conflictingGenes.ContainsKey(c1)) {
                                        conflictingGenes[c1].Add(c2);
                                    } else {
                                        conflictingGenes.Add(c1, new List<SolutionGene>() { c2 });
                                    }
                                    
                                }
                            }
                        }

                        
                    }
                }
            }

            float score = RoomConflictWeight*roomConflicts;
            return score;
            
        }

        private float CAN_SHARE_ROOM(List<SolutionGene> cGenes) {
            //This doesn't seem to be a real constraint, more of a property.
            return 0;
        }



    }
}
