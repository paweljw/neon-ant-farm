using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace akwarium
{
    class FoodPlacerThread
    {
        private Form1 parent;
        int ticks = 0;

        bool running = false;

        Thread thr;

        public FoodPlacerThread(Form1 p)
        {
            parent = p;
        }

        private void runner()
        {
            while (running)
            {
                ticks++;

                Random rnd = new Random();

                if (rnd.Next(1000) < 5) // magic number: food probability
                {
                    parent.Invoke(new Form1.placeGlobDelegate(parent.invokePlaceGlobDelegate));
                }

                Thread.Sleep(1000 / 60); // magic number: fps
            }
        }

        public void start()
        {
            // I'm just gonna do some preliminary food here

            for (int i = 0; i < 50; i++) parent.Invoke(new Form1.placeGlobDelegate(parent.invokePlaceGlobDelegate));

            running = true;
            thr = new Thread(new ThreadStart(runner));

            thr.IsBackground = true;
            thr.Start();
        }

        public void stop()
        {
            running = false;
        }

        public void waitForExit()
        {
            running = false;

            while (thr.IsAlive) { }
        }
    }
}
