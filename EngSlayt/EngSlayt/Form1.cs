using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EngSlayt.Helper;

namespace EngSlayt
{
    public partial class Form1 : Form
    {
        FileStream fs = null;
        FileInfo fi = null;
        DirectoryInfo di = null;
        List<string> fileList = null;
        System.Windows.Forms.Timer slaytTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer windowVisibleTimer = new System.Windows.Forms.Timer();
        int imageIndex = 0;
        Random random = new Random();
        int slaytStartMinValue;
        int slaytStartMaxValue;
        int slaytDurationValue;
        IniFile ini = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.Restore();
                ini = new IniFile(@"C:\EngSlayt\Options.ini");
                slaytStartMinValue = Convert.ToInt32(ini.IniReadValue("SlaytStartValues", "slaytStartMinValue"));
                slaytStartMaxValue = Convert.ToInt32(ini.IniReadValue("SlaytStartValues", "slaytStartMaxValue"));
                slaytDurationValue = Convert.ToInt32(ini.IniReadValue("SlaytStartValues", "slaytDurationValue"));

                windowVisibleTimer.Tick += new System.EventHandler(windowVisibleTimer_Tick);
                windowVisibleTimer.Interval = random.Next(slaytStartMinValue, slaytStartMaxValue);

                slaytTimer.Tick += new System.EventHandler(slaytTimer_Tick);
                slaytTimer.Interval = slaytDurationValue;

                fileList = new List<string>();
                di = new DirectoryInfo(@"C:\EngSlayt");
                string[] files = Directory.GetFiles(di.FullName, "*.jpg");
                foreach (string item in files)
                {
                    int pos = item.LastIndexOf("||");
                    string FName = item.Substring(pos + 1);
                    fileList.Add(FName);
                }
                if (fileList.Count > 0)
                {
                    windowVisibleTimer.Start();
                }
                else
                {
                    MessageBox.Show("Klasörde Slayt Bulunamadı!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void windowVisibleTimer_Tick(object sender, EventArgs e)
        {
            this.Visible = true;
            if (fileList.Count > 0)
            {
                slaytTimer.Start();
                this.WindowState = FormWindowState.Normal;
                windowVisibleTimer.Stop();
            }
        }

        private void slaytTimer_Tick(object sender, EventArgs e)
        {
            // Util.Animate(pictureBox1, Util.Effect.Roll, 250, 250);

            if (imageIndex + 1 > fileList.Count)
            {
                imageIndex = 0;
                slaytTimer.Stop();
                this.Visible = false;
                windowVisibleTimer.Start();
            }

            pictureBox1.ImageLocation = fileList[imageIndex];
            this.Text = (imageIndex+1).ToString();
            pictureBox1.Refresh();
            imageIndex++;
            //Thread.Sleep(1000);
            // Util.Animate(pictureBox1, Util.Effect.Roll, 250, 250);
        }


    }
    
}
