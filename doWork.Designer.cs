namespace sandGlass
{
    partial class doWork
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(doWork));
            this.getGame = new System.Windows.Forms.Button();
            this.lbGamePath = new System.Windows.Forms.Label();
            this.getGameFile = new System.Windows.Forms.OpenFileDialog();
            this.per = new System.Windows.Forms.Label();
            this.lbShowChannel = new System.Windows.Forms.Label();
            this.plChannel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.makeRNo = new System.Windows.Forms.RadioButton();
            this.makeRYes = new System.Windows.Forms.RadioButton();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtKeyPwd = new System.Windows.Forms.TextBox();
            this.txtSignPwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbGameApk = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbGame = new System.Windows.Forms.Label();
            this.btnGetPwdFile = new System.Windows.Forms.Button();
            this.lbPwdFile = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.getPwdFile = new System.Windows.Forms.OpenFileDialog();
            this.btnPackage = new System.Windows.Forms.Button();
            this.curEvent = new System.Windows.Forms.Label();
            this.pbT = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTxtInfo = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // getGame
            // 
            this.getGame.Location = new System.Drawing.Point(87, 254);
            this.getGame.Name = "getGame";
            this.getGame.Size = new System.Drawing.Size(75, 23);
            this.getGame.TabIndex = 0;
            this.getGame.Text = "选择游戏包";
            this.getGame.UseVisualStyleBackColor = true;
            this.getGame.Click += new System.EventHandler(this.getGame_Click);
            // 
            // lbGamePath
            // 
            this.lbGamePath.AutoSize = true;
            this.lbGamePath.Location = new System.Drawing.Point(129, 45);
            this.lbGamePath.Name = "lbGamePath";
            this.lbGamePath.Size = new System.Drawing.Size(0, 12);
            this.lbGamePath.TabIndex = 1;
            // 
            // getGameFile
            // 
            this.getGameFile.Filter = "游戏母包|*.apk";
            this.getGameFile.FileOk += new System.ComponentModel.CancelEventHandler(this.getGameFile_FileOk);
            // 
            // per
            // 
            this.per.AutoSize = true;
            this.per.Location = new System.Drawing.Point(193, 100);
            this.per.Name = "per";
            this.per.Size = new System.Drawing.Size(0, 12);
            this.per.TabIndex = 3;
            // 
            // lbShowChannel
            // 
            this.lbShowChannel.AutoSize = true;
            this.lbShowChannel.Location = new System.Drawing.Point(27, 292);
            this.lbShowChannel.Name = "lbShowChannel";
            this.lbShowChannel.Size = new System.Drawing.Size(53, 12);
            this.lbShowChannel.TabIndex = 4;
            this.lbShowChannel.Text = "渠道列表";
            // 
            // plChannel
            // 
            this.plChannel.AutoScroll = true;
            this.plChannel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.plChannel.Location = new System.Drawing.Point(29, 331);
            this.plChannel.Name = "plChannel";
            this.plChannel.Size = new System.Drawing.Size(413, 220);
            this.plChannel.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.makeRNo);
            this.panel1.Controls.Add(this.makeRYes);
            this.panel1.Controls.Add(this.txtAlias);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtKeyPwd);
            this.panel1.Controls.Add(this.txtSignPwd);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lbGameApk);
            this.panel1.Controls.Add(this.lbName);
            this.panel1.Controls.Add(this.lbGame);
            this.panel1.Controls.Add(this.btnGetPwdFile);
            this.panel1.Controls.Add(this.lbPwdFile);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.getGame);
            this.panel1.Location = new System.Drawing.Point(29, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 277);
            this.panel1.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "是否重新生成渠道资源：";
            // 
            // makeRNo
            // 
            this.makeRNo.AutoSize = true;
            this.makeRNo.Checked = true;
            this.makeRNo.Location = new System.Drawing.Point(207, 69);
            this.makeRNo.Name = "makeRNo";
            this.makeRNo.Size = new System.Drawing.Size(35, 16);
            this.makeRNo.TabIndex = 25;
            this.makeRNo.TabStop = true;
            this.makeRNo.Text = "否";
            this.makeRNo.UseVisualStyleBackColor = true;
            // 
            // makeRYes
            // 
            this.makeRYes.AutoSize = true;
            this.makeRYes.Location = new System.Drawing.Point(148, 69);
            this.makeRYes.Name = "makeRYes";
            this.makeRYes.Size = new System.Drawing.Size(35, 16);
            this.makeRYes.TabIndex = 24;
            this.makeRYes.Text = "是";
            this.makeRYes.UseVisualStyleBackColor = true;
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(87, 131);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(235, 21);
            this.txtAlias.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "别名：";
            // 
            // txtKeyPwd
            // 
            this.txtKeyPwd.Location = new System.Drawing.Point(87, 160);
            this.txtKeyPwd.Name = "txtKeyPwd";
            this.txtKeyPwd.Size = new System.Drawing.Size(235, 21);
            this.txtKeyPwd.TabIndex = 20;
            // 
            // txtSignPwd
            // 
            this.txtSignPwd.Location = new System.Drawing.Point(87, 97);
            this.txtSignPwd.Name = "txtSignPwd";
            this.txtSignPwd.Size = new System.Drawing.Size(235, 21);
            this.txtSignPwd.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "keyStore密码";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "签名密码：";
            // 
            // lbGameApk
            // 
            this.lbGameApk.AutoSize = true;
            this.lbGameApk.Location = new System.Drawing.Point(73, 235);
            this.lbGameApk.Name = "lbGameApk";
            this.lbGameApk.Size = new System.Drawing.Size(0, 12);
            this.lbGameApk.TabIndex = 16;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(85, 16);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(0, 12);
            this.lbName.TabIndex = 15;
            // 
            // lbGame
            // 
            this.lbGame.AutoSize = true;
            this.lbGame.Location = new System.Drawing.Point(85, 44);
            this.lbGame.Name = "lbGame";
            this.lbGame.Size = new System.Drawing.Size(0, 12);
            this.lbGame.TabIndex = 14;
            // 
            // btnGetPwdFile
            // 
            this.btnGetPwdFile.Location = new System.Drawing.Point(89, 219);
            this.btnGetPwdFile.Name = "btnGetPwdFile";
            this.btnGetPwdFile.Size = new System.Drawing.Size(75, 23);
            this.btnGetPwdFile.TabIndex = 13;
            this.btnGetPwdFile.Text = "更改";
            this.btnGetPwdFile.UseVisualStyleBackColor = true;
            this.btnGetPwdFile.Click += new System.EventHandler(this.btnGetPwdFile_Click);
            // 
            // lbPwdFile
            // 
            this.lbPwdFile.AutoSize = true;
            this.lbPwdFile.Location = new System.Drawing.Point(93, 197);
            this.lbPwdFile.Name = "lbPwdFile";
            this.lbPwdFile.Size = new System.Drawing.Size(0, 12);
            this.lbPwdFile.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-2, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "签名文件：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "游戏缩写：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "游戏名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "游戏：";
            // 
            // getPwdFile
            // 
            this.getPwdFile.Filter = "签名文件|*.keystore;*.jks";
            // 
            // btnPackage
            // 
            this.btnPackage.Location = new System.Drawing.Point(177, 557);
            this.btnPackage.Name = "btnPackage";
            this.btnPackage.Size = new System.Drawing.Size(75, 23);
            this.btnPackage.TabIndex = 8;
            this.btnPackage.Text = "开始打包";
            this.btnPackage.UseVisualStyleBackColor = true;
            this.btnPackage.Click += new System.EventHandler(this.btnPackage_Click);
            // 
            // curEvent
            // 
            this.curEvent.AutoSize = true;
            this.curEvent.Location = new System.Drawing.Point(141, 292);
            this.curEvent.Name = "curEvent";
            this.curEvent.Size = new System.Drawing.Size(0, 12);
            this.curEvent.TabIndex = 10;
            // 
            // pbT
            // 
            this.pbT.Location = new System.Drawing.Point(104, 302);
            this.pbT.Name = "pbT";
            this.pbT.Size = new System.Drawing.Size(325, 23);
            this.pbT.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.richTxtInfo);
            this.panel2.Location = new System.Drawing.Point(481, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(407, 527);
            this.panel2.TabIndex = 12;
            // 
            // richTxtInfo
            // 
            this.richTxtInfo.Location = new System.Drawing.Point(4, 4);
            this.richTxtInfo.Name = "richTxtInfo";
            this.richTxtInfo.Size = new System.Drawing.Size(400, 520);
            this.richTxtInfo.TabIndex = 0;
            this.richTxtInfo.Text = "";
            this.richTxtInfo.TextChanged += new System.EventHandler(this.richTxtInfo_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(481, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "compileInfo";
            // 
            // doWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 585);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pbT);
            this.Controls.Add(this.curEvent);
            this.Controls.Add(this.btnPackage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.plChannel);
            this.Controls.Add(this.lbShowChannel);
            this.Controls.Add(this.per);
            this.Controls.Add(this.lbGamePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "doWork";
            this.Text = "step 3. 打包";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.doWork_FormClosing);
            this.Load += new System.EventHandler(this.doWork_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getGame;
        private System.Windows.Forms.Label lbGamePath;
        private System.Windows.Forms.OpenFileDialog getGameFile;
        private System.Windows.Forms.Label per;
        private System.Windows.Forms.Label lbShowChannel;
        private System.Windows.Forms.Panel plChannel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetPwdFile;
        private System.Windows.Forms.Label lbPwdFile;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbGame;
        private System.Windows.Forms.Label lbGameApk;
        private System.Windows.Forms.OpenFileDialog getPwdFile;
        private System.Windows.Forms.Button btnPackage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label curEvent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSignPwd;
        private System.Windows.Forms.TextBox txtKeyPwd;
        private System.Windows.Forms.TextBox txtAlias;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar pbT;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox richTxtInfo;
        private System.Windows.Forms.RadioButton makeRYes;
        private System.Windows.Forms.RadioButton makeRNo;
        private System.Windows.Forms.Label label9;
    }
}