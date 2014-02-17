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
        }

        private void button1_Click(object sender, EventArgs e) {
            string[] s = new string[] { "3x1-3x2-4x3<=360" };
            cTable t = new cTable(s);

            for (int i = 0; i < t.Numbers.Count; i++) {
                string tmp = "";
                for (int j = 0; j < t.Numbers[i].Length; j++) {
                    tmp += t.Numbers[i][j] + " ";
                }
                rExpressions.Text += tmp;
            }
            rExpressions.Text += "\n";

            for (int i = 0; i < t.Functions.Length; i++) {
                rExpressions.Text += t.Functions[i];
            }

            
        }
    }
}
