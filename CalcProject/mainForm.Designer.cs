namespace CalcProject {
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
            this.rExpressions = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nuovoSistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aiutoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rExpressions
            // 
            this.rExpressions.BackColor = System.Drawing.Color.White;
            this.rExpressions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rExpressions.Location = new System.Drawing.Point(64, 58);
            this.rExpressions.Name = "rExpressions";
            this.rExpressions.Size = new System.Drawing.Size(422, 180);
            this.rExpressions.TabIndex = 0;
            this.rExpressions.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Inserisci una espressione per riga";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 120F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-31, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 208);
            this.label2.TabIndex = 2;
            this.label2.Text = "{";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(104, 273);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(352, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Analizza";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuovoSistemaToolStripMenuItem,
            this.aiutoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(548, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nuovoSistemaToolStripMenuItem
            // 
            this.nuovoSistemaToolStripMenuItem.Name = "nuovoSistemaToolStripMenuItem";
            this.nuovoSistemaToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.nuovoSistemaToolStripMenuItem.Text = "Nuovo Sistema";
            // 
            // aiutoToolStripMenuItem
            // 
            this.aiutoToolStripMenuItem.Name = "aiutoToolStripMenuItem";
            this.aiutoToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aiutoToolStripMenuItem.Text = "Aiuto";
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(64, 32);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(422, 20);
            this.txtZ.TabIndex = 5;
            this.txtZ.Text = "Inserire la funzione Z";
            this.txtZ.Enter += new System.EventHandler(this.txtZ_Enter);
            this.txtZ.Leave += new System.EventHandler(this.txtZ_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(439, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 39);
            this.label3.TabIndex = 6;
            this.label3.Text = "Z: 3x1+3x2+4x3\r\n3x1-3x2-4x3<=360\r\n2x1+3x2-4x3<=100";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 469);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtZ);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rExpressions);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainForm";
            this.Text = "Simple Simplex";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rExpressions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nuovoSistemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aiutoToolStripMenuItem;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label label3;
    }
}

