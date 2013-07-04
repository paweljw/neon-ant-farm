using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace akwarium
{
    abstract class Behavior
    {
        protected IAgent parent;

        public abstract bool isEligible();
        public abstract void process();

        public Behavior(IAgent p)
        {
            parent = p;
        }

        public Behavior() { }
    }
}
