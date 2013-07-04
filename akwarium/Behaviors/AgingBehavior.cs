using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akwarium.Agents;

namespace akwarium.Behaviors
{
    class AgingBehavior : Behavior
    {
        int AgeSpeed = 0;

        public AgingBehavior()
        {
            AgeSpeed = 1;
        }

        public AgingBehavior(IAgent p)
            : this()
        {
            parent = p;
        }

        public AgingBehavior(IAgent parent, int _AgeSpeed)
            : this(parent)
        {
            AgeSpeed = _AgeSpeed;
        }

        public override void process()
        {
            parent.Age(parent.Age() + AgeSpeed);
        }

        public override bool isEligible()
        {
            // Any agent is always eligible to age
            return true;
        }
    }
}
