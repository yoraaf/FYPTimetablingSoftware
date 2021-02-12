using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATest01 {
    class Klas {
        //gave it a Dutch name because its easy to guess and did not want to make a class called class
        public string[][] Time { get; set;}
        public string[][] Room { get; set; }
        
        public string Instructor { get; set; }
        public int ID { get; set; }
        public int Offering { get; set; }
        public int Config { get; set; }
        public int Subpart { get; set; }
        public int ClassLimit { get; set; }
        public int Department { get; set; }
        //scheduler and commited seem to be the same for every single entry so they are not in this list

        public Klas(int id, int offering, int config, int subpart, int classLimit, int department, string instructor, string[][] time, string[][] room) {
            ID = id; Offering = offering; Config = config; Subpart = subpart; ClassLimit = classLimit; 
            Department = department; Instructor = instructor; Time = time; Room = room;

        }
    }
}
