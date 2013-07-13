using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using akwarium.Behaviors;

namespace akwarium.Agents
{
    class SimpleAgent : IAgent
    {
        double fed = 80;
        int fov = 20;

        Color col;

        bool dead = false;

        public int x;
        public int y;

        public int clipX;
        public int clipY;

        List<Behavior> mb = new List<Behavior>();

        int age = 0;

        Glob g;

        int id;

        static int created_Agents = 0;
        static int alive_Agents = 0;

        Random rnd = new Random();

        public DrawSymbols drawAs()
        {
            return DrawSymbols.DOT;
        }
        
        public int FOV() { return fov; }
        public void FOV(int f) { fov = f; }

        public Glob Glob() { return g; }
        public void Glob(Glob _g) { g = _g; }

        public int Age() { return age; }
        public void Age(int a) { age = a; }

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
            return fed;
        }

        public void Fed(double f)
        {
            fed = f;
        }

        public static int CreatedAgents() { return created_Agents; }
        public static int AliveAgents() { return alive_Agents; }

        public SimpleAgent()
        {
            created_Agents++;
            alive_Agents++;

            ID(created_Agents);

            // Age by 1 every cycle
            mb.Add(new AgingBehavior(this, 1));

            // Get hungrier by 0.75 every cycle
            mb.Add(new HungerBehavior(this, 0.75f));

            // Define how we behave while well fed (>90)
            mb.Add(new WellFedMovementBehavior(this));

            int lim = 1;
            // Define how we behave while wed less-than-well (<=90)
            mb.Add(new UnderfedMovementBehavior(this, lim));

            // Define how we eat
            mb.Add(new FeedBehavior(this));

            // Define how we multiply
            mb.Add(new MultiplicationBehavior(this, 110));

            // Sometimes we die.
            mb.Add(new DeathBehavior(this));

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

        public SimpleAgent(Point clp) : this()
        {
            Clip(clp);
            Pos(new Point(rnd.Next(clipX), rnd.Next(clipY)));
        }

        public SimpleAgent(Point clp, Glob _g)
            : this(clp)
        {
            g = _g;
        }

        public SimpleAgent(Point clp, Point p, Glob _g) : this(clp, _g)
        {
            Pos(p);
        }

        public SimpleAgent(Point clp, Point p, Glob _g, Color c)
            : this(clp, p, _g)
        {
            Col(c);
        }

        public Color Col() 
        { 
            return col; 
        }

        public void Col(Color c)
        {
            col = c;
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
            return id;
        }

        public void ID(int i)
        {
            id = i;
        }

        ~SimpleAgent()
        {
            alive_Agents--;
        }
    }
}
