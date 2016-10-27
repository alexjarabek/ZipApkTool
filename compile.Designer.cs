namespace sandGlass
{
    partial class compile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(compile));
            this.getFile = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGameName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGame = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.channelBoxs = new System.Windows.Forms.CheckedListBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbIco = new System.Windows.Forms.Label();
            this.btnPic = new System.Windows.Forms.Button();
            this.picBoxIco = new System.Windows.Forms.PictureBox();
            this.getPicFile = new System.Windows.Forms.OpenFileDialog();
            this.signPanel = new System.Windows.Forms.Panel();
            this.cfgLb = new System.Windows.Forms.Label();
            this.choseSignFileBtn = new System.Windows.Forms.Button();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtKeyPwd = new System.Windows.Forms.TextBox();
            this.txtSignPwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbPwdFile = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.getPwdFile = new System.Windows.Forms.OpenFileDialog();
            this.btnNewtool = new System.Windows.Forms.Button();
            this.skinUI1 = new DotNetSkin.SkinUI();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxIco)).BeginInit();
            this.signPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "游戏名 ：";
            // 
            // txtGameName
            // 
            this.txtGameName.Location = new System.Drawing.Point(120, 93);
            this.txtGameName.Name = "txtGameName";
            this.txtGameName.Size = new System.Drawing.Size(205, 21);
            this.txtGameName.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "游戏缩写：";
            // 
            // txtGame
            // 
            this.txtGame.Location = new System.Drawing.Point(120, 132);
            this.txtGame.Name = "txtGame";
            this.txtGame.Size = new System.Drawing.Size(133, 21);
            this.txtGame.TabIndex = 10;
            this.txtGame.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtGame_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(442, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "渠道列表";
            // 
            // channelBoxs
            // 
            this.channelBoxs.FormattingEnabled = true;
            this.channelBoxs.Location = new System.Drawing.Point(434, 89);
            this.channelBoxs.Name = "channelBoxs";
            this.channelBoxs.Size = new System.Drawing.Size(290, 420);
            this.channelBoxs.TabIndex = 13;
            this.channelBoxs.SelectedIndexChanged += new System.EventHandler(this.btnPic_Click);
            this.channelBoxs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.channelBoxs_MouseUp);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(536, 525);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(61, 412);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "图标：";
            // 
            // lbIco
            // 
            this.lbIco.Location = new System.Drawing.Point(76, 536);
            this.lbIco.Name = "lbIco";
            this.lbIco.Size = new System.Drawing.Size(278, 12);
            this.lbIco.TabIndex = 17;
            // 
            // btnPic
            // 
            this.btnPic.Location = new System.Drawing.Point(141, 551);
            this.btnPic.Name = "btnPic";
            this.btnPic.Size = new System.Drawing.Size(75, 23);
            this.btnPic.TabIndex = 18;
            this.btnPic.Text = "选择ico";
            this.btnPic.UseVisualStyleBackColor = true;
            this.btnPic.Click += new System.EventHandler(this.btnPic_Click);
            // 
            // picBoxIco
            // 
            this.picBoxIco.Location = new System.Drawing.Point(117, 354);
            this.picBoxIco.Name = "picBoxIco";
            this.picBoxIco.Size = new System.Drawing.Size(192, 192);
            this.picBoxIco.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxIco.TabIndex = 19;
            this.picBoxIco.TabStop = false;
            // 
            // getPicFile
            // 
            this.getPicFile.Filter = "游戏ico|*.png;*.jpg";
            // 
            // signPanel
            // 
            this.signPanel.Controls.Add(this.cfgLb);
            this.signPanel.Controls.Add(this.choseSignFileBtn);
            this.signPanel.Controls.Add(this.txtAlias);
            this.signPanel.Controls.Add(this.label7);
            this.signPanel.Controls.Add(this.txtKeyPwd);
            this.signPanel.Controls.Add(this.txtSignPwd);
            this.signPanel.Controls.Add(this.label1);
            this.signPanel.Controls.Add(this.label3);
            this.signPanel.Controls.Add(this.lbPwdFile);
            this.signPanel.Controls.Add(this.label8);
            this.signPanel.Location = new System.Drawing.Point(40, 159);
            this.signPanel.Name = "signPanel";
            this.signPanel.Size = new System.Drawing.Size(365, 189);
            this.signPanel.TabIndex = 20;
            // 
            // cfgLb
            // 
            this.cfgLb.AutoSize = true;
            this.cfgLb.Location = new System.Drawing.Point(109, 157);
            this.cfgLb.Name = "cfgLb";
            this.cfgLb.Size = new System.Drawing.Size(35, 12);
            this.cfgLb.TabIndex = 32;
            this.cfgLb.Text = "cfgLb";
            // 
            // choseSignFileBtn
            // 
            this.choseSignFileBtn.Location = new System.Drawing.Point(275, 114);
            this.choseSignFileBtn.Name = "choseSignFileBtn";
            this.choseSignFileBtn.Size = new System.Drawing.Size(75, 23);
            this.choseSignFileBtn.TabIndex = 31;
            this.choseSignFileBtn.Text = "签名文件";
            this.choseSignFileBtn.UseVisualStyleBackColor = true;
            this.choseSignFileBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(101, 50);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(235, 21);
            this.txtAlias.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "别名：";
            // 
            // txtKeyPwd
            // 
            this.txtKeyPwd.Location = new System.Drawing.Point(101, 79);
            this.txtKeyPwd.Name = "txtKeyPwd";
            this.txtKeyPwd.Size = new System.Drawing.Size(235, 21);
            this.txtKeyPwd.TabIndex = 28;
            // 
            // txtSignPwd
            // 
            this.txtSignPwd.Location = new System.Drawing.Point(101, 16);
            this.txtSignPwd.Name = "txtSignPwd";
            this.txtSignPwd.Size = new System.Drawing.Size(235, 21);
            this.txtSignPwd.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "keyStore密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "签名密码：";
            // 
            // lbPwdFile
            // 
            this.lbPwdFile.AutoSize = true;
            this.lbPwdFile.Location = new System.Drawing.Point(107, 119);
            this.lbPwdFile.Name = "lbPwdFile";
            this.lbPwdFile.Size = new System.Drawing.Size(59, 12);
            this.lbPwdFile.TabIndex = 24;
            this.lbPwdFile.Text = "lbPwdFile";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "签名文件：";
            // 
            // getPwdFile
            // 
            this.getPwdFile.Filter = "签名文件|*.keystore;*.jks";
            // 
            // btnNewtool
            // 
            this.btnNewtool.Location = new System.Drawing.Point(57, 13);
            this.btnNewtool.Name = "btnNewtool";
            this.btnNewtool.Size = new System.Drawing.Size(139, 39);
            this.btnNewtool.TabIndex = 21;
            this.btnNewtool.Text = "打包工具2.0入口";
            this.btnNewtool.UseVisualStyleBackColor = true;
            this.btnNewtool.Click += new System.EventHandler(this.btnNewtool_Click);
            // 
            // skinUI1
            // 
            this.skinUI1.Active = true;
            this.skinUI1.Button = true;
            this.skinUI1.Caption = true;
            this.skinUI1.CheckBox = true;
            this.skinUI1.ComboBox = true;
            this.skinUI1.ContextMenu = true;
            this.skinUI1.DisableTag = 999;
            this.skinUI1.Edit = true;
            this.skinUI1.GroupBox = true;
            this.skinUI1.ImageList = null;
            this.skinUI1.MaiMenu = true;
            this.skinUI1.Panel = true;
            this.skinUI1.Progress = true;
            this.skinUI1.RadioButton = true;
            this.skinUI1.ScrollBar = true;
            this.skinUI1.SkinFile = "F:\\WinForm\\dotnetskin2005\\skins\\black-BLACK.skn";
            this.skinUI1.SkinSteam = null;
            this.skinUI1.Spin = true;
            this.skinUI1.StatusBar = true;
            this.skinUI1.SystemMenu = true;
            this.skinUI1.TabControl = true;
            this.skinUI1.Text = "Mycontrol1=edit\r\nMycontrol2=edit\r\n";
            this.skinUI1.ToolBar = true;
            this.skinUI1.TrackBar = true;
            // 
            // compile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 586);
            this.Controls.Add(this.btnNewtool);
            this.Controls.Add(this.signPanel);
            this.Controls.Add(this.picBoxIco);
            this.Controls.Add(this.btnPic);
            this.Controls.Add(this.lbIco);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.channelBoxs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGame);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtGameName);
            this.Controls.Add(this.label4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "compile";
            this.Text = "step 1.游戏设置    v1.0.1";
            this.Load += new System.EventHandler(this.compile_Load);
            this.Click += new System.EventHandler(this.compile_Click);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxIco)).EndInit();
            this.signPanel.ResumeLayout(false);
            this.signPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog getFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGameName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox channelBoxs;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbIco;
        private System.Windows.Forms.Button btnPic;
        private System.Windows.Forms.PictureBox picBoxIco;
        private System.Windows.Forms.OpenFileDialog getPicFile;
        private System.Windows.Forms.Panel signPanel;
        private System.Windows.Forms.TextBox txtAlias;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtKeyPwd;
        private System.Windows.Forms.TextBox txtSignPwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbPwdFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button choseSignFileBtn;
        private System.Windows.Forms.OpenFileDialog getPwdFile;
        private System.Windows.Forms.Label cfgLb;
        private System.Windows.Forms.Button btnNewtool;
        private DotNetSkin.SkinUI skinUI1;
    }
}