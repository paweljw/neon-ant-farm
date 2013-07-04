using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using akwarium.Agents;

namespace akwarium
{
    public partial class Form1 : Form
    {
        public delegate void placeGlobDelegate();
        public delegate void addAgentXYDelegate(int x, int y, Color c);

        public bool GECK = false;

        int repaintCounter = 0;

        Graphics g;
        Bitmap bwutw;

        Glob glob;

        List<IAgent> lst;

        Thread moverThread;
        bool keepMoving = false;

        FoodPlacerThread fpt;

        bool nodraw = false;

        private void resetGlob()
        {
            if (fpt != null) fpt.stop();
            keepMoving = false;
            if(moverThread != null)
                while (moverThread.IsAlive) { } // spin

            glob = null;
            glob = new Glob(panel1.Width+1, panel1.Height+1, this);

            lst = null;
            lst = new List<IAgent>();

            bwutw = new Bitmap(glob.x, glob.y);

            repaint();
        }

        private void moveAllAgents()
        {
            List<IAgent> deaths = new List<IAgent>();

            while(keepMoving)
            {
                deaths.Clear();

                // Process behaviors
                for(int i = 0; i < lst.Count; i++)
                {
                    IAgent ag = lst[i];
                    ag.move();
                    if (ag.Dead()) deaths.Add(ag);
                }

                // Remove dead agents
                foreach (IAgent i in deaths) 
                    lst.Remove(i);

                try
                {
                    this.Invoke(new placeGlobDelegate(repaint));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(1000 / 60);
            }
        }

        private void moveThread()
        {
            if (keepMoving == false)
            {
                keepMoving = true;
                moverThread = new Thread(new ThreadStart(moveAllAgents));

                moverThread.IsBackground = true;
                moverThread.Start();
            }
            else
            {
                keepMoving = false;
            }
        }

        private void repaint()
        {
            repaintCounter++;
            if (nodraw && repaintCounter % 60 != 0) return;

            if (repaintCounter > 60) repaintCounter = 1;

            // paint food et al.
            LockBitmap lb = new LockBitmap(bwutw);
            lb.LockBits();

            for (int i = 0; i < glob.x; i++)
                for (int j = 0; j < glob.y; j++)
                {
                    if (glob.arr[i][j].Food == 0)
                        lb.SetPixel(i, j, Color.Black);
                    else
                        lb.SetPixel(i, j, glob.arr[i][j].FoodColor);
                }

            // paint agents
            for (int i = 0; i < lst.Count; i++)
            {
                Point p = lst[i].Pos();

                // red rectangles <3
                if (lst[i].drawAs() == DrawSymbols.DOT)
                {
                    for (int j = 0; j < 3; j++)
                        for (int k = 0; k < 3; k++)
                            if (p.X + j < bwutw.Width && p.Y + k < bwutw.Height)
                                lb.SetPixel(p.X + j, p.Y + k, lst[i].Col());
                }
                else if (lst[i].drawAs() == DrawSymbols.CROSS)
                {
                    int wd = glob.Upgrades(p)*5;
                    int XB = p.X - wd;
                    int XT = p.X + wd;

                    int YB = p.Y - wd;
                    int YT = p.Y + wd;

                    for(int j=XB; j<=XT; j++)
                        if(j < bwutw.Width)
                            lb.SetPixel(j, p.Y, lst[i].Col());

                    for (int j = YB; j <= YT; j++)
                        if (j < bwutw.Height)
                            lb.SetPixel(p.X, j, lst[i].Col());
                }
            }

            lb.UnlockBits();

            g.DrawImage(bwutw, new Rectangle(0, 0, panel1.Width, panel1.Height));

            Text = "NeonAntFarm (" + SimpleAgent.CreatedAgents() + " agents created, " + SimpleAgent.AliveAgents() + " agents alive)";
        }

        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();

            bwutw = new Bitmap(panel1.Width, panel1.Height);

            panel1.Paint += panel1_Paint;
            
            glob = new Glob(panel1.Width, panel1.Height, this);
            lst = new List<IAgent>();

            this.Resize += Form1_Resize;

            this.Activated += raiseFlag;
            this.Deactivate += lowerFlag;

            colorByGeck();
        }

        private void lowerFlag(object sender, EventArgs e)
        {
            nodraw = true;
        }

        private void raiseFlag(object sender, EventArgs e)
        {
            nodraw = false;
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;

            panel1.Width = this.Width;
            panel1.Height = this.Height;

            g = panel1.CreateGraphics();

            Console.WriteLine("Panel dimensions: " + panel1.Width + ", " + panel1.Height);

            resetGlob();
            repaint();

            Console.WriteLine("Glob dimensions: " + glob.x + ", " + glob.y);
            Console.WriteLine("BMP dimensions: " + bwutw.Width + ", " + bwutw.Height);
        }

        private void closeCall(object sender, FormClosingEventArgs e)
        {
            keepMoving = false;
            
            if (fpt != null)
            {
                fpt.stop();
                fpt.waitForExit();
            }

            if(moverThread != null)
                while (moverThread.IsAlive) { }


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            repaint();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.ForeColor = Color.White;
            button2.BackColor = Color.DarkGreen;

            fpt = new FoodPlacerThread(this);
            fpt.start();
        }

        public void invokePlaceGlobDelegate()
        {
            glob.placeFoodGlob();
            repaint();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addAgent();
        }

        public void addAgent()
        {
            lst.Add(new SimpleAgent(new Point(panel1.Width, panel1.Height), glob));
            repaint();
        }

        public void addAgentXY(int x, int y, Color c)
        {
            lst.Add(new SimpleAgent(new Point(panel1.Width, panel1.Height), new Point(x,y), glob, c));
            repaint();
        }

        public void addFTAgentXY(int x, int y, Color c)
        {
            lst.Add(new FoodTickerAgent(new Point(panel1.Width, panel1.Height), new Point(x, y), glob, c));
            repaint();
        }

        public void addTenAgents()
        {
            for (int i = 0; i < 10; i++) addAgent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            moveThread();
            colorByGeck();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addTenAgents();
        }

        private void colorByGeck()
        {
            button5.ForeColor = Color.White;
            if (GECK) button5.BackColor = Color.DarkGreen;
            else button5.BackColor = Color.DarkRed;

            button3.ForeColor = Color.White;
            if (keepMoving) button3.BackColor = Color.DarkGreen;
            else button3.BackColor = Color.DarkRed;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (GECK == false) GECK = true;
            else GECK = false;

            colorByGeck();
            
            resetGlob();
            repaint();
        }
    }
}
