using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using File = System.IO.File;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wallpaper_Searcher
{
    public partial class setting : Form
    {
        public string imagePath;

        [DllImport("user32.dll")]
        public static extern bool SetSysColors(int cElements, int[] lpaElements, int[] lpaRgbValues);

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

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

        public static ProductData data = ReadJson(@"script\data.json"); //data.jsonを読み込み、グローバル変数として定義

        public setting()
        {
            InitializeComponent();
        }

        private void setting_Load(object sender, EventArgs e)
        {
            //検索ワード一覧の初期化
            wordList.ColumnHeadersVisible = false; // 一番上の列を非表示にする
            wordList.RowHeadersVisible = false;  // 一番左の列を非表示にする
            wordList.AllowUserToAddRows = false; // 一番下の列を非表示にする
            wordList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //列幅を最大にする
            wordList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //行の高さを自動調整する
            wordList.ColumnCount = 1; // カラム数を指定
            foreach (List<string> word in data.words) //表を作成
            {
                wordList.Rows.Add(word[0]);
            }
            //調整方法の初期化
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true); //https://learn.microsoft.com/en-us/answers/questions/803547/how-to-change-the-wallpaper-position-or-fit-using.html
            string wallpaperStyleValue = key.GetValue(@"WallpaperStyle").ToString();
            string tileWallpaperValue = key.GetValue(@"TileWallpaper").ToString();
            if (wallpaperStyleValue == "10" && tileWallpaperValue == "0")
            {
                comboBox1.SelectedIndex = 0;
            }
            else if(wallpaperStyleValue == "6" && tileWallpaperValue == "0")
            {
                comboBox1.SelectedIndex = 1;
            }
            else if (wallpaperStyleValue == "2" && tileWallpaperValue == "0")
            {
                comboBox1.SelectedIndex = 2;
            }
            else if (wallpaperStyleValue == "0" && tileWallpaperValue == "1")
            {
                comboBox1.SelectedIndex = 3;
            }
            else if (wallpaperStyleValue == "0" && tileWallpaperValue == "0")
            {
                comboBox1.SelectedIndex = 4;
            }
            else if (wallpaperStyleValue == "22" && tileWallpaperValue == "0")
            {
                comboBox1.SelectedIndex = 5;
            }
            key.Close();
            //数値入力欄の初期化
            numericVertical.Value = data.setting.vertical;
            numericHorizontal.Value = data.setting.horizontal;
            numericMinVertical.Value = data.setting.min_vertical;
            numericMinHorizontal.Value = data.setting.min_horizontal;
            numericSearchRange.Value = data.setting.range;
            //色選択の初期化
            key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors", true); //https://learn.microsoft.com/en-us/answers/questions/803547/how-to-change-the-wallpaper-position-or-fit-using.html
            string colorValue = key.GetValue(@"Background").ToString();
            List<int> backgroundRgb = new List<int>();
            foreach (string rgb in colorValue.Split(' '))
            {
                backgroundRgb.Add(int.Parse(rgb));
            }
            colorPanel.BackColor = Color.FromArgb(backgroundRgb[0], backgroundRgb[1], backgroundRgb[2]);
            key.Close();
            //ラジオボタンの初期化
            if (data.setting.visible)
            {
                radioButton3.Checked = true;
            }
            else
            {
                radioButton4.Checked = true;
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorPanel.BackColor = colorDialog.Color;
            }
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

        private void dataSave()
        {
            data.setting.vertical = (int)numericVertical.Value;
            data.setting.min_vertical = (int)numericMinVertical.Value;
            data.setting.horizontal = (int)numericHorizontal.Value;
            data.setting.min_horizontal = (int)numericMinHorizontal.Value;
            data.setting.range = (int)numericSearchRange.Value;
            WriteJson(data, @"script\data.json");
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true); //https://learn.microsoft.com/en-us/answers/questions/803547/how-to-change-the-wallpaper-position-or-fit-using.html
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    key.SetValue(@"WallpaperStyle", "10");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case 1:
                    key.SetValue(@"WallpaperStyle", "6");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case 2:
                    key.SetValue(@"WallpaperStyle", "2");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case 3:
                    key.SetValue(@"WallpaperStyle", "0");
                    key.SetValue(@"TileWallpaper", "1");
                    break;
                case 4:
                    key.SetValue(@"WallpaperStyle", "0");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case 5:
                    key.SetValue(@"WallpaperStyle", "22");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                default:
                    MessageBox.Show("この値は使用できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            imagePath = key.GetValue(@"WallPaper").ToString();
            key.Close();
            int[] testList = {1};
            int[] testColor = {ColorTranslator.ToWin32(colorPanel.BackColor)};
            SetSysColors(testList.Length, testList, testColor);
            key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors", true); //https://learn.microsoft.com/en-us/answers/questions/803547/how-to-change-the-wallpaper-position-or-fit-using.html
            key.SetValue(@"Background", colorPanel.BackColor.R.ToString() + ' ' + colorPanel.BackColor.G.ToString() + ' ' + colorPanel.BackColor.B.ToString());
            key.Close();
            SystemParametersInfo(20, 0, imagePath, 1 | 2);
            for (int i = 0; i < data.words.Count; i++)
            {
                if (data.words[i][1] == "")
                {
                    ProcessStartInfo info = new ProcessStartInfo(@"script\findurl.exe");
                    Process.Start(info);
                    break;
                }
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            dataSave();
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "ファイルを選択してください";
            dialog.FileName = @"script\data.json";
            dialog.InitialDirectory = "C:/";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("現在の設定が上書きされます。置き換えますか?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    ProductData loadData = ReadJson(dialog.FileName);
                    WriteJson(loadData, @"script\data.json");
                }
            }
        }

        private void menuReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("現在の設定がリセットされます。よろしいですか?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                ProductData resetData = ReadJson(@"script\default-data.json");
                WriteJson(resetData, @"script\data.json");
                Application.Restart();
            }
        }

        private void menuDev_Click(object sender, EventArgs e)
        {
            groupBox6.Enabled = !groupBox6.Enabled;
            groupBox9.Enabled = !groupBox9.Enabled;
            menuDev.Text = "開発者向けの設定を" + (groupBox6.Visible == true ? "非" : "") + "表示にする";
        }

        private void menuSaveExit_Click(object sender, EventArgs e)
        {
            dataSave();
            Close();
        }

        private void menuUnsaveExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void wordAddButton_Click(object sender, EventArgs e)
        {
            data.words.Add(new List<string> { wordAdd.Text, "" });
            wordList.Rows.Add(wordAdd.Text);
            wordList.ClearSelection();　//選択行の更新
            wordList.Rows[data.words.Count - 1].Selected = true;
            wordAdd.Text = null; //テキストの削除
        }

        private void wordDelButton_Click(object sender, EventArgs e)
        {
            if (data.words.Count > wordList.SelectedRows.Count)
            {
                foreach (DataGridViewRow item in wordList.SelectedRows)
                {

                    data.words.RemoveAt(item.Index);
                    wordList.Rows.Remove(item); //選択行の削除
                }
            }
            else
            {
                MessageBox.Show("検索ワードは最低でも1つ必要です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void wordSearchButton_Click(object sender, EventArgs e)
        {
            wordList.ClearSelection();
            int selectHeader = 0; //先頭行を格納
            for (int i = data.words.Count - 1; i >= 0; i--)
            {
                if (data.words[i][0] == wordSearch.Text)
                {
                    wordList.Rows[i].Selected = true;
                    selectHeader = i;
                }
            }
            wordList.FirstDisplayedScrollingRowIndex = selectHeader; //結果までスクロール
            wordAdd.Text = null; //テキストの削除
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            data.setting.visible = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            data.setting.visible = false;
        }

        private void checkBoxStartUp_CheckedChanged(object sender, EventArgs e)
        {
            string originalPath = "script/start-up/shortcut/scraping.lnk";
            string startUpPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "/scraping.lnk";　//スタートアップのパス
            if (File.Exists(startUpPath)) //スタートアップ有無
            {
                try
                {
                    File.Delete(startUpPath); //スタートアップフォルダから削除
                }
                catch (FileNotFoundException)
                {
                    if (File.Exists(startUpPath)) //元のフォルダに戻されたか確認
                    {
                        checkBoxStartUp.Checked = true;
                        MessageBox.Show("スタートアップからの削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                try
                {
                    File.Copy(originalPath, startUpPath); //スタートアップフォルダにショートカットを移動
                }
                catch (FileNotFoundException)
                {
                    if (!File.Exists(startUpPath)) //スタートアップフォルダに登録されたか確認
                    {
                        checkBoxStartUp.Checked = false;
                        MessageBox.Show("スタートアップへの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
