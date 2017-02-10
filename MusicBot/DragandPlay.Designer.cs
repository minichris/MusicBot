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
            this.Scanner = new System.ComponentModel.BackgroundWorker();
            this.SongPlayer = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.DragBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DragBox
            // 
            this.DragBox.Location = new System.Drawing.Point(12, 38);
            this.DragBox.Name = "DragBox";
            this.DragBox.Size = new System.Drawing.Size(260, 214);
            this.DragBox.TabIndex = 1;
            this.DragBox.TabStop = false;
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(73, 12);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(145, 20);
            this.NameBox.TabIndex = 2;
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
            // Scanner
            // 
            this.Scanner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Scanner_DoWork);
            // 
            // SongPlayer
            // 
            this.SongPlayer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SongPlayer_DoWork);
            // 
            // DragandPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.DragBox);
            this.Name = "DragandPlay";
            this.Text = "DragandPlay";
            ((System.ComponentModel.ISupportInitialize)(this.DragBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox DragBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Button FindButton;
        private System.ComponentModel.BackgroundWorker SongPlayer;
        public System.ComponentModel.BackgroundWorker Scanner;
    }
}