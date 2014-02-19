﻿using System;
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
            string[] s = new string[] { "3x1-3x2-4x3<=360", "2x1+3x2-4x3<=100" };
            cTable t = new cTable(txtZ.Text, s);
            rExpressions.Visible = label1.Visible = label2.Visible =  button1.Visible = false;


            DataGridView dg = new DataGridView();
            //CB, Base, Var decisionali + Var scarto, B
            dg.ColumnCount = 3 + t.nSVars;
            dg.RowCount = t.Functions.Length + 1;
            dg.Location = new Point(rExpressions.Location.X - 5, rExpressions.Location.Y);
            dg.Size = new Size(rExpressions.Size.Width, rExpressions.Size.Height);
            this.Controls.Add(dg);

            for (int i = 0; i < t.Functions.Length - 1; i++) {
                dg[0, i].Value = 0;
                dg[1, i].Value = "x" + t.Functions.Length + 1 + i;
            }


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
            rExpressions.Dispose();
            label1.Dispose();
            label2.Dispose();
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