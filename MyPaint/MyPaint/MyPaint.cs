using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class MainForm : Form
    {
        private Canvas image;                                                            //картинка
        private ComboBox drawArea = new ComboBox();                                      //область для рисования
        private object[] arraylines;                                                     //значения для comboBox
        private ToolsClass tool;                                                         //сохраням текущий инструмент
        public static string filePath = null;                                            //путь к файлу
        public static bool wasChanged = false;
        public static int formCounter = 1;
        //public static ToolStripNumberControl corners = new ToolStripNumberControl();   //количество углов звезды

        public MainForm()
        {
            InitializeComponent();
        }

        private void MyPaint_Load(object sender, EventArgs e)                            //загрузка
        {
            arraylines = new object[] { 0, 1, 2, 3, 4 };

            drawArea.Size = new Size(55, 25);
            drawArea.DrawMode = DrawMode.OwnerDrawFixed;
            drawArea.DropDownStyle = ComboBoxStyle.DropDownList;
            drawArea.FormattingEnabled = true;
            drawArea.DrawItem += OnDrawItem;
            drawArea.SelectedIndexChanged += OnSelectChange;
            drawArea.Items.AddRange(arraylines);
            drawArea.SelectedIndex = 0;

            image = new Canvas();
            image.MdiParent = this;
            image.Show();
            image.Text = "Рисунок 1";
            окноToolStripMenuItem.DropDownItemClicked += WinDropDownItemClick;

            ToolsClass.CurColor = Color.Black;
            image.tool = new Pencil(image.Controls[0] as PictureBox);

            LayoutMdi(MdiLayout.Cascade);
        }

        #region Файл
        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)             //новый
        {
            formCounter++;
            Canvas frmChild = new Canvas();
            frmChild.MdiParent = this;
            frmChild.Text = "Рисунок " + formCounter;
            frmChild.Show();


            //frmChild.Name = MdiChildren.Length + "New";
            //for (int i = 0; i < окноToolStripMenuItem.DropDownItems.Count; i++)
            //{
            //    (окноToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Checked = false;
            //}
            // ?????????????????????????????????????????????????????????????????????????????????????????????

            //(окноToolStripMenuItem.DropDownItems[окноToolStripMenuItem.DropDownItems.Count - 1] as ToolStripMenuItem).Checked = false;           

            (окноToolStripMenuItem.DropDownItems[окноToolStripMenuItem.DropDownItems.Count - 1] as ToolStripMenuItem).Checked = true;
            drawArea.SelectedItem = 0;
        }
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var OpenFileDialogue = new OpenFileDialog();

                OpenFileDialogue.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                OpenFileDialogue.Filter = @"All files (*.*)|*.*| File jpg (*.jpg)|*.jpg| File bmp (*.bmp)|*.bmp| File gif (*.gif)|*.gif| File png (*.png)|*.png";

                if (OpenFileDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = OpenFileDialogue.FileName;
                    var bitmap = new Bitmap(filePath);

                    if ((ActiveMdiChild as Canvas).m_Image != null)
                    {
                        (ActiveMdiChild as Canvas).m_Image.Dispose();
                    }

                    (ActiveMdiChild as Canvas).m_Image = bitmap;

                    (ActiveMdiChild as Canvas).Size = bitmap.Size;
                }
            }
            catch
            {
                MessageBox.Show("Сначала создайте холст");
            }
        }        //открыть
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (filePath != null)
                //{
                    if (filePath == null)
                    {
                        var SaveFileDialogue = new SaveFileDialog();
                        SaveFileDialogue.Filter = @"*.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png|*.gif|*.gif";
                        SaveFileDialogue.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        if (SaveFileDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {

                            filePath = SaveFileDialogue.FileName;

                            if ((ActiveMdiChild as Canvas).m_Image != null)
                            {
                                (ActiveMdiChild as Canvas).m_Image.Save(filePath);
                            }
                            else
                                MessageBox.Show("Сначала нарисуйте Ваш шедевр :)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("Укажите путь", "Изображение не сохранено", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        (ActiveMdiChild as Canvas).m_Image.Save(filePath);
                //}
                //else MessageBox.Show("Ошибка. Возможно, файл был перемещен или удален в процессе работы программы", "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка. Возможно, файл был перемещен или удален в процессе работы программы", "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }      //сохранить
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)      //сохранить как
        {
            filePath = ((Canvas)ActiveMdiChild).SaveAs();
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }          //выход
        #endregion Файл
        #region Рисунок
        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)     //размер холста
        {
            try
            {
                CanvasSize cs = new CanvasSize(ActiveMdiChild.Size.Width, ActiveMdiChild.Size.Height);
                if (cs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    ActiveMdiChild.Size = new Size(cs.SizeWidth, cs.SizeHeight);
                    (ActiveMdiChild.Controls[0] as PictureBox).Size = new Size(cs.SizeWidth, cs.SizeHeight);
                    Bitmap image = new Bitmap(cs.SizeWidth, cs.SizeHeight);
                    using (var graphics = Graphics.FromImage(image))
                    {
                        graphics.Clear(Color.White);
                        graphics.DrawImage((ActiveMdiChild.Controls[0] as PictureBox).Image, new Rectangle(new Point(0, 0), new Size((ActiveMdiChild.Controls[0] as PictureBox).Image.Width, (ActiveMdiChild.Controls[0] as PictureBox).Image.Height)));
                    }
                    (ActiveMdiChild.Controls[0] as PictureBox).Image = image;
                }
            }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }
        }
        #endregion Рисунок
        #region Окно
        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)          //каскадом
        {
            LayoutMdi(MdiLayout.Cascade);
            (окноToolStripMenuItem.DropDownItems[0] as ToolStripMenuItem).Checked = true;
            (окноToolStripMenuItem.DropDownItems[1] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[2] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[3] as ToolStripMenuItem).Checked = false;
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)      //слева направо
        {
            LayoutMdi(MdiLayout.TileHorizontal);
            (окноToolStripMenuItem.DropDownItems[1] as ToolStripMenuItem).Checked = true;
            (окноToolStripMenuItem.DropDownItems[0] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[2] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[3] as ToolStripMenuItem).Checked = false;
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)        //сверху вниз
        {
            LayoutMdi(MdiLayout.TileVertical);
            (окноToolStripMenuItem.DropDownItems[2] as ToolStripMenuItem).Checked = true;
            (окноToolStripMenuItem.DropDownItems[1] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[0] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[3] as ToolStripMenuItem).Checked = false;
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e) //упорядочить
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
            (окноToolStripMenuItem.DropDownItems[3] as ToolStripMenuItem).Checked = true;
            (окноToolStripMenuItem.DropDownItems[1] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[2] as ToolStripMenuItem).Checked = false;
            (окноToolStripMenuItem.DropDownItems[0] as ToolStripMenuItem).Checked = false;
        }
        #endregion Окно
        #region Справка
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)       //о программе
        {
            AboutPaint a = new AboutPaint();
            a.Show();
        }
        #endregion Справка

        #region Цвета
        private void красныйToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ToolsClass.CurColor = Color.Red;
        }

        private void синийToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ToolsClass.CurColor = Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ToolsClass.CurColor = Color.Green;
        }

        private void другойToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                ToolsClass.CurColor = colorDialog.Color;
        }
        #endregion Цвета

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)          //сохранение при закрытии
        {
            //try
            //{
            //    if (wasChanged)
            //    {
            //        ClosingSave cs = new ClosingSave(ActiveMdiChild.Size.Width, ActiveMdiChild.Size.Height);
            //        if (cs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //        {
            //            //ActiveMdiChild.Size = new Size(cs.SizeWidth, cs.SizeHeight);

            //            if (filePath == null)
            //            {
            //                var SaveFileDialogue = new SaveFileDialog();
            //                SaveFileDialogue.Filter = @"*.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png|*.gif|*.gif";
            //                SaveFileDialogue.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //                if (SaveFileDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                {

            //                    filePath = SaveFileDialogue.FileName;

            //                    if ((ActiveMdiChild as Canvas).m_Image != null)
            //                    {
            //                        (ActiveMdiChild as Canvas).m_Image.Save(filePath);
            //                        filePath = null;
            //                    }
            //                    else
            //                        MessageBox.Show("Сначала нарисуйте Ваш шедевр :)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                }
            //                else
            //                {
            //                    MessageBox.Show("Укажите путь", "Изображение не сохранено", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                    e.Cancel = true;
            //                }
            //            }
            //            else
            //            {
            //                (ActiveMdiChild as Canvas).m_Image.Save(filePath);
            //                filePath = null;
            //            }
            //        }
            //        else if (cs.DialogResult == DialogResult.Cancel) e.Cancel = true;
            //    }
            //}
            //catch { }
        }

        #region Фильтры
        private void фильтр1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter1(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image; }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }
        }

        private void фильтр2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter2(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image; }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

        }

        private void фильтр3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter3(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image; }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

        }

        private void фильтр7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter7(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image; }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

        }

        private void фильтр8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Filters.Filter8(sender, e, (ActiveMdiChild.Controls[0] as PictureBox));

                //(ActiveMdiChild as Canvas).Size = (ActiveMdiChild.Controls[0] as PictureBox).Size;
                int sizeVal = (ActiveMdiChild as Canvas).Height;
                (ActiveMdiChild as Canvas).Height = (ActiveMdiChild as Canvas).Width;
                (ActiveMdiChild as Canvas).Width = sizeVal;
                image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image;
            }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

        }

        private void фильтр9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter9(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

            int sizeVal = (ActiveMdiChild as Canvas).Height;
            (ActiveMdiChild as Canvas).Height = (ActiveMdiChild as Canvas).Width;
            (ActiveMdiChild as Canvas).Width = sizeVal;

            image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image;
        }

        private void фильтр10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter10(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image; }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

        }

        private void фильтр11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Filters.Filter11(sender, e, (ActiveMdiChild.Controls[0] as PictureBox)); image.m_Image = (ActiveMdiChild.Controls[0] as PictureBox).Image; }
            catch (Exception) { MessageBox.Show("Сначала создайте холст"); }

        }

        //private void фильтр4ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Filters.Function4_Click(sender, e, (ActiveMdiChild.Controls[0] as PictureBox));
        //}

        //private void фильтр5ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Filters.Function5_Click(sender, e, (ActiveMdiChild.Controls[0] as PictureBox));
        //}

        //private void фильтр6ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Filters.Function6_Click(sender, e, (ActiveMdiChild.Controls[0] as PictureBox));
        //}
        #endregion Фильтры
        #region Инструменты
        private void toolStripButton1_Click(object sender, EventArgs e)                   //карандаш
        {
            try
            {
                ToolsClass.CurColor = Color.Black;
                tool = new Pencil((ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
            }
            catch { }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)                   //эллипс
        {
            try
            {
                tool = new EllipseTool((ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
            }
            catch { }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)                   //четырехугольник
        {
            try
            {
                tool = new RectangleTool((ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
            }
            catch { }
        }
        private void toolStripButton4_Click(object sender, EventArgs e)                   //звезда
        {
            try
            {
                tool = new StarTool(Convert.ToInt32(toolStripNumberControl.Text), (ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
            }
            catch { }
        }
        private void toolStripButton5_Click(object sender, EventArgs e)                   //линия
        {
            try
            {
                tool = new LineTool((ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
            }
            catch { }
        }
        private void toolStripButton6_Click(object sender, EventArgs e)                   //ластик
        {
            try
            {
                tool = new Pencil((ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
                ToolsClass.CurColor = Color.White;
            }
            catch { }
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            image.zoomPlus();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            image.zoomMinus();
        }
        #endregion Инструменты

        #region События

        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            wasChanged = true;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            //e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            e.DrawBackground();

            using (var pen = new Pen(e.ForeColor, 2.0f))
            {

                pen.DashStyle = (DashStyle)e.Index;
                int y = (e.Bounds.Top + e.Bounds.Bottom) / 2;
                e.Graphics.DrawLine(pen, e.Bounds.Left, y, e.Bounds.Right, y);
            }

            e.DrawFocusRectangle();
        }
        protected void WinDropDownItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            var item = (e.ClickedItem as ToolStripMenuItem).Checked;
            //for (int i = 0; i < окноToolStripMenuItem.DropDownItems.Count; i++)
            //{
            //    (окноToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Checked = false;
            //}
            if (item != true)
                (e.ClickedItem as ToolStripMenuItem).Checked = true;

            for (int j = 0; j < MdiChildren.Length; j++)
            {

                if (MdiChildren[j].Name == e.ClickedItem.Name)
                    MdiChildren[j].Activate();
            }

        }
        protected void OnSelectChange(object sender, EventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            ToolsClass.SelectedItem = item;
        }

        #region Изменение значений
        private void txtBrushSize_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                ToolsClass.CurWidth = int.Parse(txtBrushSize.Text);
            }
            catch
            {
                MessageBox.Show("Значение должно быть целым числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }            //кисть
        private void toolStripNumberControl_ValueChanged(object sender, EventArgs e)      //количество углов звезды
        {
            int value = Convert.ToInt32(toolStripNumberControl.Text);
            if (value < 4)
            {
                toolStripNumberControl.Text = "5";
                MessageBox.Show("Количество углов звезды не может быть меньше трех", "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                tool = new StarTool(value + 1, (ActiveMdiChild as Canvas).Controls[0] as PictureBox);
                (ActiveMdiChild as Canvas).tool = tool;
            }
        }
        #endregion Изменение значений

        #endregion События
    }
}
