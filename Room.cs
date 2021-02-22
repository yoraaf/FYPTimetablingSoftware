using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    class Room {
        public int ID { get; }
        public bool Constraint { get; }
        public int Cap { get; }
        public int[] Loc { get; }
        //public double Pref { get; } This will have to be added later for each class
        public RoomSharing Sharing { get; }

        public Room(int id, bool constraint, int cap, int[] loc, RoomSharing sharing) {
            ID = id;
            Constraint = constraint;
            Cap = cap;
            Loc = loc;
            //Pref = pref;
            Sharing = sharing;

        }
        public Room(int id, bool constraint, int cap, int[] loc) {
            ID = id;
            Constraint = constraint;
            Cap = cap;
            Loc = loc;
            //Sharing = null;
            //Pref = pref;

        }
        public override string ToString() {
            return "Room: id:" + ID + "; cap:" + Cap+"; loc("+Loc[0]+","+Loc[1]+") ";
        }

    }
}
