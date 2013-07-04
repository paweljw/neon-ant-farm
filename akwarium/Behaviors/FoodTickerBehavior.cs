using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akwarium.Agents;

namespace akwarium.Behaviors
{
    class FoodTickerBehavior : Behavior
    {
        int tick = 0;
        int every = 10;

        public FoodTickerBehavior()
        {
        }

        public FoodTickerBehavior(IAgent p)
            : this()
        {
            parent = p;
        }

        public FoodTickerBehavior(IAgent p, int ticks)
            : this(p)
        {
            every = ticks;
        }

        public override bool isEligible()
        {
            tick++;
            if (tick % every == 0) return true;
            return false;
        }

        

        public override void process()
        {
            parent.Glob().placeFoodGlob(parent.Pos(), parent.Glob().Upgrades(parent.Pos()) * 5);
        }
    }
}
