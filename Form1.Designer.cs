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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.generationLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fitnessLbl = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.resumeButton = new System.Windows.Forms.Button();
            this.fitnessChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.targetTextBox = new System.Windows.Forms.TextBox();
            this.bestGeneBox = new System.Windows.Forms.TextBox();
            this.graphDataTextBox = new System.Windows.Forms.TextBox();
            this.XMLTestButton = new System.Windows.Forms.Button();
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
            this.AllMembersBox.Text = "aaaa\r\na\r\na\r\nb\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Best Gene";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Generation";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // generationLbl
            // 
            this.generationLbl.AutoSize = true;
            this.generationLbl.Location = new System.Drawing.Point(118, 174);
            this.generationLbl.Name = "generationLbl";
            this.generationLbl.Size = new System.Drawing.Size(71, 13);
            this.generationLbl.TabIndex = 4;
            this.generationLbl.Text = "generationLbl";
            this.generationLbl.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fitness";
            this.label4.Click += new System.EventHandler(this.label2_Click);
            // 
            // fitnessLbl
            // 
            this.fitnessLbl.AutoSize = true;
            this.fitnessLbl.Location = new System.Drawing.Point(118, 144);
            this.fitnessLbl.Name = "fitnessLbl";
            this.fitnessLbl.Size = new System.Drawing.Size(51, 13);
            this.fitnessLbl.TabIndex = 4;
            this.fitnessLbl.Text = "fitnessLbl";
            this.fitnessLbl.Click += new System.EventHandler(this.label3_Click);
            // 
            // startButton
            // 
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
            chartArea1.AxisY.Maximum = 1D;
            chartArea1.AxisY.Minimum = 0D;
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
            // targetTextBox
            // 
            this.targetTextBox.AcceptsReturn = true;
            this.targetTextBox.AcceptsTab = true;
            this.targetTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.targetTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.targetTextBox.Location = new System.Drawing.Point(121, 15);
            this.targetTextBox.Multiline = true;
            this.targetTextBox.Name = "targetTextBox";
            this.targetTextBox.ReadOnly = true;
            this.targetTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.targetTextBox.Size = new System.Drawing.Size(473, 45);
            this.targetTextBox.TabIndex = 9;
            this.targetTextBox.Text = "aaaa\r\na\r\na\r\nb\r\n";
            // 
            // bestGeneBox
            // 
            this.bestGeneBox.AcceptsReturn = true;
            this.bestGeneBox.AcceptsTab = true;
            this.bestGeneBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bestGeneBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.bestGeneBox.Location = new System.Drawing.Point(121, 82);
            this.bestGeneBox.Multiline = true;
            this.bestGeneBox.Name = "bestGeneBox";
            this.bestGeneBox.ReadOnly = true;
            this.bestGeneBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bestGeneBox.Size = new System.Drawing.Size(473, 45);
            this.bestGeneBox.TabIndex = 9;
            this.bestGeneBox.Text = "aaaa\r\na\r\na\r\nb\r\n";
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
            this.XMLTestButton.Text = "XML Test";
            this.XMLTestButton.UseVisualStyleBackColor = true;
            this.XMLTestButton.Click += new System.EventHandler(this.XMLTestButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 738);
            this.Controls.Add(this.XMLTestButton);
            this.Controls.Add(this.bestGeneBox);
            this.Controls.Add(this.targetTextBox);
            this.Controls.Add(this.fitnessChart);
            this.Controls.Add(this.resumeButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.fitnessLbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.generationLbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label generationLbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label fitnessLbl;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button resumeButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart fitnessChart;
        private System.Windows.Forms.TextBox targetTextBox;
        private System.Windows.Forms.TextBox bestGeneBox;
        private System.Windows.Forms.TextBox graphDataTextBox;
        private System.Windows.Forms.Button XMLTestButton;
    }
}

