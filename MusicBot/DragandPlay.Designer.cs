namespace MusicBot
{
    partial class DragandPlay
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
            this.DragBox = new System.Windows.Forms.PictureBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.FindButton = new System.Windows.Forms.Button();
            this.SongPlayer = new System.ComponentModel.BackgroundWorker();
            this.GeneralButton = new System.Windows.Forms.Button();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.SampleRateBox = new System.Windows.Forms.NumericUpDown();
            this.SampleRateLabel = new System.Windows.Forms.Label();
            this.DragLabel = new System.Windows.Forms.Label();
            this.YoutubeLabel = new System.Windows.Forms.Label();
            this.YoutubeGetButton = new System.Windows.Forms.Button();
            this.YoutubeBox = new System.Windows.Forms.TextBox();
            this.GetYoutube = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.DragBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SampleRateBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DragBox
            // 
            this.DragBox.Location = new System.Drawing.Point(12, 91);
            this.DragBox.Name = "DragBox";
            this.DragBox.Size = new System.Drawing.Size(260, 113);
            this.DragBox.TabIndex = 1;
            this.DragBox.TabStop = false;
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(73, 12);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(145, 20);
            this.NameBox.TabIndex = 2;
            this.NameBox.Text = "Zaperox";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(12, 15);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(55, 13);
            this.UsernameLabel.TabIndex = 3;
            this.UsernameLabel.Text = "Username";
            // 
            // FindButton
            // 
            this.FindButton.Location = new System.Drawing.Point(224, 12);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(48, 20);
            this.FindButton.TabIndex = 4;
            this.FindButton.Text = "Find";
            this.FindButton.UseVisualStyleBackColor = true;
            // 
            // SongPlayer
            // 
            this.SongPlayer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SongPlayer_DoWork);
            // 
            // GeneralButton
            // 
            this.GeneralButton.Location = new System.Drawing.Point(12, 210);
            this.GeneralButton.Name = "GeneralButton";
            this.GeneralButton.Size = new System.Drawing.Size(260, 20);
            this.GeneralButton.TabIndex = 5;
            this.GeneralButton.Text = "Join 77p Egg General";
            this.GeneralButton.UseVisualStyleBackColor = true;
            this.GeneralButton.Click += new System.EventHandler(this.GeneralButton_Click);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(12, 236);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(260, 20);
            this.DisconnectButton.TabIndex = 6;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(224, 38);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(48, 24);
            this.StopButton.TabIndex = 7;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // SampleRateBox
            // 
            this.SampleRateBox.Location = new System.Drawing.Point(86, 38);
            this.SampleRateBox.Maximum = new decimal(new int[] {
            96000,
            0,
            0,
            0});
            this.SampleRateBox.Name = "SampleRateBox";
            this.SampleRateBox.Size = new System.Drawing.Size(132, 20);
            this.SampleRateBox.TabIndex = 8;
            this.SampleRateBox.Value = new decimal(new int[] {
            48000,
            0,
            0,
            0});
            // 
            // SampleRateLabel
            // 
            this.SampleRateLabel.AutoSize = true;
            this.SampleRateLabel.Location = new System.Drawing.Point(12, 40);
            this.SampleRateLabel.Name = "SampleRateLabel";
            this.SampleRateLabel.Size = new System.Drawing.Size(68, 13);
            this.SampleRateLabel.TabIndex = 9;
            this.SampleRateLabel.Text = "Sample Rate";
            // 
            // DragLabel
            // 
            this.DragLabel.AutoSize = true;
            this.DragLabel.Location = new System.Drawing.Point(83, 146);
            this.DragLabel.Name = "DragLabel";
            this.DragLabel.Size = new System.Drawing.Size(110, 13);
            this.DragLabel.TabIndex = 10;
            this.DragLabel.Text = "Drag your MP3s here!";
            // 
            // YoutubeLabel
            // 
            this.YoutubeLabel.AutoSize = true;
            this.YoutubeLabel.Location = new System.Drawing.Point(12, 69);
            this.YoutubeLabel.Name = "YoutubeLabel";
            this.YoutubeLabel.Size = new System.Drawing.Size(72, 13);
            this.YoutubeLabel.TabIndex = 11;
            this.YoutubeLabel.Text = "Youtube URL";
            // 
            // YoutubeGetButton
            // 
            this.YoutubeGetButton.Enabled = false;
            this.YoutubeGetButton.Location = new System.Drawing.Point(224, 64);
            this.YoutubeGetButton.Name = "YoutubeGetButton";
            this.YoutubeGetButton.Size = new System.Drawing.Size(48, 23);
            this.YoutubeGetButton.TabIndex = 12;
            this.YoutubeGetButton.Text = "Get";
            this.YoutubeGetButton.UseVisualStyleBackColor = true;
            this.YoutubeGetButton.Click += new System.EventHandler(this.YoutubeGetButton_Click);
            // 
            // YoutubeBox
            // 
            this.YoutubeBox.Enabled = false;
            this.YoutubeBox.Location = new System.Drawing.Point(86, 65);
            this.YoutubeBox.Name = "YoutubeBox";
            this.YoutubeBox.Size = new System.Drawing.Size(132, 20);
            this.YoutubeBox.TabIndex = 13;
            // 
            // DragandPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.YoutubeBox);
            this.Controls.Add(this.YoutubeGetButton);
            this.Controls.Add(this.YoutubeLabel);
            this.Controls.Add(this.DragLabel);
            this.Controls.Add(this.SampleRateLabel);
            this.Controls.Add(this.SampleRateBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.GeneralButton);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.DragBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DragandPlay";
            this.Text = "DragandPlay";
            this.Deactivate += new System.EventHandler(this.DragandPlay_Deactivate);
            ((System.ComponentModel.ISupportInitialize)(this.DragBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SampleRateBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox DragBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Button FindButton;
        private System.ComponentModel.BackgroundWorker SongPlayer;
        private System.Windows.Forms.Button GeneralButton;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.NumericUpDown SampleRateBox;
        private System.Windows.Forms.Label SampleRateLabel;
        private System.Windows.Forms.Label DragLabel;
        private System.Windows.Forms.Label YoutubeLabel;
        private System.Windows.Forms.Button YoutubeGetButton;
        private System.Windows.Forms.TextBox YoutubeBox;
        private System.ComponentModel.BackgroundWorker GetYoutube;
    }
}