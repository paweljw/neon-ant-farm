using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace akwarium
{
    class Glob
    {
        public int x;
        public int y;

        public Form1 parent;

        public Cell[][] arr;

        Random rnd = new Random();

        public double Food(Point p)
        {
            return arr[p.X][p.Y].Food;
        }

        public void Eaten(Point p)
        {
            arr[p.X][p.Y].eaten();
        }

        public Glob(int _x, int _y, Form1 p)
        {
            x = _x;
            y = _y;

            parent = p;

            arr = new Cell[x][];

            // No to dopiero jest pojebane jak lato z radiem
            for (int i = 0; i < x; i++)
            {
                arr[i] = new Cell[y];
                for (int j = 0; j < y; j++) 
                { 
                    arr[i][j] = new Cell();  
                    if(parent.GECK)
                        arr[i][j].Food = 0.3f; 
                }
            }
        }

        public void placeFoodGlob()
        {
            int radius = rnd.Next(x/40, x/20);

            int rx = rnd.Next(radius, x - radius);
            int ry = rnd.Next(radius, y - radius);

            for (int i = rx - radius; i <= rx + radius; i++)
            {
                for (int j = ry - radius; j <= ry + radius; j++)
                {
                    // dlugosc wektora
                    // Removed Math.Pow too
                    int deltaX2 = (rx - i) * (rx - i);
                    int deltaY2 = (ry - j) * (ry - j);

                    double len = deltaX2 + deltaY2; // Math.Sqrt is much more expensive.
                    
                    if (len > radius*radius) continue;

                    double normal = (1 - (len / (radius*radius))) / 3.0f;

                    arr[i][j].Food = (float)normal;
                }
            }
        }

        public void placeFoodGlob(Point p, int radius)
        {
            int rx = p.X;
            int ry = p.Y;

            for (int i = rx - radius; i <= rx + radius; i++)
            {
                for (int j = ry - radius; j <= ry + radius; j++)
                {
                    if (i >= x || j >= y || i<0 || j<0) continue;
                    // dlugosc wektora
                    // Removed Math.Pow too
                    int deltaX2 = (rx - i) * (rx - i);
                    int deltaY2 = (ry - j) * (ry - j);

                    double len = deltaX2 + deltaY2; // Math.Sqrt is much more expensive.

                    if (len > radius * radius) continue;

                    double normal = (1 - (len / (radius * radius))) / 3.0f;

                    arr[i][j].Food = (float)normal;
                }
            }
        }
    }
}
