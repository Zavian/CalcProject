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

            //Gestione contenuti e grafica della listbox
            this.listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            this.listBox1.DataSource = Tables;
            this.listBox1.DisplayMember = "exName";
            this.listBox1.ValueMember = "exName";
            this.listBox1.ItemHeight = 20;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.myListBox_DrawItem);


            //Debug stuff
            txtZ.Text = "3x1+3x2+4x3";
            //button1_Click(null, null); //<--- ROBA DA DEBUG IMPORTANTE
            //---------------
        }

        private void myListBox_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
            //Gestione della grafica della listbox
            //Questo metodo permette di creare il rettangolo grigio attorno al nome dell'esercizio

            e.DrawBackground();
            Font myFont;
            Brush myBrush = Brushes.Black;
            int i = e.Index;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {            
                e.Graphics.FillRectangle(Brushes.White, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                myBrush = new SolidBrush(Color.FromArgb(255, 71, 71, 71));
            }
            myFont = new Font("Arial", 12, FontStyle.Bold);
            Rectangle myRect = e.Bounds;
            e.Graphics.DrawString(Tables[i].exName, myFont, myBrush, myRect, StringFormat.GenericTypographic);
        }

        cTable table;
        BindingList<cTable> Tables = new BindingList<cTable>();


        private DataGridView getDG() { return this.Controls["dg"] as DataGridView; }
        private double[] getColumn(DataGridView dg, int columIndex) {
            double[] ris;
            int add = columIndex == 0 ? 0 : 1;
            ris = new double[table.Functions.Count() + add];
            if (add == 1) ris[0] = Convert.ToDouble(dg[columIndex, table.Functions.Count()].Value);
            for (int i = 0; i < table.Functions.Count(); i++) {
                ris[i + add] = Convert.ToDouble(dg[columIndex, i].Value);
            }

            return ris;
        }
        private string enteringVar(DataGridView dg) {
            int dc = table.nVariabili;
            double[] myArray = new double[table.nVariabiliScarto];
            for (int i = 0; i < myArray.Length; i++) { myArray[i] = double.MinValue; }
            for (int i = 2; i < dg.Columns.Count - 1; i++) {
                string header = dg.Columns[i].HeaderText;
                if (isBaseVar(header, dg) == -1) { //Se non è una variabile di base
                    double ris = 0;
                    double[] column = getColumn(dg, i), cb = getColumn(dg, 0);

                    ris = column[0]; //cj
                    double parteDestra = 0;
                    for (int k = 1; k < column.Length; k++) {
                        parteDestra += column[k] * cb[k - 1];
                    }

                    ris -= parteDestra;
                    myArray[i - 2] = ris;
                }
                
            }

            double maxValue = myArray.Max();
            int maxIndex = myArray.ToList().IndexOf(maxValue);
            maxIndex += 1;
            MessageBox.Show("La variabile entrante è: x" + maxIndex.ToString());
            return "x" + maxIndex.ToString();
        }
        private int isBaseVar(string s, DataGridView dg) {
            for (int r = 0; r < dg.RowCount - 2; r++) {
                string tmp = dg[1, r].Value.ToString();
                if (tmp == s) return r;
            }
            return -1;
        }       
        private int getColumnByHeader(string s, DataGridView dg) {
            for (int i = 0; i < dg.Columns.Count; i++) {
                if (dg.Columns[i].HeaderText == s) return i;
            }
            return -1;
        }     
        private string validNumbers(string t) /*Questo metodo serve per gestire il fatto che non si metta un numero davanti alla X */ {
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


            //Debug staff
            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100"};
            string tmp = validNumbers(txtZ.Text); //<--- ricorda questo 

            table = new cTable("Patata", tmp, s);
            Tables.Add(table);
            //--------------

            rExpressions.Visible = label1.Visible =  button1.Visible = txtZ.Visible = false;


            DataGridView dg = createDataGrid(table); 
            writeHeaders(table, dg);
            writeTableNumbers(table, dg);
            writeB(table, dg);
            writeCb(table, dg);
            writeSab(dg);




            Button bNext, bPrev;
            bNext = new Button();
            bNext.Name = "bNext";
            bNext.Text = ">";
            bNext.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) + 5, dg.Location.Y + dg.Height + 5);
            this.Controls.Add(bNext);
            bNext.Click += bNext_Click;

            bPrev = new Button();
            bPrev.Name = "bPrev";
            bPrev.Text = "<";
            bPrev.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) - bNext.Width, dg.Location.Y + dg.Height + 5);
            bPrev.Enabled = false;
            this.Controls.Add(bPrev);
            bPrev.Click += bPrev_Click;


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
            txtZ.Dispose();
            button1.Dispose();

            
        }

        void bPrev_Click(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
        void bNext_Click(object sender, EventArgs e) {
            DataGridView dg = getDG();
            enteringVar(dg);
        }

        private DataGridView createDataGrid(cTable table) {
            DataGridView dg = new DataGridView();
            dg.Name = "dg";
            //CB, Base, Var decisionali + Var scarto, B
            dg.ColumnCount = 3 + table.nVariabiliScarto;
            dg.RowCount = table.Functions.Length + 2;
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

            for (int i = 0; i < table.nVariabiliScarto; i++) {
                dg.Columns[i + 2].HeaderText = "x" + (i + 1);
            }
            dg.Columns[table.nVariabiliScarto + 2].HeaderText = "b";
            return dg;
        }
        private static void writeHeaders(cTable table, DataGridView dg) {
            for (int i = 0; i < table.Functions.Length; i++) {
                dg[0, i].Value = 0;
                dg[1, i].Value = "x" + (table.nVariabili + (1 + i));
            }
            dg[1, table.Functions.Length].Value = "Cj"; //GTA San Andreas cit.
            dg[1, table.Functions.Length + 1].Value = "SAB";
        }
        private static void writeTableNumbers(cTable t, DataGridView dg) {
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
        }

        private static void writeB(cTable t, DataGridView dg) {
            for (int r = 0; r < t.Vars.Count; r++) {
                dg[dg.Columns.Count - 1, r].Value = t.Vars[r][3];
            }
        }
        private void writeCb(cTable t, DataGridView dg) {
            for (int r = 0; r < t.Functions.Length; r++) {
                string str = dg[1, r].Value.ToString();
                int column = getColumnByHeader(str, dg);
                int row = t.Functions.Length + 1;
                dg[0, r].Value = dg[column, row].Value;
            }
        }
        private void writeSab(DataGridView dg) {
            int sabIndex = dg.RowCount - 1;
            for (int c = 2; c < dg.ColumnCount - 1; c++) {
                string header = dg.Columns[c].HeaderText;
                int BaseIndex = isBaseVar(header, dg);
                if (BaseIndex > -1) {

                    dg[c, sabIndex].Value = dg[dg.ColumnCount - 1, BaseIndex].Value;
                }
            }
        }

        

        private void txtZ_Enter(object sender, EventArgs e) {
            txtZ.Text = "";
        }
        private void txtZ_Leave(object sender, EventArgs e) {
            if (txtZ.Text.Trim() == "") { txtZ.Text = "Inserire la funzione Z"; return; }
            button1.Enabled = true;
            //Da aggiungere la gestione cazzate
        }

        private void button2_Click(object sender, EventArgs e) {
            Panel insertingWindow = new Panel();
            insertingWindow.BorderStyle = BorderStyle.FixedSingle;
            insertingWindow.Size = new Size(this.Size.Width * 2 / 3, this.Size.Height * 3/4);
            insertingWindow.Location = new Point(this.Size.Width/5, -1);
            insertingWindow.Name = "insertingWindow";
            this.Controls.Add(insertingWindow);

            TransPanel background = new TransPanel(this.Width, 
                                                   this.Height, 
                                                   Color.FromArgb(200, 192, 192, 192)
                                                   );
            background.Size = this.Size;
            background.Location = new Point(0, 0);
            background.Name = "background";
            background.BackColor = Color.FromArgb(0, 192, 192, 192);
            this.Controls.Add(background);


            

            List<Object> insertingWindowObjects = new List<object>();

            #region exerciseName
            TextBox exerciseName = new TextBox();
            exerciseName.Size = new Size(200, 20);
            exerciseName.MaxLength = 30;
            exerciseName.Text = "Inserire il nome dell'esercizio";
            exerciseName.Name = "exerciseName";
            exerciseName.Location = new Point(insertingWindow.Size.Width / 10, 15);
            exerciseName.Enter += dataTXTEnter;
            exerciseName.Leave += dataTXTLeave;

            insertingWindowObjects.Add(exerciseName);
            #endregion      
            #region cbMinMax
            ComboBox cbMinMax = new ComboBox();
            cbMinMax.Size = new Size(125, 20);
            cbMinMax.Name = "cbMinMax";
            string[] tmp = { "Problema di Massimo", "Problema di Minimo" };
            cbMinMax.Items.AddRange(tmp);
            cbMinMax.Location = new Point(
                exerciseName.Location.X + exerciseName.Width + 5,
                15
                );
            cbMinMax.SelectedIndex = 0;

            insertingWindowObjects.Add(cbMinMax);
            #endregion

            #region exerciseZ
            TextBox exerciseZ = new TextBox();
            exerciseZ.Size = new Size(375, 20);
            exerciseZ.Text = "Inserire la Z del problema";
            exerciseZ.Name = "exerciseZ";
            exerciseZ.Location = new Point(
                insertingWindow.Size.Width / 10,
                exerciseName.Location.Y + 30
                );
            exerciseZ.Enter += dataTXTEnter;
            exerciseZ.Leave += dataTXTLeave;

            insertingWindowObjects.Add(exerciseZ);
            #endregion            
            #region exerciseFunctions
            TextBox exerciseFunctions = new TextBox();
            exerciseFunctions.Multiline = true;
            exerciseFunctions.Size = new Size(375, 100);
            exerciseFunctions.Text = "Inserire le funzioni del problema (una per riga)";
            exerciseFunctions.Name = "exerciseFunctions";
            exerciseFunctions.Location = new Point(
                insertingWindow.Size.Width / 10,
                exerciseZ.Location.Y + 30
                );
            exerciseFunctions.Enter += dataTXTEnter;
            exerciseFunctions.Leave += dataTXTLeave;

            insertingWindowObjects.Add(exerciseFunctions);
            #endregion

            #region bClear
            Button bClear = new Button();
            bClear.Size = new Size(125, 23);
            bClear.Name = "bClear";
            bClear.Location = new Point(
                insertingWindow.Size.Width / 10, 
                exerciseFunctions.Location.Y + exerciseFunctions.Height + 15
                );
            bClear.Text = "Ripulisci";
            bClear.Click += bClear_Click;

            insertingWindowObjects.Add(bClear);
            #endregion
            #region bAnalize
            Button bAnalize = new Button();
            bAnalize.Size = new Size(235, 23);
            bAnalize.Name = "bAnalize";
            bAnalize.Location = new Point(
                insertingWindow.Size.Width / 10 + bClear.Width + 5, 
                exerciseFunctions.Location.Y + exerciseFunctions.Height + 15
                );
            bAnalize.Text = "Analizza";
            bAnalize.Click += bAnalize_Click;

            insertingWindowObjects.Add(bAnalize);
            #endregion


            foreach (var item in insertingWindowObjects) insertingWindow.Controls.Add(item as Control);
            this.ActiveControl = insertingWindow;

            background.BringToFront();
            insertingWindow.BringToFront();            
        }

        void bAnalize_Click(object sender, EventArgs e) {
            //Debug staff
            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100" };
            string tmp = validNumbers(txtZ.Text); //<--- ricorda questo 

            table = new cTable("Patata", tmp, s);
            Tables.Add(table);
            //--------------

            rExpressions.Visible = label1.Visible = button1.Visible = txtZ.Visible = false;


            DataGridView dg = createDataGrid(table);
            writeHeaders(table, dg);
            writeTableNumbers(table, dg);
            writeB(table, dg);
            writeCb(table, dg);
            writeSab(dg);




            Button bNext, bPrev;
            bNext = new Button();
            bNext.Name = "bNext";
            bNext.Text = ">";
            bNext.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) + 5, dg.Location.Y + dg.Height + 5);
            this.Controls.Add(bNext);
            bNext.Click += bNext_Click;

            bPrev = new Button();
            bPrev.Name = "bPrev";
            bPrev.Text = "<";
            bPrev.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) - bNext.Width, dg.Location.Y + dg.Height + 5);
            bPrev.Enabled = false;
            this.Controls.Add(bPrev);
            bPrev.Click += bPrev_Click;

            this.Controls["insertingWindow"].Dispose();
            this.Controls["background"].Dispose();
            this.ActiveControl = dg;

        }

        void bClear_Click(object sender, EventArgs e) {
            foreach (var item in this.Controls["insertingWindow"].Controls) {
                if (item is TextBox) {
                    (item as TextBox).Text = "";
                    dataTXTLeave(item as TextBox, null); //<-- Che genio che sono
                }
            }
        }

        void dataTXTLeave(object sender, EventArgs e) {
            TextBox t = sender as TextBox;
            if (t.Text.Trim() == "") {
                switch (t.Name) {
                    case "exerciseName": t.Text = "Inserire il nome dell'esercizio"; break;
                    case "exerciseZ": t.Text = "Inserire la Z del problema"; break;
                    case "exerciseFunctions": t.Text = "Inserire le funzioni del problema (una per riga)"; break;
                }
            }
        }
        void dataTXTEnter(object sender, EventArgs e) {
            TextBox t = sender as TextBox;
            if (t.Text.Contains("Inserire")) t.Clear();
        }
    }
}
