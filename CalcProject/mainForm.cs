using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalcProject {
    public partial class mainForm : Form {
        public mainForm() {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) {

            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100", 
            "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100"};
            cTable t = new cTable(txtZ.Text, s);
            rExpressions.Visible = label1.Visible = label2.Visible =  button1.Visible = txtZ.Visible = false;


            #region Creazione oggetto
            DataGridView dg = new DataGridView();
            dg.Name = "dg";
            //CB, Base, Var decisionali + Var scarto, B
            dg.ColumnCount = 3 + t.nVariabiliScarto;
            dg.RowCount = t.Functions.Length + 2;
            //i = 2; le prime due non le conta, Columns.Count - 1; l'ultima non la conta
            for (int i = 2; i < dg.Columns.Count - 1; i++) {
                for (int j = 0; j < t.Functions.Length; j++) {
                    dg.Rows[j].Cells[i].Value = "0";
                }
            }
            for (int i = 0; i < dg.Columns.Count; i++) { dg.Columns[i].Width = 50; }
            dg.Location = new Point(rExpressions.Location.X - 5, rExpressions.Location.Y);
            dg.Size = new Size(rExpressions.Size.Width, rExpressions.Size.Height);
            dg.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            dg.Columns[0].HeaderText = "Cb";
            dg.Columns[1].HeaderText = /* All your */"Base" /* are belong to us */;
            this.Controls.Add(dg);
            for (int i = 0; i < t.nVariabiliScarto; i++) {
                dg.Columns[i + 2].HeaderText = "x" + (i + 1);
            }
            dg.Columns[t.nVariabiliScarto + 2].HeaderText = "b"; 
            #endregion

            #region Scrittura headers
            //Riempimento prime due colonne
            for (int i = 0; i < t.Functions.Length; i++) {
                dg[0, i].Value = 0;
                dg[1, i].Value = "x" + (t.nVariabili + (1 + i));
            }
            dg[1, t.Functions.Length].Value = "Cj"; //GTA San Andreas cit.
            dg[1, t.Functions.Length + 1].Value = "SAB"; 
            #endregion

            #region Popolamento tabella
            for (int r = 0; r < t.Functions.Length /*Da aggiungere il +1 per Cj */; r++) {
                //t.nVars per far si che riempisse tutte le caselle delle var decisionali
                //+1 per riempire quella dello scarto
                for (int c = 0; c < t.nVariabili + 1; c++) {
                    if (c < t.nVariabili) dg[c + 2, r].Value = t.coefficientTerms[r][c];
                    else { //Ho scritto tutti i coefficienti
                        dg[c + 2 + r, r].Value = 1;
                        //c + 2 per il fatto delle prime due da non contare
                        //+ r per contare a che funzione sono arrivato
                        //quindi per arrivare all'nesimo scarto
                    }
                }
            }

            //Scrittura dei termini di destra
            for (int r = 0; r < t.Vars.Count; r++) {
                dg[dg.Columns.Count - 1, r].Value = t.Vars[r][3];
            }
            #endregion

            

            #region #DEPRECATED
            //for (int i = 0; i < t.Numbers.Count; i++) {
            //    string tmp = "";
            //    for (int j = 0; j < t.Numbers[i].Length; j++) {
            //        tmp += t.Numbers[i][j] + " ";
            //    }
            //    rExpressions.Text += tmp;
            //}
            //rExpressions.Text += "\n";

            //for (int i = 0; i < t.Functions.Length; i++) {
            //    rExpressions.Text += t.Functions[i];
            //}
            
            #endregion

            rExpressions.Dispose();
            label1.Dispose();
            label2.Dispose();
            txtZ.Dispose();
            button1.Dispose();

            
        }

        private void txtZ_Enter(object sender, EventArgs e) {
            txtZ.Text = "";
        }

        private void txtZ_Leave(object sender, EventArgs e) {
            if (txtZ.Text.Trim() == "") { txtZ.Text = "Inserire la funzione Z"; return; }
            button1.Enabled = true;
            //Da aggiungere la gestione cazzate
        }
    }
}
