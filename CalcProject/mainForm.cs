using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CalcProject {
    public partial class mainForm : Form {
        public mainForm() {
            InitializeComponent();
            button1.Enabled = false;

            txtZ.Text = "3x1+3x2+4x3";
            button1_Click(null, null);
        }


        private int isBaseVar(string s, DataGridView dg) {
            for (int r = 0; r < dg.RowCount - 2; r++) {
                if (dg[1, r].Value == s) return r;
            }
            return -1;
        }
       
        private int getColumnByHeader(string s, DataGridView dg) {
            for (int i = 0; i < dg.Columns.Count; i++) {
                if (dg.Columns[i].HeaderText == s) return i;
            }
            return -1;
        }

        private string getHeaderByColumn(int s, DataGridView dg) {
            return dg.Columns[s].HeaderText;
        }

        //Questo metodo serve per gestire il fatto che non si metta un numero davanti alla X
        private string validNumbers(string t) {
            string[] tmp = Regex.Split(t, "([-|\\+]{0,1}\\d{0,}x\\d{1,})");
            tmp = tmp.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
            for (int i = 0; i < tmp.Length; i++) {
                int n = tmp[i][0] == '+' || tmp[i][0] == '-' ? 1 : 0;
                if (tmp[i][n] > '9' || tmp[i][n] < '0') tmp[i] = tmp[i].Insert(n, "1");
            }
            string s = "";
            for (int i = 0; i < tmp.Length; i++) {
                s += tmp[i];
            }
            return s;
        }


        private void button1_Click(object sender, EventArgs e) {

            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100", 
            "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100"};
            string tmp = validNumbers(txtZ.Text);

            cTable t = new cTable(tmp, s);
            rExpressions.Visible = label1.Visible = label2.Visible =  button1.Visible = txtZ.Visible = false;


            #region Creazione oggetto
            DataGridView dg = new DataGridView();
            dg.Name = "dg";
            //CB, Base, Var decisionali + Var scarto, B
            dg.ColumnCount = 3 + t.nVariabiliScarto;
            dg.RowCount = t.Functions.Length + 2;
            //i = 2; le prime due non le conta, Columns.Count - 1; l'ultima non la conta
            for (int i = 2; i < dg.Columns.Count; i++) {
                for (int j = 0; j < dg.Rows.Count; j++) {
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
                    if (c < t.nVariabili) {
                        dg[c + 2, r].Value = t.coefficientTerms[r][c];

                        //Scrittura Cj
                        dg[c + 2, t.nVariabiliScarto - t.nVariabili].Value = t.ZArray[c];
                    }
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

            //Scrittura della cb
            for (int r = 0; r < t.Functions.Length; r++) {
                string str = dg[1, r].Value.ToString();
                int column = getColumnByHeader(str, dg);
                int row = t.Functions.Length + 1;
                dg[0, r].Value = dg[column, row].Value;
            }

            int sabIndex = dg.RowCount - 1;
            for (int c = 2; c < dg.ColumnCount-1; c++) {
                string header = dg.Columns[c].HeaderText;
                int BaseIndex = isBaseVar(header, dg);
                if (BaseIndex > -1) {
                    dg[c, sabIndex].Value = dg[dg.ColumnCount, BaseIndex].Value;
                }
            }

            #endregion

            Button bNext, bPrev;
            bNext = new Button();
            bNext.Name = "bNext";
            bNext.Text = ">";
            bNext.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) + 5, dg.Location.Y + dg.Height + 5);
            this.Controls.Add(bNext);

            bPrev = new Button();
            bPrev.Name = "bPrev";
            bPrev.Text = "<";
            bPrev.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) - bNext.Width, dg.Location.Y + dg.Height + 5);
            bPrev.Enabled = false;
            this.Controls.Add(bPrev);


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
