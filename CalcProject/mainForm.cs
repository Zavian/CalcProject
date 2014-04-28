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

        double M;   //Variabile che avrà come valore la M

        /// <summary>
        /// Metodo di calcolo del risultato finale.
        /// </summary>
        /// <param name="dg">Datagrid da cui ricavarlo.</param>
        /// <returns></returns>
        double calcRis(DataGridView dg) {
            int riga = table.Functions.Length; //qui sono alla riga cj
            double ris = 0;
            for (int i = 2; i < dg.ColumnCount - 1; i++) {
                ris += getNumber(dg[i, riga].Value.ToString()) * getNumber(dg[i, riga + 1].Value.ToString());
            }
            return ris;
        }


        /// <summary>
        /// Metodo per la ricerca del controllo selezionato.
        /// </summary>
        /// <param name="name">Nome del controllo.</param>
        /// <returns></returns>
        bool existsControl(string name) {
            return this.Controls.Find(name, true).Length > 0;
        }

        /// <summary>
        /// Metodo per capire se esiste la M nella colonna.
        /// </summary>
        /// <param name="dg">Datagrid in cui controlla.</param>
        /// <param name="columnIndex">Colonna in cui controlla.</param>
        /// <returns></returns>
        bool isThereM(DataGridView dg, int columnIndex) {
            double[] tmp = getColumn(dg, columnIndex);
            for (int i = 0; i < tmp.Length; i++) {
                if (tmp[i] == M && M != 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo per la gestione della lunghezza dei double.
        /// </summary>
        /// <param name="value">Valore da troncare.</param>
        /// <param name="precision">Precisione della troncatura</param>
        /// <returns></returns>
        public double TruncateDouble(double value, int precision) {
            double step = (double)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }

        public mainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Posiziona la ListBox all'interno della form.
        /// </summary>
        private void placeListBox() {
            stripMenu.Visible = true;
            ListBox lst = new ListBox();
            lst.Name = "lstExercises";
            lst.BackColor = Color.Silver;
            lst.DrawMode = DrawMode.OwnerDrawVariable;
            lst.DataSource = Tables;
            lst.DisplayMember = "exName";
            lst.ValueMember = "tableIndex";
            lst.ItemHeight = 20;
            lst.DrawItem += new DrawItemEventHandler(myListBox_DrawItem);

            lst.Dock = DockStyle.Left;
            lst.Width = 140;
            lst.SelectedIndexChanged += lst_SelectedIndexChanged;
            this.Controls.Add(lst);
        }

        
        string oldSelected = ""; //Variabile necessaria per lo switch tra i data grids        
        bool throughCode = false; //Variabile necessaria per impostare via codice il cambio indice

        /// <summary>
        /// Cambiamento dell'indice.
        /// </summary>
        /// <param name="index">Indice in cui cambiare.</param>
        void changeIndex(int index) { (this.Controls["lstExercises"] as ListBox).SelectedIndex = index; throughCode = true; }
        void lst_SelectedIndexChanged(object sender, EventArgs e) {
            //Semplicemente qui esso riscrive il datagrid nel caso si abbia cambiato tabella
            if (throughCode) { throughCode = false; return; }
            ListBox lst = sender as ListBox;
            int index = lst.SelectedIndex;
            if (oldSelected != Tables[index].exName) {
                if (existsControl("dg")) this.Controls["dg"].Dispose();
                writeDG(Tables[index]);
                this.Controls["bNext"].Enabled = true;
                this.Controls["bEnd"].Enabled = true;
                this.Controls["labelUscente"].Text = "";
                this.Controls["labelEntrante"].Text = "";
                this.Controls["labelFine"].Text = ""; 
                oldSelected = Tables[index].exName;
            }
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

        #region Metodi di controllo
        private string validNumbers(string t) /*Questo metodo serve per gestire il fatto che non si metta un numero davanti alla X */ {
            //http://regex101.com/r/aM4wZ6/#debugger
            string[] tmp = Regex.Split(t, @"([-|\+]{0,1}\d*\,?\d{0,}x\d*\d{1,})");
            tmp = tmp.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote

            int ender = t.Contains('=') ? tmp.Length - 1 : tmp.Length; //<-- Per non considerare la parte destra dell'espressione
            for (int i = 0; i < ender; i++) {
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


        #region Sezione finestra di inserimento
        /// <summary>
        /// Metodo selezione della combobox all'interno della finestra di inserimento.
        /// </summary>
        /// <param name="name">Nome della combobox.</param>
        /// <returns></returns>
        private ComboBox getMiniCombo(string name) { return this.Controls["insertingWindow"].Controls[name] as ComboBox; }

        /// <summary>
        /// Metodo selezione della textbox all'interno della finestra di inserimento.
        /// </summary>
        /// <param name="name">Nome della textbox.</param>
        /// <returns></returns>
        private TextBox getMiniTextBox(string name) { return this.Controls["insertingWindow"].Controls[name] as TextBox; }

        /// <summary>
        /// Metodo che mostra la finestra di inserimento.
        /// </summary>
        private void showInsertingWindow() {
            TransPanel background = new TransPanel(this.Width,
                                                   this.Height,
                                                   Color.FromArgb(200, 192, 192, 192) //Argento con trasparenza 200 (max 255 = opaco)
                                                   );
            background.Size = this.Size;
            background.Name = "background";
            background.Location = new Point(0, 0);
            background.BackColor = Color.FromArgb(200, 192, 192, 192);
            this.Controls.Add(background); //Per far sì che il controllo sia presente


            //Sezione per la creazione del pannello "insertingWindow".
            Panel insertingWindow = new Panel(); //Oggetto, che per comodità si chiamerà come il controllo
            insertingWindow.BorderStyle = BorderStyle.FixedSingle; //Border del pannello
            //Larga 2/3 la form
            //Alta 3/4 la form
            insertingWindow.Size = new Size(this.Size.Width * 2 / 3, this.Size.Height * 3 / 4);
            insertingWindow.Location = new Point(this.Size.Width / 5, -insertingWindow.Height); //-1 per non mostrare il bordo superiore
            insertingWindow.Name = "insertingWindow"; //Nome con cui verrà visualizzato il controllo
            createInsertingControls(insertingWindow); //Solo ed esclusivamente per ordine del cordice
            this.Controls.Add(insertingWindow); //Per far sì che il controllo sia presente
            for (int i = 0; i < this.Size.Height * 3 / 4 - 1; i+=4) {
                insertingWindow.Location = new Point(insertingWindow.Location.X, i - insertingWindow.Height);
            }
            
            //Sezione per la creazione del pannello "background"
            //Oggetto, che per comodità si chiamerà come il controllo
            this.ActiveControl = insertingWindow; //Per rendere il tutto piacevole alla vista

            background.BringToFront(); //Mette in primo piano il background
            insertingWindow.BringToFront(); //Mette in primo piano la finestra
        }

        /// <summary>
        /// Metodo che crea i controlli all'interno della finestra di inserimento.
        /// </summary>
        /// <param name="insertingWindow">Il pannello della finestra.</param>
        private void createInsertingControls(Panel insertingWindow) {
            //Semplicemente per dopo andare a scriver tutto con il foreach
            List<Object> insertingWindowObjects = new List<object>();
            //Di qui in poi c'è la creazione di tutti gli oggetti che conterrà la insertingWindow
            //Non andrò a spiegare in dettaglio. Sono solo impostazioni di grandezze, font & co.

            #region exerciseName
            TextBox exerciseName = new TextBox();
            exerciseName.Size = new Size(200, 20);
            exerciseName.Font = new Font("Calibri", 12, FontStyle.Regular);
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
            cbMinMax.Size = new Size(170, 20);
            cbMinMax.Font = new Font("Calibri", 12, FontStyle.Regular);
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
            exerciseZ.Font = new Font("Calibri", 12, FontStyle.Regular);
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
            exerciseFunctions.Font = new Font("Calibri", 12, FontStyle.Regular);
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
            bClear.Size = new Size(125, 30);
            bClear.Font = new Font("Calibri", 12, FontStyle.Regular);
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
            bAnalize.Size = new Size(235, 30);
            bAnalize.Font = new Font("Calibri", 12, FontStyle.Regular);
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

        /// <summary>
        /// Metodo per la chiusura.
        /// </summary>
        /// <param name="sender">Ciò che attiva l'evento.</param>
        /// <param name="e">Gli eventi del suddetto.</param>
        void closeInsertingWindow(object sender, EventArgs e) {
            //Metodo per chiudere la finestra dell'inserimento
            this.Controls["insertingWindow"].Dispose();
            this.Controls["background"].Dispose();
        }
        void bAnalize_Click(object sender, EventArgs e) {
            //Ancora da commentare dato che il metodo può essere soggetto a modifiche
            /* Controlli all'interno della miniForm
             * exerciseName
             * cbMinMax
             * exerciseZ
             * exerciseFunctions
             * bClear
             * bAnalize
             * bClose
            */
            if (existsControl("button2") && existsControl("label1") && existsControl("button3")) {
                label1.Dispose();
                button2.Dispose();
                button3.Dispose();
            }


            //Controlla i coefficienti nella Z

            //Controlla i coefficienti nelle funzioni
            TextBox exerciseFunctions = getMiniTextBox("exerciseFunctions");
            exerciseFunctions.Lines = exerciseFunctions.Lines.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //Elimina le linee vuote

            string[] functions = exerciseFunctions.Lines;
            exerciseFunctions.Clear();
            for (int i = 0; i < functions.Length; i++) {
                functions[i] = validNumbers(functions[i]);
                exerciseFunctions.Text += functions[i] + Environment.NewLine;
            }
            


            string errore;

            table = new cTable(
                getMiniTextBox("exerciseName").Text,            //Nome dell'esercizio
                validNumbers(getMiniTextBox("exerciseZ").Text), //Z dell'esercizio
                exerciseFunctions.Lines,                        //Funzioni dell'esercizio
                getMiniCombo("cbMinMax").SelectedText == "Problema di Massimo" ? "max" : "min",
                out errore                                      //Eventuali errori
            );
            if (errore != null) { MessageBox.Show(errore); return; }
            Tables.Add(table);

            //Se non c'è posiziona la Listbox
            if (!existsControl("lstExercises")) {
                placeListBox();
                oldSelected = table.exName;
            }
            else changeIndex(Tables.Count - 1);


            //rExpressions.Visible = label1.Visible = button1.Visible = txtZ.Visible = false;

            //Crea la tabella
            DataGridView dg;


            if (existsControl("dg")) this.Controls["dg"].Dispose();
            dg = writeDG(table);

            if (!existsControl("bNext")) {
                createControlsUnderDG(dg);
            }
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


        void bNext_Click(object sender, EventArgs e) {
            DataGridView dg = getDG();
            string entering = enteringVar(dg);
            if (entering == "fine") { 
                this.Controls["bNext"].Enabled = this.Controls["bEnd"].Enabled = false;
                this.Controls["labelFine"].Text = "Il risultato finale è uguale a: " + calcRis(dg).ToString();
                return; 
            }

            string[] exiting = exitingVar(dg, entering);
            if (isThereM(dg, getColumnByHeader(dg, exiting[0])))
                dg.Columns.RemoveAt(getColumnByHeader(dg, exiting[0]));
            calculateNewTable(dg, entering, exiting[0], exiting[1]);
            calculateGauss(dg, Convert.ToDouble(exiting[1]), Convert.ToInt32(exiting[2]), Convert.ToInt32(exiting[3]));
            writeCb(table, dg);
            writeSab(dg);
            
            this.Controls["dg"].Refresh();
        }
        #endregion

        #region Sezione metodi di richiesta
        #region GetNumber
        /// <summary>
        /// Metodo utilizzato per estrarre i numeri all'interno della stringa (solitamente datagridview).
        /// </summary>
        /// <param name="t">Stringa contente il numero.</param>
        /// <returns></returns>
        double getNumber(string t) {
            t = t.Trim();
            if (t == "-M") 
                return M = -table.MaxNumber*10000.00;
            double returner = itIsDivided(t) ? soDivideIt(t) : Convert.ToDouble(t);
            return TruncateDouble(returner, 2);
        }

        /// <summary>
        /// Controllo (non testato) sulla divisione dei numeri.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool itIsDivided(string t) {
            return t.Contains('\\');
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

        /// <summary>
        /// Metodo per ricavare il datagridview attivo.
        /// </summary>
        /// <returns></returns>
        private DataGridView getDG() { return this.Controls["dg"] as DataGridView; }

        /// <summary>
        /// Metodo per estrapolare la colonna.
        /// </summary>
        /// <param name="dg">Datagridview da cui si deve estrapolare.</param>
        /// <param name="columIndex">La colonna da cui si devono prendere i dati.</param>
        /// <returns>Il primo valore è la cj, il resto è la colonna restante</returns>
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

        /// <summary>
        /// Ricava l'indice della colonna in base all'header dato.
        /// </summary>
        /// <param name="dg">Datagridview da cui si deve estrapolare.</param>
        /// <param name="s">Header richiesto.</param>
        /// <returns></returns>
        private int getColumnByHeader(DataGridView dg, string s) {
            for (int i = 0; i < dg.Columns.Count; i++) {
                if (dg.Columns[i].HeaderText == s) return i;
            }
            return -1;
        }
        #endregion


        #region Sezione calcoli tabelle e simplessi

        /// <summary>
        /// Metodo scritto per comodità per l'avvio di più metodi.
        /// </summary>
        /// <param name="table">L'oggetto della tabella.</param>
        /// <returns></returns>
        private DataGridView writeDG(cTable table) {
            this.table = table;
            DataGridView dg = createDataGrid(table);
            writeHeaders(table, dg);
            writeTableNumbers(table, dg);
            writeB(table, dg);
            writeCb(table, dg);
            writeSab(dg);
            return dg;
        }

        /// <summary>
        /// Metodo per la creazione del DataGridView.
        /// </summary>
        /// <param name="table">L'oggetto della tabella.</param>
        /// <returns></returns>
        private DataGridView createDataGrid(cTable table) {
            DataGridView dg = new DataGridView();
            dg.Name = "dg";
            dg.ReadOnly = true;
            dg.AllowUserToAddRows = false;
            dg.AllowUserToDeleteRows = false;
            dg.AllowUserToResizeColumns = false;
            dg.AllowUserToResizeRows = false;
            dg.Font = new Font("Calibri", 12, FontStyle.Regular);
            //CB, Base, Var decisionali + Var scarto, B
            int select = table.nVariabiliArtificiali == 0 ? table.nVariabiliScarto - 1 : table.nVariabiliArtificiali - 1;
            dg.ColumnCount = 3 + select;
            dg.RowCount = table.Functions.Length + 2;
            //le prime due non le conta, Columns.Count - 1; l'ultima non la conta
            for (int i = 2; i < dg.Columns.Count; i++) {
                for (int j = 0; j < dg.Rows.Count; j++) {
                    dg.Rows[j].Cells[i].Value = "0";
                }
            }
            for (int i = 0; i < dg.Columns.Count; i++) { dg.Columns[i].Width = 50; }

            Control relativePosition = this.Controls["lstExercises"];
            Size relativeSize = new Size(375, 100); //La grandezza della textbox delle funzioni
            dg.Location = new Point(relativePosition.Location.X + relativePosition.Width + 30, relativePosition.Height / 10);
            dg.Size = new Size(relativeSize.Width + 125, relativeSize.Height + 125);
            dg.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            dg.Columns[0].HeaderText = "Cb";
            dg.Columns[1].HeaderText = /* All your */"Base" /* are belong to us */;
            this.Controls.Add(dg);

            for (int i = 0; i < select; i++) {
                dg.Columns[i + 2].HeaderText = "x" + (i + 1);
            }
            dg.Columns[select + 2].HeaderText = "b";
            return dg;
        }

        /// <summary>
        /// Metodo per scrivere gli headers (xn, SAB, Cj).
        /// </summary>
        /// <param name="table">Tabella da cui prendere i dati.</param>
        /// <param name="dg">DataGridView in cui scrivere.</param>
        private static void writeHeaders(cTable table, DataGridView dg) {
            for (int i = 0; i < table.Functions.Length; i++) {
                int index = table.Functions[i].IndexOf('<');
                if(index < 0) index = table.Functions[i].IndexOf('>');
                if(index < 0) index = table.Functions[i].IndexOf('=');

                string tmp = table.Functions[i].Substring(0, index);
                string[] tmpArray = Regex.Split(tmp, @"(x\d{1,})");
                tmpArray = tmpArray.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote

                dg[0, i].Value = 0;
                dg[1, i].Value = tmpArray[tmpArray.Length - 1];
            }
            dg[1, table.Functions.Length].Value = "Cj"; //GTA San Andreas cit.
            dg[1, table.Functions.Length + 1].Value = "SAB";
        }

        /// <summary>
        /// Metodo per scrivere i numeri all'interno del datagridview.
        /// </summary>
        /// <param name="t">Tabella da cui prendere i dati.</param>
        /// <param name="dg">DataGridView in cui scrivere.</param>
        private void writeTableNumbers(cTable t, DataGridView dg) {
            for (int r = 0; r < t.Functions.Length; r++) {
                //t.nVars per far si che riempisse tutte le caselle delle var decisionali
                //+1 per riempire quella dello scarto
                string v = null;
                if (t.Functions[r].Contains(">=") || t.Functions[r].Contains("=") && !t.Functions[r].Contains("<")) {
                    int index = t.Functions[r].IndexOf('<');
                    if (index < 0) index = t.Functions[r].IndexOf('>');
                    if (index < 0) index = t.Functions[r].IndexOf('=');

                    string tmp = t.Functions[r].Substring(0, index);
                    string[] tmpArray = Regex.Split(tmp, @"(x\d{1,})");
                    tmpArray = tmpArray.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
                    v = tmpArray[tmpArray.Length - 1];
                }
                for (int c = 0; c < t.nVariabili + 1; c++) {
                    if (c < t.nVariabili) {
                        
                        dg[c + 2, r].Value = t.coefficientTerms[r][c];

                        //Scrittura Cj
                        dg[c + 2, dg.RowCount - 2].Value = t.ZArray[c];
                    }
                    else { //Ho scritto tutti i coefficienti
                        //Scritto la B
                        
                            
                        dg[c + 2 + r, r].Value = t.coefficientTerms[r][c];

                        //Scrittura M
                        if (v != null) {
                            int columnIndex = getColumnByHeader(dg, v);
                            dg[columnIndex, dg.RowCount - 2].Value = "-M";
                        }
                        //c + 2 per il fatto delle prime due da non contare
                        //+ r per contare a che funzione sono arrivato
                        //quindi per arrivare all'nesimo scarto

                    }
                }
                if (t.nVariabiliArtificiali > 0) {
                    for(int c = (t.nVariabiliArtificiali - t.nVariabili) + 2; c < dg.ColumnCount - 1; c++) {
                        if (t.Functions[r].Contains(">=") || t.Functions[r].Contains("=") && !t.Functions[r].Contains("<")) {
                            dg[c, r].Value = t.coefficientTerms[r][c - t.nVariabili - 1];
                        }
                        else break;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo per scrivere la B. (Ultima colonna)
        /// </summary>
        /// <param name="t">Tabella da cui prendere i dati.</param>
        /// <param name="dg">DataGridView in cui scrivere.</param>
        private static void writeB(cTable t, DataGridView dg) {
            for (int r = 0; r < t.Vars.Count; r++) {
                dg[dg.Columns.Count - 1, r].Value = t.Vars[r][t.Vars[r].Length - 1];
            }
        }

        /// <summary>
        /// Metodo per scrivere le variabili di base.
        /// </summary>
        /// <param name="t">Tabella da cui prendere i dati.</param>
        /// <param name="dg"><DataGridView in cui scrivere.</param>
        private void writeCb(cTable t, DataGridView dg) {
            for (int r = 0; r < t.Functions.Length; r++) {
                string str = dg[1, r].Value.ToString();
                int column = getColumnByHeader(dg, str);
                int row = t.Functions.Length;
                dg[0, r].Value = dg[column, row].Value;
            }
        }

        /// <summary>
        /// Metodo per scrivere la SAB.
        /// </summary>
        /// <param name="dg">DataGridView in cui scrivere.</param>
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

        /// <summary>
        /// Calcola la nuova tabella con il metodo di Gauss.
        /// </summary>
        /// <param name="dg">DataGridView in cui scrivere.</param>
        /// <param name="pivot">Numero pivot.</param>
        /// <param name="colonnaPivot">Colonna del numero pivot.</param>
        /// <param name="rigaPivot">Riga del numero pivot.</param>
        private void calculateGauss(DataGridView dg, double pivot, int colonnaPivot, int rigaPivot) {
            for (int r = 0; r < dg.RowCount - 2; r++) {
                double coefficienteAnnulamento = -Convert.ToDouble(dg[colonnaPivot, r].Value);
                if (r != rigaPivot) {
                    dg[colonnaPivot, r].Value = 0;

                    for (int c = 0; c < dg.ColumnCount - 2; c++) {
                        if (c + 2 != colonnaPivot) {
                            dg[c + 2, r].Value =
                                (
                                    Convert.ToDouble(dg[c + 2, rigaPivot].Value) *
                                    coefficienteAnnulamento
                                )
                                + Convert.ToDouble(dg[c + 2, r].Value);
                        }
                    }
                }
            }

            //double[] column = getColumn(dg, colonnaPivot);
            //for (int i = 1; i < column.Length - 1; i++) { //1 -> per non contare il primo elemento che è la cj
            //    if (i - 1 != rigaPivot) {
            //        double coefficienteAnnullamento = -column[i];
            //        column[i] = 0;
            //        for (int j = 0; j < dg.RowCount - 2; j++) {
            //            double[] row = getRow(dg, j);
            //            for (int k = 0; k < row.Length; k++) {
            //                row[k] += getNumber(dg[k + 2, rigaPivot].Value.ToString()) * coefficienteAnnullamento;
            //            }
            //            writeRow(dg, row, j);
            //        }
            //    }
            //}
            
            //for(int i = 0; i < arrayScelta.Length; i++) {
            //    if (isLast) {
            //        arrayScelta[i] = table.Functions.Length - 2 - i;
            //    }
            //    else {
            //        arrayScelta[i] = 

            //    }
            //}
        }

        /// <summary>
        /// Metodo per determinare se è una variabile di base.
        /// </summary>
        /// <param name="dg">DataGridView da cui prendere i dati.</param>
        /// <param name="s">Variabile da verificare.</param>
        /// <returns></returns>
        private int isBaseVar(DataGridView dg, string s) {
            //Questo metodo prende le stringhe nella prima colonna
            //e le compara con la stringa richiesta
            for (int r = 0; r < dg.RowCount - 2; r++) {
                string tmp = dg[1, r].Value.ToString();
                if (tmp == s) return r;
            }
            return -1;
        }       

        /// <summary>
        /// Metodo per determinare la variabile entrante.
        /// </summary>
        /// <param name="dg">DataGridView da cui prendere i dati.</param>
        /// <returns></returns>
        private string enteringVar(DataGridView dg) {
            int dc = table.nVariabili;
            int select = table.nVariabiliArtificiali == 0 ? table.nVariabiliScarto - 1 : table.nVariabiliArtificiali - 1;
            double[] myArray = new double[select];
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
                (this.Controls["labelEntrante"] as Label).Text = "La variabile entrante è: x" + maxIndex.ToString();
                return "x" + maxIndex.ToString();
            }
            else { MessageBox.Show("Non è più possibile andare avanti."); return "fine"; }
        }

        /// <summary>
        /// Metodo per la determinazione della variabile uscente.
        /// </summary>
        /// <param name="dg">Datagridview da cui prendere i dati.</param>
        /// <param name="enteringVar">Variabile entrante.</param>
        /// <returns></returns>
        private string[] exitingVar(DataGridView dg, string enteringVar) {
            int dc = table.nVariabili;
            int select = table.nVariabiliArtificiali == 0 ? table.nVariabiliScarto - 1 : table.nVariabiliArtificiali - 1;
            double[] myArray = new double[select];
            for (int i = 0; i < myArray.Length; i++) { myArray[i] = double.MaxValue; }

            int columnIndex = getColumnByHeader(dg, enteringVar);
            for (int i = 0; i < table.Functions.Length; i++) {
                double[] colonna = getColumn(dg, dg.ColumnCount - 1);
                myArray[i] = getColumn(dg, dg.ColumnCount - 1)[i] / getNumber(dg[columnIndex, i].Value.ToString());
            }
            double minValue = myArray.Where(x=> x > 0).ToArray().Min();

            int minIndex = myArray.ToList().IndexOf(minValue);
            double pivot = getNumber(dg[columnIndex, minIndex].Value.ToString());
            (this.Controls["labelUscente"] as Label).Text =  "La variabile uscente è: " + dg[1, minIndex].Value.ToString();
            string[] returner = { dg[1, minIndex].Value.ToString(), pivot.ToString(), columnIndex.ToString(), minIndex.ToString() };
            if (minValue < -table.MaxNumber * 900) {
                returner = new string[5] { dg[1, minIndex].Value.ToString(), pivot.ToString(), columnIndex.ToString(), minIndex.ToString(), "delete" };
            }
            
            
            return returner;
        }

        /// <summary>
        /// Metodo per calcolare la nuova tabella risultato della variabile entrante e uscente.
        /// </summary>
        /// <param name="dg">Datagridview da cui prendere i dati.</param>
        /// <param name="enteringVar">Variabile entrante.</param>
        /// <param name="exitingVar">Variabile uscente.</param>
        /// <param name="pivot">Pivot</param>
        private void calculateNewTable(DataGridView dg, string enteringVar, string exitingVar, string pivot) {
            int rowIndex = editBaseVar(dg, enteringVar, exitingVar);
            if (rowIndex == -1) return;
            for (int i = 0; i < dg.ColumnCount - 2; i++) {
                dg[i + 2, rowIndex].Value = getNumber(dg[i + 2, rowIndex].Value.ToString()) / getNumber(pivot);
            }
        }

        /// <summary>
        /// Scrittura delle nuove variabili di base
        /// </summary>
        /// <param name="dg">Datagridview in cui scrivere.</param>
        /// <param name="enteringVar">Variabile entrante.</param>
        /// <param name="exitingVar">Variabile uscente.</param>
        /// <returns></returns>
        private int editBaseVar(DataGridView dg, string enteringVar, string exitingVar) {
            for (int i = 0; i < dg.RowCount; i++) {
                if (dg[1, i].Value.ToString() == exitingVar) { dg[1, i].Value = enteringVar; return i; }
            }
            return -1;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e) {
            showInsertingWindow();
            //label1.Dispose();
            //button2.Dispose();
            //Debug staff
            //string[] dbgF = new string[] { "x1+x2>=1", "5x1+x2<=5", "x1+4x2<=4" };
            //string dbgZ = "4x1+6x2";
            ////--------------------//
            //TextBox exerciseFunctions = getMiniTextBox("exerciseFunctions");
            //getMiniTextBox("exerciseZ").Text = dbgZ;
            //this.ActiveControl = exerciseFunctions;
            //for (int i = 0; i < dbgF.Length; i++) {
            //    exerciseFunctions.Text += dbgF[i] + Environment.NewLine;
            //}
        }



        private void nuovoSistemaDaGUIToolStripMenuItem_Click(object sender, EventArgs e) {
            showInsertingWindow();
        }

        private void nuovoSistemaDaFileToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult res = findFile.ShowDialog();
            if (res == DialogResult.OK || res == DialogResult.Yes) {
                string errore;
                table = new cTable(findFile.FileName, out errore);
                if (errore == "errore") {
                    table = Tables[(this.Controls["lstExercises"] as ListBox).SelectedIndex];
                    MessageBox.Show("Seleziona un file adatto a come scritto nella guida");
                }
                else {
                    if (errore != null) { MessageBox.Show(errore); return; }
                    Tables.Add(table);

                    if (existsControl("button2") && existsControl("label1") && existsControl("button3")) {
                        label1.Dispose();
                        button2.Dispose();
                        button3.Dispose();
                    }

                    //Se non c'è posiziona la Listbox
                    if (!existsControl("lstExercises")) {
                        placeListBox();
                        oldSelected = Tables[0].exName;
                    }
                    else changeIndex(Tables.Count - 1);


                    //rExpressions.Visible = label1.Visible = button1.Visible = txtZ.Visible = false;

                    //Crea la tabella
                    DataGridView dg;


                    if (existsControl("dg")) this.Controls["dg"].Dispose();
                    dg = writeDG(table);

                    if (!existsControl("bNext")) {
                        createControlsUnderDG(dg);
                    }
                    else { 
                        this.Controls["bNext"].Enabled = true;
                        this.Controls["bEnd"].Enabled = true;
                        this.Controls["labelUscente"].Text = "";
                        this.Controls["labelEntrante"].Text = "";
                        this.Controls["labelFine"].Text = ""; 
                    }

                    if (existsControl("insertingWindow") && existsControl("background")) {
                        this.Controls["insertingWindow"].Dispose();
                        this.Controls["background"].Dispose();
                    }
                    this.ActiveControl = dg;

                }
            }
        }

        /// <summary>
        /// Metodo per creare i bottoni e le label sotto il DataGridView.
        /// </summary>
        /// <param name="dg">DataGridView a cui fare riferimento.</param>
        private void createControlsUnderDG(DataGridView dg) {
            Button bNext, bEnd;
            bNext = new Button();
            bNext.Font = new Font("Calibri", 12, FontStyle.Regular);
            bNext.Name = "bNext";
            bNext.Text = ">";
            bNext.Size = new Size(bNext.Width, bNext.Height + 10);
            bNext.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) + 80, dg.Location.Y + dg.Height + 5);
            this.Controls.Add(bNext);
            bNext.Click += bNext_Click;

            bEnd = new Button();
            bEnd.Font = new Font("Calibri", 12, FontStyle.Regular);
            bEnd.Name = "bEnd";
            bEnd.Text = "Tenta di finire l'esercizio";
            bEnd.Size = new Size(175, bEnd.Height + 10);
            bEnd.Location = new Point(dg.Location.X + (this.Size.Width / 2 - dg.Location.X) - bEnd.Width + 75, dg.Location.Y + dg.Height + 5);
            bEnd.Enabled = true;
            this.Controls.Add(bEnd);
            bEnd.Click += bEnd_Click;

            Label labelEntrante = new Label();
            labelEntrante.Font = new Font("Calibri", 12, FontStyle.Regular);
            labelEntrante.AutoSize = true;
            labelEntrante.Name = "labelEntrante";
            labelEntrante.Text = "";
            labelEntrante.Location = new Point(dg.Location.X, bNext.Location.Y + 50);
            this.Controls.Add(labelEntrante);

            Label labelUscente = new Label();
            labelUscente.Font = new Font("Calibri", 12, FontStyle.Regular);
            labelUscente.AutoSize = true;
            labelUscente.Name = "labelUscente";
            labelUscente.Text = "";
            labelUscente.Location = new Point(labelEntrante.Location.X, labelEntrante.Location.Y + 20);
            this.Controls.Add(labelUscente);

            Label labelFine = new Label();
            labelFine.Font = new Font("Calibri", 12, FontStyle.Regular);
            labelFine.AutoSize = true;
            labelFine.Name = "labelFine";
            labelFine.Text = "";
            labelFine.Location = new Point(labelEntrante.Location.X, labelUscente.Location.Y + 20);
            this.Controls.Add(labelFine);
        }

        void bEnd_Click(object sender, EventArgs e) {
            string entering = "";
            int index = 1;
            while (entering != "fine" && index % 10 != 0) {
                DataGridView dg = getDG();
                entering = enteringVar(dg);
                if (entering == "fine") { 
                    this.Controls["bNext"].Enabled = this.Controls["bEnd"].Enabled = false;
                    this.Controls["labelFine"].Text = "Il risultato finale è uguale a: " + calcRis(dg).ToString();
                    return; 
                }

                string[] exiting = exitingVar(dg, entering);
                if (isThereM(dg, getColumnByHeader(dg, exiting[0])))
                    dg.Columns.RemoveAt(getColumnByHeader(dg, exiting[0]));
                calculateNewTable(dg, entering, exiting[0], exiting[1]);
                calculateGauss(dg, Convert.ToDouble(exiting[1]), Convert.ToInt32(exiting[2]), Convert.ToInt32(exiting[3]));
                writeCb(table, dg);
                writeSab(dg);

                index++;
                this.Controls["dg"].Refresh(); 
            }
            if (index % 10 == 0) MessageBox.Show("Mi sono fermato perché sto andando molto avanti...");
        }

        private void button3_Click(object sender, EventArgs e) {
            DialogResult res = findFile.ShowDialog();
            if (res == DialogResult.OK || res == DialogResult.Yes) {
                string errore;
                table = new cTable(findFile.FileName, out errore);
                if (errore == "errore") {
                    if(existsControl("lstExercises")) table = Tables[(this.Controls["lstExercises"] as ListBox).SelectedIndex];
                    MessageBox.Show("Seleziona un file adatto a come scritto nella guida");
                }
                else {
                    if (errore != null) 
                    { 
                        MessageBox.Show(errore); 
                        return; 
                    }
                    Tables.Add(table);

                    if (existsControl("button2") && existsControl("label1") && existsControl("button3")) {
                        label1.Dispose();
                        button2.Dispose();
                        button3.Dispose();
                    }

                    //Se non c'è posiziona la Listbox
                    if (!existsControl("lstExercises")) {
                        placeListBox();
                        oldSelected = Tables[0].exName;
                    }
                    else changeIndex(Tables.Count - 1);


                    //rExpressions.Visible = label1.Visible = button1.Visible = txtZ.Visible = false;

                    //Crea la tabella
                    DataGridView dg;


                    if (existsControl("dg")) this.Controls["dg"].Dispose();
                    dg = writeDG(table);

                    if (!existsControl("bNext")) {
                        createControlsUnderDG(dg);
                    }
                    if (existsControl("insertingWindow") && existsControl("background")) {
                        this.Controls["insertingWindow"].Dispose();
                        this.Controls["background"].Dispose();
                    }
                    this.ActiveControl = dg;

                }
            }
        }

        private void aiutoToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("Guida.pdf");
        }
    }
}
