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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Random rng;

        Graphics g;

        Bitmap canvas;
        Graphics b;

        List<Color> caveColors = new List<Color>();

        private void MainForm_Load(object sender, EventArgs e)
        {
            rng = new Random();
            /*
            map = new Tile[width, height];

            previewPanel.Width = width;
            previewPanel.Height = height;
            */

            Generator.initialize(this);

            g = previewPanel.CreateGraphics();
            canvas = new Bitmap(previewPanel.Width, previewPanel.Height);
            b = Graphics.FromImage(canvas);

        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            Generator.generate();
        }

        public void scalePanel(int w, int h)
        {
            previewPanel.Width = w;
            previewPanel.Height = h;
        }

        public void refreshPanel()
        {
            previewPanel.Invalidate();            
        }

        public bool safe(int x, int y)
        {
            return (x >= 0 && x < previewPanel.Width && y >= 0 && y < previewPanel.Height);
        }

        private void previewPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!Generator.generated)
                return;

            int width = previewPanel.Width;
            int height = previewPanel.Height;

            for(int k = 0; k <= Generator.caves; k++)
            {
                caveColors.Add(Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255)));
            }

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Rectangle drawRect = new Rectangle(i, j, 1, 1);

                    if (Generator.map[i, j].wall)
                    {
                        if (Generator.map[i, j].edge)
                            b.DrawRectangle(Pens.White, drawRect);
                        else
                            b.DrawRectangle(Pens.Black, drawRect);
                    }
                    else
                    {
                        b.DrawRectangle(new Pen(caveColors[Generator.map[i, j].caveId], 1), drawRect);
                    }
                    if (Generator.map[i, j].tunnel)
                        b.DrawRectangle(Pens.White, drawRect);

                }
            }

            g.DrawImage(canvas, new Point(0, 0));

        }

        private void previewPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetImage(canvas);
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            /*
            List<Vector3> vertices = new List<Vector3>();
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    int surroundingCt = 0;
                    for(int m = -1; m <= 1; m++)
                    {
                        for(int n = -1; n <= 1; n++)
                        {
                            if(safe(i + m, j + n) && map[i + m, j + n].wall && m != n)
                            {
                                surroundingCt++;
                            }
                        }
                    }
                    if(surroundingCt < 6 && map[i, j].wall)
                    {
                        vertices.Add(new Vector3(i, 0, j));
                        //vertices.Add(new Vector3(i, 1, j));
                    }
                }
            }

            string verticesOutput = "cavePoints = new Vector3[]\n{\n";
            for(int k = 0; k < vertices.Count; k++)
            {
                verticesOutput += "new Vector3(" + vertices[k].x + ", " + vertices[k].y + ", " + vertices[k].z + "),\n";
            }
            verticesOutput += "};\n\n";

            Clipboard.SetText(verticesOutput);
            */

        }

        private void extrapolateButton_Click(object sender, EventArgs e)
        {
            ExtrapolateForm extrapolator = new ExtrapolateForm();
            extrapolator.Show();
        }
    }
}
