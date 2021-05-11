﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Timers;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using CsvHelper;

namespace FYPTimetablingSoftware {
    public partial class Form1 : Form {
        private delegate void SafeTextEdit(SolutionGene[] bestGenes, float bestFitness, int generation, int populationSize);
        Dispatcher GUIDispatcher;
        private System.Timers.Timer aTimer;

        private readonly SynchronizationContext SyncContext;
        private DateTime StartTime;
        readonly int populationSize = Program.PopulationSize;
        readonly float mutationRate = Program.MutationRate;
        readonly int elitism = Program.Elitism;
        bool enabled = false;
        bool aRunning = false;
        float oldFitness = Int32.MaxValue;
        Series fitnessSeries;
        private Constraint[] SoftConstraints;
        private Constraint[] HardConstraints;
        private Stopwatch stopwatch = new Stopwatch();
        private string[] constraintResultsArr = new string[Program.PopulationSize];
        private long AverageTimePerGen = 0;
        public static int TotalConstraintNr;
        public static float MaxViolationWeight;
        public static float MinViolationWeight;
        public static string PropertiesString;

        private bool LoopRunning = true;
        private String startTimeString;
        private string workingDirectory;
        private string projectDirectory;
        private GeneticAlgorithm ga;
        private System.Random random;
        
        public Form1() {
            workingDirectory = Environment.CurrentDirectory;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            SyncContext = SynchronizationContext.Current;
            GUIDispatcher = Dispatcher.CurrentDispatcher;
            InitializeComponent();
        }
        private void initAlgorithm() {
            if(Program.CrossoverMethod == "Tournament") {
                PropertiesString = "P_" + Program.PopulationSize.ToString().PadLeft(4, '0') + "-S_" + Program.SelectionMethod.PadRight(10, '-') + "-C_Tournament" + Program.TournamentRatio.ToString().PadRight(4,'0') + "-M_" + Program.MutationRate.ToString().PadRight(5, '0') + "_";
            } else {
                PropertiesString = "P_" + Program.PopulationSize.ToString().PadLeft(4, '0') + "-S_" + Program.SelectionMethod.PadRight(10, '-') + "-C_" + Program.CrossoverMethod.PadRight(7, '-')+"-M_"+Program.MutationRate.ToString().PadRight(5,'0')+"_";
            }
            
            StartTime = DateTime.Now;
            var culture = new CultureInfo("en-GB");
            StartTimeValueLbl.Text = StartTime.ToString(culture);
            
            startTimeString = StartTime.ToString(culture);
            startTimeString = startTimeString.Replace('/', '-');
            startTimeString = startTimeString.Replace(' ', '_');
            startTimeString = startTimeString.Replace(':', '-');
            Debug.WriteLine(startTimeString);
            stopwatch.Start();
            fitnessSeries = fitnessChart.Series.Add("fitness");
            fitnessSeries.ChartType = SeriesChartType.Line;
            aTimer = new System.Timers.Timer(1);

            random = new System.Random();
            ga = new GeneticAlgorithm(populationSize, XMLParser.GetKlasList().Length, random, GetRandomSolutionGene, FitnessFunction, UpdateAlgorithm, elitism, mutationRate);
        }

