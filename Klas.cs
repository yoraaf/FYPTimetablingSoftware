using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    public class Klas {
        //gave it a Dutch name because its easy to guess and did not want to make a class called class
        public KlasTime[] Times { get; private set; }
        public Room[] Rooms { get; private set; }
        public int Instructor { get; private set; }
        public int ID { get; private set; }
        public int Offering { get; private set; }
        public int Config { get; private set; }
        public int Subpart { get; private set; }
        public int ClassLimit { get; private set; }
        public int Department { get; private set; }
        public int Parent { get; private set; }
        public int[] Can_Share_Room;
        public Dictionary<int, double> RoomPref { get; private set; } //was considering making a dict for this linking the room to the pref but since they're both arrays they'll share the same index anyway
        //some rooms have a parent attribute, these don't have their own offering or config
        //scheduler and commited seem to be the same for every single entry so they are not in this list

        /*public KlasTime SolutionTime { get; set; }
        public Room SolutionRoom { get; set; }*/


        public Klas(int id, int offering, int config, int subpart, int classLimit, int department, int instructor, KlasTime[] times, Room[] rooms, Dictionary<int, double> roomPref) {
            ID = id; Parent = -1; Offering = offering; Config = config; Subpart = subpart; ClassLimit = classLimit; 
            Department = department; Instructor = instructor; Times = times; Rooms = rooms; RoomPref = roomPref;
            //^assign all the parameters^
        }
        public Klas(int id, int offering, int config, int subpart, int classLimit, int department, int instructor, KlasTime[] times) {
            ID = id; Parent = -1; Offering = offering; Config = config; Subpart = subpart; ClassLimit = classLimit;
            Department = department; Instructor = instructor; Times = times; Rooms = null; RoomPref = null;
            //^assign all the parameters^
        }
        public Klas(int id, int parent, int subpart, int classLimit, int department, int instructor, KlasTime[] times, Room[] rooms, Dictionary<int, double> roomPref) {
            ID = id; Parent = parent; Offering = -1; Config = -1; Subpart = subpart; ClassLimit = classLimit;
            Department = department; Instructor = instructor; Times = times; Rooms = rooms; RoomPref = roomPref;
            //^assign all the parameters^
        }

        public override string ToString() {
            return "Klas: id:" + ID + "; Offering:" + Offering + "; " + "; ClassLimit:" + ClassLimit + "; " + "; Rooms:" + Rooms.Length + "; " + "; Times:" + Times.Length + "; ";
        }

    }
}
