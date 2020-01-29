using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public class Pencil : ToolsClass
    {

        public Pencil(PictureBox image) : base(image)
        {

        }

        public override void Draw(List<Canvas.TwoPoints> m_list, Point point1, Point point2, Graphics e)
        {
            Pen pencil = null;
            try
            {
                pencil = new Pen(CurColor, CurWidth);

                using (var graphics = Graphics.FromImage(image.Image))
                {
                    //pencil.StartCap = pencil.EndCap = LineCap.Round;

                    pencil.StartCap = LineCap.Round;
                    pencil.EndCap = LineCap.Round;
                    pencil.DashCap = DashCap.Round;
                    pencil.LineJoin = LineJoin.Round;

                    pencil.DashStyle = (DashStyle)SelectedItem;
                    graphics.DrawLine(pencil, point1, point2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pencil.Dispose();
            }

        }



    }
}
