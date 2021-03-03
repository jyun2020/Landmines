using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Landmines
{
    class Landmines : Button
    {
        public int x
        {
            get;
            set;
        }
        public int y
        {
            get;
            set;
        }
        public int landmines
        {
            get;
            set;
        }
        public Point point
        {
            get;
            set;
        }
        public Landmines(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.Size = new Size(40, 40);
            this.BackColor = Color.Gray;
            this.ForeColor = Color.Gray;
            this.Location = new Point(40 * x, 40 * y);
            this.Tag = 0;
            this.Text = null;
        }
    }
}
