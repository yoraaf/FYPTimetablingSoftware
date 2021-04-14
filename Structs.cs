using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        public SolutionGene(int id, Room solutionRoom, KlasTime solutionTime) {
            ID = id;
            SolutionTime = solutionTime;
            SolutionRoom = solutionRoom;
        }
        public override string ToString() {
            return "SolutionGene: "+ID+" <" + SolutionRoom+"> <"+ SolutionTime + ">";
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