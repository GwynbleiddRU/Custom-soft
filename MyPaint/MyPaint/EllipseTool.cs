using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MyPaint
{
    public class EllipseTool : ToolsClass
    {
        public EllipseTool(PictureBox forma) : base(forma)
        {

        }

        public override void Draw(List<Canvas.TwoPoints> m_list, Point point1, Point point2, Graphics grph)
        {
            Pen ellipsePen = null;
            try
            {
                ellipsePen = new Pen(CurColor, CurWidth);
                ellipsePen.DashStyle = (DashStyle)SelectedItem;
                if (!Canvas.can_write)
                    using (var graphics = Graphics.FromImage(image.Image))
                    {
                        //foreach (Canvas.TwoPoints tp in m_list)
                        //{
                        //    graphics.DrawEllipse(ellipsePen, tp.point1.X, tp.point1.Y, tp.point2.X, tp.point2.Y);
                        //}
                        graphics.DrawEllipse(ellipsePen, point1.X, point1.Y, point2.X-point1.X, point2.Y- point1.Y);
                    }
                else
                {

                    //foreach (Canvas.TwoPoints tp in m_list)
                    //{
                    //    grph.DrawEllipse(ellipsePen, tp.point1.X, tp.point1.Y, tp.point2.X, tp.point2.Y);
                    //}
                    grph.DrawEllipse(ellipsePen, point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                ellipsePen.Dispose();
            }
        }
    }
}
