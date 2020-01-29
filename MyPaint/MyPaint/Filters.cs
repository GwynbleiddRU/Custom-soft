using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public class Filters
    {
        private static Bitmap bmap;
        public static Random rand = new Random();
        public static void Filter1(object sender, EventArgs e, PictureBox pictureBox1)
        {
            bmap = new Bitmap(pictureBox1.Image); //в оригинале "PictureBox"
            pictureBox1.Image = bmap;
            var tempbmp = new Bitmap(pictureBox1.Image);
            int i, j;
            int DispX = 1;
            int DispY = 1;
            int red, green, blue;
            {
                var withBlock = tempbmp;
                var loopTo = withBlock.Height - 2;
                for (i = 0; i <= loopTo; i++)
                {
                    var loopTo1 = withBlock.Width - 2;
                    for (j = 0; j <= loopTo1; j++)
                    {
                        System.Drawing.Color pixel1, pixel2;
                        pixel1 = withBlock.GetPixel(j, i);
                        pixel2 = withBlock.GetPixel(j + DispX, i + DispY);
                        red = Math.Min(Math.Abs(Convert.ToInt32(pixel1.R) - Convert.ToInt32(pixel2.R)) + 128, 255);
                        green = Math.Min(Math.Abs(Convert.ToInt32(pixel1.G) - Convert.ToInt32(pixel2.G)) + 128, 255);
                        blue = Math.Min(Math.Abs(Convert.ToInt32(pixel1.B) - Convert.ToInt32(pixel2.B)) + 128, 255);
                        bmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
                    }

                    if (i % 10 == 0)
                    {
                        pictureBox1.Invalidate();
                        pictureBox1.Refresh();
                        //this.Text = (100 * i / (pictureBox1.Image.Height - 2)) + "%";
                    }
                }
            }
            pictureBox1.Refresh();
        }
        
        public static void Filter2(object sender, EventArgs e, PictureBox pictureBox1)
        {
            bmap = new Bitmap(pictureBox1.Image);
            pictureBox1.Image = bmap;
            var tempbmp = new Bitmap(pictureBox1.Image);
            int DX = 1;
            int DY = 1;
            int red, green, blue;

            int i, j;
            {
                var withBlock = tempbmp;
                var loopTo = withBlock.Height - DX - 1;
                for (i = DX; i <= loopTo; i++)
                {
                    var loopTo1 = withBlock.Width - DY - 1;
                    for (j = DY; j <= loopTo1; j++)
                    {
                        red = Convert.ToInt32((double)Convert.ToInt32(withBlock.GetPixel(j, i).R) + 0.5 * (double)Convert.ToInt32(withBlock.GetPixel(j, i).R - Convert.ToInt32(withBlock.GetPixel(j - DX, i - DY).R)));
                        green = Convert.ToInt32((double)Convert.ToInt32(withBlock.GetPixel(j, i).G) + 0.7 * (double)Convert.ToInt32(withBlock.GetPixel(j, i).G - Convert.ToInt32(withBlock.GetPixel(j - DX, i - DY).G)));
                        blue = Convert.ToInt32((double)Convert.ToInt32(withBlock.GetPixel(j, i).B) + 0.5 * (double)Convert.ToInt32(withBlock.GetPixel(j, i).B - Convert.ToInt32(withBlock.GetPixel(j - DX, i - DY).B)));
                        red = Math.Min(Math.Max(red, 0), 255);
                        green = Math.Min(Math.Max(green, 0), 255);
                        blue = Math.Min(Math.Max(blue, 0), 255);
                        bmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
                    }
                    if (i % 10 == 0)
                    {
                        pictureBox1.Invalidate();
                        //this.Text = (100 * i / (pictureBox1.Image.Height - 2)) + "%";
                        pictureBox1.Refresh();
                    }
                }
            }
            pictureBox1.Refresh();
        }

        
        public static void Filter3(object sender, EventArgs e, PictureBox pictureBox1)
        {
            bmap = new Bitmap(pictureBox1.Image);
            pictureBox1.Image = bmap;
            var tempbmp = new Bitmap(pictureBox1.Image);
            int DX = 1;
            int DY = 1;
            int red, green, blue;

            int i, j;
            {
                var withBlock = tempbmp;
                var loopTo = withBlock.Height - DX - 1;
                for (i = DX; i <= loopTo; i++)
                {
                    var loopTo1 = withBlock.Width - DY - 1;
                    for (j = DY; j <= loopTo1; j++)
                    {
                        red = Convert.ToInt32((double)(Convert.ToInt32(withBlock.GetPixel(j - 1, i - 1).R) + Convert.ToInt32(withBlock.GetPixel(j - 1, i).R) + Convert.ToInt32(withBlock.GetPixel(j - 1, i + 1).R) + Convert.ToInt32(withBlock.GetPixel(j, i - 1).R) + Convert.ToInt32(withBlock.GetPixel(j, i).R) + Convert.ToInt32(withBlock.GetPixel(j, i + 1).R) + Convert.ToInt32(withBlock.GetPixel(j + 1, i - 1).R) + Convert.ToInt32(withBlock.GetPixel(j + 1, i).R) + Convert.ToInt32(withBlock.GetPixel(j + 1, i + 1).R)) / (double)9);

                        green = Convert.ToInt32((double)(Convert.ToInt32(withBlock.GetPixel(j - 1, i - 1).G) + Convert.ToInt32(withBlock.GetPixel(j - 1, i).G) + Convert.ToInt32(withBlock.GetPixel(j - 1, i + 1).G) + Convert.ToInt32(withBlock.GetPixel(j, i - 1).G) + Convert.ToInt32(withBlock.GetPixel(j, i).G) + Convert.ToInt32(withBlock.GetPixel(j, i + 1).G) + Convert.ToInt32(withBlock.GetPixel(j + 1, i - 1).G) + Convert.ToInt32(withBlock.GetPixel(j + 1, i).G) + Convert.ToInt32(withBlock.GetPixel(j + 1, i + 1).G)) / (double)9);

                        blue = Convert.ToInt32((double)(Convert.ToInt32(withBlock.GetPixel(j - 1, i - 1).B) + Convert.ToInt32(withBlock.GetPixel(j - 1, i).B) + Convert.ToInt32(withBlock.GetPixel(j - 1, i + 1).B) + Convert.ToInt32(withBlock.GetPixel(j, i - 1).B) + Convert.ToInt32(withBlock.GetPixel(j, i).B) + Convert.ToInt32(withBlock.GetPixel(j, i + 1).B) + Convert.ToInt32(withBlock.GetPixel(j + 1, i - 1).B) + Convert.ToInt32(withBlock.GetPixel(j + 1, i).B) + Convert.ToInt32(withBlock.GetPixel(j + 1, i + 1).B)) / (double)9);
                        red = Math.Min(Math.Max(red, 0), 255);
                        green = Math.Min(Math.Max(green, 0), 255);
                        blue = Math.Min(Math.Max(blue, 0), 255);
                        bmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
                    }
                    if (i % 10 == 0)
                    {
                        pictureBox1.Invalidate();
                        pictureBox1.Refresh();
                        //this.Text = (100 * i / (pictureBox1.Image.Height - 2)) + "%";
                    }
                }
            }
            pictureBox1.Refresh();
        }
        
        //public static void Function4_Click(object sender, EventArgs e, PictureBox pictureBox1)
        //{
        //    pictureBox1.Width = pictureBox1.Image.Width;
        //    pictureBox1.Height = pictureBox1.Image.Height;
        //}


        //public static void Function5_Click(object sender, EventArgs e, PictureBox pictureBox1)
        //{
        //    pictureBox1.Width = pictureBox1.Width / 2;
        //    pictureBox1.Height = pictureBox1.Height / 2;
        //}

        //public static void Function6_Click(object sender, EventArgs e, PictureBox pictureBox1)
        //{
        //    pictureBox1.Width = pictureBox1.Width * 2;
        //    pictureBox1.Height = pictureBox1.Height * 2;
        //}
        
        public static void Filter7(object sender, EventArgs e, PictureBox pictureBox1)
        {
            bmap = new Bitmap(pictureBox1.Image);
            pictureBox1.Image = bmap;
            var tempbmp = new Bitmap(pictureBox1.Image);
            int i;
            int j;
            int DX;
            int DY;
            int red;
            int green;
            int blue;
            {
                var withBlock = tempbmp;
                var loopTo = withBlock.Height - 3;
                for (i = 3; i <= loopTo; i++)
                {
                    var loopTo1 = withBlock.Width - 3;
                    for (j = 3; j <= loopTo1; j++)
                    {
                        DX = Convert.ToInt32(rand.NextDouble() * 4 - 2);
                        DY = Convert.ToInt32(rand.NextDouble() * 4 - 2);
                        red = withBlock.GetPixel(j + DX, i + DY).R;
                        green = withBlock.GetPixel(j + DX, i + DY).G;
                        blue = withBlock.GetPixel(j + DX, i + DY).B;
                        bmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
                    }
                    //this.Text = (100 * i / (withBlock.Height - 2)) + "%";
                    if (i % 10 == 0)
                    {
                        pictureBox1.Invalidate();
                        pictureBox1.Refresh();
                        //this.Text = (100 * i / (pictureBox1.Image.Height - 2)) + "%";
                    }
                }
            }
            pictureBox1.Refresh();
        }
        
        public static void Filter8(object sender, EventArgs e, PictureBox pictureBox1)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipX);
            pictureBox1.Width = pictureBox1.Height * pictureBox1.Image.Width / pictureBox1.Image.Height;

            //меняется пикчербокс, канва остается та же
            //(MainForm.ActiveMdiChild as Canvas).Size = pictureBox1.Size;
            pictureBox1.Refresh();
        }

        public static void Filter9(object sender, EventArgs e, PictureBox pictureBox1)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Width = pictureBox1.Height * pictureBox1.Image.Width / pictureBox1.Image.Height;
            pictureBox1.Refresh();
        }

        public static void Filter10(object sender, EventArgs e, PictureBox pictureBox1)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
            pictureBox1.Refresh();
        }

        public static void Filter11(object sender, EventArgs e, PictureBox pictureBox1)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox1.Refresh();
        }
        
    }
}
