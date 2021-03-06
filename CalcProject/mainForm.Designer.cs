﻿namespace CalcProject {
    partial class mainForm {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.stripMenu = new System.Windows.Forms.MenuStrip();
            this.nuovoSistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuovoSistemaDaInterfacciaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuovoSistemaDaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aiutoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.findFile = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.stripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // stripMenu
            // 
            this.stripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.stripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuovoSistemaToolStripMenuItem,
            this.aiutoToolStripMenuItem});
            this.stripMenu.Location = new System.Drawing.Point(0, 0);
            this.stripMenu.Name = "stripMenu";
            this.stripMenu.Size = new System.Drawing.Size(712, 24);
            this.stripMenu.TabIndex = 4;
            this.stripMenu.Text = "menuStrip1";
            this.stripMenu.Visible = false;
            // 
            // nuovoSistemaToolStripMenuItem
            // 
            this.nuovoSistemaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuovoSistemaDaInterfacciaToolStripMenuItem,
            this.nuovoSistemaDaToolStripMenuItem});
            this.nuovoSistemaToolStripMenuItem.Name = "nuovoSistemaToolStripMenuItem";
            this.nuovoSistemaToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.nuovoSistemaToolStripMenuItem.Text = "Nuovo...";
            // 
            // nuovoSistemaDaInterfacciaToolStripMenuItem
            // 
            this.nuovoSistemaDaInterfacciaToolStripMenuItem.Name = "nuovoSistemaDaInterfacciaToolStripMenuItem";
            this.nuovoSistemaDaInterfacciaToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.nuovoSistemaDaInterfacciaToolStripMenuItem.Text = "Nuovo Progetto da Interfaccia";
            this.nuovoSistemaDaInterfacciaToolStripMenuItem.Click += new System.EventHandler(this.nuovoSistemaDaGUIToolStripMenuItem_Click);
            // 
            // nuovoSistemaDaToolStripMenuItem
            // 
            this.nuovoSistemaDaToolStripMenuItem.Name = "nuovoSistemaDaToolStripMenuItem";
            this.nuovoSistemaDaToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.nuovoSistemaDaToolStripMenuItem.Text = "Nuovo Progetto da File";
            this.nuovoSistemaDaToolStripMenuItem.Click += new System.EventHandler(this.nuovoSistemaDaFileToolStripMenuItem_Click);
            // 
            // aiutoToolStripMenuItem
            // 
            this.aiutoToolStripMenuItem.Name = "aiutoToolStripMenuItem";
            this.aiutoToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aiutoToolStripMenuItem.Text = "Aiuto";
            this.aiutoToolStripMenuItem.Click += new System.EventHandler(this.aiutoToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(212, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(307, 36);
            this.button2.TabIndex = 9;
            this.button2.Text = "Nuovo Progetto";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(184, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(373, 156);
            this.label1.TabIndex = 10;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(212, 212);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(307, 36);
            this.button3.TabIndex = 11;
            this.button3.Text = "Carica Progetto";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // findFile
            // 
            this.findFile.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(212, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(307, 36);
            this.button1.TabIndex = 12;
            this.button1.Text = "Aiuto";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.aiutoToolStripMenuItem_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(712, 404);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.stripMenu);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.stripMenu;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iSimplex";
            this.stripMenu.ResumeLayout(false);
            this.stripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip stripMenu;
        private System.Windows.Forms.ToolStripMenuItem nuovoSistemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aiutoToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog findFile;
        private System.Windows.Forms.ToolStripMenuItem nuovoSistemaDaInterfacciaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuovoSistemaDaToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}

