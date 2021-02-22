﻿namespace FYPTimetablingSoftware {

    public struct RoomSharing {
        public RoomSharing(string pattern, string ffa, string notAvailable, int[,] departments) {
            //departments as a 2d array where the first dimension is each department tag and the second dimension always has a length of 2, first value then ID
            Pattern = pattern;
            FFA = ffa;
            NotAvailable = notAvailable;
            Departments = departments;
        }
        public string Pattern { get; }
        public string FFA { get; }
        public string NotAvailable { get; }
        public int[,] Departments { get; }

    }

    public struct KlasTime {
        public KlasTime(string days, int start, int length, int breakTime, double pref) {
            Days = days;
            Start = start;
            Length = length;
            BreakTime = breakTime;
            Pref = pref;

        }
        public string Days { get; }
        public int Start { get; }
        public int Length { get; }
        public int BreakTime { get; }
        public double Pref{ get; }

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