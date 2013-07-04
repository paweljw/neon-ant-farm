using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akwarium
{
    class HungerBehavior : Behavior
    {
        float hungerRate = 0;

        public HungerBehavior()
        {
            hungerRate = 1;
        }

        public HungerBehavior(IAgent p)
            : this()
        {
            parent = p;
        }

        public HungerBehavior(IAgent parent, float hr)
            : this(parent)
        {
            hungerRate = hr;
        }

        public override void process()
        {
            parent.Fed(parent.Fed() - hungerRate);
        }

        public override bool isEligible()
        {
            // Any agent is always eligible to get hungry
            return true;
        }
    }

}
