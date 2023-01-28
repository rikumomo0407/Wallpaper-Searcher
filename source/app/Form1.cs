using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wallpaper_Searcher
{
    public partial class home : Form
    {
        public class ProductData
        {
            public ProductSetting setting { get; set; }
            public List<List<string>> words { get; set; }
            public List<string> rejected { get; set; }
        }

        public class ProductSetting
        {
            public bool visible { get; set; }
            public int range { get; set; }
            public int vertical { get; set; }
            public int horizontal { get; set; }
            public int min_vertical { get; set; }
            public int min_horizontal { get; set; }
        }

        public static ProductData data = ReadJson(@"materials\data.json");

        public setting settingForm = null;

        public home()
        {
            InitializeComponent();
        }

        private static ProductData ReadJson(string path)
        {
            StreamReader r = new StreamReader(path, Encoding.UTF8);
            string jsonStr = r.ReadToEnd();
            r.Close();
            ProductData jsonData = JsonSerializer.Deserialize<ProductData>(jsonStr);
            return jsonData;
        }

        static void WriteJson(ProductData data, string path)
        {
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonStr);
        }

        private static bool CheckActiveApp()
        {
            ManagementClass mc = new ManagementClass("Win32_Process");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (mo["Name"].ToString() == "scraping.exe")
                {

                    MessageBox.Show("現在、壁紙を検索中です。\n少し時間を空けてから再度実行してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                mo.Dispose();
            }
            mc.Dispose();
            return true;
        }

        private void MainButton_Click(object sender, EventArgs e)
        {
            if (CheckActiveApp())
            {
                ProcessStartInfo info = new ProcessStartInfo("scraping.exe");
                Process.Start(info);
                Application.Exit();
            }
        }

        private void buttonRejected_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("※除外リストに登録されている間この壁紙は一切表示されません\n（除外リストから解除すると表示されるようになります）", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                if (CheckActiveApp())
                {
                    data.rejected.Add(data.rejected[data.rejected[1] == "" ? 0 : 1]);
                    WriteJson(data, @"materials\data.json");
                    ProcessStartInfo info = new ProcessStartInfo("scraping.exe");
                    Process.Start(info);
                    Application.Exit();
                }
            }
        }

        private void Current_Click(object sender, EventArgs e)
        {
            info infoForm = new info();
            infoForm.Show();
        }

        private void RejectedListButton_Click(object sender, EventArgs e)
        {
            rejected rejectedForm = new rejected();
            rejectedForm.Show();
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            if (settingForm == null || settingForm.IsDisposed)
            {
                settingForm = new setting();
            }
            if (settingForm.Visible)
            {
                settingForm.Activate();
            }
            else
            {
                settingForm.Show();
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
