namespace FYPTimetablingSoftware {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.AllMembersBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.generationLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fitnessLbl = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.resumeButton = new System.Windows.Forms.Button();
            this.fitnessChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.graphDataTextBox = new System.Windows.Forms.TextBox();
            this.XMLTestButton = new System.Windows.Forms.Button();
            this.randomTestButton = new System.Windows.Forms.Button();
            this.StartTimeLbl = new System.Windows.Forms.Label();
            this.StartTimeValueLbl = new System.Windows.Forms.Label();
            this.TimeElapsedValueLbl = new System.Windows.Forms.Label();
            this.TimeElapsedLbl = new System.Windows.Forms.Label();
            this.AverageValueLbl = new System.Windows.Forms.Label();
            this.AverageLbl = new System.Windows.Forms.Label();
            this.TestButton2 = new System.Windows.Forms.Button();
            this.NrOfConstraintsLbl = new System.Windows.Forms.Label();
            this.NrOfConstraintsValuelbl = new System.Windows.Forms.Label();
            this.MaxWeightValueLbl = new System.Windows.Forms.Label();
            this.MaxWeightLbl = new System.Windows.Forms.Label();
            this.MinWeightValueLbl = new System.Windows.Forms.Label();
            this.MinWeightLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fitnessChart)).BeginInit();
            this.SuspendLayout();
            // 
            // AllMembersBox
            // 
            this.AllMembersBox.AcceptsReturn = true;
            this.AllMembersBox.AcceptsTab = true;
            this.AllMembersBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.AllMembersBox.Location = new System.Drawing.Point(610, 12);
            this.AllMembersBox.Multiline = true;
            this.AllMembersBox.Name = "AllMembersBox";
            this.AllMembersBox.ReadOnly = true;
            this.AllMembersBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AllMembersBox.Size = new System.Drawing.Size(317, 280);
            this.AllMembersBox.TabIndex = 0;
            this.AllMembersBox.Text = "---";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Generation";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // generationLbl
            // 
            this.generationLbl.AutoSize = true;
            this.generationLbl.Location = new System.Drawing.Point(118, 26);
            this.generationLbl.Name = "generationLbl";
            this.generationLbl.Size = new System.Drawing.Size(71, 13);
            this.generationLbl.TabIndex = 4;
            this.generationLbl.Text = "generationLbl";
            this.generationLbl.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fitness";
            this.label4.Click += new System.EventHandler(this.label2_Click);
            // 
            // fitnessLbl
            // 
            this.fitnessLbl.AutoSize = true;
            this.fitnessLbl.Location = new System.Drawing.Point(118, 54);
            this.fitnessLbl.Name = "fitnessLbl";
            this.fitnessLbl.Size = new System.Drawing.Size(51, 13);
            this.fitnessLbl.TabIndex = 4;
            this.fitnessLbl.Text = "fitnessLbl";
            this.fitnessLbl.Click += new System.EventHandler(this.label3_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(15, 230);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(121, 230);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 6;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // resumeButton
            // 
            this.resumeButton.Location = new System.Drawing.Point(227, 230);
            this.resumeButton.Name = "resumeButton";
            this.resumeButton.Size = new System.Drawing.Size(75, 23);
            this.resumeButton.TabIndex = 7;
            this.resumeButton.Text = "Resume";
            this.resumeButton.UseVisualStyleBackColor = true;
            this.resumeButton.Click += new System.EventHandler(this.resumeButton_Click);
            // 
            // fitnessChart
            // 
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.Name = "ChartArea1";
            this.fitnessChart.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.fitnessChart.Legends.Add(legend1);
            this.fitnessChart.Location = new System.Drawing.Point(15, 302);
            this.fitnessChart.Name = "fitnessChart";
            this.fitnessChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            this.fitnessChart.Series.Add(series1);
            this.fitnessChart.Size = new System.Drawing.Size(579, 424);
            this.fitnessChart.TabIndex = 8;
            this.fitnessChart.Text = "chart1";
            // 
            // graphDataTextBox
            // 
            this.graphDataTextBox.AcceptsReturn = true;
            this.graphDataTextBox.AcceptsTab = true;
            this.graphDataTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.graphDataTextBox.Location = new System.Drawing.Point(610, 302);
            this.graphDataTextBox.Multiline = true;
            this.graphDataTextBox.Name = "graphDataTextBox";
            this.graphDataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.graphDataTextBox.Size = new System.Drawing.Size(317, 424);
            this.graphDataTextBox.TabIndex = 0;
            this.graphDataTextBox.WordWrap = false;
            // 
            // XMLTestButton
            // 
            this.XMLTestButton.Location = new System.Drawing.Point(348, 229);
            this.XMLTestButton.Name = "XMLTestButton";
            this.XMLTestButton.Size = new System.Drawing.Size(75, 23);
            this.XMLTestButton.TabIndex = 10;
            this.XMLTestButton.Text = "Load XML";
            this.XMLTestButton.UseVisualStyleBackColor = true;
            this.XMLTestButton.Click += new System.EventHandler(this.XMLTestButton_Click);
            // 
            // randomTestButton
            // 
            this.randomTestButton.Location = new System.Drawing.Point(467, 229);
            this.randomTestButton.Name = "randomTestButton";
            this.randomTestButton.Size = new System.Drawing.Size(44, 24);
            this.randomTestButton.TabIndex = 11;
            this.randomTestButton.Text = "Break";
            this.randomTestButton.UseVisualStyleBackColor = true;
            this.randomTestButton.Click += new System.EventHandler(this.randomTestButton_Click);
            // 
            // StartTimeLbl
            // 
            this.StartTimeLbl.AutoSize = true;
            this.StartTimeLbl.Location = new System.Drawing.Point(362, 26);
            this.StartTimeLbl.Name = "StartTimeLbl";
            this.StartTimeLbl.Size = new System.Drawing.Size(55, 13);
            this.StartTimeLbl.TabIndex = 12;
            this.StartTimeLbl.Text = "Start Time";
            // 
            // StartTimeValueLbl
            // 
            this.StartTimeValueLbl.AutoSize = true;
            this.StartTimeValueLbl.Location = new System.Drawing.Point(464, 26);
            this.StartTimeValueLbl.Name = "StartTimeValueLbl";
            this.StartTimeValueLbl.Size = new System.Drawing.Size(79, 13);
            this.StartTimeValueLbl.TabIndex = 13;
            this.StartTimeValueLbl.Text = "StartTimeValue";
            // 
            // TimeElapsedValueLbl
            // 
            this.TimeElapsedValueLbl.AutoSize = true;
            this.TimeElapsedValueLbl.Location = new System.Drawing.Point(464, 54);
            this.TimeElapsedValueLbl.Name = "TimeElapsedValueLbl";
            this.TimeElapsedValueLbl.Size = new System.Drawing.Size(95, 13);
            this.TimeElapsedValueLbl.TabIndex = 15;
            this.TimeElapsedValueLbl.Text = "TimeElapsedValue";
            // 
            // TimeElapsedLbl
            // 
            this.TimeElapsedLbl.AutoSize = true;
            this.TimeElapsedLbl.Location = new System.Drawing.Point(362, 54);
            this.TimeElapsedLbl.Name = "TimeElapsedLbl";
            this.TimeElapsedLbl.Size = new System.Drawing.Size(71, 13);
            this.TimeElapsedLbl.TabIndex = 14;
            this.TimeElapsedLbl.Text = "Time Elapsed";
            // 
            // AverageValueLbl
            // 
            this.AverageValueLbl.AutoSize = true;
            this.AverageValueLbl.Location = new System.Drawing.Point(464, 85);
            this.AverageValueLbl.Name = "AverageValueLbl";
            this.AverageValueLbl.Size = new System.Drawing.Size(96, 13);
            this.AverageValueLbl.TabIndex = 17;
            this.AverageValueLbl.Text = "[Calculated at g25]";
            // 
            // AverageLbl
            // 
            this.AverageLbl.AutoSize = true;
            this.AverageLbl.Location = new System.Drawing.Point(362, 85);
            this.AverageLbl.Name = "AverageLbl";
            this.AverageLbl.Size = new System.Drawing.Size(47, 13);
            this.AverageLbl.TabIndex = 16;
            this.AverageLbl.Text = "Average";
            // 
            // TestButton2
            // 
            this.TestButton2.Location = new System.Drawing.Point(548, 229);
            this.TestButton2.Name = "TestButton2";
            this.TestButton2.Size = new System.Drawing.Size(46, 23);
            this.TestButton2.TabIndex = 18;
            this.TestButton2.Text = "Test";
            this.TestButton2.UseVisualStyleBackColor = true;
            this.TestButton2.Click += new System.EventHandler(this.TestButton2_Click);
            // 
            // NrOfConstraintsLbl
            // 
            this.NrOfConstraintsLbl.AutoSize = true;
            this.NrOfConstraintsLbl.Location = new System.Drawing.Point(13, 84);
            this.NrOfConstraintsLbl.Name = "NrOfConstraintsLbl";
            this.NrOfConstraintsLbl.Size = new System.Drawing.Size(84, 13);
            this.NrOfConstraintsLbl.TabIndex = 19;
            this.NrOfConstraintsLbl.Text = "Nr of constraints";
            // 
            // NrOfConstraintsValuelbl
            // 
            this.NrOfConstraintsValuelbl.AutoSize = true;
            this.NrOfConstraintsValuelbl.Location = new System.Drawing.Point(121, 84);
            this.NrOfConstraintsValuelbl.Name = "NrOfConstraintsValuelbl";
            this.NrOfConstraintsValuelbl.Size = new System.Drawing.Size(110, 13);
            this.NrOfConstraintsValuelbl.TabIndex = 20;
            this.NrOfConstraintsValuelbl.Text = "Number of constraints";
            // 
            // MaxWeightValueLbl
            // 
            this.MaxWeightValueLbl.AutoSize = true;
            this.MaxWeightValueLbl.Location = new System.Drawing.Point(121, 120);
            this.MaxWeightValueLbl.Name = "MaxWeightValueLbl";
            this.MaxWeightValueLbl.Size = new System.Drawing.Size(75, 13);
            this.MaxWeightValueLbl.TabIndex = 22;
            this.MaxWeightValueLbl.Text = "Max Violations";
            // 
            // MaxWeightLbl
            // 
            this.MaxWeightLbl.AutoSize = true;
            this.MaxWeightLbl.Location = new System.Drawing.Point(13, 120);
            this.MaxWeightLbl.Name = "MaxWeightLbl";
            this.MaxWeightLbl.Size = new System.Drawing.Size(75, 13);
            this.MaxWeightLbl.TabIndex = 21;
            this.MaxWeightLbl.Text = "Max Violations";
            // 
            // MinWeightValueLbl
            // 
            this.MinWeightValueLbl.AutoSize = true;
            this.MinWeightValueLbl.Location = new System.Drawing.Point(121, 145);
            this.MinWeightValueLbl.Name = "MinWeightValueLbl";
            this.MinWeightValueLbl.Size = new System.Drawing.Size(64, 13);
            this.MinWeightValueLbl.TabIndex = 24;
            this.MinWeightValueLbl.Text = "Max Weight";
            // 
            // MinWeightLbl
            // 
            this.MinWeightLbl.AutoSize = true;
            this.MinWeightLbl.Location = new System.Drawing.Point(13, 145);
            this.MinWeightLbl.Name = "MinWeightLbl";
            this.MinWeightLbl.Size = new System.Drawing.Size(58, 13);
            this.MinWeightLbl.TabIndex = 23;
            this.MinWeightLbl.Text = "Min weight";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 738);
            this.Controls.Add(this.MinWeightValueLbl);
            this.Controls.Add(this.MinWeightLbl);
            this.Controls.Add(this.MaxWeightValueLbl);
            this.Controls.Add(this.MaxWeightLbl);
            this.Controls.Add(this.NrOfConstraintsValuelbl);
            this.Controls.Add(this.NrOfConstraintsLbl);
            this.Controls.Add(this.TestButton2);
            this.Controls.Add(this.AverageValueLbl);
            this.Controls.Add(this.AverageLbl);
            this.Controls.Add(this.TimeElapsedValueLbl);
            this.Controls.Add(this.TimeElapsedLbl);
            this.Controls.Add(this.StartTimeValueLbl);
            this.Controls.Add(this.StartTimeLbl);
            this.Controls.Add(this.randomTestButton);
            this.Controls.Add(this.XMLTestButton);
            this.Controls.Add(this.fitnessChart);
            this.Controls.Add(this.resumeButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.fitnessLbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.generationLbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.graphDataTextBox);
            this.Controls.Add(this.AllMembersBox);
            this.Name = "Form1";
            this.Text = "GA Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fitnessChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AllMembersBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label generationLbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label fitnessLbl;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button resumeButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart fitnessChart;
        private System.Windows.Forms.TextBox graphDataTextBox;
        private System.Windows.Forms.Button XMLTestButton;
        private System.Windows.Forms.Button randomTestButton;
        private System.Windows.Forms.Label StartTimeLbl;
        private System.Windows.Forms.Label StartTimeValueLbl;
        private System.Windows.Forms.Label TimeElapsedValueLbl;
        private System.Windows.Forms.Label TimeElapsedLbl;
        private System.Windows.Forms.Label AverageValueLbl;
        private System.Windows.Forms.Label AverageLbl;
        private System.Windows.Forms.Button TestButton2;
        private System.Windows.Forms.Label NrOfConstraintsLbl;
        private System.Windows.Forms.Label NrOfConstraintsValuelbl;
        private System.Windows.Forms.Label MaxWeightValueLbl;
        private System.Windows.Forms.Label MaxWeightLbl;
        private System.Windows.Forms.Label MinWeightValueLbl;
        private System.Windows.Forms.Label MinWeightLbl;
    }
}

