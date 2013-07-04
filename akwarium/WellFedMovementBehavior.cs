using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace akwarium
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
            Point position  = parent.Pos();
            Point clip      = parent.Clip();

            int moveX = rnd.Next(-1, 2);
            int moveY = rnd.Next(-1, 2);

            if (position.X + moveX < 0 || position.X + moveX >= clip.X) moveX *= -1;
            if (position.Y + moveY < 0 || position.Y + moveY >= clip.Y) moveY *= -1;

            parent.Pos(new Point(position.X + moveX, position.Y + moveY));
        }

        public WellFedMovementBehavior(IAgent p)
        {
            parent = p;
        }

    }
}
