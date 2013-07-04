using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akwarium.Agents;

namespace akwarium.Behaviors
{
    class DeathBehavior : Behavior
    {
        public DeathBehavior()
        {
        }

        public DeathBehavior(IAgent p)
            : this()
        {
            parent = p;
        }

        public override bool isEligible()
        {
            // Everybody dies
            return true;
        }

        public override void process()
        {
            // As of right now, only two conditions would cause an agent to die
            if (parent.Fed() <= 0)      parent.Dead(true);
            if (parent.Age() > 1000)    parent.Dead(true);

            // Dead ants are a good source of potassium!
            if (parent.Dead())
                parent.Glob().placeFoodGlob(parent.Pos(), (int)(parent.Fed()/10 + 10));
        }
    }
}
