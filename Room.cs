using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    public class Room {
        public int ID { get; }
        public bool Constraint { get; }
        public int Cap { get; }
        public int[] Loc { get; }
        public RoomSharing Sharing { get; }
        private bool HasSharing = false;

        public Room(int id, bool constraint, int cap, int[] loc, RoomSharing sharing) {
            ID = id;
            Constraint = constraint;
            Cap = cap;
            Loc = loc;
            Sharing = sharing;
            HasSharing = true;

        }
        public Room(int id, bool constraint, int cap, int[] loc) {
            ID = id;
            Constraint = constraint;
            Cap = cap;
            Loc = loc;
            HasSharing = false;

        }

        public int CalculateRoomDistance(Room b) {
            double distance = 10 * Math.Pow((Math.Pow(((double)(b.Loc[0] - Loc[0])), 2d) + Math.Pow(((double)(b.Loc[1] - Loc[1])), 2d)),0.5);
            return (int)distance;
        }

        public override string ToString() {
            return "Room: id:" + ID + "; cap:" + Cap + "; loc(" + Loc[0] + "," + Loc[1] + "); HasSharing:"+HasSharing;
        }

    }
}
