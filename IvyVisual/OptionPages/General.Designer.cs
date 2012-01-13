namespace IvyVisual.OptionPages
{
    partial class General
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
            this.autoResolve = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dependenciesRetrievePattern = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.workspacePath = new System.Windows.Forms.TextBox();
            this.retrieveCommand = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // autoResolve
            // 
            this.autoResolve.AutoSize = true;
            this.autoResolve.Location = new System.Drawing.Point(6, 25);
            this.autoResolve.Name = "autoResolve";
            this.autoResolve.Size = new System.Drawing.Size(15, 14);
            this.autoResolve.TabIndex = 0;
            this.autoResolve.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dependencies retrieve pattern";
            // 
            // dependenciesRetrievePattern
            // 
            this.dependenciesRetrievePattern.Location = new System.Drawing.Point(6, 97);
            this.dependenciesRetrievePattern.Name = "dependenciesRetrievePattern";
            this.dependenciesRetrievePattern.Size = new System.Drawing.Size(390, 20);
            this.dependenciesRetrievePattern.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(417, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Map binary references to project references when a solution opens and unmap at cl" +
    "ose";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(453, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Workspace path, this is the path that IvyVisual should search for ivy.xml files t" +
    "o map references";
            // 
            // workspacePath
            // 
            this.workspacePath.Location = new System.Drawing.Point(6, 136);
            this.workspacePath.Name = "workspacePath";
            this.workspacePath.Size = new System.Drawing.Size(390, 20);
            this.workspacePath.TabIndex = 6;
            // 
            // retrieveCommand
            // 
            this.retrieveCommand.Location = new System.Drawing.Point(6, 58);
            this.retrieveCommand.Name = "retrieveCommand";
            this.retrieveCommand.Size = new System.Drawing.Size(390, 20);
            this.retrieveCommand.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(440, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "The command that should be executed in the project folder. Eg. (c:\\ant\\bin\\ant.ba" +
    "t retrieve)";
            // 
            // General
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.retrieveCommand);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.workspacePath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dependenciesRetrievePattern);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.autoResolve);
            this.Name = "General";
            this.Size = new System.Drawing.Size(510, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoResolve;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox dependenciesRetrievePattern;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox workspacePath;
        private System.Windows.Forms.TextBox retrieveCommand;
        private System.Windows.Forms.Label label2;
    }
}
