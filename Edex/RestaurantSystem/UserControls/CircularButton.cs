using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.RestaurantSystem.UserControls
{
    class CircularButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
        
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, Width, Height);

            this.Region = new Region(gp);

            base.OnPaint(pevent);
        }
    }
}