        private SolutionGene GetRandomSolutionGene(Klas k) {
            Room a = (k.Rooms.Length>0) ? k.Rooms[GeneticAlgorithm.LockedRandomInt(0, k.Rooms.Length)] : null;
            KlasTime b = (k.Times.Length>0) ? k.Times[GeneticAlgorithm.LockedRandomInt(0, k.Times.Length)] : null;
            SolutionGene output = new SolutionGene(k.ID, a, b);
            return output;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {
            if (!aRunning) { //check if the previous call is still running 
                UpdateAlgorithm();
            }
        }

        private void AlgorithmLoop() {
            Debug.WriteLine("Loop started");
            while (LoopRunning) {
                if (!aRunning) {
                    UpdateAlgorithm();
                }
            }
            Debug.WriteLine("Loop ended/paused");
        }

        public void UpdateAlgorithm() {
            if (!enabled) { return; } //if something paused or stopped the algorithm, don't update 
            aRunning = true;
            ga.NewGeneration();
            try {
                SafeTextEdit del = new SafeTextEdit(UpdateText);
                GUIDispatcher.Invoke(del, new object[] { ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count });
            } catch (Exception e){
                Console.WriteLine("{0} Exception caught.", e);
            }

            if(ga.Generation == 25) {
                AverageTimePerGen = stopwatch.ElapsedMilliseconds / 25;
            }

            if (ga.Generation == Program.FinalGeneration) {
                if (stopwatch.IsRunning) {
                    stopwatch.Stop();
                }
                Console.WriteLine("Reached Gen "+ga.Generation+", stopping...");
                Console.WriteLine("Generation: " + ga.Generation + " ; Fitness: " + ga.BestFitness);
                
                enabled = false;
                LoopRunning = false;
                aTimer.Enabled = enabled;

                //The popup is to draw the user's attention when gen 500 is reached 
                var popup = new Form();
                popup.ShowDialog();
            }
            aRunning = false;
        }

        public void ResumeAlgorithm() {
            enabled = true;
            LoopRunning = true;
            AlgorithmLoop();
            aTimer.Enabled = enabled;
        }

        private float FitnessFunction(int index) { //calculate fitness of 1 member of the population (DNA)
            float score = 0;
            DNA dna = ga.Population[index];

            foreach (var key in dna.ConstraintViolations.Keys.ToList()) {
                dna.ConstraintViolations[key] = 0;
            }
            dna.TotalViolations = 0;

            string constraintResults = "";
            for (int i = 0; i < SoftConstraints.Length; i++) {
                float fitness = SoftConstraints[i].GetFitness(dna.Genes);
                constraintResults += SoftConstraints[i].Type + " "+fitness + " ; ";
                if(SoftConstraints[i].Type == "ROOM_CONFLICTS") {
                    int v = (int)Math.Floor(fitness / Constraint.RoomConflictWeight);
                    dna.ConstraintViolations[SoftConstraints[i].Type] += v;
                    dna.TotalViolations +=v;
                } else if (fitness > 0) {
                    dna.ConstraintViolations[SoftConstraints[i].Type] += 1;
                    dna.TotalViolations++;
                }
                score += fitness;
            }

            for (int i = 0; i < HardConstraints.Length; i++) {
                float fitness = HardConstraints[i].GetFitness(dna.Genes);
                constraintResults += HardConstraints[i].Type + " " + fitness + " ; ";
                if (fitness > 0) {
                    dna.ConstraintViolations[HardConstraints[i].Type] += 1;
                    dna.TotalViolations++;
                }
                score += fitness;
            }
            float timePref = 0;
            for(int i = 0; i < dna.Genes.Length; i++) {
                timePref += (float)dna.Genes[i].SolutionTime.Pref;
            }
            float roomPref = 0;
            for (int i = 0; i < dna.Genes.Length; i++) {
                if (dna.Genes[i].SolutionRoom != null) {
                    int id = dna.Genes[i].SolutionRoom.ID;
                    roomPref += (float)dna.KlasArr[i].RoomPref[id];
                }
            }
            
            constraintResults += "Time "+timePref + " ; ";
            constraintResults += "Room " + roomPref + " ; ";
            score += timePref * 0.1f;
            score += roomPref * 0.1f;
            constraintResults += "T " + score + "\r\n";
            dna.ConstraintResult = constraintResults;
            constraintResultsArr[index] = constraintResults;
            return score;
        }

        private long LastTimeTaken = 0;
        private List<CsvData> CsvRecords = new List<CsvData>();

        private void UpdateText(SolutionGene[] bestGenes, float bestFitness, int generation, int populationSize) {
            float improvement = (bestFitness - oldFitness)*-1;
            int timeTaken = (int)(stopwatch.ElapsedMilliseconds - LastTimeTaken);
            LastTimeTaken = stopwatch.ElapsedMilliseconds;

            CsvData dataObject = new CsvData(ga.Generation, ga.BestDNA.Fitness, ga.BestDNA.ConstraintViolations, timeTaken);
            CsvRecords.Add(dataObject);
            using (var writer = new StreamWriter(projectDirectory+"\\output\\" + PropertiesString + startTimeString + ".csv")) 
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                csv.WriteRecords(CsvRecords);
            }
            string constraintViolations = "";
            foreach(var entry in ga.BestDNA.ConstraintViolations) {
                constraintViolations += entry.Key + ": " + entry.Value + "\r\n";
            }

            AllMembersBox.Text = constraintViolations+"Total Violations: "+ga.BestDNA.TotalViolations;
            if (improvement > 0.0001) {
                fitnessSeries.Points.AddXY(generation, bestFitness*-1);
                graphDataTextBox.AppendText(generation+"\t ; \t"+bestFitness + "\r\n");
                oldFitness = bestFitness;
            }
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",ts.Hours, ts.Minutes, ts.Seconds);
            TimeElapsedValueLbl.Text = elapsedTime;

            fitnessLbl.Text = bestFitness.ToString();
            generationLbl.Text = generation.ToString();
            if(ga.Generation == 26) {
                AverageValueLbl.Text = AverageTimePerGen + "ms";
            }
        }

