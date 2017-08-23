using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace sharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap myBitmap = null;
        private void button1_Click(object sender, EventArgs e) //載入圖檔
        {
            FileDialog tomatos = new OpenFileDialog();

            if (tomatos.ShowDialog() == DialogResult.OK)   ////由對話框選取圖檔
            {
                myBitmap = new Bitmap(tomatos.FileName);
                pictureBox1.Image = myBitmap;
            }
            pictureBox2.Image = null;
        }

        private void button2_Click(object sender, EventArgs e)  //均化按鈕
        {
            pictureBox2.Image = compute(myBitmap); 
        }
        private Bitmap compute(Bitmap source)
        {
            //丟到RGB

            Bitmap b = new Bitmap(pictureBox1.Image);
            //output
            int[] tomato= new int[256], strawberry = new int[256];
            int total = 0,  cdfmin = 233;
            for (int i = 0; i < 256; i++)
            {
                tomato[i] = 0;
                strawberry[i] = 0;
            }
            Color a;
            for (int i = 0; i < b.Height; i++)//存count
            {
                for (int j = 0; j < b.Width; j++)
                {
                    a = myBitmap.GetPixel(j, i);
                    
                    tomato[a.B]++;

                }
            }

            //存cdf
            for (int i = 0; i < 256; i++)
                strawberry[i] = tomato[i];
            for (int i = 1; i < 256; i++)
            {
                strawberry[i] += strawberry[i - 1];
            }
            total = strawberry[255];
            //存banana

            int banana=0;
            for (int i = 1; i < 256; i++)
            {
                if (strawberry[i] == total)
                {
                    banana = i;
                    break;
                }
            }
            
            int[] guava = new int[256];
            for (int i = 0; i <= banana; i++)
            {
                guava[i] = (int)((((float)(strawberry[i] - cdfmin) / (float)(myBitmap.Width * myBitmap.Height - cdfmin))) * 255);
            }
            //開始製圖了!!!!
            Color c;
            for (int i = 0; i < myBitmap.Height; i++)//存count
            {
                for (int j = 0; j < myBitmap.Width; j++)
                {
                    c= myBitmap.GetPixel(j, i);
                    
                    b.SetPixel(j, i, Color.FromArgb(guava[c.B], guava[c.B], guava[c.B]));
                }
            }

            return b; //傳結果
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }


}