using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyPaint
{
    public abstract class ToolsClass
    {
        protected PictureBox image;
        //protected ToolStrip toolBar;
        public static object SelectedItem { get; set; }
        public static MouseButtons mouseDown = MouseButtons.None;
        public static Color CurColor { get; set; }
        public static int CurWidth { get; set; }

        public ToolsClass(PictureBox image)
        {
            this.image = image;
        }

        //public abstract void Cursor();
        public abstract void Draw(List<Canvas.TwoPoints> m_list, Point point1, Point point2, Graphics e = null);

    }
}