        private async void startButton_Click(object sender, EventArgs e) {
            initAlgorithm();
            startButton.Enabled = false;
            await Task.Run(() => {
                ResumeAlgorithm();
            });
            
        }

        private async void pauseButton_Click(object sender, EventArgs e) {
            await Task.Run(() => {
                if (stopwatch.IsRunning) {
                    stopwatch.Stop();
                }
                Console.WriteLine("Paused algorithm \r\nGeneration: " + ga.Generation + " ; Fitness: " + ga.BestFitness);
                enabled = false;
                LoopRunning = false;
                aTimer.Enabled = enabled;
            });
            
        }

        private async void resumeButton_Click(object sender, EventArgs e) {
            await Task.Run(() => {
                if (!stopwatch.IsRunning) {
                    stopwatch.Start();
                }
                ResumeAlgorithm();
            });
        }
        
        
        
        
        private void label2_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void XMLTestButton_Click(object sender, EventArgs e) {
            XMLParser p = new XMLParser(projectDirectory+ "/DataSet/pu-fal07-llr_FYP_fix.xml");

            SoftConstraints = XMLParser.GetSoftConstraints();
            HardConstraints = XMLParser.GetHardConstraints();
            Console.BackgroundColor = ConsoleColor.Green;
            foreach(var entry in Constraint.ConstraintCounts) {
                Console.WriteLine(entry.Key + " \t " + entry.Value);
            }
            Console.WriteLine("Dataset loaded succesfully");
            startButton.Enabled = true;
            TotalConstraintNr = SoftConstraints.Length + HardConstraints.Length;
            MaxViolationWeight = 0;
            for(int i = 0; i < SoftConstraints.Length; i++) {
                if (SoftConstraints[i].Pref > 0) {
                    MaxViolationWeight += SoftConstraints[i].Pref;
                } else if(SoftConstraints[i].Pref < 0) {
                    MinViolationWeight += SoftConstraints[i].Pref;
                }
            }
            for(int i = 0; i < HardConstraints.Length; i++) {
                if (HardConstraints[i].Pref > 0) {
                    MaxViolationWeight += HardConstraints[i].Pref;
                } else if(HardConstraints[i].Pref > 0){
                    MinViolationWeight += HardConstraints[i].Pref;
                }
            }
            int NumberOfConstraints = HardConstraints.Length + SoftConstraints.Length;
            MaxWeightValueLbl.Text = "" + NumberOfConstraints + " + "+ XMLParser.GetKlasList().Length;
            MinWeightValueLbl.Text = "" + MinViolationWeight;
            NrOfConstraintsValuelbl.Text = ""+TotalConstraintNr;
        }

        private void randomTestButton_Click(object sender, EventArgs e) {
            
            Console.WriteLine("Activated breakpoint");

        }

        private void TestButton2_Click(object sender, EventArgs e) {
            Random r = new System.Random(666);
            int[] arr = new int[10000000];
            double fitnessBias = 2.5;
            for (int i = 0; i < 10000000; i++) {
                int index;
                do {
                    //index = r.Next(0, Program.PopulationSize) + r.Next(0, Program.PopulationSize) - Program.PopulationSize - 1; //-1 to prevent out of bounds
                    index = (int)(Program.PopulationSize * (fitnessBias - Math.Sqrt(fitnessBias * fitnessBias - 4.0 * (fitnessBias - 1) * r.NextDouble())) / 2.0 / (fitnessBias - 1));
                } while (index < 0);
                arr[i] = index;
            }

            Dictionary<string, int> nums = new Dictionary<string, int>();
            foreach(int x in arr) {
                int y = (int)Math.Floor(x / 10.0);
                if (nums.ContainsKey(y.ToString())) {
                    nums[y.ToString()]++;
                } else {
                    nums.Add(y.ToString(), 1);
                }
            }
            List<int> otherTest = new List<int>();
            for(int i = 0; i < 50; i++) {
                if (nums.ContainsKey(i.ToString())) {
                    Debug.WriteLine("" + nums[i.ToString()]);
                } else {
                    otherTest.Add(i);
                }
            }

            Debug.WriteLine("Numbers done");
        }

    }
}

