using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Wallpaper_Searcher
{
    public partial class info : Form
    {

        public info()
        {
            InitializeComponent();
        }

        private void info_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = @"script\images\wallpaper.png";
        }

        private void buttonReference_Click(object sender, EventArgs e)
        {
            StreamReader r = new StreamReader(@"script\images\detail.txt", Encoding.UTF8);
            string detailURL = r.ReadLine();
            r.Close();
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = detailURL,
                UseShellExecute = true,
            };
            Process.Start(info);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "画像を保存";
            dialog.FileName = "wallpaper.png";
            dialog.InitialDirectory = "C:/";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(@"script\images\wallpaper.png", dialog.FileName);
            }
        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
