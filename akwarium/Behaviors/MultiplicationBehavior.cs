﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akwarium.Agents;

namespace akwarium.Behaviors
{
    class MultiplicationBehavior : Behavior
    {
        int fedBarrier;

        public MultiplicationBehavior()
        {
            fedBarrier = 110;
        }

        public MultiplicationBehavior(IAgent p)
            : this()
        {
            parent = p;
        }

        public MultiplicationBehavior(IAgent p, int fb)
            : this(p)
        {
            fedBarrier = fb;
        }

        public override bool isEligible()
        {
            if (parent.Fed() >= fedBarrier) return true;
            return false;
        }

        public override void process()
        {
            try
            {
                parent.Glob().parent.Invoke(
                    new Form1.addAgentXYDelegate(
                        parent.Glob().parent.addAgentXY
                        ),
                    new object[] 
                    { 
                        parent.Pos().X, 
                        parent.Pos().Y, 
                        parent.Col() 
                    }
                );

                parent.Fed(80);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
