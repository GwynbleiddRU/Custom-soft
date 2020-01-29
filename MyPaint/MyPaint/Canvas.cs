using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class Canvas : Form
    {
        private Bitmap image;
        private List<TwoPoints> m_list;
        public ToolsClass tool { get; set; }
        private Point startpoint;
        private Point endpoint;
        //private string str = string.Empty;
        public static bool can_write = false;
        private bool wasChanged = false;

        public struct TwoPoints
        {
            public Point point1;
            public Point point2;
            public TwoPoints(Point point1, Point point2)
            {
                this.point1 = point1;
                this.point2 = point2;
            }
        }

        public Canvas()
        {
            InitializeComponent();
            image = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = image;
        }
        public Canvas(String FileName)
        {
            InitializeComponent();
            image = new Bitmap(FileName);
            Graphics g = Graphics.FromImage(image);
            pictureBox1.Width = image.Width;
            pictureBox1.Height = image.Height;
            pictureBox1.Image = image;
        }
        public Image m_Image
        {

            get
            {
                return pictureBox1.Image;
            }
            set
            {
                pictureBox1.Image = value;
            }
        }

        private void Canvas_Load(object sender, EventArgs e)
        {
            tool = new Pencil(pictureBox1);
            m_list = new List<TwoPoints>();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (var graphics = Graphics.FromImage(image))
            {
               graphics.Clear(Color.White);
            }

            pictureBox1.Image = image;
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                //e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                //e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                tool.Draw(m_list, startpoint, endpoint, e.Graphics);
                wasChanged = true;
            }
            catch
            {

            }
        }

        #region MouseEvents
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                ////wasChanged = true;
                //endpoint = e.Location;
                //tool.Draw(m_list, e.Location, e.Location);

                ////startpoint = e.Location;

                //pictureBox1.Invalidate();
                Console.WriteLine("клик");
            }
            catch { }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (tool is LineTool || tool is RectangleTool || tool is EllipseTool || tool is StarTool)
            {
                if (can_write)
                {

                    endpoint = e.Location;
                    //tool.Draw(m_list, startpoint, endpoint);
                    pictureBox1.Invalidate();
                    MainForm.wasChanged = true;
                }

            }
            else if (can_write)
            {
                endpoint = e.Location;
                try
                {
                    tool.Draw(m_list, startpoint, endpoint);
                    startpoint = e.Location;
                }
                catch
                {

                }
                pictureBox1.Invalidate();
                MainForm.wasChanged = true;
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    m_list.Clear();
                    can_write = true;
                    startpoint = e.Location;
                    pictureBox1.Paint += OnPaint;
                }
            }
            catch
            {

            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                pictureBox1.Paint -= OnPaint;
                can_write = false;
                m_list.Add(new TwoPoints(startpoint, endpoint));
                tool.Draw(m_list, startpoint, endpoint);
                pictureBox1.Invalidate();
                MainForm.wasChanged = true;
            }
            catch
            {

            }
        }
        #endregion MouseEvents

        public string SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
            //dlg.ShowDialog();
            ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                image.Save(dlg.FileName, ff[dlg.FilterIndex - 1]);
            }
            return dlg.FileName;
        }

        private void Canvas_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (wasChanged)
                {
                    ClosingSave cs = new ClosingSave(image.Size.Width, image.Size.Height, this.Text);
                    if (cs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //ActiveMdiChild.Size = new Size(cs.SizeWidth, cs.SizeHeight);

                        if (MainForm.filePath == null)
                        {
                            var SaveFileDialogue = new SaveFileDialog();
                            SaveFileDialogue.Filter = @"*.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png|*.gif|*.gif";
                            SaveFileDialogue.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                            if (SaveFileDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                MainForm.filePath = SaveFileDialogue.FileName;

                                if (m_Image != null)
                                {
                                    pictureBox1.Image.Save(MainForm.filePath);
                                    wasChanged = false;
                                    MainForm.filePath = null;
                                }
                                else
                                    MessageBox.Show("Сначала нарисуйте Ваш шедевр :)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                MessageBox.Show("Укажите путь", "Изображение не сохранено", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            pictureBox1.Image.Save(MainForm.filePath);
                            MainForm.filePath = null;
                        }
                    }
                    else if (cs.DialogResult == DialogResult.Cancel) e.Cancel = true;
                }
            }
            catch { }
        }

        #region Zoom
        public Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            return result;
        }

        public void zoomPlus()
        {
            try
            {
                image = ResizeBitmap(image, image.Width + 25, image.Height + 25);
                pictureBox1.Image = image;
            }
            catch { }
        }

        public void zoomMinus()
        {
            try
            {
                image = ResizeBitmap(image, image.Width - 25, image.Height - 25);
                pictureBox1.Image = image;
            }
            catch { }
        }
        #endregion Zoom

    }
}
