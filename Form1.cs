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

namespace FYPTimetablingSoftware {
    public partial class Form1 : Form {
        private delegate void SafeTextEdit(char[] bestGenes, float bestFitness, int generation, int populationSize);
        Dispatcher GUIDispatcher;
        private System.Timers.Timer aTimer;

        private readonly SynchronizationContext SyncContext;
        private DateTime previousTime = DateTime.Now;
        //Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eget ligula dapibus, volutpat sapien a, sodales eros. Vestibulum et fringilla nibh. Mauris viverra lacus vel nunc fringilla, nec viverra orci pellentesque.
        string targetString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eget ligula dapibus, volutpat sapien a, sodales eros. Vestibulum et fringilla nibh. Mauris viverra lacus vel nunc fringilla, nec viverra orci pellentesque.";
        string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        int nOfMembersToDisplay = 20;
        int populationSize = 500;
        float mutationRate = 0.01f;
        int elitism = 5;
        bool enabled = false;
        bool aRunning = false;
        float oldFitness = 0;
        Series fitnessSeries;

        private GeneticAlgorithm<char> ga;
        private System.Random random;
        public Form1() {
            SyncContext = SynchronizationContext.Current;
            GUIDispatcher = Dispatcher.CurrentDispatcher;
            InitializeComponent();
            targetTextBox.Text = targetString;
            initAlgorithm();
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
            ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, random, GetRandomCharacter, FitnessFunction, UpdateAlgorithm, elitism, mutationRate);
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {
            if (!aRunning) {
                UpdateAlgorithm();
            }
        }

        public void UpdateAlgorithm() {
            if (!enabled) { return; } //if something paused or stopped the algorithm, don't update anymore
            aRunning = true;
            ga.NewGeneration();
            SafeTextEdit del = new SafeTextEdit(UpdateText);
            GUIDispatcher.Invoke(del, new object[] { ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count});

            
            if (ga.Generation == 100 || ga.BestFitness == 1) { 
                enabled = false;
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
            DNA<char> dna = ga.Population[index];

            for (int i = 0; i < dna.Genes.Length; i++) {
                if (dna.Genes[i] == targetString[i]) {
                    score += 1;
                }
            }

            score /= targetString.Length;

            score = (float)((Math.Pow(2, score) - 1) / (2 - 1));

            return score;
        }

        private char[] getGenes(int j) {
            return ga.Population[j].Genes;
        }

        private void UpdateText(char[] bestGenes, float bestFitness, int generation, int populationSize) {
            float improvement = bestFitness - oldFitness;
            if (improvement > 0.0001) {
                fitnessSeries.Points.AddXY(generation, bestFitness);
                graphDataTextBox.AppendText(generation+"\t ; \t"+bestFitness + "\r\n");
                
            }

            oldFitness = bestFitness;

            bestGeneBox.Text = CharArrayToString(bestGenes);
            fitnessLbl.Text = bestFitness.ToString();
            generationLbl.Text = generation.ToString();
            Console.WriteLine("Generation" + generation.ToString() + "\t Fitness" + bestFitness.ToString() + "\n genes:" + CharArrayToString(bestGenes));
            int n = nOfMembersToDisplay;
            if (nOfMembersToDisplay > populationSize) { n = populationSize;  }
            var sb = new StringBuilder();
            for (int i = 0; i < n; i++) { //loop through the top n of population
                foreach (var c in getGenes(i)) {
                    sb.Append(c);
                }
                sb.AppendLine();
            }
            AllMembersBox.Text = sb.ToString();
        }

        private string CharArrayToString(char[] charArray) {
            var sb = new StringBuilder();
            foreach (var c in charArray) {
                sb.Append(c);
            }

            return sb.ToString();
        }

        

        private async void startButton_Click(object sender, EventArgs e) {
            await Task.Run(() => {
                ResumeAlgorithm();
            });
            
        }

        private async void pauseButton_Click(object sender, EventArgs e) {
            await Task.Run(() => {
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
            XMLParser p = new XMLParser(projectDirectory+ "/DataSet/pu-fal07-llr_FYP.xml");
        }
    }
}
