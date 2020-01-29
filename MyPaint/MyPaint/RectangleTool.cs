using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyPaint
{
    public class RectangleTool : ToolsClass
    {
        public RectangleTool(PictureBox forma) : base(forma)
        {

        }

        public override void Draw(List<Canvas.TwoPoints> m_list, Point point1, Point point2, Graphics grph)
        {
            Pen rectanglePen = null;
            try
            {
                rectanglePen = new Pen(CurColor, CurWidth);
                rectanglePen.DashStyle = (DashStyle)SelectedItem;
                if (!Canvas.can_write)
                    using (var graphics = Graphics.FromImage(image.Image))
                    {
                        //foreach (Canvas.TwoPoints tp in m_list)
                        //{

                        //    graphics.DrawRectangle(rectanglePen, tp.point1.X, tp.point1.Y, tp.point2.X, tp.point2.Y);

                        //}
                        graphics.DrawRectangle(rectanglePen, point1.X, point1.Y, point2.X- point1.X, point2.Y- point1.Y);
                    }
                else
                {

                    //foreach (Canvas.TwoPoints tp in m_list)
                    //{

                    //    grph.DrawRectangle(rectanglePen, tp.point1.X, tp.point1.Y, tp.point2.X, tp.point2.Y);

                    //}
                    grph.DrawRectangle(rectanglePen, point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y);
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                rectanglePen.Dispose();                
            }
        }
    }
}
