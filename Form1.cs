using System;
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

namespace FYPTimetablingSoftware {
    public partial class Form1 : Form {
        private delegate void SafeTextEdit(SolutionGene[] bestGenes, float bestFitness, int generation, int populationSize);
        Dispatcher GUIDispatcher;
        private System.Timers.Timer aTimer;

        private readonly SynchronizationContext SyncContext;
        private DateTime previousTime = DateTime.Now;
        //Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eget ligula dapibus, volutpat sapien a, sodales eros. Vestibulum et fringilla nibh. Mauris viverra lacus vel nunc fringilla, nec viverra orci pellentesque.
        readonly string targetString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eget ligula dapibus, volutpat sapien a, sodales eros. Vestibulum et fringilla nibh. Mauris viverra lacus vel nunc fringilla, nec viverra orci pellentesque.";
        readonly string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        readonly int nOfMembersToDisplay = 20;
        readonly int populationSize = 500;
        readonly float mutationRate = 0.01f;
        readonly int elitism = 5;
        bool enabled = false;
        bool aRunning = false;
        float oldFitness = 0;
        Series fitnessSeries;
        private Constraint[] SoftConstraints;

        private GeneticAlgorithm<SolutionGene> ga;
        private System.Random random;
        public Form1() {
            SyncContext = SynchronizationContext.Current;
            GUIDispatcher = Dispatcher.CurrentDispatcher;
            InitializeComponent();
            targetTextBox.Text = targetString;
            //initAlgorithm();
        }
        private void initAlgorithm() {
            if (string.IsNullOrEmpty(targetString)) {
                Console.Error.WriteLine("Target string is null or empty");
                enabled = false;
            }
            fitnessSeries = fitnessChart.Series.Add("fitness");
            fitnessSeries.ChartType = SeriesChartType.Line;
            aTimer = new System.Timers.Timer(1);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = enabled;

            random = new System.Random();
            ga = new GeneticAlgorithm<SolutionGene>(populationSize, XMLParser.GetKlasList().Length, random, GetRandomSolutionGene, FitnessFunction, UpdateAlgorithm, elitism, mutationRate);
        }

        private SolutionGene GetRandomSolutionGene(Klas k) {
            int a = random.Next(0, k.Rooms.Length);
            int b = random.Next(0, k.Times.Length);
            SolutionGene output = new SolutionGene(k.ID, k.Rooms[a], k.Times[b]);
            return output;
        }

        private Klas GetRandomKlasGene(Klas k) {
            int a = random.Next(0, k.Rooms.Length);
            int b = random.Next(0, k.Times.Length);
            k.SolutionRoom = k.Rooms[a];
            k.SolutionTime = k.Times[b];
            Console.WriteLine("kID: " + k.ID + " \tb: " + b +" \t<"+ k.SolutionTime+">");
            return k;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {
            if (!aRunning) { //check if the previous call is still running 
                UpdateAlgorithm();
            }
        }

        public void UpdateAlgorithm() {
            if (!enabled) { return; } //if something paused or stopped the algorithm, don't update anymore
            aRunning = true;
            ga.NewGeneration();
            try {
                SafeTextEdit del = new SafeTextEdit(UpdateText);
                GUIDispatcher.Invoke(del, new object[] { ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count });
            } catch (Exception e){
                Console.WriteLine("{0} Exception caught.", e);
            }
            
            if (ga.Generation == 100 || ga.BestFitness == 1) { 
                enabled = false;
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.WriteLine("Reached generation 100");
                Console.WriteLine("------------------------------------------------------------------------------");
            }
            aRunning = false;
        }

        public void ResumeAlgorithm() {
            enabled = true;
            aTimer.Enabled = enabled;
        }

        private char GetRandomCharacter() {
            int i = random.Next(validCharacters.Length);
            return validCharacters[i];
        }

        private float FitnessFunction(int index) {
            float score = 0;
            DNA<SolutionGene> dna = ga.Population[index];
            for(int i = 0; i < SoftConstraints.Length; i++) {
                score += SoftConstraints[i].GetFitness(dna.Genes);
            }

            //score = (float)((Math.Pow(2, score) - 1) / (2 - 1));

            return score;
        }

        private SolutionGene[] getGenes(int j) {
            return ga.Population[j].Genes;
        }

        private void UpdateText(SolutionGene[] bestGenes, float bestFitness, int generation, int populationSize) {
            float improvement = (bestFitness - oldFitness)*-1;
            if (improvement > 0.0001) {
                fitnessSeries.Points.AddXY(generation, bestFitness*-1);
                graphDataTextBox.AppendText(generation+"\t ; \t"+bestFitness + "\r\n");
                oldFitness = bestFitness;
            }

            

            //bestGeneBox.Text = CharArrayToString(bestGenes);
            fitnessLbl.Text = bestFitness.ToString();
            generationLbl.Text = generation.ToString();
            Console.WriteLine("Generation" + generation.ToString() + "\t Fitness" + bestFitness.ToString() /*+"\n genes:" + CharArrayToString(bestGenes)*/);
            /*int n = nOfMembersToDisplay;
            if (nOfMembersToDisplay > populationSize) { n = populationSize;  }
            var sb = new StringBuilder();
            for (int i = 0; i < n; i++) { //loop through the top n of population
                foreach (var c in getGenes(i)) {
                    sb.Append(c);
                }
                sb.AppendLine();
            }
            AllMembersBox.Text = sb.ToString();*/
        }

        private string CharArrayToString(char[] charArray) {
            var sb = new StringBuilder();
            foreach (var c in charArray) {
                sb.Append(c);
            }

            return sb.ToString();
        }

        

        private async void startButton_Click(object sender, EventArgs e) {
            initAlgorithm();
            await Task.Run(() => {
                ResumeAlgorithm();
            });
            
        }

        private async void pauseButton_Click(object sender, EventArgs e) {
            await Task.Run(() => {
                Console.WriteLine("Paused algorithm \r\nGeneration: " + ga.Generation + " ; Fitness: " + ga.BestFitness);
                enabled = false;
                aTimer.Enabled = enabled;
            });
            
        }

        private async void resumeButton_Click(object sender, EventArgs e) {
            await Task.Run(() => {
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
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            XMLParser p = new XMLParser(projectDirectory+ "/DataSet/SmallTest02.xml");
            // /DataSet/pu-fal07-llr_FYP_fix.xml
            // SmallTest01.xml
            SoftConstraints = XMLParser.GetSoftConstraints();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Dataset loaded succesfully");
        }

        private void randomTestButton_Click(object sender, EventArgs e) {

        }
    }
}
