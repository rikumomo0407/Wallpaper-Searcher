using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wallpaper_Searcher
{
    public partial class rejected : Form
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


        public static ProductData data = ReadJson(@"materials\data.json"); //data.jsonを読み込み、グローバル変数として定義

        private List<PictureBox> imageList = new List<PictureBox>();

        private List<PictureBox> checkMarkList = new List<PictureBox>();

        private List<bool> imageState = new List<bool>();

        public rejected()
        {
            InitializeComponent();
        }

        private void rejected_Load(object sender, EventArgs e)
        {
            AddImage();
            flowLayoutPanel.VerticalScroll.Visible = true;
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

        public void AddImage()
        {
            for (int i = 2; i < data.rejected.Count; i++)
            {
                PictureBox picture = new PictureBox();
                picture.Size = new Size(400, 300);
                picture.ImageLocation = data.rejected[i];
                //picture setting
                picture.SizeMode = PictureBoxSizeMode.Zoom;
                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.BackColor = Color.AliceBlue;
                picture.Cursor = Cursors.Hand;
                //define function and tag
                picture.Tag = i - 2;
                picture.Click += Picture_Click;
                //checkmark
                PictureBox checkMark = new PictureBox();
                checkMark.Size = new Size(100, 100);
                checkMark.BackColor = Color.Transparent;
                checkMark.ImageLocation = @"materials\checkmark.png";
                checkMark.SizeMode = PictureBoxSizeMode.Zoom;
                checkMark.Visible = false;
                checkMark.Click += CheckMark_Click;
                checkMark.Tag = i - 1;
                //add to list
                imageList.Add(picture);
                imageState.Add(false);
                flowLayoutPanel.Controls.Add(picture);

                picture.Controls.Add(checkMark);
                checkMarkList.Add(checkMark);
            }
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            int tagNum = (int)((PictureBox)sender).Tag;
            imageState[tagNum] = !imageState[tagNum];
            checkMarkList[tagNum].Visible = !checkMarkList[tagNum].Visible;
        }

        private void CheckMark_Click(object sender, EventArgs e)
        {
            int tagNum = (int)((PictureBox)sender).Tag;
            imageState[tagNum] = !imageState[tagNum];
            checkMarkList[tagNum].Visible = !checkMarkList[tagNum].Visible;
        }

        private void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < imageState.Count; i++) imageState[i] = true;
            foreach (PictureBox check in checkMarkList) check.Visible = true;
        }

        private void ButtonSelectCancel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < imageState.Count; i++) imageState[i] = false;
            foreach (PictureBox check in checkMarkList) check.Visible = false;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            for(int i = imageState.Count - 1; i >= 0; i--)
            {
                if(imageState[i] == true)
                {
                    data.rejected.RemoveAt(i + 2);
                }
            }
            WriteJson(data, @"materials\data.json");
            Close();
        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

