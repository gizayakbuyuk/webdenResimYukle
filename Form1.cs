using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Picturebox_a_webden_resim_yüklemek
{
    public partial class Form1 : Form
    {
        static string kaynakKoduGetir(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            using (StreamReader sRead = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                return sRead.ReadToEnd();
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        void kucukResimOlustur(string yol)
        {
            try
            {
                PictureBox kResim = new PictureBox();
                kResim.Load(yol);
                kResim.Width = 100;
                kResim.Height = 100;
                kResim.SizeMode = PictureBoxSizeMode.StretchImage;
                //kResim.BorderStyle = BorderStyle.FixedSingle;
                kResim.Tag = yol;
                kResim.Click += new EventHandler(kResim_Click);
                kResim.MouseHover += new EventHandler(kResim_Üzerinde);
                flowLayoutPanel1.Controls.Add(kResim);
            }
            catch  { }
            
        }
        private void kResim_Üzerinde(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                ((PictureBox)flowLayoutPanel1.Controls[i]).BorderStyle = BorderStyle.None;
            ((PictureBox)sender).BorderStyle = BorderStyle.FixedSingle;

        }
        private void kResim_Click(object sender, EventArgs e)
        {
            string yol=((PictureBox)sender).Tag.ToString();
            pictureBox1.Load(yol);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
                //pictureBox1.Load(textBox1.Text);
                string html= kaynakKoduGetir(textBox1.Text);
                listBox1.Items.Clear();
                flowLayoutPanel1.Controls.Clear();
                progressBar1.Maximum = html.Length;
                progressBar1.Value = 0;

                while (true)
                {
                    int imgbul = html.IndexOf("<img")+4;
                    if (imgbul == 3) break;
                    int srcbul=html.IndexOf("src=",imgbul)+5;
                    if (srcbul == 4) break;
                    int trnkbul = html.IndexOf('"', srcbul );
                    if (trnkbul == -1) break;
                    string yol = html.Substring(srcbul, trnkbul - srcbul);
                    if (yol != "")
                    {
                        if (yol.StartsWith("http://") == false)
                            yol = textBox1.Text + yol;
                        if (listBox1.Items.IndexOf(yol) == -1)
                        {
                            listBox1.Items.Add(yol);
                            kucukResimOlustur(yol);
                        }
                    }
                    html = html.Substring(trnkbul, html.Length - trnkbul);
                    progressBar1.Value = progressBar1.Maximum - html.Length;
                    Application.DoEvents();
                }
                progressBar1.Value = 0;
                
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Load(listBox1.Text);
            }
            catch (Exception)
            {
                
                
            }
            
        }
    }
}
