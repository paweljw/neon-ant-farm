using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using akwarium.Agents;

namespace akwarium.Behaviors
{
    class UnderfedMovementBehavior : MovementBehavior
    {
        Random rnd = new Random();
        int Lim = 1;

        public override bool isEligible()
        {
            if (parent.Fed() <= 90) return true;
            return false;
        }

        public override void process()
        {
            //Console.WriteLine("Underfed process fire");

            Point pos = parent.Pos();
            Point clip = parent.Clip();

            if (parent.Fed() < 30) parent.FOV(100);
            if (parent.Fed() >= 30 && parent.Fed() <= 70) parent.FOV(75);
            if (parent.Fed() > 70) parent.FOV(50);

            int fov = parent.FOV();

            int lowerX = pos.X - fov;
            int lowerY = pos.Y - fov;

            int upperX = pos.X + fov;
            int upperY = pos.Y + fov;

            if (lowerX < 0) lowerX = 0;
            if (lowerY < 0) lowerY = 0;

            if (upperX > clip.X) upperX = clip.X-1;
            if (upperY > clip.Y) upperY = clip.Y-1;

            int bestX = -1, bestY = -1;
            double best = 0;

            for (int i = lowerX; i < upperX; i+=Lim)
            {
                for (int j = lowerY; j < upperY; j+=Lim)
                {
                    if (i == pos.X && j == pos.Y) continue; // skip current point, no sitting around!
                    // dlugosc wektora
                    // Removed Math.Pow too
                    int deltaX2 = (pos.X - i) * (pos.X - i);
                    int deltaY2 = (pos.Y - j) * (pos.Y - j);

                    double len = deltaX2 + deltaY2; // Math.Sqrt is much more expensive.

                    if (len > fov * fov) continue; // we only want a circle

                    double lBest = 1 / len * parent.Glob().arr[i][j].Food;

                    if (lBest > best) { bestX = i; bestY = j; best = lBest; }
                }
            }

            if (bestX == -1 || bestY == -1) // nothing around here, walking at random
            {
                int moveX = rnd.Next(-1, 2);
                int moveY = rnd.Next(-1, 2);

                if (pos.X + moveX < 0 || pos.X + moveX >= clip.X) moveX *= -1;
                if (pos.Y + moveY < 0 || pos.Y + moveY >= clip.Y) moveY *= -1;

                int newx = pos.X + moveX;
                int newy = pos.Y + moveY;

                parent.Pos(new Point(newx, newy));
            }
            else
            {

                // move in the direction of food

                int moveX = 0, moveY = 0;

                if (bestX > pos.X) moveX = 1;
                if (bestX < pos.X) moveX = -1;

                if (bestY > pos.Y) moveY = 1;
                if (bestY < pos.Y) moveY = -1;

                if (pos.X + moveX < 0 || pos.X + moveX >= clip.X) moveX *= -1;
                if (pos.Y + moveY < 0 || pos.Y + moveY >= clip.Y) moveY *= -1;

                int newx = pos.X + moveX;
                int newy = pos.Y + moveY;

                parent.Pos(new Point(newx, newy));
            }
        }

        public UnderfedMovementBehavior(IAgent p)
        {
            parent = p;
        }

        public UnderfedMovementBehavior(IAgent p, int lim)
            : this(p)
        {
            Lim = lim;
        }
    }
}
