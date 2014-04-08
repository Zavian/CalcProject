using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

//A questa classe vengono passate solo stringhe già controllate
//e funzionanti

namespace CalcProject {
    class cTable {

        public string exName { get { return nomeEsercizio; } }


        /// <summary>
        /// Ricevi l'array degli elementi della Z
        /// </summary>
        public int[] ZArray { get { 
                //Da togliere le X
            string[] elemZ = Regex.Split(sFunzioneZ, "([-|\\+]{0,1}\\d{0,}x\\d{1,})");
                elemZ = elemZ.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
                for (int i = 0; i < elemZ.Length; i++) {
                    elemZ[i] = elemZ[i].Remove(elemZ[i].LastIndexOf('x'));
                }
                int[] returner = new int[elemZ.Length];
                for (int i = 0; i < elemZ.Length; i++) returner[i] = Convert.ToInt32(elemZ[i]);
                return returner;
            } 
        }
        
        /// <summary>
        /// Ricevi il numero delle variabili decisionali
        /// </summary>
        public int nVariabili { get { return iBMax; } }
        /// <summary>
        /// Ricevi il numero delle variabili totali
        /// </summary>
        public int nVariabiliScarto { get { return iSMax; } }
        
        /// <summary>
        /// Ricevi la lista dei coefficienti
        /// </summary>
        public List<String[]> coefficientTerms { get { return sNumbers; } }
        /// <summary>
        /// Ricevi la lista delle variabili decisionali
        /// </summary>
        public List<String[]> Vars { get { return sVars; } }
        /// <summary>
        /// Ricevi l'array di stringhe delle funzioni
        /// </summary>
        public string[] Functions { get { return sFunzioni; } }
        
         string nomeEsercizio = "";
         string problema = ""; //min|max
        
        string sFunzioneZ;
        string[] sFunzioni;
       
        int iBMax = 0;
        int iSMax = 0;
        List<String[]> sVars = new List<string[]>();
        List<String[]> sNumbers = new List<String[]>();
        
        
        //public cTable(string file) {
        //    using(StreamReader s = new StreamReader(File)) {
        //        //nome es
        //        //max|min
        //        //coefficienti Z divisi da ;
        //        //vincoli coi coefficienti divisi ;
        //        //ultima riga #
                
        //        //Esempio:
        //        //Es Pippo
        //        //Max
        //        //3; 4; 6 x1 -> x3
        //        //3; 4; >=; 450
        //        //#
                
        //        string line = "";
        //        int counter = 0;
        //        while((line = file.ReadLine()) != null)
        //        {
        //           switch(counter) {
        //               default: throw new System.ArgumentException(
        //                   "Seguire il tutorial (F1) per la generazione del file"
        //                   );
        //                   break;
                           
        //               case 0: nomeEsercizio = line; break;
        //               case 1: 
        //                    line = line.ToLowerCase();
        //                    if(line == "min" || line == "minimo" || line == "max" || line == "massimo") {
                                
        //                    }
                       
        //           }
        //           counter++;
        //        }
        //    }
        //}

        /// <summary>
        /// Inizializzazione oggetto
        /// </summary>
        /// <param name="Z">Stringa Z</param>
        /// <param name="sFunzioni">Insieme delle funzioni del sistema</param>
        /// <param name="exName">Nome dell'esercizio verrà visualizzato</param>
        public cTable(string exName, string Z, string[] sFunzioni) {
            this.nomeEsercizio = exName;
            this.sFunzioni = sFunzioni;
            this.sFunzioneZ = Z;
            //Lettura della Z
            string[] elemZ = Regex.Split(sFunzioneZ, "([-|\\+]{0,1}\\d{0,}x\\d{1,})");
            elemZ = elemZ.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //<-- Cancella le celle vuote
            iBMax = elemZ.Length;
            

            //sFunzioni[0] = 3x1-3x2-4x3
            //sVars[0] = {3x1, -3x2, -4x3}
            for (int i = 0; i < sFunzioni.Length; i++) {
                //http://regex101.com/

                //Selezione dei termini singoli della funzione
                string[] tmp = Regex.Split(sFunzioni[i], "([-|\\+]{0,1}\\d{0,}x\\d{1,})");
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

            #region #DEPRECATED
            //Prendo il massimo di variabili di base nel sistema
            //sVars[i].Length - 1 perché altrimenti conta la parte destra del sistema
            //for (int i = 0; i < sVars.Count; i++) { 
            //    iBMax = Math.Max(sVars[i].Length - 1, iBMax); 
            //} 
            #endregion

            for (int i = 0; i < sFunzioni.Length; i++) {
                string tmp = setScarto(sFunzioni[i]);
                if (tmp == "Errore") return; //Da creare gestione errore
                else sFunzioni[i] = tmp;
            }

            
        }
        
        /// <summary>
        /// Imposta la funzione selezionata con lo scarto
        /// </summary>
        /// <param name="sFunzione">Funzione a cui aggiungere la variabile di scarto</param>
        private string setScarto(string sFunzione) {
            if (sFunzione.Contains("=") && !sFunzione.Contains("<=")) return sFunzione;


            iSMax = iSMax == 0 ? iBMax + 1 : iSMax+1;
            if (sFunzione.Contains("<=")) {
                int index = sFunzione.IndexOf('<');
                index = index--;
                string s = sFunzione.Insert(index, "+x" + iSMax);
                return s.Replace("<=", "=");            
            }
            return "Errore";
        }
    }
}
