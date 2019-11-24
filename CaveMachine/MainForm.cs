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

        /*
        int width = 200;
        int height = 100;
        public Tile[,] map;

        int refine = 5;

        int caves = 0;

        Dictionary<int, int> caveArea = new Dictionary<int, int>();
        Dictionary<int, int> caveConnections = new Dictionary<int, int>();

        bool generated = false;
        */

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

        /*
        public void fillCave(FillRule rule)
        {
            int x = rule.x;
            int y = rule.y;
            int caveId = rule.id;
            if (safe(x, y))
            {
                if (!map[x, y].wall && map[x, y].caveId != caveId)
                {
                    map[x, y].caveId = caveId;
                    caveArea[caveId]++;

                    fillQueue.Add(new FillRule(x + 1, y, caveId));
                    fillQueue.Add(new FillRule(x - 1, y, caveId));
                    fillQueue.Add(new FillRule(x, y + 1, caveId));
                    fillQueue.Add(new FillRule(x, y - 1, caveId));
                }
            }
            fillQueue.RemoveAt(0);

        }

        public void findTunnel(int aCave, int bCave)
        {
            List<Vector2> aCheck = new List<Vector2>();
            List<Vector2> bCheck = new List<Vector2>();

            int bestDistance = width + height + 1;
            Vector2 aNear = new Vector2(0, 0);
            Vector2 bNear = new Vector2(0, 0);

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if (map[i, j].caveId == aCave && map[i, j].edge)
                        aCheck.Add(new Vector2(i, j));
                    if (map[i, j].caveId == bCave && map[i, j].edge)
                        bCheck.Add(new Vector2(i, j));
                }
            }

            foreach(Vector2 aPoint in aCheck)
            {
                foreach(Vector2 bPoint in bCheck)
                {
                    int manDistance = Math.Abs(aPoint.x - bPoint.x) + Math.Abs(aPoint.y - bPoint.y);
                    if(manDistance < bestDistance)
                    {
                        bestDistance = manDistance;
                        aNear = aPoint;
                        bNear = bPoint;
                    }
                }
            }

            map[aNear.x, aNear.y].portal = true;
            map[bNear.x, bNear.y].portal = true;

            int xDir = 1;
            int yDir = 1;
            if (aNear.x > bNear.x)
                xDir = -1;
            if (aNear.y > bNear.y)
                yDir = -1;

            List<Vector2> tunnelPath = new List<Vector2>();
            bool validPath = true;

            for(int i = 0; i < Math.Abs(aNear.x - bNear.x); i++)
            {
                int x = aNear.x + (i * xDir);
                tunnelPath.Add(new Vector2(x, aNear.y));
                if (map[x, aNear.y].caveId != aCave && map[x, aNear.y].caveId != bCave && !map[x, aNear.y].wall)
                    validPath = false;
            }
            for(int j = 0; j < Math.Abs(aNear.y - bNear.y); j++)
            {
                int y = aNear.y + (j * yDir);
                tunnelPath.Add(new Vector2(bNear.x, y));
                if (map[bNear.x, y].caveId != aCave && map[bNear.x, y].caveId != bCave && !map[bNear.x, y].wall)
                    validPath = false;
            }

            if (caveConnections[aCave] > Math.Floor(caveArea[aCave] / 3.0) || caveConnections[bCave] > Math.Floor(caveArea[bCave] / 3.0))
                validPath = false;

            if(validPath)
            {
                foreach(Vector2 v in tunnelPath)
                {
                    map[v.x, v.y].tunnel = true;
                    map[v.x, v.y].wall = false;
                    caveConnections[aCave]++;
                    caveConnections[bCave]++;
                }
            }

        }

        public void findEdges()
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    map[i, j].edge = isEdge(i, j);
                }
            }
        }

        public bool isEdge(int x, int y)
        {
            int caveId = map[x, y].caveId;
            int surroundingCt = 0;
            for (int m = -1; m <= 1; m++)
            {
                for (int n = -1; n <= 1; n++)
                {
                    if (safe(x + m, y + n))
                    {
                        if (map[x + m, y + n].wall)
                            surroundingCt++;
                    }
                }
            }
            if (surroundingCt > 0)
                return true;
            else
                return false;
        }
        */

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
                        b.DrawRectangle(Pens.Black, drawRect);
                    }
                    else
                    {
                        if (!Generator.map[i, j].edge)
                            b.DrawRectangle(new Pen(caveColors[Generator.map[i, j].caveId - 1], 1), drawRect);
                        else
                            b.DrawRectangle(Pens.White, drawRect);
                        //b.DrawRectangle(new Pen(caveColors[Generator.map[i, j].caveId], 1), drawRect);
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
