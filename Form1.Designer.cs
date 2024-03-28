namespace pong
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.points1 = new System.Windows.Forms.Label();
            this.points2 = new System.Windows.Forms.Label();
            this.pause_txt = new System.Windows.Forms.Label();
            this.paddle = new System.Windows.Forms.PictureBox();
            this.paddle2 = new System.Windows.Forms.PictureBox();
            this.ball = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.paddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paddle2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ball)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.moveBall);
            // 
            // points1
            // 
            this.points1.AutoSize = true;
            this.points1.ForeColor = System.Drawing.Color.White;
            this.points1.Location = new System.Drawing.Point(3, 6);
            this.points1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.points1.Name = "points1";
            this.points1.Size = new System.Drawing.Size(59, 16);
            this.points1.TabIndex = 3;
            this.points1.Text = "Player: 0";
            // 
            // points2
            // 
            this.points2.AutoSize = true;
            this.points2.ForeColor = System.Drawing.Color.White;
            this.points2.Location = new System.Drawing.Point(3, 27);
            this.points2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.points2.Name = "points2";
            this.points2.Size = new System.Drawing.Size(48, 16);
            this.points2.TabIndex = 4;
            this.points2.Text = "CPU: 0";
            // 
            // pause_txt
            // 
            this.pause_txt.AutoSize = true;
            this.pause_txt.ForeColor = System.Drawing.Color.White;
            this.pause_txt.Location = new System.Drawing.Point(160, 142);
            this.pause_txt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pause_txt.Name = "pause_txt";
            this.pause_txt.Size = new System.Drawing.Size(54, 16);
            this.pause_txt.TabIndex = 5;
            this.pause_txt.Text = "Paused";
            // 
            // paddle
            // 
            this.paddle.Image = ((System.Drawing.Image)(resources.GetObject("paddle.Image")));
            this.paddle.Location = new System.Drawing.Point(132, 284);
            this.paddle.Margin = new System.Windows.Forms.Padding(4);
            this.paddle.Name = "paddle";
            this.paddle.Size = new System.Drawing.Size(100, 28);
            this.paddle.TabIndex = 6;
            this.paddle.TabStop = false;
            // 
            // paddle2
            // 
            this.paddle2.Image = ((System.Drawing.Image)(resources.GetObject("paddle2.Image")));
            this.paddle2.Location = new System.Drawing.Point(132, 15);
            this.paddle2.Margin = new System.Windows.Forms.Padding(4);
            this.paddle2.Name = "paddle2";
            this.paddle2.Size = new System.Drawing.Size(100, 28);
            this.paddle2.TabIndex = 7;
            this.paddle2.TabStop = false;
            // 
            // ball
            // 
            this.ball.Image = ((System.Drawing.Image)(resources.GetObject("ball.Image")));
            this.ball.Location = new System.Drawing.Point(165, 129);
            this.ball.Margin = new System.Windows.Forms.Padding(4);
            this.ball.Name = "ball";
            this.ball.Size = new System.Drawing.Size(31, 28);
            this.ball.TabIndex = 8;
            this.ball.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "dễ",
            "vừa ",
            "khó"});
            this.comboBox1.Location = new System.Drawing.Point(248, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(139, 24);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(389, 327);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.paddle2);
            this.Controls.Add(this.paddle);
            this.Controls.Add(this.pause_txt);
            this.Controls.Add(this.points2);
            this.Controls.Add(this.points1);
            this.Controls.Add(this.ball);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Ping Pong";
            this.ResizeEnd += new System.EventHandler(this.movePaddles);
            this.Click += new System.EventHandler(this.pause);
            ((System.ComponentModel.ISupportInitialize)(this.paddle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paddle2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ball)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label points1;
        private System.Windows.Forms.Label points2;
        private System.Windows.Forms.Label pause_txt;
        private System.Windows.Forms.PictureBox paddle;
        private System.Windows.Forms.PictureBox paddle2;
        private System.Windows.Forms.PictureBox ball;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

