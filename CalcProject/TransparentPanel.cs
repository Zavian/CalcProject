using System;
using System.Drawing;
using System.Windows.Forms;


/* DISLAMER
 * Questa è una classe che ho trovato in internet
 * Semplicemente essa va a modificare l'OnPaint del panel in modo tale che diventi
 * trasparente.
 * http://bobpowell.net/transcontrols.aspx
*/

namespace CalcProject {
    public class TransPanel : Panel {
        int pWidth;
        int pHeight;
        Color c;


        /// <summary>
        /// Crea un pannello che permette la trasparenza.
        /// </summary>
        /// <param name="Width">Larghezza del pannello</param>
        /// <param name="Height">Altezza del pannello</param>
        /// <param name="RGBColor">Colore RGB da applicare al pannello</param>
        public TransPanel(int Width, int Height, Color RGBColor) {
            c = RGBColor;
            pWidth = Width;
            pHeight = Height;
        }

        protected void TickHandler(object sender, EventArgs e) {
            this.InvalidateEx();
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected void InvalidateEx() {
            if (Parent == null)
                return;

            Rectangle rc = new Rectangle(this.Location, this.Size);
            Parent.Invalidate(rc, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {
            //do not allow the background to be painted 
        }

        Random r = new Random();

        protected override void OnPaint(PaintEventArgs e) {
            Rectangle rect = new Rectangle(0, 0, pWidth, pHeight);
            Brush b = new SolidBrush(c);
            e.Graphics.FillRectangle(b, rect);
            b.Dispose();
        }
    }

}
