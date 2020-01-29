using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyPaint
{
    public class StarTool : ToolsClass
    {
        public static int corners;
        public StarTool(int cornersAmount, PictureBox forma) : base(forma)
        {
            corners = cornersAmount;
        }

        public override void Draw(List<Canvas.TwoPoints> m_list, Point point1, Point point2, Graphics grph)
        {
            //int n = 5;                    // число вершин
            //double R = 25, r = 50;   
            double r = point2.X - point1.X; // радиусы
            double R = r / 2;

            double alpha = 45;              // поворот

            Pen starPen = new Pen(CurColor, CurWidth);
            starPen.DashStyle = (DashStyle)SelectedItem;
            PointF[] points = new PointF[2 * corners + 1];
            double a = alpha, da = Math.PI / corners, l;
            for (int k = 0; k < 2 * corners + 1; k++)
            {
                l = k % 2 == 0 ? r : R;
                points[k] = new PointF((float)(point1.X + l * Math.Cos(a)), (float)(point1.Y + l * Math.Sin(a)));
                a += da;
            }

            try
            {
                if (!Canvas.can_write)
                using (var graphics = Graphics.FromImage(image.Image))
                {
                    //foreach (Canvas.TwoPoints tp in m_list)
                    //{
                    //    graphics.DrawLines(starPen, points);
                    //}
                    graphics.DrawLines(starPen, points);
                }
                else
                {
                    grph.DrawLines(starPen, points);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                starPen.Dispose();
            }
        }
    }
}
