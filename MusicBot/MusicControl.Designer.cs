namespace MusicBot
{
    partial class MusicControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameLabel = new System.Windows.Forms.Label();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(15, 9);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(61, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "NameLabel";
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(15, 31);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(55, 13);
            this.FilenameLabel.TabIndex = 1;
            this.FilenameLabel.Text = "PathLabel";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(255, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(25, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.TabStop = false;
            this.CloseButton.Text = "X";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MusicControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.NameLabel);
            this.Name = "MusicControl";
            this.Size = new System.Drawing.Size(283, 52);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Button CloseButton;
    }
}
