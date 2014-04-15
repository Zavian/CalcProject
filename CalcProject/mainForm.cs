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

        //I summary verranno messi a codice completo, dato che può essere
        //soggetto a modifiche (e i summary sono fastidiosi)
        //Per ora si avranno summary delle regions



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
        List<DataGridView> DGs = new List<DataGridView>();
        BindingList<cTable> Tables = new BindingList<cTable>();

        #region Metodi di controllo
        private string validNumbers(string t) /*Questo metodo serve per gestire il fatto che non si metta un numero davanti alla X */ {
            //http://regex101.com/r/aM4wZ6/#debugger
            string[] tmp = Regex.Split(t, "([-|\\+]{0,1}\\d{0,}x\\d{1,})");
            tmp = tmp.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
            for (int i = 0; i < tmp.Length; i++) {
                //Se il coeffieciente non è esplicito (quindi es: x1)
                //va ad aggiungerlo
                int n = tmp[i][0] == '+' || tmp[i][0] == '-' ? 1 : 0;
                if (tmp[i][n] > '9' || tmp[i][n] < '0') tmp[i] = tmp[i].Insert(n, "1");
            }
            string s = "";
            for (int i = 0; i < tmp.Length; i++) {
                s += tmp[i];
            }
            return s;
        }

        #endregion


        /// <summary>
        /// Questi metodo permette di creare la finestra di inserimento.
        /// Essi va a creare tutti gli oggetti al suo interno e gestiscono il loro comportamento.
        /// All'interno della sezione è possibile trovare (in ordine di apparizione):
        ///     - Il pannello "insertingWindow"
        ///     - Il pannello "background"
        ///     - Il metodo "createInsertingControls"
        ///     - I metodi di gestione dei controlli nella finestra.
        /// </summary>
        ///
        #region Sezione finestra di inserimento
        private void showInsertingWindow() {
            //Sezione per la creazione del pannello "insertingWindow".
            Panel insertingWindow = new Panel(); //Oggetto, che per comodità si chiamerà come il controllo
            insertingWindow.BorderStyle = BorderStyle.FixedSingle; //Border del pannello
            //Larga 2/3 la form
            //Alta 3/4 la form
            insertingWindow.Size = new Size(this.Size.Width * 2 / 3, this.Size.Height * 3 / 4);
            insertingWindow.Location = new Point(this.Size.Width / 5, -1); //-1 per non mostrare il bordo superiore
            insertingWindow.Name = "insertingWindow"; //Nome con cui verrà visualizzato il controllo
            this.Controls.Add(insertingWindow); //Per far sì che il controllo sia presente

            //Sezione per la creazione del pannello "background"
            //Oggetto, che per comodità si chiamerà come il controllo
            TransPanel background = new TransPanel(this.Width,
                                                   this.Height,
                                                   Color.FromArgb(200, 192, 192, 192) //Argento con trasparenza 200 (max 255 = opaco)
                                                   );
            background.Name = "background";
            background.Click += closeInsertingWindow;
            createInsertingControls(insertingWindow); //Solo ed esclusivamente per ordine del cordice


            this.Controls.Add(background); //Per far sì che il controllo sia presente
            this.ActiveControl = insertingWindow; //Per rendere il tutto piacevole alla vista

            background.BringToFront(); //Mette in primo piano il background
            insertingWindow.BringToFront(); //Mette in primo piano la finestra
        }
        private void createInsertingControls(Panel insertingWindow) {
            //Semplicemente per dopo andare a scriver tutto con il foreach
            List<Object> insertingWindowObjects = new List<object>();
            //Di qui in poi c'è la creazione di tutti gli oggetti che conterrà la insertingWindow
            //Non andrò a spiegare in dettaglio. Sono solo impostazioni di grandezze, font & co.

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
            #region bClose
            Button bClose = new Button();
            bClose.Size = new Size(100, 23);
            bClose.Name = "bClose";
            bClose.Location = new Point(
                insertingWindow.Size.Width - 125,
                insertingWindow.Size.Height - 40
                );
            bClose.Text = "Chiudi";
            bClose.Click += closeInsertingWindow;

            insertingWindowObjects.Add(bClose);
            #endregion

            //Per inserire tutti i controlli che ho appena creato nella insertingWindo (notare che la lista è di oggetti
            //e non di controlli, come richiesto)
            foreach (var item in insertingWindowObjects) insertingWindow.Controls.Add(item as Control);
        }

        void closeInsertingWindow(object sender, EventArgs e) {
            //Metodo per chiudere la finestra dell'inserimento
            this.Controls["insertingWindow"].Dispose();
            this.Controls["background"].Dispose();
        }
        void bAnalize_Click(object sender, EventArgs e) {
            //Ancora da commentare dato che il metodo può essere soggetto a modifiche

            //Debug staff
            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100" };
            string tmp = validNumbers(txtZ.Text); //<--- ricorda questo 

            string errore;
            table = new cTable("Patata", tmp, s, out errore);
            if (errore != null) { MessageBox.Show(errore); return; }
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
            //Gestione del clear all'interno della "insertingWindow"
            foreach (var item in this.Controls["insertingWindow"].Controls) { //Scorre tutti i controlli all'interno della suddetta
                if (item is TextBox) { //Scorre solo le textbox
                    (item as TextBox).Text = ""; //Le pulisce
                    dataTXTLeave(item as TextBox, null); //Fa si che si reimposti il testo
                    //Che genio che sono
                }
            }
        }
        void dataTXTLeave(object sender, EventArgs e) {
            //Metodo che reimposta il testo all'interno delle textbox
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

        void bPrev_Click(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
        void bNext_Click(object sender, EventArgs e) {
            DataGridView dg = getDG();
            string entering = enteringVar(dg);
            if (entering == "fine") { this.Controls["bNext"].Enabled = false; return; }

            string[] exiting = exitingVar(dg, entering);
            calculateNewTable(dg, entering, exiting[0], exiting[1]);
            writeSab(dg);
            writeCb(table, dg);            
            this.Controls["dg"].Refresh();
        }
        #endregion

        /// <summary>
        /// Questi metodi sono stati creati dato che vengono più e più volte
        /// richiamati all'interno del programma per avere oggetti e array.
        /// All'interno della sezione è possibile trovare (in ordine di apparizione):
        ///     - GetNumber:            Vari metodi per la gestione dei numeri dentro la form.
        ///     - getDG:                Ritorna il DataGridView attivo nel programma.
        ///     - getColumn:            Restituisce la colonna richiesta.
        ///     - getColumnByHeader:    Restituisce la colonna in base all'header.
        ///     
        /// </summary>
        /// 
        #region Sezione request object
        #region GetNumber
        double getNumber(string t) {
            t = t.Trim();
            return itIsDivided(t) ? soDivideIt(t) : Convert.ToDouble(t);
        }
        bool itIsDivided(string t) {
            for (int i = 0; i < t.Length; i++) {
                if (t[i] == '/') return true;
            }
            return false;
        }
        private static double soDivideIt(string t) {
            double p;
            string t1, t2;
            t1 = t.Split('/')[0];
            t2 = t.Split('/')[1];
            p = Convert.ToDouble(t1) / Convert.ToDouble(t2);
            return p;
        }
        #endregion
        private DataGridView getDG() { return this.Controls["dg"] as DataGridView; }
        private double[] getColumn(DataGridView dg, int columIndex) {
            double[] ris;
            //Se l'indice è 0 gestisce la prima colonna in modo corretto
            //dato che in quel caso non deve prendere la cj
            int add = columIndex == 0 || columIndex == dg.ColumnCount - 1 ? 0 : 1;
            ris = new double[table.Functions.Count() + add];  //+ add se c'è la cj
            if (add == 1) ris[0] = getNumber(dg[columIndex, table.Functions.Count()].Value.ToString());
            for (int i = 0; i < table.Functions.Count(); i++) {
                ris[i + add] = getNumber(dg[columIndex, i].Value.ToString());
            }

            //Il return in base che la colonna non sia l'ultima o la prima
            //[0] -> Cj, il resto la colonna.
            return ris;
        }
        private int getColumnByHeader(DataGridView dg, string s) {
            for (int i = 0; i < dg.Columns.Count; i++) {
                if (dg.Columns[i].HeaderText == s) return i;
            }
            return -1;
        }
        #endregion


        /// <summary>
        /// Questi metodi sono stati creati per la generazione di tabelle
        /// e la gestione delle stesse nel cambio di tabella e nei calcoli.
        /// All'interno della sezione è possibile trovare (in ordine di apparizione):
        ///     -createDataGrid:    Creazione dell'oggetto iniziale
        ///     -writeHeaders:      Scrittura degli headers dell'oggetto
        ///     -writeTableNumbers: Scrittura dei numeri nella tabella
        ///     -writeB:            Scrittura della B
        ///     -writeCb:           Scrittura della Cb
        ///     -writeSab:          Scrittura della Sab
        ///     -isBaseVar:         Controllo se la variabile è di base
        ///     -enteringVar:       Calcolo variabile entrante
        ///     -exitingVar:        Calcolo variabile uscente
        ///     -calculateNewTable: Calcolo della tabella successiva
        ///     -editBaseVar:       Sostituzione della variabile di base uscente con quella entrante
        /// </summary>
        ///
        #region Sezione calcoli tabelle e simplessi
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
                int column = getColumnByHeader(dg, str);
                int row = t.Functions.Length + 1;
                dg[0, r].Value = dg[column, row].Value;
            }
        }
        private void writeSab(DataGridView dg) {
            int sabIndex = dg.RowCount - 1;
            for (int c = 2; c < dg.ColumnCount - 1; c++) {
                string header = dg.Columns[c].HeaderText;
                int BaseIndex = isBaseVar(dg, header);
                if (BaseIndex > -1) {

                    dg[c, sabIndex].Value = dg[dg.ColumnCount - 1, BaseIndex].Value;
                }
            }
        }

        private int isBaseVar(DataGridView dg, string s) {
            //Questo metodo prende le stringhe nella prima colonna
            //e le compara con la stringa richiesta
            for (int r = 0; r < dg.RowCount - 2; r++) {
                string tmp = dg[1, r].Value.ToString();
                if (tmp == s) return r;
            }
            return -1;
        }       
        private string enteringVar(DataGridView dg) {
            int dc = table.nVariabili;
            double[] myArray = new double[table.nVariabiliScarto];
            for (int i = 0; i < myArray.Length; i++) { myArray[i] = double.MinValue; }
            for (int i = 2; i < dg.Columns.Count - 1; i++) {
                string header = dg.Columns[i].HeaderText;
                if (isBaseVar(dg, header) == -1) { //Se non è una variabile di base
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
            int negNumbers = 0;
            for (int i = 0; i < myArray.Length; i++) { negNumbers += myArray[i] <= 0 ? 1 : 0; }
            bool allNeg = negNumbers == myArray.Length;

            if (!allNeg) {
                double maxValue = myArray.Max();
                int maxIndex = myArray.ToList().IndexOf(maxValue);
                maxIndex += 1;
                MessageBox.Show("La variabile entrante è: x" + maxIndex.ToString());
                return "x" + maxIndex.ToString();
            }
            else { MessageBox.Show("Non è più possibile andare avanti."); return "fine"; }
        }
        private string[] exitingVar(DataGridView dg, string enteringVar) {
            int dc = table.nVariabili;
            double[] myArray = new double[table.nVariabiliScarto];
            for (int i = 0; i < myArray.Length; i++) { myArray[i] = double.MaxValue; }

            getColumn(dg, dg.ColumnCount - 1);
            int columnIndex = getColumnByHeader(dg, enteringVar);
            for (int i = 0; i < table.Functions.Length; i++) {
                myArray[i] = getNumber(dg[columnIndex, i].Value.ToString()) / getColumn(dg, dg.ColumnCount - 1)[i];
            }
            double minValue = myArray.Min();
            int minIndex = myArray.ToList().IndexOf(minValue);
            double pivot = getNumber(dg[columnIndex, minIndex].Value.ToString());
            MessageBox.Show("La variabile uscente è: " + dg[1, minIndex].Value.ToString());
            string[] returner = { dg[1, minIndex].Value.ToString(), pivot.ToString() };
            return returner;
        }

        private void calculateNewTable(DataGridView dg, string enteringVar, string exitingVar, string pivot) {
            DGs.Add(dg);
            int rowIndex = editBaseVar(dg, enteringVar, exitingVar);
            if (rowIndex == -1) return;
            for (int i = 0; i < dg.ColumnCount - 2; i++) {
                dg[i + 2, rowIndex].Value = getNumber(dg[i + 2, rowIndex].Value.ToString()) / getNumber(pivot);
            }
        }
        private int editBaseVar(DataGridView dg, string enteringVar, string exitingVar) {
            for (int i = 0; i < dg.RowCount; i++) {
                if (dg[1, i].Value.ToString() == exitingVar) { dg[1, i].Value = enteringVar; return i; }
            }
            return -1;
        }
        #endregion

        #region Event Handlers Form
        //Questo è temporaneo
        private void button1_Click(object sender, EventArgs e) {
            //Debug staff
            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100" };
            string tmp = validNumbers(txtZ.Text); //<--- ricorda questo 

            string errore;
            table = new cTable("Patata", tmp, s, out errore);
            if (errore != null) { MessageBox.Show(errore); return; }
            Tables.Add(table);
            //--------------

            rExpressions.Visible = label1.Visible = button1.Visible = txtZ.Visible = false;


            DataGridView dg = createDataGrid(table);
            writeHeaders(table, dg);
            writeTableNumbers(table, dg);
            writeB(table, dg);
            writeSab(dg);
            writeCb(table, dg);
            




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


        private void txtZ_Enter(object sender, EventArgs e) {
            txtZ.Text = "";
        }
        private void txtZ_Leave(object sender, EventArgs e) {
            if (txtZ.Text.Trim() == "") { txtZ.Text = "Inserire la funzione Z"; return; }
            button1.Enabled = true;
            //Da aggiungere la gestione cazzate
        }

        //Questo si chiamerà in un altro modo
        private void button2_Click(object sender, EventArgs e) {
            showInsertingWindow();
        }
        #endregion











        

        
        

        




        
    }
}
