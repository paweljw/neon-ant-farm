using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akwarium
{
    class Cell
    {
        private float food;

        private bool structure = false;
        private int upgrades = 0;

        public System.Drawing.Color FoodColor
        {
            get
            {
                System.Drawing.Color c;

                // if (food < 0 || food > 1) return System.Drawing.Color.Red;

                c = System.Drawing.Color.FromArgb(0, (int)((food * 255) % 256), 0);

                return c;
            }
        }

        public float Food
        {
            get
            {
                return food * 3;
            }
            set
            {
                if (value > 1.0f) food += 1.0f;
                if (value < 0.0f) food += 0.0f;
                else food += value;

                if (food > 1.0f) food = 1.0f;
            }
        }

        public bool Structure
        {
            get
            {
                return structure;
            }
            set
            {
                structure = value;
            }
        }

        public int Upgrades
        {
            get
            {
                return upgrades;
            }
            set
            {
                upgrades = value;
            }
        }

        public void eaten()
        {
            food = 0;
        }

        public Cell()
        {
            food = 0;
        }
    }
}
