using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akwarium
{
    class FeedBehavior : Behavior
    {
        public FeedBehavior()
        {
        }

        public FeedBehavior(IAgent p)
            : this()
        {
            parent = p;
        }

        public override bool isEligible()
        {
            // Everybody eats
            return true;
        }

        public override void process()
        {
            if (parent.Glob().Food(parent.Pos()) > 0)
            {
                parent.Fed(parent.Fed() + parent.Glob().Food(parent.Pos()));
                parent.Glob().Eaten(parent.Pos());
            }
        }
    }
}
