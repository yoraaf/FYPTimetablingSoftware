using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FYPTimetablingSoftware {
    static class Program {
        public static readonly float HardConstraintWeight = 40; //default 40
        public static readonly float TournamentRatio = 0.02f;   //default 0.02f
        public static readonly float MutationRate = 0.0f;      //default 0.01f
        public static readonly int PopulationSize = 500;        //default 500
        public static readonly int Elitism = 1;                 //default 5
        public static readonly string CrossoverMethod = "Discrete"; //Either Discrete or Violation
        public static readonly string SelectionMethod = "Tournament";

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
