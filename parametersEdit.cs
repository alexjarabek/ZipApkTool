using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace sandGlass
{
    public partial class parametersEdit : Form
    {
        public channelItem selChannel = null;
        public gameItem selGame = null;
        private channelManager cManager = new channelManager();
        private gameManager gManager = new gameManager();
        private compileManager compManager = new compileManager();
        private string propertiesFile = null;
        public bool ReturnValue { get; protected set; } //用这个公开属性传值
        public parametersEdit()
        {
            InitializeComponent();
            cManager.deCallBack = ChannelDeBack;
            gManager.deCallBack = GameDeBack;
        }

        private void parametersEdit_Load(object sender, EventArgs e)
        {
            ReturnValue = false;
            setGame();
            setChannel();
            validate();
            parameters_Load();

        }
        private bool checkReadme()
        {
            bool flag = false;
            if (FileUtil.checkFile(envConfig.channelPath + @"/" + selChannel.cid + @"/readme.txt"))
                flag = true;
            return flag;
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        private void parameters_Load()
        {
            //  MessageBox.Show(gamePic);         
            if (selGame.gid.Length <= 0)
                this.Close();
            // 加载游戏参数
            propertiesFile = envConfig.gameConfig + selGame.gid + @"\" + selChannel.cid + envConfig.configExt;
            if (!Directory.Exists(envConfig.gameConfig + @"\" + selGame.gid))
                Directory.CreateDirectory(envConfig.gameConfig + @"\" + selGame.gid);
            if (!FileUtil.checkFile(propertiesFile))
            {
                // envConfig.channelPath + channelVal + @"\".;
                string tplPtFile = envConfig.channels + selChannel.cid + @"\" + selChannel.cid + envConfig.configExt;
                //  MessageBox.Show(tplPtFile);
                if (FileUtil.checkFile(tplPtFile))
                {
                    File.Copy(tplPtFile, propertiesFile, true);
                }
                else
                {
                    string lineStr = "appId=0\r\n";
                    lineStr += "appKey=0\r\n";
                    lineStr += "channel=" + selChannel.cid + "\r\n";
                    lineStr += "version=" + selGame.version + "\r\n";
                    lineStr += "package=" + selGame.package + "." + selChannel.package;

                    FileUtil.writeContent(propertiesFile, lineStr);
                }
            }
            readPt();

        }
        //       public Dictionary<string, string> allProperties = new Dictionary<string, string>();
        private void readPt()
        {
            PpHelper pptHelper = new PpHelper(propertiesFile);
            Dictionary<string, string> dic = null;
            try
            {
                dic = pptHelper.getAllProperties();
            }
            catch (Exception)
            {
                MessageBox.Show("获取gameConfig参数配置异常，请检查！");
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            dicNum = dic.Count();
            int i = 1;
            foreach (KeyValuePair<string, string> p in dic)
            {
                //     this.allProperties.Add(p.Key, p.Value);
                TextBox txtBoxK = new TextBox();
                txtBoxK.Name = "K_" + i.ToString();
                txtBoxK.Text = p.Key;
                txtBoxK.Left = 0;
                txtBoxK.Top = i * 50;
                txtBoxK.Location = new Point(pannelPts.Location.X + 5, i * 25);
                pannelPts.Controls.Add(txtBoxK);

                Label l1 = new Label();
                l1.Text = "=";
                l1.Location = new Point(112, i * 25);
                l1.Size = new System.Drawing.Size(10, 23);
                pannelPts.Controls.Add(l1);

                TextBox txtBoxV = new TextBox();
                txtBoxV.Name = "V_" + i.ToString();
                txtBoxV.Text = p.Value;

                if (p.Key == "version" )
                {
                    txtBoxV.Text = selGame.version;
                     
                }

                //if (p.Key == "version" && selGame.version != p.Value)
                //{
                //    txtBoxV.Text = selGame.version;

                //    DialogResult dr = MessageBox.Show("游戏版本" + selGame.version + "号与打包参数" + p.Value + "不一致,确定采用游戏版本？ ", "版本不一致！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //    if (dr == DialogResult.OK)
                //    {
                //        txtBoxV.Text = selGame.version;
                //    }

                //}

                txtBoxV.Location = new Point(130, i * 25);
                txtBoxV.Size = new System.Drawing.Size(230, 23);
                txtBoxV.Top = i * 25;

                pannelPts.Controls.Add(txtBoxV);
                i++;
            }




        }
        /// <summary>
        /// 游戏信息
        /// </summary>
        private void setGame()
        {
            if (selGame == null) { return; }
            GC.Collect();
            game_name.Text = selGame.name;
            game_id.Text = selGame.gid;
            game_version.Text = selGame.version;
            game_key.Text = selGame.key;
            game_package.Text = selGame.package;
            game_keypwd.Text = selGame.keyPwd;
            game_alias.Text = selGame.alias;
            game_signpwd.Text = selGame.signPwd;


            string game_orientation = selGame.orientation;
            if (game_orientation.Equals("portrait"))
            {
                this.game_portrait.Checked = true;
            }
            else
            {
                this.game_landscape.Checked = true;
            }

            game_signpwd.Text = selGame.signPwd;
            game_alias.Text = selGame.alias;
            game_keypwd.Text = selGame.keyPwd;

            this.game_icon.Image = compManager.getImageByCurrentPath(selGame.icon);
            GC.Collect();
        }


        private bool validate()
        {
            bool ret = true;
            if (!FileUtil.checkFile(envConfig.currenPath + selGame.keyPath))
            {
                game_key_check.ForeColor = Color.Red;
                game_key_check.Text = " 检测签名文件不存在";
                ret = false;
            }
            else
            {
                game_key_check.ForeColor = Color.Green;
                game_key_check.Text = " 检测签名文件成功";
            }

            string apkpath = envConfig.deCompilePath + selGame.gid + @"\";
            if (!Directory.Exists(apkpath))
            {
                game_apk_check.ForeColor = Color.Red;
                this.game_apk_check.Text = "检测母包不存在";
                ret = false;

            }
            else
            {
                int num = FileUtil.getFilesNum(apkpath);
                if (num > 200)
                {
                    game_apk_check.ForeColor = Color.Green;
                    game_apk_check.Text = "检测母包成功";

                    string path = apkpath + @"smali\com\sandglass\game\model\SGConst.smali";
                    string sdk = FileUtil.getSdkVersionBySmali(path);
                    game_sdk_version.Text = sdk;
                    game_sdk_version.ForeColor = Color.Red;
                }
                else
                {
                    game_apk_check.ForeColor = Color.Red;
                    this.game_apk_check.Text = "检测母包不存在";
                    ret = false;
                }

            }
            // 渠道检测
            apkpath = envConfig.channels + selChannel.cid + @"\apkBase\";

            if (!Directory.Exists(apkpath))
            {
                c_apk_check.ForeColor = Color.Red;
                this.c_apk_check.Text = "检测母包不存在";
                ret = false;
            }
            else
            {
                int num = FileUtil.getFilesNum(apkpath);
                if (num > 120)
                {
                    c_apk_check.ForeColor = Color.Green;
                    c_apk_check.Text = "检测母包成功";
                    string path = apkpath + @"smali\com\sandglass\game\model\SGConst.smali";
                    string sdk = FileUtil.getSdkVersionBySmali(path);


                    channel_sdk_version.Text = sdk;
                    channel_sdk_version.ForeColor = Color.Red;
                }
                else
                {
                    c_apk_check.ForeColor = Color.Red;
                    this.c_apk_check.Text = "检测母包不存在";
                    ret = false;
                }
            }

            if (keyTrue.Checked)
            {
                return checkChannelKey();
            }

            return ret;

        }


        private void setChannel()
        {
            if (selChannel == null) { return; }

            GC.Collect();
            if(selChannel.cid=="egame"){
                e_game_panel.Visible = true;
            }
            this.cName.Text = selChannel.name;
            this.cId.Text = selChannel.cid;
            this.cPackage.Text = selChannel.package;

            this.cVersion.Text = selChannel.version;
            this.cDes.Text = selChannel.des;
            // 角标显示
            if (selChannel.isfoot.Equals("true"))
            {
                this.cFootTrue.Checked = true;
                if (FileUtil.checkFile(envConfig.currenPath + @"\" + selChannel.foot))
                {
                    this.CfootPic.Image = compManager.getImageByCurrentPath(selChannel.foot);
                }

            }
            else
            {
                this.cFootFalse.Checked = true;
                this.CfootPic.Image = null;
            }

            if (selChannel.isR.Equals("true"))
            {
                this.cRTrue.Checked = true;
            }
            else
            {
                this.cRFalse.Checked = true;
            }
            if (selChannel.isOwnKey == "true")
            {
                keyTrue.Checked = true;
                btnGetCalias.Enabled = true;
                channel_alias.ReadOnly = false;
                channel_key_pwd.ReadOnly = false;
                channel_sign_pwd.ReadOnly = false;

            }
            else
            {
                keyTrue.Checked = false;

            }

            GC.Collect();
        }

        private void btnSaveCfg_Click(object sender, EventArgs e)
        {
            if (dicNum > 0)
            {
                Dictionary<int, string> tmpDataK = new Dictionary<int, string>();
                Dictionary<int, string> tmpDataV = new Dictionary<int, string>();
                string writeFileStr = "";
                for (int i = 1; i <= dicNum; i++)
                {
                    foreach (Control c in pannelPts.Controls)
                    {
                        if (c.Name == "K_" + i)
                        {
                            TextBox k = (TextBox)c;
                            if (k.Text.ToString().Length > 0)
                                tmpDataK[i] = k.Text.ToString();
                        }
                        else if (c.Name == "V_" + i)
                        {
                            TextBox v = (TextBox)c;
                            if (v.Text.ToString().Length > 0)
                                tmpDataV[i] = v.Text.ToString();
                        }
                        else
                        {
                        }
                    }
                    if (tmpDataK.Count() > 0 && tmpDataK[i].Trim().Length > 0 && tmpDataV[i].Trim().Length > 0)
                    {
                        string lineStr = tmpDataK[i] + "=" + tmpDataV[i] + "\r\n";
                        writeFileStr += lineStr;

                        if (tmpDataK[i] == "version")
                        {
                            if (tmpDataV[i] != selGame.version)
                            {
                                DialogResult dr = MessageBox.Show("游戏版本" + selGame.version + "号与打包参数" + tmpDataV[i] + "不一致，确定继续？ ", "版本提示！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                if (dr != DialogResult.OK)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                FileUtil.writeContent(propertiesFile, writeFileStr);
            }
            else
            {
                MessageBox.Show("没有添加属性!");
            }
            string gameV = game_sdk_version.Text.Replace(".", "");
            string channelV = channel_sdk_version.Text.Replace(".", "");

            try
            {
                int gV = Convert.ToInt32(gameV);
                int cV = Convert.ToInt32(channelV);
                if (gV >= 141 && cV < gV)
                {
                    MessageBox.Show("请升级渠道母包sdk版本！");
                    return;
                }
            }
            catch (Exception)
            {

                DialogResult dr = MessageBox.Show("未检测到接入sdk版本号，确定继续？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr != DialogResult.OK)
                {
                    return;
                }

            }

            // 当前到channels 下
            string tempFile = envConfig.channels + selChannel.cid + @"\" + selChannel.cid + envConfig.configExt;
            if (!FileUtil.checkFile(tempFile))
            {
                File.Copy(propertiesFile, tempFile, true);
            }
            // 渠道独立keystore
            if (selChannel.isOwnKey == "true")
            {
                saveSign();
            }
            GC.Collect();
            if (!getOtherInfo())
            {
                return;
            }



            ReturnValue = validate();

            this.DialogResult = DialogResult.OK;

        }
        /// <summary>
        /// 爱游戏 获取支付点
        /// </summary>
        public bool getOtherInfo()
        {
            // 爱游戏支付点
            if (selChannel.cid == "egame")
            {
                string feelInfo = "feeInfo.dat";
                string fileName = envConfig.channels + selChannel.cid + @"\" + selGame.gid + @"\" + feelInfo;
                string to = envConfig.channels + selChannel.cid + @"\apkBase\assets\" + feelInfo;

                if (File.Exists(fileName))
                {
                    // 认为游戏已经解压成功
                    if (!Directory.Exists(to))
                    {
                        Directory.CreateDirectory(envConfig.channels + selChannel.cid + @"\apkBase\assets\");
                    }
                    File.Copy(fileName, to, true);
                }
                else
                {
                    MessageBox.Show("未检测到" + selGame.name + "支付点文件！");
                    return false;
                }
            }
            //  自定义属性
            string attrs = envConfig.channels + selChannel.cid + @"\attrs.xml";
            string toAttrs = envConfig.channels + selChannel.cid + @" \apkBase\res\values\attrs.xml";


            if (File.Exists(attrs))
            {
                // 认为游戏已经解压成功
                if (!Directory.Exists(toAttrs))
                {
                    Directory.CreateDirectory(envConfig.channels + selChannel.cid + @"\apkBase\res\values\");

                }
                File.Copy(attrs, toAttrs, true);
            }
            return true;
        }




        public int dicNum = 0;
        private void addAtr_Click(object sender, EventArgs e)
        {
            dicNum += 1;
            // int num = allProperties.Count();
            int i = dicNum;
            TextBox txtBoxK = new TextBox();
            txtBoxK.Name = "K_" + i.ToString();
            txtBoxK.Text = "";
            txtBoxK.Left = 0;
            txtBoxK.Top = i * 50;
            txtBoxK.Location = new Point(pannelPts.Location.X, i * 25);
            pannelPts.Controls.Add(txtBoxK);

            Label l1 = new Label();
            l1.Text = "=";
            l1.Location = new Point(120, i * 25);
            l1.Size = new System.Drawing.Size(10, 23);
            pannelPts.Controls.Add(l1);

            TextBox txtBoxV = new TextBox();
            txtBoxV.Name = "V_" + i.ToString();
            txtBoxV.Text = "";
            txtBoxV.Location = new Point(130, i * 25);
            txtBoxV.Size = new System.Drawing.Size(230, 23);
            txtBoxV.Top = i * 25;

            pannelPts.Controls.Add(txtBoxV);
            i++;
        }
        /// <summary>
        /// 选择文件复制到ASSETS  爱游戏-支付点文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToAssets_Click(object sender, EventArgs e)
        {
            if (this.selChannel == null)
            {
                copyToAssets.Enabled = false;
                MessageBox.Show("请选择渠道");
            }
            this.openFileDialog.InitialDirectory = envConfig.channels + selChannel.cid + @"\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.SafeFileName.ToString();

                this.assetsPath.Text = openFileDialog.FileName.ToString();

                //    string to = envConfig.channels + selChannel.cid + @"\apkBase\assets\" + fileName;

                string to = envConfig.channels + selChannel.cid + @"\" + selGame.gid + @"\";

                if (!Directory.Exists(to))
                {
                    Directory.CreateDirectory(to);
                }

                FileUtil.copy(assetsPath.Text, to + "feeInfo.dat");
            }
            openFileDialog.Dispose();
        }
        /// <summary>
        /// 选择文件复制到VALUES  悠悠村，嗨游-自定义属性文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void copyToValues_Click(object sender, EventArgs e)
        //{
        //    if (this.selChannel == null)
        //    {
        //        copyToValues.Enabled = false;
        //        MessageBox.Show("请选择渠道");
        //    }

        //    this.openFileDialog.InitialDirectory = envConfig.channels + selChannel.cid + @"\";
        //    this.openFileDialog.FileName = "attrs.xml";
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        string fileName = openFileDialog.SafeFileName.ToString();
        //        this.valuesPath.Text = openFileDialog.FileName.ToString();
        //        string to = envConfig.channels + selChannel.cid + @" \apkBase\res\values\" + fileName;
        //        FileUtil.copy(valuesPath.Text, to);
        //    }
        //    openFileDialog.Dispose();
        //}
        private void btnCgetApk_Click(object sender, EventArgs e)
        {
            string dir = compManager.getValue("channelSelectFolder") + @"\" + selChannel.cid + @"\";
            if (!Directory.Exists(dir))
            {
                dir = compManager.getValue("channelSelectFolder");
            }
            getApk.InitialDirectory = dir;
            if (getApk.ShowDialog() == DialogResult.OK)
            {
                //   selChannel.apk = getApk.FileName.ToString();
                cManager.deCompile(selChannel, getApk.FileName.ToString(), cLog, btnGetCApk, btnSaveCfg);
            }
            getApk.Dispose();
        }

        private void game_getApk_Click(object sender, EventArgs e)
        {

            //string dir = compManager.getValue("gameSelectFolder") + @"\" + selGame.gid + @"\";
            //if (!Directory.Exists(dir))
            //{
            //    dir = compManager.getValue("gameSelectFolder");
            //}

            string dir = compManager.getValue("gameSelectFolder") + @"\" + selGame.gid + @"\";
            if (!Directory.Exists(dir))
            {
                dir = compManager.getValue("gameSelectFolder") + @"\" + selGame.name + @"\";
                if (!Directory.Exists(dir))
                {
                    dir = compManager.getValue("gameSelectFolder");
                }

            }


            getApk.InitialDirectory = dir;
            if (getApk.ShowDialog() == DialogResult.OK)
            {
                btnSaveCfg.Enabled = false;
                //    selGame.apk = getApk.FileName.ToString();
                gManager.deCompile(getApk.FileName.ToString(), selGame.gid, gLog, game_getApk, btnSaveCfg);

            }
            getApk.Dispose();
        }
        /// <summary>
        /// 选择渠道签名 -- 当乐，新浪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCalias_Click(object sender, EventArgs e)
        {
            GC.Collect();
            getPwdFile.InitialDirectory = compManager.getValue("channelSelectFolder") + @"\" + selChannel.cid + @"\";
            if (getPwdFile.ShowDialog() == DialogResult.OK)
            {
                channel_key.ForeColor = Color.Green;
                channel_key.Text = getPwdFile.FileName.ToString().Trim();
                string to = envConfig.channels + selChannel.cid + @"\" + selChannel.cid + ".keystore";

                FileUtil.copy(channel_key.Text, to);

            }
            getPwdFile.Dispose();
            GC.Collect();

        }
        /// <summary>
        /// 保存渠道签名文件
        /// </summary>
        /// <param name="item"></param>
        public void saveSign()
        {
            string lineStr = "";
            lineStr += "alias=" + channel_alias.Text + "\r\n";
            lineStr += "keyPwd=" + channel_key_pwd.Text + "\r\n";
            lineStr += "keyPath=" + @"\channels\" + selChannel.cid + @"\" + selChannel.cid + ".keystore" + "\r\n"; // 签名路径
            lineStr += "signPwd=" + channel_sign_pwd.Text + "\r\n";
            lineStr += "keyOldPath=" + channel_key.Text + "\r\n"; // 签名原始路径

            FileUtil.writeContent(envConfig.channels + selChannel.cid + @"\sign" + envConfig.configExt, lineStr);
        }

        /// <summary>
        /// 检测渠道 签名 property文件 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool checkChannelKey()
        {
            string fileName = envConfig.channels + selChannel.cid + @"\sign" + envConfig.configExt;

            if (FileUtil.checkFile(fileName))
            {

                PpHelper helper = new PpHelper(fileName);
                string alias = helper.GetPropertiesText("alias");
                string keypwd = helper.GetPropertiesText("keyPwd");
                string signpwd = helper.GetPropertiesText("signPwd");
                string key = helper.GetPropertiesText("keyPath");

                if (alias == "" || keypwd == "" || signpwd == "" || key == "")
                {
                    channel_key.ForeColor = Color.Red;
                    channel_key.Text = "检测渠道签名文件失败";
                    return false;
                }

                channel_key.Text = key;
                channel_key_pwd.Text = keypwd;
                channel_sign_pwd.Text = signpwd;
                channel_alias.Text = alias;

                channel_key.ForeColor = Color.Green;
                channel_key.Text = "检测渠道签名文件成功";

            }
            else
            {
                channel_key.ForeColor = Color.Red;
                channel_key.Text = "检测渠道签名文件失败";
                return false;
            }
            GC.Collect();
            return true;

        }
        /// <summary>
        /// 解包回调
        /// </summary> 
        private void ChannelDeBack(string key, string msg)
        {
            GC.Collect();
            string apkpath = envConfig.channels + selChannel.cid + @"\apkBase\";
            if (!Directory.Exists(apkpath))
            {
                c_apk_check.ForeColor = Color.Red;
                this.c_apk_check.Text = "检测母包不存在";
            }
            else
            {
                int num = FileUtil.getFilesNum(apkpath);
                if (num > 120)
                {
                    c_apk_check.ForeColor = Color.Green;
                    c_apk_check.Text = "检测母包成功";

                    string path = key + @"smali\com\sandglass\game\model\SGConst.smali";
                    string sdk = FileUtil.getSdkVersionBySmali(path);
                    channel_sdk_version.Text = sdk;
                }
                else
                {
                    c_apk_check.ForeColor = Color.Red;
                    this.c_apk_check.Text = "检测母包不存在";
                }

            }




        }
        /// <summary>
        /// 解包回调
        /// </summary> 
        private void GameDeBack(string key, string msg)
        {
            GC.Collect();
            string apkpath = envConfig.deCompilePath + selGame.gid + @"\";
            if (!Directory.Exists(apkpath))
            {
                game_apk_check.ForeColor = Color.Red;
                this.game_apk_check.Text = "检测母包不存在";
            }
            else
            {
                int num = FileUtil.getFilesNum(apkpath);
                if (num > 200)
                {
                    game_apk_check.ForeColor = Color.Green;
                    game_apk_check.Text = "检测母包成功";

                    string path = key + @"smali\com\sandglass\game\model\SGConst.smali";
                    string sdk = FileUtil.getSdkVersionBySmali(path);
                    game_sdk_version.Text = sdk;
                }
                else
                {
                    game_apk_check.ForeColor = Color.Red;
                    this.game_apk_check.Text = "检测母包不存在";
                }

            }


        }
        /////////////////////////////////

    }
}
