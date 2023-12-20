using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace görüntü_işleme_projesi
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog sdf = new OpenFileDialog();
            sdf.Filter = "resimler|.*bmp*|All Files|*.*";
            if (sdf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;

            }
            pictureBox1.ImageLocation = sdf.FileName;
        }
        private static Rectangle FindImageOnScreen(Bitmap bmpMatch, bool ExactMatch)
        {
            Bitmap ScreenBmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(ScreenBmp))
            {
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.Location, Point.Empty, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }

            BitmapData ImgBmd = bmpMatch.LockBits(new Rectangle(0, 0, bmpMatch.Width, bmpMatch.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData ScreenBmd = ScreenBmp.LockBits(Screen.PrimaryScreen.Bounds, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            byte[] ImgByts = new byte[(Math.Abs(ImgBmd.Stride) * bmpMatch.Height) - 1 + 1];
            byte[] ScreenByts = new byte[(Math.Abs(ScreenBmd.Stride) * ScreenBmp.Height) - 1 + 1];
            Marshal.Copy(ImgBmd.Scan0, ImgByts, 0, ImgByts.Length);
            Marshal.Copy(ScreenBmd.Scan0, ScreenByts, 0, ScreenByts.Length);

            bool FoundMatch = false;
            Rectangle rct = Rectangle.Empty;
            int sindx, iindx;
            int spc, ipc;

            int skpx = System.Convert.ToInt32((bmpMatch.Width - 1) / (double)10);
            if (skpx < 1 | ExactMatch)
                skpx = 1;
            int skpy = System.Convert.ToInt32((bmpMatch.Height - 1) / (double)10);
            if (skpy < 1 | ExactMatch)
                skpy = 1;

            for (int si = 0; si <= ScreenByts.Length - 1; si += 3)
            {
                FoundMatch = true;
                for (int iy = 0; iy <= ImgBmd.Height - 1; iy += skpy)
                {
                    for (int ix = 0; ix <= ImgBmd.Width - 1; ix += skpx)
                    {
                        sindx = (iy * ScreenBmd.Stride) + (ix * 3) + si;
                        iindx = (iy * ImgBmd.Stride) + (ix * 3);
                        spc = Color.FromArgb(ScreenByts[sindx + 2], ScreenByts[sindx + 1], ScreenByts[sindx]).ToArgb();
                        ipc = Color.FromArgb(ImgByts[iindx + 2], ImgByts[iindx + 1], ImgByts[iindx]).ToArgb();
                        if (spc != ipc)
                        {
                            FoundMatch = false;
                            iy = ImgBmd.Height - 1;
                            ix = ImgBmd.Width - 1;
                        }
                    }
                }
                if (FoundMatch)
                {
                    double r = si / (double)(ScreenBmp.Width * 3);
                    double c = ScreenBmp.Width * (r % 1);
                    if (r % 1 >= 0.5)
                        r -= 1;
                    rct.X = System.Convert.ToInt32(c);
                    rct.Y = System.Convert.ToInt32(r);
                    rct.Width = bmpMatch.Width;
                    rct.Height = bmpMatch.Height;
                    break;
                }
            }
            bmpMatch.UnlockBits(ImgBmd);
            ScreenBmp.UnlockBits(ScreenBmd);
            ScreenBmp.Dispose();
            return rct;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            {
                OpenFileDialog sdf = new OpenFileDialog();
                sdf.Filter = "resimler|.*bmp*|All Files|*.*";
                if (sdf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;

                }
                pictureBox2.ImageLocation = sdf.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //Bitmap ImgToFind = new Bitmap(@"C:\Users\xxxx\bin\Release\image2.png");
            //Bitmap ImgToFind1 = new Bitmap(ImgToFind.Width, ImgToFind.Height, PixelFormat.Format24bppRgb);
            Bitmap ImgToFind = new Bitmap(pictureBox2.Image);
            var ImgToFind1 = new Bitmap(pictureBox1.Image);
            int Width1 = ImgToFind.Width;
            int Height1 = ImgToFind.Height;


            Rectangle rect = FindImageOnScreen(ImgToFind1, true);
            if (rect != Rectangle.Empty)
            {
                Console.WriteLine(rect.Location.X + " " + rect.Location.Y);
                Point cntr = new Point(rect.X + System.Convert.ToInt32(rect.Width / (double)2), rect.Y + System.Convert.ToInt32(rect.Height / (double)2));
                //Point cntr = new Point(rect.X , rect.Y);
                Cursor.Position = cntr;
                //Graphics cızım;
                int re = rect.X;
                int rey = rect.Y;

                Graphics g;
                Pen kalem = new Pen(Color.Red, 3);
                g = pictureBox2.CreateGraphics();
                g.DrawEllipse(kalem, Cursor.Position.X - 76, Cursor.Position.Y - 165, 35, 35);
                //cızım = pbxkolerasyonyeni.CreateGraphics();
                //cızım.DrawEllipse(kalem2, 150 ,rey,5,5);
                //MessageBox.Show("bulundu");
            }
            else
                MessageBox.Show("Image not found");
        }
    }
}
    

