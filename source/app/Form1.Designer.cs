namespace Wallpaper_Searcher
{
    partial class home
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(home));
            this.mainButton = new System.Windows.Forms.Button();
            this.settingButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRejected = new System.Windows.Forms.Button();
            this.rejectedListButton = new System.Windows.Forms.Button();
            this.buttonCurrent = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainButton
            // 
            this.mainButton.Location = new System.Drawing.Point(236, 378);
            this.mainButton.Name = "mainButton";
            this.mainButton.Size = new System.Drawing.Size(181, 55);
            this.mainButton.TabIndex = 0;
            this.mainButton.Text = "壁紙を変更";
            this.mainButton.UseVisualStyleBackColor = true;
            this.mainButton.Click += new System.EventHandler(this.MainButton_Click);
            // 
            // settingButton
            // 
            this.settingButton.Location = new System.Drawing.Point(236, 521);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(181, 55);
            this.settingButton.TabIndex = 1;
            this.settingButton.Text = "設定";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(450, 521);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(181, 55);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "終了";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(213, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(448, 53);
            this.label1.TabIndex = 3;
            this.label1.Text = "Wallpaper Searcher";
            // 
            // buttonRejected
            // 
            this.buttonRejected.Location = new System.Drawing.Point(450, 378);
            this.buttonRejected.Name = "buttonRejected";
            this.buttonRejected.Size = new System.Drawing.Size(181, 55);
            this.buttonRejected.TabIndex = 4;
            this.buttonRejected.Text = "壁紙を除外";
            this.buttonRejected.UseVisualStyleBackColor = true;
            this.buttonRejected.Click += new System.EventHandler(this.buttonRejected_Click);
            // 
            // rejectedListButton
            // 
            this.rejectedListButton.Location = new System.Drawing.Point(450, 448);
            this.rejectedListButton.Name = "rejectedListButton";
            this.rejectedListButton.Size = new System.Drawing.Size(181, 55);
            this.rejectedListButton.TabIndex = 5;
            this.rejectedListButton.Text = "除外リスト";
            this.rejectedListButton.UseVisualStyleBackColor = true;
            this.rejectedListButton.Click += new System.EventHandler(this.RejectedListButton_Click);
            // 
            // buttonCurrent
            // 
            this.buttonCurrent.Location = new System.Drawing.Point(236, 448);
            this.buttonCurrent.Name = "buttonCurrent";
            this.buttonCurrent.Size = new System.Drawing.Size(181, 55);
            this.buttonCurrent.TabIndex = 6;
            this.buttonCurrent.Text = "壁紙について";
            this.buttonCurrent.UseVisualStyleBackColor = true;
            this.buttonCurrent.Click += new System.EventHandler(this.Current_Click);
            // 
            // home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(902, 668);
            this.Controls.Add(this.buttonCurrent);
            this.Controls.Add(this.rejectedListButton);
            this.Controls.Add(this.buttonRejected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.settingButton);
            this.Controls.Add(this.mainButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "home";
            this.Text = "Wallpaper Searcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button mainButton;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonRejected;
        private System.Windows.Forms.Button rejectedListButton;
        private System.Windows.Forms.Button buttonCurrent;
    }
}

