using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyPaint
{
    public class LineTool : ToolsClass
    {

        public LineTool(PictureBox forma) : base(forma)
        {

        }

        public override void Draw(List<Canvas.TwoPoints> m_list, Point point1, Point point2, Graphics grph)
        {
            Pen m_pen = null;
            try
            {
                m_pen = new Pen(CurColor, CurWidth);
                m_pen.DashStyle = (DashStyle)SelectedItem;
                if (!Canvas.can_write)
                    using (var graphics = Graphics.FromImage(image.Image))
                    {
                        foreach (Canvas.TwoPoints tp in m_list)
                        {

                            graphics.DrawLine(m_pen, tp.point1, tp.point2);

                        }

                        graphics.DrawLine(m_pen, point1, point2);
                    }
                else
                {

                    foreach (Canvas.TwoPoints tp in m_list)
                    {

                        grph.DrawLine(m_pen, tp.point1, tp.point2);

                    }

                    grph.DrawLine(m_pen, point1, point2);

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                m_pen.Dispose();
            }
        }
    }
}
