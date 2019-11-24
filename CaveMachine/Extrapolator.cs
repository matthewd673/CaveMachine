using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveMachine
{
    public static class Extrapolator
    {

        public static int width = 50;
        public static int height = 50;
        public static int scale = 10;

        static int scaledW = 1;
        static int scaledH = 1;

        static List<Vector2> expandedPoints;
        public static Tile[,] sMap = new Tile[50, 50];

        static ExtrapolateForm parent;

        public static void initialize(ExtrapolateForm parentForm)
        {

            parent = parentForm;

            width = Generator.width;
            height = Generator.height;

            scaledW = width * scale;
            scaledH = height * scale;

            expandedPoints = Generator.buildPoints();

            sMap = new Tile[scaledW, scaledH];
            for (int i = 0; i < scaledW; i++)
            {
                for (int j = 0; j < scaledH; j++)
                {
                    sMap[i, j] = new Tile(wall: false);
                }
            }

            expandedPoints = spaceAndPlotPoints();
        }

        static List<Vector2> spaceAndPlotPoints()
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

        public static void connectPoints()
        {
            List<Vector2> points = expandedPoints;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (i != j)
                    {
                        float distance = getDistance(points[i], points[j]);
                        if (distance >= scale && distance <= scale * Math.Sqrt(2))
                        {
                            plotLine(points[i], points[j]);
                        }
                    }
                }
            }

            parent.refreshPanel();

        }

        static float getDistance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }

        static void plotLine(Vector2 a, Vector2 b)
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
                sMap[x, y] = new Tile(wall: true, extrapolated: true);
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

    }
}
