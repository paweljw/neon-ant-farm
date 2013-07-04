using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace akwarium.Agents
{
    interface IAgent
    {
        void move();

        int ID();
        void ID(int i);

        Point Pos();
        void Pos(Point p);
        
        double Fed();
        void Fed(double f);

        int Age();
        void Age(int a);
        
        Color Col();
        void Col(Color c);

        Point Clip();
        void Clip(Point c);

        int FOV();
        void FOV(int f);

        Glob Glob();
        void Glob(Glob g);

        bool Dead();
        void Dead(bool d);

        DrawSymbols drawAs();
    }
}
