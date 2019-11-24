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
        public int scale = 10;

        public int scaledW = 1;
        public int scaledH = 1;

        List<Vector2> expandedPoints;
        Tile[,] sMap = new Tile[50, 50];

        Graphics g;

        Bitmap canvas;
        Graphics b;

        private void ExtrapolateForm_Load(object sender, EventArgs e)
        {

            width = Generator.width;
            height = Generator.height;

            scaledW = width * scale;
            scaledH = height * scale;

            previewPanel.Width = scaledW;
            previewPanel.Height = scaledH;

            expandedPoints = Generator.buildPoints();

            sMap = new Tile[scaledW, scaledH];
            for(int i = 0; i < scaledW; i++)
            {
                for(int j = 0; j < scaledH; j++)
                {
                    sMap[i, j] = new Tile(wall: false);
                }
            }

            g = previewPanel.CreateGraphics();
            canvas = new Bitmap(previewPanel.Width, previewPanel.Height);
            b = Graphics.FromImage(canvas);

            expandedPoints = spaceAndPlotPoints();
            connectPoints();

        }
        
        List<Vector2> spaceAndPlotPoints()
        {
            List<Vector2> points = Generator.buildPoints();

            for (int i = 0; i < points.Count; i++)
            {
                Vector2 currentPoint = points[i];
                currentPoint = new Vector2(points[i].x * scale, points[i].y * scale);
                points[i] = currentPoint;

                sMap[currentPoint.x, currentPoint.y] = new Tile(wall: true);

            }

            return points;

        }

        void connectPoints()
        {
            List<Vector2> points = expandedPoints;

            for(int i = 0; i < points.Count; i++)
            {
                for(int j = 0; j < points.Count; j++)
                {
                    if(i != j)
                    {
                        float distance = getDistance(points[i], points[j]);
                        if(distance < scale * 1.5)
                        {
                            plotLine(points[i], points[j]);
                        }
                    }
                }
            }
        }

        float getDistance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }

        public void plotLine(Vector2 a, Vector2 b)
        {

            int x = a.x;
            int y = a.y;
            int x2 = b.x;
            int y2 = b.y;

            //modified from: https://stackoverflow.com/a/11683720

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                sMap[x, y] = new Tile(wall: true);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        void refreshPanel()
        {
            previewPanel.Invalidate();
        }

        private void previewPanel_Paint(object sender, PaintEventArgs e)
        {
            /*
            for(int i = 0; i < expandedPoints.Count; i++)
            {
                int x = expandedPoints[i].x;
                int y = expandedPoints[i].y;

                Rectangle drawRect = new Rectangle(x, y, 1, 1);
                
                b.DrawRectangle(Pens.Black, drawRect);
            }
            */

            for(int i = 0; i < scaledW; i++)
            {
                for(int j = 0; j < scaledH; j++)
                {
                    Rectangle drawRect = new Rectangle(i, j, 1, 1);

                    if (sMap[i, j].wall)
                        b.DrawRectangle(Pens.Black, drawRect);
                }
            }

            g.DrawImage(canvas, new Point(0, 0));

        }
    }
}
