using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;


namespace CalcProject {
    class cTable {

        /// <summary>
        /// Metodo per negare la funzione.
        /// </summary>
        /// <param name="t">Funzione da negare</param>
        /// <returns></returns>
        private string negateZ(string t) {
            string tmp = "";
            if (t[0] != '+' && t[0] != '-') tmp += '-';
            for (int i = 0; i < t.Length; i++) {
                if (t[i] == '-') tmp += '+';
                else if (t[i] == '+') tmp += '-';
                else tmp += t[i];
            }
            return tmp;
        }

        /// <summary>
        /// Metodo per controllare se la stringa contiene numeri.
        /// </summary>
        /// <param name="t">Stringa da controllare.</param>
        /// <returns></returns>
        bool containsNumbers(string t) {
            for (int i = 0; i < t.Length; i++) {
                if (t[i] >= '0' && t[i] <= '9') return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo per controllare se ci sono solo X all'interno della stringa.
        /// </summary>
        bool onlyX(string t) {
            int counter = 0;
            for (int i = 0; i < t.Length; i++) {
                bool isNumber = t[i] >= '0' && t[i] <= '9' || t[i] == '-' || t[i] == '+' || t[i] == '=' || t[i] == '<' || t[i] == '>';
                if (!isNumber) { 
                    if (t[i] != 'x') return false; 
                    else counter += 1; 
                }
            }
            return counter > 0;
        }

        /// <summary>
        /// Metodo per controllare se ci sono numeri consecutivi all'interno della stringa.
        /// </summary>
        /// <param name="t">Stringa di richiesta.</param>
        /// <returns></returns>
        bool consecutiveNumbers(string t) {
            List<int> tmp = new List<int>();
            for (int i = 0; i < t.Length; i++) {
                if (t[i] == 'x') tmp.Add(Convert.ToInt32(t[i + 1]) - 48);
            }

            //Input = { 5, 6, 7, 8 }
            //Select restituisce { (5-0=)5, (6-1=)5, (7-2=)5, (8-3=)5 }
            //Distinct restituisce { 5 }
            //Skip restituisce { (5 skipped, nothing left) }
            //Any ritorna false
            if (tmp[0] != 1) return false;
            return !tmp.Select((i, j) => i - j).Distinct().Skip(1).Any();
        }


        /// <summary>
        /// Ricevi il nome dell'esercizio.
        /// </summary>
        public string exName { get { return nomeEsercizio; } }

        /// <summary>
        /// Numero massimo all'interno della tabella.
        /// </summary>
        public double MaxNumber { get {
            double[] myArray = new double[sNumbers.Count];
            for (int i = 0; i < sNumbers.Count(); i++)
			{
                double[] tmp = new double[sNumbers[i].Length];
                for (int j = 0; j < sNumbers[i].Length; j++) {
                    tmp[j] = Convert.ToDouble(sNumbers[i][j]);
                }
                myArray[i] = tmp.Max();
			}
            return myArray.Max();
        }
            
        }

        /// <summary>
        /// Ricevi l'array degli elementi della Z.
        /// </summary>
        public double[] ZArray { get { 
                //Da togliere le X
            string[] elemZ = Regex.Split(sFunzioneZ, @"([-|\+]{0,1}\d*\,?\d{0,}x\d*\d{1,})");
                elemZ = elemZ.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
                for (int i = 0; i < elemZ.Length; i++) {
                    elemZ[i] = elemZ[i].Remove(elemZ[i].LastIndexOf('x'));
                }
                double[] returner = new double[elemZ.Length];
                for (int i = 0; i < elemZ.Length; i++) returner[i] = Convert.ToDouble(elemZ[i]);
                return returner;
            } 
        }
        
        /// <summary>
        /// Ricevi il numero delle variabili decisionali.
        /// </summary>
        public int nVariabili { get { return iBMax; } }
        /// <summary>
        /// Ricevi il numero delle variabili totali.
        /// </summary>
        public int nVariabiliScarto { get { return iSMax; } }

        // <summary>
        /// Ricevi il numero delle variabili totali.
        /// </summary>
        public int nVariabiliArtificiali { get { return iAMax; } }

        /// <summary>
        /// Ricevi la lista dei coefficienti.
        /// </summary>
        public List<String[]> coefficientTerms { get { return sNumbers; } }
        /// <summary>
        /// Ricevi la lista delle variabili decisionali.
        /// </summary>
        public List<String[]> Vars { get { return sVars; } }
        /// <summary>
        /// Ricevi l'array di stringhe delle funzioni.
        /// </summary>
        public string[] Functions { get { return sFunzioni; } }

        

        string nomeEsercizio = "";
        string problema = ""; //min|max
        
        string sFunzioneZ;
        string[] sFunzioni;

        int tIndex = 0;
        int iBMax = 0;
        int iSMax = 0;
        int iAMax = 0;
        List<String[]> sVars = new List<string[]>();
        List<String[]> sNumbers = new List<String[]>();


        public cTable(string file, out string errore) {
            
            using (StreamReader s = new StreamReader(file)) {
                List<string> funzioni = new List<string>();
                errore = null;
                //nome es
                //max|min
                //coefficienti Z divisi da ;
                //vincoli coi coefficienti divisi ;
                //ultima riga #

                //Esempio:
                //Es Pippo
                //max
                //3; 4; 6 x1 -> x3
                //3; 4; >=; 450
                //#

                string line = "";
                string tmpZ = "";
                int counter = 0;
                #region Lettura del file
                while ((line = s.ReadLine()) != null) {
                    switch (counter) {
                        default:
                            if (containsNumbers(line) && line.Contains('=')) {
                                funzioni.Add(line);
                            }
                            else {
                                if (line == "#") break;
                                else { errore = "errore"; return; }
                            }
                            break;

                        case 0: nomeEsercizio = line; break;
                        case 1:
                            line = line.ToLower();
                            if (line == "min" || line == "minimo" || line == "max" || line == "massimo") {
                                line = line == "minimo" ? "min" :
                                    line == "massimo" ? "max" : 
                                    line;
                                problema = line;
                            }
                            else { errore = "errore"; return; }
                            break;

                        case 2:
                            line = line.ToLower();
                            if (!line.Contains('<') && !line.Contains('>') && !line.Contains('=') && containsNumbers(line)) {
                                tmpZ = line;
                            }
                            else { errore = "errore"; return; }
                            break;

                        case 3:
                            line = line.ToLower();
                            if (containsNumbers(line) && line.Contains('=')) {
                                funzioni.Add(line);
                            }
                            else { errore = "errore"; return; }
                            break;
                    }
                    counter++;
                }
                #endregion

                sFunzioneZ = "";
                int index = 1;
                string tmp = "";
                string[] tmpArrayZ = tmpZ.Split(';');
                for (int i = 0; i < tmpArrayZ.Length; i++) {
                    tmpArrayZ[i] = tmpArrayZ[i].Replace('.', ',');
                    if (containsNumbers(tmpArrayZ[i])) {
                        if (i < tmpArrayZ.Length - 1) {
                            tmp += tmpArrayZ[i] + "x" + index;
                            if (tmpArrayZ[i + 1][0] != '-') tmp += "+";
                        }
                        else tmp += tmpArrayZ[i] + "x" + index;
                    }
                    else {
                        errore = "errore";
                        return;
                    }
                    index = index + 1;
                }
                sFunzioneZ = tmp;

                if (problema == "min") sFunzioneZ = negateZ(sFunzioneZ);

                sFunzioni = new string[funzioni.Count];
                int j = 0;
                foreach (var item in funzioni) {
                    index = 1;
                    string[] tmpArray = item.Split(';');
                    if (tmpArray.Length - 2 > tmpArrayZ.Length) { errore = "errore"; return; }

                    tmp = "";
                    for (int i = 0; i < tmpArray.Length; i++) {
                        tmpArray[i] = tmpArray[i].Replace(".", ",");
                        if (containsNumbers(tmpArray[i]) && i < tmpArray.Length - 1) {
                            if (i < tmpArray.Length - 3) {
                                tmp += tmpArray[i] + "x" + index;
                                if (tmpArray[i + 1][0] != '-') tmp += "+";
                            }
                            else tmp += tmpArray[i] + "x" + index;
                        }
                        else {
                            tmp += tmpArray[i];
                        }
                        index = index + 1;
                    }
                    sFunzioni[j] = tmp;
                    j = j + 1;
                }

                string[] elemZ = Regex.Split(sFunzioneZ, @"([-|\+]{0,1}\d*\,?\d{0,}x\d*\d{1,})");
                elemZ = elemZ.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
                iBMax = elemZ.Length;
                iSMax = iBMax + 1;
                for (int i = 0; i < sFunzioni.Length; i++) {
                    tmp = "";
                    if (sFunzioni[i].Contains("<="))
                        tmp = setScarto(sFunzioni[i]);
                    else if (sFunzioni[i].Contains(">=")) {
                        tmp = setScarto(sFunzioni[i]);
                    }
                    sFunzioni[i] = tmp;
                }

                bool fareArt = false;
                for (int i = 0; i < sFunzioni.Length; i++) {
                    if (sFunzioni[i].Contains('>')) { fareArt = true; break; }
                    if (sFunzioni[i].Contains('=') && !sFunzioni[i].Contains('>') && !sFunzioni[i].Contains('<')) { fareArt = true; break; }
                }

                if (fareArt) {
                    iAMax = iSMax;
                    for (int i = 0; i < sFunzioni.Length; i++) {
                        tmp = "";
                        tmp = setArtific(sFunzioni[i]);
                        sFunzioni[i] = tmp;
                    }

                }

                for (int i = 0; i < sFunzioni.Length; i++) {
                    //http://regex101.com/

                    //Selezione dei termini singoli della funzione
                    string[] tmpArray = Regex.Split(sFunzioni[i], @"([-|\+]{0,1}\d*\,?\d{0,}x\d*\d{1,})");
                    tmpArray = tmpArray.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
                    tmpArray[tmpArray.Length - 1] = tmpArray[tmpArray.Length - 1].Replace("=", "");
                    tmpArray[tmpArray.Length - 1] = tmpArray[tmpArray.Length - 1].Replace("<", "");
                    tmpArray[tmpArray.Length - 1] = tmpArray[tmpArray.Length - 1].Replace(">", "");
                    sVars.Add(tmpArray);


                    //Selezione dei termini noti della funzione
                    //tmp1 = variabile finale
                    for (j = 0; j < tmpArray.Length - 1; j++) {
                        tmpArray[j] = tmpArray[j].Remove(tmpArray[j].LastIndexOf('x'));
                    }
                    sNumbers.Add(tmpArray);
                }

            }
        }

        /// <summary>
        /// Inizializzazione oggetto
        /// </summary>
        /// <param name="Z">Stringa Z</param>
        /// <param name="sFunzioni">Insieme delle funzioni del sistema</param>
        /// <param name="exName">Nome dell'esercizio verrà visualizzato</param>
        public cTable(string exName, string Z, string[] sFunzioni, string problema, out string error) {
            error = null;


            //Questi if per gestire eventuali cavolate nell'inserimento dei dati
            #region Gestione errore
            
            string tut = "\nFare riferimento al tutorial";
            if (!containsNumbers(Z) || !onlyX(Z)) { error = ("Errore nell'inserimento della Z" + tut); return; }
            if (!consecutiveNumbers(Z)) { error = ("Errore nell'ordine delle variabili della Z" + tut); return; }

            sFunzioni = sFunzioni.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for(int i = 0; i < sFunzioni.Length; i++) {
                if (!containsNumbers(sFunzioni[i]) || !onlyX(sFunzioni[i]))
                    { error = ("Errore nell'inserimento delle funzioni" + tut); return; }
                if (!consecutiveNumbers(sFunzioni[i]))
                    { error = ("Errore nell'ordine delle variabili delle funzioni" + tut); return; }
            }
            #endregion

            //Manipolazione delle variabili
            //per correggere eventuali baggianate che fa l'utonto
            Z = Z.Trim();
            Z = Z.Replace('.', ',');

            sFunzioni = sFunzioni.Select(x => x.Trim()).ToArray(); //Trimma tutto
            for (int i = 0; i < sFunzioni.Length; i++) {
                sFunzioni[i] = sFunzioni[i].Replace('.', ',');
            }
            

            this.nomeEsercizio = exName;
            this.sFunzioni = sFunzioni;
            this.sFunzioneZ = Z;
            problema = this.problema;
            if (problema == "min") this.sFunzioneZ = negateZ(this.sFunzioneZ);
            //Lettura della Z
            //http://regex101.com/r/aM4wZ6/#debugger
            string[] elemZ = Regex.Split(sFunzioneZ, @"([-|\+]{0,1}\d*\,?\d{0,}x\d*\d{1,})");
            elemZ = elemZ.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
            iBMax = elemZ.Length;


            //sFunzioni[0] = 3x1-3x2-4x3
            //sVars[0] = {3x1, -3x2, -4x3}

           

            #region #DEPRECATED
            //Prendo il massimo di variabili di base nel sistema
            //sVars[i].Length - 1 perché altrimenti conta la parte destra del sistema
            //for (int i = 0; i < sVars.Count; i++) { 
            //    iBMax = Math.Max(sVars[i].Length - 1, iBMax); 
            //} 
            #endregion

            iSMax = iBMax + 1;
            for (int i = 0; i < sFunzioni.Length; i++) {
                string tmp = "";
                if (sFunzioni[i].Contains("<="))
                    tmp = setScarto(sFunzioni[i]);
                else if (sFunzioni[i].Contains(">=")) {
                    tmp = setScarto(sFunzioni[i]);
                }
                sFunzioni[i] = tmp;
            }

            bool fareArt = false;
            for (int i = 0; i < sFunzioni.Length; i++) {
                if (sFunzioni[i].Contains('>')) { fareArt = true; break; }
                if (sFunzioni[i].Contains('=') && !sFunzioni[i].Contains('>') && !sFunzioni[i].Contains('<')) { fareArt = true; break; }
            }

            if (fareArt) {
                iAMax = iSMax;
                for (int i = 0; i < sFunzioni.Length; i++) {
                    string tmp = "";
                    tmp = setArtific(sFunzioni[i]);
                    sFunzioni[i] = tmp;
                }
                
            }

            for (int i = 0; i < sFunzioni.Length; i++) {
                //http://regex101.com/

                //Selezione dei termini singoli della funzione
                string[] tmp = Regex.Split(sFunzioni[i], @"([-|\+]{0,1}\d*\,?\d{0,}x\d*\d{1,})");
                tmp = tmp.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
                tmp[tmp.Length - 1] = tmp[tmp.Length - 1].Replace("=", "");
                tmp[tmp.Length - 1] = tmp[tmp.Length - 1].Replace("<", "");
                tmp[tmp.Length - 1] = tmp[tmp.Length - 1].Replace(">", "");
                sVars.Add(tmp);


                //Selezione dei termini noti della funzione
                //tmp1 = variabile finale
                for (int j = 0; j < tmp.Length - 1; j++) {
                    tmp[j] = tmp[j].Remove(tmp[j].LastIndexOf('x'));
                }
                sNumbers.Add(tmp);
            }

            
        }
        
        /// <summary>
        /// Imposta la funzione selezionata con lo scarto.
        /// </summary>
        /// <param name="sFunzione">Funzione a cui aggiungere la variabile di scarto.</param>
        private string setScarto(string sFunzione) {
            if (sFunzione.Contains('<') || sFunzione.Contains('>')) {
                int index = sFunzione.IndexOf('>');
                if (index < 0) index = sFunzione.IndexOf('<');
                //index -= 1;

                string s = "";
                if (sFunzione.Contains('>')) s = sFunzione.Insert(index, "-1x" + iSMax);
                else if (sFunzione.Contains('<')) s = sFunzione.Insert(index, "+1x" + iSMax);
                iSMax += 1;
                return s;
            }
            else return sFunzione;
        }

        private string setArtific(string sFunzione)
        {
            if ((sFunzione.Contains('>') || sFunzione.Contains('=')) && !sFunzione.Contains('<')){
                int index = sFunzione.IndexOf('>');
                if (index < 0) index = sFunzione.IndexOf('=');
                //index -= 1;
                string s = sFunzione.Insert(index, "+1x" + iAMax);
                iAMax += 1;
                return s;
            }
            else return sFunzione;
        }
    }
}
