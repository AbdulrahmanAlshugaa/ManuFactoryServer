﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
﻿using System;
using System.Drawing;
using DevExpress.XtraEditors;
namespace Edex.RestaurantSystem.UserControls
{
    
   public class Button_WOC : Button
    {
        private Color _borderColor = Color.Silver;
      //  private Color _onHoverBorderColor = Color.Gray;
        private Color _buttonColor = Color.Red;
      //  private Color _onHoverButtonColor = Color.Yellow;
        private Color _textColor = Color.White;
   //     private Color _onHoverTextColor = Color.Gray;

        private bool _isHovering;
        private int _borderThickness = 3;
        private int _borderThicknessByTwo = 1;


        public Button_WOC()
        {
            DoubleBuffered = true;
            //MouseEnter += (sender, e) =>
            //{
            //    _isHovering = true;
            //    Invalidate();
            //};
            //MouseLeave += (sender, e) =>
            //{
            //    _isHovering = false;
            //    Invalidate();
            //};
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush brush = new SolidBrush(_buttonColor);

            //Border


            g.FillEllipse(brush, 0, 0, Height, Height);
            g.FillEllipse(brush, Width - Height, 0, Height, Height);
           
            g.FillRectangle(brush, Height / 2, 0, Width - Height, Height);
            g.FillRectangle(brush, 1, 2*Height / 3-2, Width, Height / 2);

            brush.Dispose();
           brush = new SolidBrush( _buttonColor);

            //Inner part. Button itself

            //g.FillEllipse(brush, _borderThicknessByTwo, _borderThicknessByTwo, Height - _borderThickness,
            //    Height - _borderThickness);
            //g.FillEllipse(brush, (Width - Height) + _borderThicknessByTwo, _borderThicknessByTwo,
            //    Height - _borderThickness, Height - _borderThickness);


            //g.FillRectangle(brush, Height / 2 + _borderThicknessByTwo, _borderThicknessByTwo,
            //    Width - Height - _borderThickness, Height - _borderThickness);

            brush.Dispose();
            brush = new SolidBrush( _textColor);

            //Button Text
            SizeF stringSize = g.MeasureString(Text, Font);
            g.DrawString(Text, Font, brush, (Width - stringSize.Width) / 2, (Height - stringSize.Height) / 2);
        }


        public Color BorderColor
        {
            get {return _borderColor;}
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        //public Color OnHoverBorderColor
        //{
        //    get { return _onHoverBorderColor; }
        //    set
        //    {
        //        _onHoverBorderColor = value;
        //        Invalidate();
        //    }
        //}

        public Color ButtonColor
        {
            get {return _buttonColor;}
            set
            {
                _buttonColor = value;
                Invalidate();
            }
        }

        //public Color OnHoverButtonColor
        //{
        //    get {return _onHoverButtonColor;}
        //    set
        //    {
        //        _onHoverButtonColor = value;
        //        Invalidate();
        //    }
        //}

        public Color TextColor
        {
            get {return _textColor;}
            set
            {
                _textColor = value;
                Invalidate();
            }
        }

        //public Color OnHoverTextColor
        //{
        //    get {return _onHoverTextColor;}
        //    set
        //    {
        //        _onHoverTextColor = value;
        //        Invalidate();
        //    }
        //}
    }
}