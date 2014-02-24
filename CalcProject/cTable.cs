using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

//A questa classe vengono passate solo stringhe già controllate
//e funzionanti

namespace CalcProject {
    class cTable {
        public int nVars { get { return iBMax; } }
        public int nSVars { get { return iSMax; } }
        

        public List<String[]> coefficientTerms { get { return sNumbers; } }

        public string[] Functions { get { return sFunzioni; } }

        string[] sFunzioni;
        int iBMax = 0;
        int iSMax = 0;
        List<String[]> sVars = new List<string[]>();
        List<String[]> sNumbers = new List<String[]>();
        public cTable(string Z, string[] sFunzioni) {
            this.sFunzioni = sFunzioni;

            //sFunzioni[0] = 3x1-3x2-4x3
            //sVars[0] = {3x1, -3x2, -4x3}
            for (int i = 0; i < sFunzioni.Length; i++) {
                //http://regex101.com/

                //Selezione dei termini singoli della funzione
                string[] tmp = Regex.Split(sFunzioni[i], "([-|\\+]{0,1}\\d{0,}x\\d{1,})");
                tmp = tmp.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                tmp[tmp.Length - 1] = tmp[tmp.Length - 1].Replace("=", "");
                tmp[tmp.Length - 1] = tmp[tmp.Length - 1].Replace("<", "");
                tmp[tmp.Length - 1] = tmp[tmp.Length - 1].Replace(">", "");
                sVars.Add(tmp);

                
                //Selezione dei termini noti della funzione
                //tmp1 = variabile finale
                for (int j = 0; j < tmp.Length; j++) {
                    if (!tmp[j].Contains("<=")) //Questo blocca il ciclo a fine espressione sinistra
                    {
                        tmp[j] = tmp[j].Remove(tmp[j].LastIndexOf('x'));
                    }
                }
                sNumbers.Add(tmp);
            }

            //Prendo il massimo di variabili di base nel sistema
            //sVars[i].Length - 1 perché altrimenti conta la parte destra del sistema
            for (int i = 0; i < sVars.Count; i++) { 
                iBMax = Math.Max(sVars[i].Length - 1, iBMax); 
            }

            for (int i = 0; i < sFunzioni.Length; i++) {
                string tmp = setScarto(sFunzioni[i]);
                if (tmp == "Errore") return; //Da creare gestione errore
                else sFunzioni[i] = tmp;
            }


            
        }

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
