using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using akwarium.Agents;

namespace akwarium.Behaviors
{
    class WellFedMovementBehavior : MovementBehavior
    {
        Random rnd = new Random();

        public override bool isEligible()
        {
            if (parent.Fed() > 90) return true;
            return false;
        }

        public override void process()
        {
        //    Console.WriteLine("Well fed pop");
            Point pos  = parent.Pos();
            Point clip      = parent.Clip();

            // first check - are we standing on a structure?
            if (parent.Glob().Structure(pos))
            {
                // we're on a structure
                if (parent.Glob().Upgrades(pos) < 5)
                {
                    parent.Glob().UpgradeStructure(pos);
                    parent.Fed(50);
                }
                return;
            }

            // we're not on a structure; let's just keep walking
            int fov = parent.FOV();

            int lowerX = pos.X - fov;
            int lowerY = pos.Y - fov;

            int upperX = pos.X + fov;
            int upperY = pos.Y + fov;

            if (lowerX < 0) lowerX = 0;
            if (lowerY < 0) lowerY = 0;

            if (upperX > clip.X) upperX = clip.X-1;
            if (upperY > clip.Y) upperY = clip.Y-1;

            int dirX = -1, dirY = -1;

            for(int i = lowerX; i < upperX; i++)
                for (int j = lowerY; j < upperY; j++)
                {
                    // if fully developed structure in FOV, walk around freely
                    if (parent.Glob().Structure(new Point(i, j))
                        &&
                        parent.Glob().Upgrades(new Point(i, j)) >= 5
                        )
                    {
                        dirX = -2;
                        dirY = -2;
                        i = upperX;
                        j = upperY;
                    }

                    // if structure found, get on it
                    if (parent.Glob().Structure(new Point(i, j)) 
                        && 
                        parent.Glob().Upgrades(new Point(i,j)) < 5
                        ) 
                    { 
                        dirX = i; 
                        dirY = j; 
                        i = upperX; 
                        j = upperY; 
                    }
                }

            if (dirX < 0 || dirY < 0) // nothing around here, walking at random
            {
                if (dirX == -1 || dirX == -1)
                {
                    // before walking randomly, place structure
                    parent.Glob().PlaceStructure(pos);
                    parent.Fed(30); // even more tiring than upgrade

                    // Place the actual ticker object
                    parent.Glob().parent.Invoke(
                        new Form1.addAgentXYDelegate(
                            parent.Glob().parent.addFTAgentXY
                            ),
                        new object[] 
                        { 
                            parent.Pos().X, 
                            parent.Pos().Y, 
                            parent.Col() 
                        }
                    );
                }

                // Now walk away whistling
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
                int moveX = 0, moveY = 0;

                if (dirX > pos.X) moveX = 1;
                if (dirX < pos.X) moveX = -1;

                if (dirY > pos.Y) moveY = 1;
                if (dirY < pos.Y) moveY = -1;

                if (pos.X + moveX < 0 || pos.X + moveX >= clip.X) moveX *= -1;
                if (pos.Y + moveY < 0 || pos.Y + moveY >= clip.Y) moveY *= -1;

                int newx = pos.X + moveX;
                int newy = pos.Y + moveY;

                parent.Pos(new Point(newx, newy));
            }
        }

        public WellFedMovementBehavior(IAgent p)
        {
            parent = p;
        }

    }
}
