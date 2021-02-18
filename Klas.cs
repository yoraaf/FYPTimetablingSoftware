using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTimetablingSoftware {
    class Klas {
        //gave it a Dutch name because its easy to guess and did not want to make a class called class
        public KlasTime[] Times { get; private set; }
        public Room[] Rooms { get; private set; }
        public string Instructor { get; private set; }
        public int ID { get; private set; }
        public int Offering { get; private set; }
        public int Config { get; private set; }
        public int Subpart { get; private set; }
        public int ClassLimit { get; private set; }
        public int Department { get; private set; }
        //scheduler and commited seem to be the same for every single entry so they are not in this list

        public Klas(int id, int offering, int config, int subpart, int classLimit, int department, string instructor, KlasTime[] times, Room[] rooms) {
            ID = id; Offering = offering; Config = config; Subpart = subpart; ClassLimit = classLimit; 
            Department = department; Instructor = instructor; Times = times; Rooms = rooms;
            //^assign all the parameters^


        }
        public Klas(int id, int offering, int config, int subpart, int classLimit, int department) {
            ID = id; Offering = offering; Config = config; Subpart = subpart; ClassLimit = classLimit;
            Department = department; 
            //^assign all the parameters^


        }
    }
}
