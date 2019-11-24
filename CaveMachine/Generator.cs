using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaveMachine
{
    public static class Generator
    {

        static Random rng;

        public static int width = 150;
        public static int height = 100;
        public static Tile[,] map;

        static int refine = 5;

        public static int caves = 0;

        static Dictionary<int, int> caveArea = new Dictionary<int, int>();
        static Dictionary<int, int> caveConnections = new Dictionary<int, int>();

        static List<FillRule> fillQueue = new List<FillRule>();

        public static bool generated = false;

        public static MainForm parent;

        public static void initialize(MainForm parentForm)
        {
            parent = parentForm;

            rng = new Random();
            map = new Tile[width, height];

            parent.scalePanel(width, height);
        }

        public static void generate()
        {

            generated = false;

            caves = 0;
            caveArea = new Dictionary<int, int>();
            caveConnections = new Dictionary<int, int>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = new Tile(Convert.ToBoolean(rng.Next(0, 2)));
                }
            }

            for (int k = 0; k < refine; k++)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        int surroundingWalls = 0;
                        for (int m = -1; m <= 1; m++)
                        {
                            for (int n = -1; n <= 1; n++)
                            {
                                if (safe(i + m, j + n))
                                {
                                    if (map[i + m, j + n].wall)
                                        surroundingWalls++;
                                }
                            }
                        }
                        if (surroundingWalls >= 5)
                            map[i, j].wall = true;
                        else
                            map[i, j].wall = false;
                    }
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!map[i, j].wall && map[i, j].caveId == 0)
                    {
                        caves++;
                        caveArea.Add(caves, 0);
                        caveConnections.Add(caves, 0);
                        fillQueue.Add(new FillRule(i, j, caves));
                        while (fillQueue.Count > 0)
                            fillCave(fillQueue[0]);
                    }
                }
            }

            findEdges();

            List<Vector2> seenCombos = new List<Vector2>();

            for (int i = 1; i < caves; i++)
            {
                for (int j = 1; j < caves; j++)
                {
                    if (i != j)
                    {
                        //if (seenCombos.Contains(new Vector2(i, j)) || seenCombos.Contains(new Vector2(j, i)))
                        //break;
                        findTunnel(i, j);
                        seenCombos.Add(new Vector2(i, j));
                    }
                }
            }

            generated = true;
            parent.refreshPanel();
        }

        public static void fillCave(FillRule rule)
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

        public static void findTunnel(int aCave, int bCave)
        {
            List<Vector2> aCheck = new List<Vector2>();
            List<Vector2> bCheck = new List<Vector2>();

            int bestDistance = width + height + 1;
            Vector2 aNear = new Vector2(0, 0);
            Vector2 bNear = new Vector2(0, 0);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map[i, j].caveId == aCave && map[i, j].edge)
                        aCheck.Add(new Vector2(i, j));
                    if (map[i, j].caveId == bCave && map[i, j].edge)
                        bCheck.Add(new Vector2(i, j));
                }
            }

            foreach (Vector2 aPoint in aCheck)
            {
                foreach (Vector2 bPoint in bCheck)
                {
                    int manDistance = Math.Abs(aPoint.x - bPoint.x) + Math.Abs(aPoint.y - bPoint.y);
                    if (manDistance < bestDistance)
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

            for (int i = 0; i < Math.Abs(aNear.x - bNear.x); i++)
            {
                int x = aNear.x + (i * xDir);
                tunnelPath.Add(new Vector2(x, aNear.y));
                if (map[x, aNear.y].caveId != aCave && map[x, aNear.y].caveId != bCave && !map[x, aNear.y].wall)
                    validPath = false;
            }
            for (int j = 0; j < Math.Abs(aNear.y - bNear.y); j++)
            {
                int y = aNear.y + (j * yDir);
                tunnelPath.Add(new Vector2(bNear.x, y));
                if (map[bNear.x, y].caveId != aCave && map[bNear.x, y].caveId != bCave && !map[bNear.x, y].wall)
                    validPath = false;
            }

            if (caveConnections[aCave] > Math.Floor(caveArea[aCave] / 3.0) || caveConnections[bCave] > Math.Floor(caveArea[bCave] / 3.0))
                validPath = false;

            if (validPath)
            {
                foreach (Vector2 v in tunnelPath)
                {
                    map[v.x, v.y].tunnel = true;
                    map[v.x, v.y].wall = false;
                    caveConnections[aCave]++;
                    caveConnections[bCave]++;
                }
            }

        }

        public static void findEdges()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j].edge = isEdge(i, j);
                }
            }
        }

        public static bool isEdge(int x, int y)
        {
            /*
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
            */

            int surroundingCt = 0;
            if (map[x, y].wall)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (safe(x + i, y + j))
                        {
                            if (map[x + i, y + j].wall && !map[x + i, y + j].tunnel)
                                surroundingCt++;
                        }
                    }
                }
            }

            if (surroundingCt < 8 && surroundingCt >= 1)
                return true;
            else
                return false;
        }

        public static bool safe(int x, int y)
        {
            return (x >= 0 && x < width && y >= 0 && y < height);
        }

        public static List<Vector2> buildPoints()
        {
            List<Vector2> points = new List<Vector2>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map[i, j].edge)
                    {
                        points.Add(new Vector2(i, j));
                    }
                }
            }

            return points;

        }

    }
}
