using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using akwarium.Behaviors;

namespace akwarium
{
    class MovementBehavior : Behavior
    {
        public override bool isEligible()
        {
            return false;
        }

        public override void process()
        {

        }

        public MovementBehavior()
        {
        }
    }
}
