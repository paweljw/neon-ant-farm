using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using akwarium.Behaviors;

namespace akwarium.Agents
{
    class FoodTickerAgent : IAgent
    {
        Color col;

        bool dead = false;

        public int x;
        public int y;

        public int clipX;
        public int clipY;

        List<Behavior> mb = new List<Behavior>();

        Glob g;

        Random rnd = new Random();

        public int FOV() { return 0; }
        public void FOV(int f) {  }

        public Glob Glob() { return g; }
        public void Glob(Glob _g) { g = _g; }

        public int Age() { return 0; }
        public void Age(int a) {  }

        public void move()
        {
            foreach(Behavior b in mb)
            {
                if(b.isEligible()) b.process();
            }
        }

        public Point Pos()
        {
            return new Point(x, y);
        }

        public void Pos(Point p)
        {
            x = p.X;
            y = p.Y;
        }

        public Point Clip()
        {
            return new Point(clipX, clipY);
        }

        public void Clip(Point p)
        {
            clipX = p.X;
            clipY = p.Y;
        }

        public double Fed()
        {
            return 100;
        }

        public void Fed(double f)
        {
            
        }

        public Color Col() 
        { 
            return col; 
        }

        public void Col(Color c)
        {
            col = c;
        }

        public FoodTickerAgent()
        {
            mb.Add(new FoodTickerBehavior(this, 100));
            // Golden ratio conjugate for uniform distribution of random colors
            double golden_ratio_conjugate = 0.618033988749895;

            // Constant saturation and luminosity
            double sat = 0.5f;
            double lum = 0.6f;

            // Random hue
            double h = rnd.NextDouble();
            h += golden_ratio_conjugate;
            h %= 1;
            
            // Generate color
            HSLColor c = new HSLColor(h * 240, sat * 240, lum * 240);

            Col(c);
        }

        public FoodTickerAgent(Point clp) : this()
        {
            Clip(clp);
            Pos(new Point(rnd.Next(clipX), rnd.Next(clipY)));
        }

        public FoodTickerAgent(Point clp, Glob _g)
            : this(clp)
        {
            g = _g;
        }

        public FoodTickerAgent(Point clp, Point p, Glob _g) : this(clp, _g)
        {
            Pos(p);
        }

        public FoodTickerAgent(Point clp, Point p, Glob _g, Color c)
            : this(clp, p, _g)
        {
            Col(c);
        }

        public bool Dead()
        {
            return dead;
        }

        public void Dead(bool b)
        {
            dead = b;
        }

        public int ID()
        {
            return 0;
        }

        public void ID(int i)
        {
            //
        }

        public DrawSymbols drawAs()
        {
            return DrawSymbols.CROSS;
        }
    }
}

