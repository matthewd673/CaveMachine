namespace CaveMachine
{
    partial class MainForm
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
            this.previewPanel = new System.Windows.Forms.Panel();
            this.generateButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.extrapolateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // previewPanel
            // 
            this.previewPanel.BackColor = System.Drawing.Color.White;
            this.previewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPanel.Location = new System.Drawing.Point(13, 72);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(200, 100);
            this.previewPanel.TabIndex = 0;
            this.previewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPanel_Paint);
            this.previewPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.previewPanel_MouseDoubleClick);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(12, 12);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(123, 23);
            this.generateButton.TabIndex = 1;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Location = new System.Drawing.Point(13, 41);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(200, 23);
            this.copyButton.TabIndex = 2;
            this.copyButton.Text = "Copy Vertex Data";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // extrapolateButton
            // 
            this.extrapolateButton.Location = new System.Drawing.Point(141, 12);
            this.extrapolateButton.Name = "extrapolateButton";
            this.extrapolateButton.Size = new System.Drawing.Size(72, 23);
            this.extrapolateButton.TabIndex = 3;
            this.extrapolateButton.Text = "Extrapolate";
            this.extrapolateButton.UseVisualStyleBackColor = true;
            this.extrapolateButton.Click += new System.EventHandler(this.extrapolateButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 185);
            this.Controls.Add(this.extrapolateButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.previewPanel);
            this.Name = "MainForm";
            this.Text = "CaveMachine";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button extrapolateButton;
    }
}

