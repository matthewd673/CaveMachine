using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaveMachine
{
    public partial class ExtrapolateForm : Form
    {
        public ExtrapolateForm()
        {
            InitializeComponent();
        }

        public int width = 50;
        public int height = 50;

        public int scaledW = 1;
        public int scaledH = 1;

        Graphics g;

        Bitmap canvas;
        Graphics b;

        private void ExtrapolateForm_Load(object sender, EventArgs e)
        {

            Extrapolator.initialize(this);

            width = Generator.width;
            height = Generator.height;

            scaledW = width * Extrapolator.scale;
            scaledH = height * Extrapolator.scale;

            previewPanel.Width = scaledW;
            previewPanel.Height = scaledH;

            g = previewPanel.CreateGraphics();
            canvas = new Bitmap(previewPanel.Width, previewPanel.Height);
            b = Graphics.FromImage(canvas);

            Extrapolator.connectPoints();

        }

        public void refreshPanel()
        {
            previewPanel.Invalidate();
        }

        private void previewPanel_Paint(object sender, PaintEventArgs e)
        {

            for(int i = 0; i < scaledW; i++)
            {
                for(int j = 0; j < scaledH; j++)
                {
                    Rectangle drawRect = new Rectangle(i, j, 1, 1);

                    if (Extrapolator.sMap[i, j].wall)
                        b.DrawRectangle(Pens.Black, drawRect);
                }
            }

            g.DrawImage(canvas, new Point(0, 0));

        }
    }
}
