using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;//
using System.Drawing.Drawing2D;//
using System.Threading;//
using System.Runtime.InteropServices;//
using System.Xml;//
using System.IO;//


namespace sandGlass
{
    public partial class Home : Form
    {
        private channelManager cManager = new channelManager();
        private gameManager gManager = new gameManager();
        private compileManager compManager = new compileManager();
        private channelItem selChannelItem = null;
        private gameItem selGameItem = null;
        private gameItem selCfgGameItem = null; // page3 gameItem;
        private channelItem selCfgChannelItem = null;
        PerformanceCounter p = new PerformanceCounter();


        #region timer事件
        private void timercpu_Tick(object sender, EventArgs e)
        {

            toolStripProgressBarCpu.Value = (int)(p.NextValue());
            cpu.Text = toolStripProgressBarCpu.Value.ToString() + "%";


        }
        #endregion
        public Home()
        {
            InitializeComponent();
           
            string skinfileName = compManager.getValue("skinfile");
            string skinfile = envConfig.currenPath + @"\skins\" + skinfileName;
            if (FileUtil.checkFile(skinfile))
            {
                this.skinUI1.SkinFile = skinfile;
                this.lab_skin.Text = skinfileName;
            }

            // cpu
            p.CounterName = @"% Processor Time"; //@"% Committed Bytes In Use";
            p.CategoryName = "Processor";//"Memory";
            p.InstanceName = "_Total";
            var category = new PerformanceCounterCategory("Process");

            CheckForIllegalCrossThreadCalls = false;// 控件，其他线程调用
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void Home_Load(object sender, EventArgs e)
        {

            GC.Collect();
            cManager.deCallBack = ChannelDeBack;
            gManager.deCallBack = GameDeBack;
            if (!Directory.Exists(envConfig.games))
            {

                Directory.CreateDirectory(envConfig.games);
            }
            if (!Directory.Exists(envConfig.channels))
            {
                Directory.CreateDirectory(envConfig.channels);
            }

            if (!File.Exists(envConfig.channelsXml))
            {
                cManager.CreateXmlConfigFile(envConfig.channelsXml);
            }

            if (!File.Exists(envConfig.gamesXml))
            {
                cManager.CreateXmlConfigFile(envConfig.gamesXml);
            }
            initPage1();
            initPage2();
            initPage3();
            initPage4();
            initPage5();
            tabControl1.SelectedIndex = 2;

        }

        private void initPage1()
        {
            GC.Collect();
            gManager.initialListView(listView1, imageList1);
            gManager.initGameData(listView1, imageList1);
        }


        ///////////////////////////////////////////////////////////////////游戏管理--华丽分割线//////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 游戏列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GC.Collect();
            if (listView1.SelectedItems.Count > 0)
            {
                page1Clear();
                foreach (ListViewItem lvi in listView1.Items)
                {
                    lvi.ForeColor = Color.Black;
                    lvi.BackColor = Color.FromArgb(239, 248, 250); //恢复默认背景色 
                }

                //修改选中项颜色
                listView1.SelectedItems[0].SubItems[0].ForeColor = Color.SaddleBrown;
                listView1.SelectedItems[0].BackColor = Color.Silver;

                selGameItem = (gameItem)listView1.SelectedItems[0].Tag;
                setPage1Data(selGameItem);
                btnGetGame.Enabled = true;

                //去掉选中项背景
                listView1.SelectedItems[0].Selected = false;// 会引发第二次 该方法的调用
                listView1.SelectedItems.Clear();

            }

        }
        private void setPage1Data(gameItem item)
        {
            GC.Collect();

            game_name.Text = item.name;
            game_id.Text = item.gid;
            game_version.Text = item.version;
            game_key.Text = item.key;
            game_package.Text = item.package;
            game_des.Text = item.des;
            game_iconpath.Text = envConfig.currenPath + item.icon;

            game_keypwd.Text = item.keyPwd;
            game_alias.Text = item.alias;
            game_signpwd.Text = item.signPwd;
            game_keypath.Text = envConfig.currenPath + item.keyPath;

            string game_orientation = item.orientation;
            if (game_orientation.Equals("portrait"))
            {
                this.game_portrait.Checked = true;
            }
            else
            {
                this.game_landscape.Checked = true;
            }


            if (!FileUtil.checkFile(envConfig.currenPath + item.keyPath))
            {
                game_key_check.ForeColor = Color.Red;
                game_key_check.Text = " 检测签名文件不存在，请选择";
            }
            else
            {
                game_key_check.ForeColor = Color.Green;
                game_key_check.Text = " 检测到" + item.gid + "的签名文件成功";
            }

            // 游戏icon

            this.game_icon.Image = compManager.getImageByCurrentPath(item.icon);

            string apkpath = envConfig.deCompilePath + item.gid + @"\smali\com\sandglass\game\model\SGConst.smali";
            string sdk = FileUtil.getSdkVersionBySmali(apkpath);
            game_sg_version.Text = "sdk版本：" + sdk;


            GC.Collect();
        }

        public void page1Clear()
        {
            GC.Collect();
            game_name.Text = "";
            game_id.Text = "";
            game_version.Text = "";
            game_key.Text = "";
            game_package.Text = "";
            game_version.Text = "";

            game_portrait.Checked = true;
            //game_tool_200.Checked = true;

            //game_signpwd.Text = "";
            //game_alias.Text = "";
            //game_keypwd.Text = "";

            game_keypath.Text = "";
            game_key_check.ForeColor = Color.Red;
            game_key_check.Text = "没有检测" + game_id.Text + "的配置文件";
            game_iconpath.Text = "";
            game_path.Text = "";
            game_des.Text = "";
            glog.Text = "";
            game_icon.Image = null;
            selGameItem = null;
            btnGetGame.Enabled = false;
            game_sg_version.Text = "";
        }
        /// <summary>
        /// 选择游戏母包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetGame_Click(object sender, EventArgs e)
        {
            GC.Collect();
            getGameApkShow();
        }


        public void getGameApkShow()
        {
            GC.Collect();
            if (this.selGameItem == null)
            {
                btnGetGame.Enabled = false;
                MessageBox.Show("请选择游戏");
                return;
            }

            string dir = compManager.getValue("gameSelectFolder") + @"\" + selGameItem.gid + @"\";
            if (!Directory.Exists(dir))
            {
                dir = compManager.getValue("gameSelectFolder") + @"\" + selGameItem.name + @"\";
                if (!Directory.Exists(dir))
                {
                    dir = compManager.getValue("gameSelectFolder");
                }

            }

            getGameApk.InitialDirectory = dir;
            if (getGameApk.ShowDialog() == DialogResult.OK)
            {
                btnGetGame.Enabled = false;
                game_path.Text = getGameApk.FileName.ToString();
                gManager.deCompile(game_path.Text, this.selGameItem.gid, this.glog, this.btnGetGame);
            }
            getGameApk.Dispose();

        }

        /// <summary>
        /// 选择icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPic_Click(object sender, EventArgs e)
        {
            GC.Collect();
            string dir = compManager.getValue("gameSelectFolder") + @"\" + game_id.Text + @"\";
            if (!Directory.Exists(dir))
            {
                dir = compManager.getValue("gameSelectFolder") + @"\" + game_name.Text + @"\";
                if (!Directory.Exists(dir))
                {
                    dir = compManager.getValue("gameSelectFolder");
                }
            }

            getPicFile.InitialDirectory = dir;

            if (getPicFile.ShowDialog() == DialogResult.OK)
            {
                game_iconpath.Text = getPicFile.FileName.ToString().Trim();
                game_icon.Image = compManager.getImage(game_iconpath.Text);
            }
            getPicFile.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// 选择签名文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnGetSign_Click(object sender, EventArgs e)
        {
            GC.Collect();
            string dir = compManager.getValue("gameSelectFolder") + @"\" + game_id.Text + @"\";
            if (!Directory.Exists(dir))
            {
                dir = compManager.getValue("gameSelectFolder") + @"\" + game_name.Text + @"\";
                if (!Directory.Exists(dir))
                {
                    dir = compManager.getValue("gameSelectFolder");
                }
            }
            getPwdFile.InitialDirectory = dir;
            if (getPwdFile.ShowDialog() == DialogResult.OK)
            {
                game_keypath.Text = getPwdFile.FileName.ToString().Trim();
                game_key_check.Text = "";
            }
            getPwdFile.Dispose();
            GC.Collect();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GC.Collect();
            if (!gameValidate())
            {
                return;
            }
            gameItem item = new gameItem();
            if (selGameItem != null)
            {
                item = selGameItem;
            }
            item.name = game_name.Text;
            item.gid = game_id.Text;
            item.version = game_version.Text;
            item.key = game_key.Text;
            item.package = game_package.Text;
            item.des = game_des.Text;
            item.signPwd = game_signpwd.Text;
            item.keyPwd = game_keypwd.Text;
            item.alias = game_alias.Text;

            if (this.game_portrait.Checked)
            {
                item.orientation = "portrait";
            }
            else
            {
                item.orientation = "landscape";

            }


            string icon = @"\games\" + item.gid + @"\" + item.gid + ".png";//  保存为相对路径
            item.icon = icon;
            if (!Directory.Exists(envConfig.games + item.gid + @"\"))
            {
                Directory.CreateDirectory(envConfig.games + item.gid + @"\");

            }
            string key = @"\games\" + item.gid + @"\" + item.gid + ".keystore";//  保存为相对路径
            item.keyPath = key;

            FileUtil.copy(this.game_iconpath.Text, envConfig.currenPath + icon);
            FileUtil.copy(this.game_keypath.Text, envConfig.currenPath + key);

            int index = gManager.saveUpdate(item);
            if (index >= 0)
            {
                btnGetGame.Enabled = true;
                selGameItem = item;
                if (!gManager.checkApkBase(item.gid))
                {
                    DialogResult dr = MessageBox.Show("基础信息保存成功,母包是否需要更新", "确认是否更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        getGameApkShow();
                    }

                }
                selGameItem = null;
                // gManager.reCreatGameXml();
                gManager.initGameData(listView1, imageList1);
                gManager.initGameData(listView3, imageList1);

            }
        }


        public bool gameValidate()
        {
            GC.Collect();

            if (game_name.Text.Equals("") || game_id.Text.Equals("") || game_version.Text.Equals("") || game_package.Text.Equals("") || game_version.Text.Equals(""))
            {
                MessageBox.Show("请检查输入完整游戏信息");
                return false;
            }
            if (game_keypath.Text.Equals(""))
            {
                MessageBox.Show("请选择签名文件");
                return false;
            }
            if (game_iconpath.Text.Equals(""))
            {
                MessageBox.Show("请选择游戏图标");
                return false;
            }
            if (!FileUtil.checkFile(game_keypath.Text))
            {
                MessageBox.Show("签名文件不存在");
                return false;
            }
            if (!FileUtil.checkFile(game_iconpath.Text))
            {
                MessageBox.Show("图标文件不存在");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 关键字查询
        /// </summary>
        private void keyword_TextChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (listView1.Items.Count > 0 && keyword.Text.Trim() != string.Empty)
            {

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[1].Text.StartsWith(keyword.Text.Trim()))
                    {
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                        return;
                    }

                    if (listView1.Items[i].SubItems[0].Text.IndexOf(keyword.Text.Trim()) >= 0)
                    {
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                        return;
                    }

                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>

        private void btnReset_Click(object sender, EventArgs e)
        {
            GC.Collect();
            page1Clear();
            ////    gManager.initGameData(listView1, imageList1);

        }
        /// <summary>
        /// 刷新游戏列表
        /// </summary>
        private void btnfresh_Click(object sender, EventArgs e)
        {
            GC.Collect();
            selGameItem = null;
            gManager.reCreatGameXml();
            gManager.initGameData(listView1, imageList1);
            gManager.initGameData(listView3, imageList1);
        }

        /////////////////////////////////////////////////////////////渠道管理--华丽分割线////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 渠道管理
        /// </summary>
        private void initPage2()
        {
            cManager.initialListView(listView2);
            cManager.initChannelData(listView2);
            GC.Collect();
        }
        /// <summary>
        /// 渠道管理列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (listView2.SelectedItems.Count > 0)
            {
                page2Clear();
                selChannelItem = (channelItem)listView2.SelectedItems[0].Tag;
                foreach (ListViewItem item in listView2.Items)
                {
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.FromArgb(239, 248, 250); //恢复默认背景色 
                    channelItem citem = (channelItem)item.Tag;
                    if (citem != null && citem.flag != "true")
                    {
                        item.ForeColor = Color.Red;
                    }

                }
                //修改选中项颜色
                listView2.SelectedItems[0].SubItems[0].ForeColor = Color.SaddleBrown;
                listView2.SelectedItems[0].BackColor = Color.Silver;

                this.btnCgetApk.Enabled = true; 

                setPage2Data(selChannelItem);
                //去掉选中项背景
                listView2.SelectedItems[0].Selected = false;// 会引发第二次 该方法的调用
                listView2.SelectedItems.Clear();
            }
        }
        private void setPage2Data(channelItem item)
        {
            GC.Collect();
            cLog.Text = "";// 清空日志
            this.cName.Text = item.name;
            this.cId.Text = item.cid;
            this.cPackage.Text = item.package;
            this.cApk.Text = "";
            this.cVersion.Text = item.version;
            this.cDes.Text = item.des;

            // 角标显示
            if (item.isfoot.Equals("true"))
            {
                this.cFootTrue.Checked = true;
                CfootPanel.Visible = true;
                Cfoot.Text = envConfig.currenPath + item.foot;
                if (FileUtil.checkFile(Cfoot.Text))
                {
                    this.CfootPic.Image = compManager.getImageByCurrentPath(item.foot);
                }

            }
            else
            {
                this.cFootFalse.Checked = true;
                CfootPanel.Visible = false;
                this.Cfoot.Text = "";
                this.CfootPic.Image = null;
            }

            if (item.isR.Equals("true"))
            {
                this.cRTrue.Checked = true;
            }
            else
            {
                this.cRFalse.Checked = true;
            }
            if (item.isOwnKey.Equals("true"))
            {
                this.keyTrue.Checked = true;
            }
            else
            {
                this.keyFalse.Checked = true;
            }

            string apkpath = envConfig.channels + item.cid + @"\apkBase\smali\com\sandglass\game\model\SGConst.smali";
            string version = FileUtil.getSdkVersionBySmali(apkpath);
            this.channel_sdk_version.Text = "sdk版本：" + version;

            GC.Collect();
        }

        private void cFootTrue_CheckedChanged(object sender, EventArgs e)
        {

            GC.Collect();
            if (cFootTrue.Checked == true)
            {
                this.CfootPanel.Visible = true;
            }
            else if (cFootFalse.Checked == true)
            {
                this.CfootPanel.Visible = false;
            }

            GC.Collect();
        }
        /// <summary>
        /// 渠道信息保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSave_Click(object sender, EventArgs e)
        {
            GC.Collect();
            if (!validate_channel())
            {
                return;
            }
            channelItem item = new channelItem();
            if (selChannelItem != null)
            {
                item = selChannelItem;
            }
            item.name = this.cName.Text;
            item.cid = this.cId.Text;
            item.package = this.cPackage.Text;
            //      item.apk = this.cApk_check.Text;
            item.version = this.cVersion.Text;
            item.des = this.cDes.Text;
            if (this.cRTrue.Checked)
            {
                item.isR = "true";
            }
            else
            {
                item.isR = "false";
            }


            if (this.keyFalse.Checked)
            {
                item.isOwnKey = "false";
            }
            else
            {
                item.isOwnKey = "true";
            }



            if (this.cFootTrue.Checked)
            {
                item.isfoot = "true";

            }
            else
            {
                item.isfoot = "false";
                item.foot = "";
            }
            GC.Collect();
            string to = @"\channels\" + item.cid + @"\" + item.cid + ".png";
            item.foot = to;
            item.flag = "true";

            FileUtil.copy(this.Cfoot.Text, envConfig.currenPath + to);
            int index = cManager.saveUpdate(item);

            if (index >= 0)
            {
                btnCgetApk.Enabled = true;
                selChannelItem = item;
                if (!cManager.checkApkBase(item.cid))
                {
                    DialogResult dr = MessageBox.Show("基础信息保存成功,母包是否需要更新", "确认是否更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        getChannelApkShow();
                    }
                }
                cManager.initChannelData(listView2);
            }


            //// 更新选择列表

            if (cfgid.Text != "")
            {
                string gid = cfgid.Text;
                cManager.initChannelCheckData(listView4, gid, radio_used.Checked);
            }

        }

        private bool validate_channel()
        {
            GC.Collect();
            if (this.cName.Text.Equals("") || this.cId.Text.Equals("") || this.cVersion.Text.Equals(""))
            {
                MessageBox.Show("请输入完整信息");
                return false;

            }

            if (this.cFootTrue.Checked)
            {
                if (Cfoot.Text.Equals(""))
                {
                    MessageBox.Show("请选择角标文件");
                    return false;
                }
                if (!FileUtil.checkFile(Cfoot.Text))
                {
                    MessageBox.Show("角标文件不存在");
                    return false;
                }

            }

            if (!Directory.Exists(envConfig.channels + this.cId.Text))
            {
                Directory.CreateDirectory(envConfig.channels + this.cId.Text);
            }
            return true;
        }

        private void btnCReset_Click(object sender, EventArgs e)
        {
            GC.Collect();
            page2Clear();
            cManager.initChannelData(listView2);
        }
        public void page2Clear()
        {
            GC.Collect();
            cName.Text = "";
            cId.Text = "";
            cVersion.Text = "";
            cPackage.Text = "";
            btnCgetApk.Enabled = false; 

            cRFalse.Checked = true;
            cFootFalse.Checked = true;
            keyFalse.Checked = true;

            this.cApk.Text = "";
            this.Cfoot.Text = "";
            cDes.Text = "";
            cLog.Text = "";
            CfootPic.Image = null;
            selChannelItem = null;

            channel_sdk_version.Text = "";

        }
        private void btnCgetApk_Click(object sender, EventArgs e)
        {
            getChannelApkShow();

        }
        private void getChannelApkShow()
        {
            GC.Collect();
            if (this.selChannelItem == null)
            {
                btnCgetApk.Enabled = false;
                MessageBox.Show("请选择渠道");
            }
            string myCh = compManager.getValue("channelSelectFolder") + @"\" + selChannelItem.cid + @"\";
            if (!Directory.Exists(myCh))
            {
                myCh = compManager.getValue("channelSelectFolder");
            }
            getChannelApk.InitialDirectory = myCh;
            if (getChannelApk.ShowDialog() == DialogResult.OK)
            {

                cApk.Text = getChannelApk.FileName.ToString();
                ///   selChannelItem.apk = cApk.Text;
                cManager.deCompile(this.selChannelItem, cApk.Text, cLog, btnCgetApk);
            }
            getChannelApk.Dispose();

        }
        private void btnGetCPic_Click(object sender, EventArgs e)
        {

            GC.Collect();

            getPicFile.InitialDirectory = envConfig.channels;
            if (getPicFile.ShowDialog() == DialogResult.OK)
            {
                Cfoot.Text = getPicFile.FileName.ToString().Trim();
                CfootPic.Image = compManager.getImage(Cfoot.Text);
            }
            getPicFile.Dispose();
            GC.Collect();

        }

        private void ckeyword_TextChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (listView2.Items.Count > 0 && ckeyword.Text.Trim() != string.Empty)
            {

                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (listView2.Items[i].SubItems[1].Text.StartsWith(ckeyword.Text.Trim()))
                    {
                        listView2.Items[i].Selected = true;
                        listView2.Items[i].EnsureVisible();
                        return;
                    }
                    if (listView2.Items[i].SubItems[0].Text.IndexOf(ckeyword.Text.Trim()) >= 0)
                    {
                        listView2.Items[i].Selected = true;
                        listView2.Items[i].EnsureVisible();
                        return;
                    }
                }
            }
        }
        private void cUpdate_Click(object sender, EventArgs e)
        {
            GC.Collect();
            selChannelItem = null;
            cManager.reCreatChannelsXml();
            cManager.initChannelData(listView2);
        }

      
        /// <summary>
        /// 描述readme 获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_checkReadme_Click(object sender, EventArgs e)
        {
            GC.Collect();
            if (this.selChannelItem == null)
            {
                MessageBox.Show("请选择渠道");
                return;
            }

            string readFile = envConfig.channels + selChannelItem.cid + @"\readme.txt";
            if (FileUtil.checkFile(readFile))
            {
                FileStream fs = new FileStream(readFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string readme = sr.ReadToEnd();
                cDes.Text = readme.Replace("\r\n\r\n", "");
                sr.Close();
                fs.Close();
            }
        }



        /////////////////////////////////////////////////////////////////////开始打包---华丽分割线////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 正确选择渠道，可以打包渠道
        /// </summary>
        //      private int checkedRight = 0;
        Dictionary<string, string> checked_dic = new Dictionary<string, string>();
        List<Control> controls = new List<Control>();
        /// <summary>
        /// 参数配置
        /// </summary>
        private void initPage3()
        {
            GC.Collect();
            gManager.initialListView(listView3, imageList1);
            string fileName = envConfig.gamesXml;
            gManager.initGameData(listView3, imageList1);

        }

        /// <summary>
        /// 游戏列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (listView3.SelectedItems.Count > 0)
            {

                foreach (ListViewItem lvi in listView3.Items)
                {
                    lvi.ForeColor = Color.Black;
                    lvi.BackColor = Color.FromArgb(239, 248, 250); //恢复默认背景色 
                }

                //修改选中项颜色
                listView3.SelectedItems[0].SubItems[0].ForeColor = Color.SaddleBrown;
                listView3.SelectedItems[0].BackColor = Color.Silver;



                selCfgGameItem = (gameItem)listView3.SelectedItems[0].Tag;
                //     readGameProperty(selGameItem.gid);
                //去掉选中项背景
                listView3.SelectedItems[0].Selected = false;// 会引发第二次 该方法的调用
                listView3.SelectedItems.Clear();
                cfgid.Text = selCfgGameItem.name + "_" + selCfgGameItem.gid;
                initChannels(selCfgGameItem.gid);
                selCfgChannelItem = null;
                GC.Collect();
            }
        }



        /// <summary>
        /// 渠道管理
        /// </summary>
        private void initChannels(string gid)
        {
            GC.Collect();
            cManager.initialListView(listView4);
            cManager.initChannelCheckData(listView4, gid, radio_used.Checked);
            GC.Collect();
        }
        /// <summary>
        /// 渠道checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView4_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            GC.Collect();
            ListViewItem item = e.Item;
            ListView.CheckedListViewItemCollection checkedItems = listView4.CheckedItems;
            foreach (ListViewItem lvi in listView4.Items)
                lvi.ForeColor = Color.Black;
            foreach (ListViewItem eitem in checkedItems)
            {
                eitem.ForeColor = Color.LightSeaGreen;
            }

            if (item.Checked)
            {
                selCfgChannelItem = (channelItem)item.Tag;
                showParamEdits();
            }

        }
        private void showParamEdits()
        {
            GC.Collect();
            if (selCfgGameItem == null)
            {
                MessageBox.Show("请选择游戏");
                return;
            }
            if (selCfgChannelItem == null)
            {
                MessageBox.Show("请选择渠道");
                return;
            }
            parametersEdit paramEdit = new parametersEdit();
            paramEdit.selChannel = selCfgChannelItem;
            paramEdit.selGame = selCfgGameItem;
            paramEdit.StartPosition = FormStartPosition.CenterScreen;
            paramEdit.ShowDialog();
            if (paramEdit.DialogResult == DialogResult.OK)
            {
                bool result = paramEdit.ReturnValue; //获得返回值123
                if (result)
                {
                    string gid = selCfgGameItem.gid;
                    string cid = selCfgChannelItem.cid;
                    //        addToPanel(gid, cid);

                    addToFlowPanel(gid + "_" + cid, selCfgGameItem.name + "-" + selCfgChannelItem.name);

                }
                else
                {
                    MessageBox.Show("条件缺失");
                }

                GC.Collect();
            }

            GC.Collect();

        }


        /// <summary>
        /// 游戏关键字查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cfgkeyword_TextChanged(object sender, EventArgs e)
        {
            if (listView3.Items.Count > 0 && cfgkeyword.Text.Trim() != string.Empty)
            {

                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    if (listView3.Items[i].SubItems[1].Text.StartsWith(cfgkeyword.Text.Trim()))
                    {
                        listView3.Items[i].Selected = true;
                        listView3.Items[i].EnsureVisible();
                        return;
                    }
                    if (listView3.Items[i].SubItems[0].Text.IndexOf(cfgkeyword.Text.Trim()) >= 0)
                    {
                        listView3.Items[i].Selected = true;
                        listView3.Items[i].EnsureVisible();
                        return;
                    }

                }
            }
        }
        /// <summary>
        /// 渠道关键字查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cfgckeyword_TextChanged(object sender, EventArgs e)
        {
            GC.Collect();
            foreach (ListViewItem eitem in listView4.Items)
            {
                eitem.BackColor = System.Drawing.SystemColors.Window;
            }

            if (listView4.Items.Count > 0 && cfgckeyword.Text.Trim() != string.Empty)
            {

                for (int i = 0; i < listView4.Items.Count; i++)
                {
                    // id查询
                    if (listView4.Items[i].SubItems[1].Text.StartsWith(cfgckeyword.Text.Trim()))
                    {
                        listView4.Items[i].Selected = true;
                        listView4.Items[i].BackColor = Color.Cyan;
                        listView4.Items[i].EnsureVisible();
                        return;
                    }

                    // 名字查询
                    if (listView4.Items[i].SubItems[0].Text.IndexOf(cfgckeyword.Text.Trim())>=0)
                    {
                        listView4.Items[i].Selected = true;
                        listView4.Items[i].BackColor = Color.Cyan;
                        listView4.Items[i].EnsureVisible();
                        return;
                    }

                }
            }
        }
        /// <summary>
        ///导出配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, EventArgs e)
        {

            btn_export.Enabled = false;
            exportThread export = new exportThread();
            export.game_id = selCfgGameItem.gid;
            export.game_name = selCfgGameItem.name;
            export.game_package = selCfgGameItem.package;
            export.check = radio_used.Checked;
            export.callback = ExportCallBack;

            Thread tWorkingThread = new Thread(export.doWork);
            tWorkingThread.Name = export.name;
            tWorkingThread.SetApartmentState(ApartmentState.STA); // 单线程
            taskThreadList.Add(tWorkingThread);
            tWorkingThread.Start();
            GC.Collect();
        }
        /// <summary>
        /// 清理回调
        /// </summary>
        private void ExportCallBack(string key, string msg)
        {
            GC.Collect();
            deleteThread(key);
            if (msg == exportThread.MSG_GAME_NULL)
            {
                MessageBox.Show("请选择一款游戏吧！");
                return;

            }
            DialogResult dr = MessageBox.Show("导出完成，确定打开导出目录？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.OK)
            {
                //return;
                if (Directory.Exists(envConfig.export_call))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = "/e,/open, " + envConfig.export_call;
                    System.Diagnostics.Process.Start(psi);
                }
            }


            btn_export.Enabled = true;
            GC.Collect();
        }
        /// <summary>
        /// 获取已打过的渠道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_used_CheckedChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (selCfgGameItem == null)
            {
                MessageBox.Show("请选择游戏");
                return;

            }
            cManager.initChannelCheckData(listView4, selCfgGameItem.gid, radio_used.Checked);

        }

        /// <summary>
        /// 定义一个队列，用于记录用户创建的线程
        /// 以便在窗体关闭的时候关闭所有用于创建的线程
        /// </summary>
        private List<Thread> taskThreadList = new List<Thread>();
        public void addToFlowPanel(string key, string value)
        {
            GC.Collect();
            if (checked_dic.ContainsKey(key))
            {
                // if (checked_dic[key] == value)
                return;
            }
            addTask(key, value);

        }

        public void addTask(string key, string value)
        {
            GC.Collect();

            Panel panel_thread = new Panel();
            Button panel_delete = new Button();
            Button panel_stop = new Button();
            Button panel_start = new Button();
            Label panel_title = new Label();
            Label panel_log = new Label();
            Label panel_time = new Label();
            Label panel_percent = new Label();
            ProgressBar panel_bar = new ProgressBar();
            // 
            // start
            // 
            panel_start.Location = new System.Drawing.Point(402, 28);
            panel_start.Name = "start";
            panel_start.Size = new System.Drawing.Size(93, 31);
            panel_start.Text = "开始";
            panel_start.Tag = key;
            panel_start.UseVisualStyleBackColor = true;
            panel_start.Click += new System.EventHandler(this.panel_start_Click);
            // 
            // delete
            // 
            panel_delete.Location = new System.Drawing.Point(560, 28);
            panel_delete.Name = "delete";
            panel_delete.Size = new System.Drawing.Size(57, 31);
            panel_delete.Text = "删除";
            panel_delete.Tag = key;
            panel_delete.UseVisualStyleBackColor = true;
            panel_delete.Click += new System.EventHandler(this.panel_delete_Click);
            //       panel_delete.Click += new System.EventHandler(this.panel_delete_Click);
            // 
            // stop
            // 
            panel_stop.Location = new System.Drawing.Point(502, 28);
            panel_stop.Name = "stop";
            panel_stop.Size = new System.Drawing.Size(57, 31);
            panel_stop.Text = "终止";
            panel_stop.UseVisualStyleBackColor = true;

            // 
            // percent
            // 
            panel_percent.AutoSize = true;
            panel_percent.Location = new System.Drawing.Point(361, 37);
            panel_percent.Name = "percent";
            panel_percent.Size = new System.Drawing.Size(29, 12);
            panel_percent.Text = "";

            // 
            // bar
            // 
            panel_bar.Location = new System.Drawing.Point(12, 32);
            panel_bar.Name = "bar";
            panel_bar.Size = new System.Drawing.Size(343, 24);
            //
            // title
            // 
            panel_title.AutoSize = true;
            panel_title.Location = new System.Drawing.Point(12, 11);
            panel_title.Name = "title";
            panel_title.Size = new System.Drawing.Size(119, 12);
            panel_title.Text = value;
            //
            //time
            // 
            panel_log.AutoSize = true;
            panel_log.Location = new System.Drawing.Point(361, 11);
            panel_log.Name = "time";
            panel_log.Size = new System.Drawing.Size(119, 12);

            // 
            // log
            // 
            panel_time.AutoSize = true;
            panel_time.Location = new System.Drawing.Point(12, 65);
            panel_time.Name = "log";
            panel_time.Size = new System.Drawing.Size(71, 12);

            // 
            // task
            // 
            panel_thread.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel_thread.Controls.Add(panel_delete);
            panel_thread.Controls.Add(panel_stop);
            panel_thread.Controls.Add(panel_time);
            panel_thread.Controls.Add(panel_percent);
            panel_thread.Controls.Add(panel_start);
            panel_thread.Controls.Add(panel_bar);
            panel_thread.Controls.Add(panel_title);
            panel_thread.Controls.Add(panel_log);

            panel_thread.Location = new System.Drawing.Point(10, 10);
            panel_thread.Margin = new System.Windows.Forms.Padding(10);
            panel_thread.Name = "task";
            panel_thread.Size = new System.Drawing.Size(630, 87);

            panel_thread.ResumeLayout(false);
            panel_thread.PerformLayout();
            flowLayoutTask.Controls.Add(panel_thread);
            checked_dic.Add(key, value);

        }


        private void panel_start_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Button start = (Button)sender;
            string key = start.Tag.ToString();
            TaskThread item = new TaskThread();
            Panel taskPanel = (Panel)start.Parent;
            item.start = start;

            foreach (Control ctrl in taskPanel.Controls)
            {
                if (ctrl.Name.Equals("bar") && ctrl is ProgressBar)
                {
                    item.probar = (ProgressBar)ctrl;
                }
                if (ctrl.Name.Equals("title") && ctrl is Label)
                {
                    item.title = (Label)ctrl;
                }
                if (ctrl.Name.Equals("log") && ctrl is Label)
                {
                    item.log = (Label)ctrl;
                }
                if (ctrl.Name.Equals("time") && ctrl is Label)
                {
                    item.time = (Label)ctrl;
                }
                if (ctrl.Name.Equals("percent") && ctrl is Label)
                {
                    item.percent = (Label)ctrl;
                }
                if (ctrl.Name.Equals("delete") && ctrl is Button)
                {
                    item.delete = (Button)ctrl;
                }
                if (ctrl.Name.Equals("stop") && ctrl is Button)
                {
                    item.stop = (Button)ctrl;
                }


            }
            string gid = "";
            string cid = "";
            string[] str = key.Split(new string[] { "_" }, StringSplitOptions.None);
            if (str.Length == 2)
            {
                gid = str[0];
                cid = str[1];
            }
            item.channelItem = cManager.getItem(cid);
            item.gameItem = gManager.getItem(gid);
            item.callback = ThreadCallBack;

            Thread tWorkingThread = new Thread(item.TaskStart);
            tWorkingThread.Name = key;
            tWorkingThread.SetApartmentState(ApartmentState.STA); // 单线程
            taskThreadList.Add(tWorkingThread);
            tWorkingThread.Start();
            GC.Collect();
        }

        /// <summary>
        /// 获取打包子线程
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Thread getThread(string name)
        {
            foreach (Thread tWorkingThread in taskThreadList)
            {
                if (tWorkingThread.Name == name)
                {
                    return tWorkingThread;

                }
            }
            return null;

        }

        public void printThreadState()
        {

            foreach (Thread tWorkingThread in taskThreadList)
            {
                Console.WriteLine(tWorkingThread.Name + " " + tWorkingThread.ThreadState);
            }
        }


        private void panel_delete_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Button b = (Button)sender;
            string key = b.Tag.ToString();
            Thread tWorkingThread = getThread(key);
            if (tWorkingThread != null)
            {

                if (tWorkingThread.ThreadState.ToString() != "Stopped" && tWorkingThread.ThreadState.ToString() != "StopRequested" && tWorkingThread.ThreadState.ToString() != "Aborted" && tWorkingThread.ThreadState.ToString() != "AbortRequested")
                {
                    DialogResult dr = MessageBox.Show("该任务尚未完成", "确认是否删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        tWorkingThread.Abort();
                        deleteThread(key);
                        flowLayoutTask.Controls.Remove(b.Parent);
                        checked_dic.Remove(key);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

            }
            flowLayoutTask.Controls.Remove(b.Parent);
            deleteThread(key);
            checked_dic.Remove(key);

            GC.Collect();
        }
        /// <summary>
        /// 窗体的关闭事件处理函数，在该事件中将之前创建的线程全部终止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
            if (taskThreadList.Count > 0)
            {
                //编列自定义队列,将所有线程终止
                foreach (Thread tWorkingThread in taskThreadList)
                {
                    tWorkingThread.Abort();
                }
            }

        }

        ///////////////////////////////////////////////////////////其他设置---华丽分割线 /////////////////////////////////////////////////////////////////////////////////

        public void initPage4()
        {
            GC.Collect();
            string apktool = compManager.getApktool();
            string version = compManager.getValue("version");
            string debug = compManager.getDebug();
            string isStart = compManager.getValue("isStart");
         
            sg_last_version.Text = envConfig.lastSdk;

            if (apktool == compileManager.ApkVersion1)
            {
                apktool_version1.Checked = true;
            }
            else
            {
                apktool_version2.Checked = true;
            }

            if (version == compileManager.Version1)
            {
                version_1.Checked = true;
            }
            else
            {
                version_2.Checked = true;
            }
            if (debug == "true")
            {
                debug_true.Checked = true;

            }
            else
            {
                debug_false.Checked = true;
            }

            if (isStart == "true")
            {
                this.radio_isStart_true.Checked = true;

            }
            else
            {
                this.radio_isStart_false.Checked = true;
            }
           
           
            initSdkSet();


            lab_gamePath.Text = compManager.getValue("gameSelectFolder");
            lab_channelPath.Text = compManager.getValue("channelSelectFolder");
        }


        /// <summary>
        /// apktool 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void game_tool_200_CheckedChanged(object sender, EventArgs e)
        {
            if (this.apktool_version1.Checked)
            {
                compManager.setApktool(compileManager.ApkVersion1);
            }
            if (this.apktool_version2.Checked)
            {
                compManager.setApktool(compileManager.ApkVersion2);
            }

        }
        /// <summary>
        /// 显示命令行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void debug_true_CheckedChanged(object sender, EventArgs e)
        {
            if (this.debug_true.Checked)
            {
                compManager.setDebug("true");
            }
            else
            {
                compManager.setDebug("false");
            }


        }
        /// <summary>
        /// 工具版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void version_1_CheckedChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (this.version_1.Checked)
            {
                compManager.setVersion(compileManager.Version1);

            }
            else
            {
                compManager.setVersion(compileManager.Version2);

            }
        }
        /// <summary>
        /// 是否显示 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_isStart_true_CheckedChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (radio_isStart_true.Checked)
            {
                compManager.setValue("isStart", "true");
            }
            else
            {
                compManager.setValue("isStart", "false");
            }

        }
      

        private void btn_gamePath_Click(object sender, EventArgs e)
        {
            GC.Collect();
            folderBrowserDialog.Description = "请设置选择游戏母包路径";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string gamePath = folderBrowserDialog.SelectedPath;
                lab_gamePath.Text = gamePath;
                compManager.setValue("gameSelectFolder", gamePath);
                envConfig.gameSelectFolder = gamePath;
            }

        }

        private void btn_channelPath_Click(object sender, EventArgs e)
        {
            GC.Collect();
            folderBrowserDialog.Description = "请设置选择渠道母包路径";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                lab_channelPath.Text = path;
                compManager.setValue("channelSelectFolder", path);
                envConfig.channelSelectFolder = path;
            }
        }
        //////////////////////////////////////////////////////渠道配置--华丽分割线///////////////////////////////////////////////////////////////////////////////


        public Dictionary<string, string> allProperties = new Dictionary<string, string>();
        private Dictionary<string, string> meatas = new Dictionary<string, string>();
        private Dictionary<string, string> cparams = new Dictionary<string, string>();
        string propertiesFile;
        private channelItem selChannelMeta;
        /// <summary>
        /// 渠道配置
        /// </summary>
        private void initPage5()
        {
            GC.Collect();
            cManager.initialListView(listView5);
            cManager.initChannelMetaedData(listView5, radio_metad.Checked);
            GC.Collect();

        }

        /// <summary>
        /// 渠道管理列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (listView5.SelectedItems.Count > 0)
            {

                selChannelMeta = (channelItem)listView5.SelectedItems[0].Tag;

                foreach (ListViewItem item in listView5.Items)
                {
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.FromArgb(239, 248, 250); //恢复默认背景色 
                    channelItem citem = (channelItem)item.Tag;
                    if (citem != null && citem.flag != "true")
                    {
                        item.ForeColor = Color.Red;
                    }

                }
                //修改选中项颜色
                listView5.SelectedItems[0].SubItems[0].ForeColor = Color.SaddleBrown;
                listView5.SelectedItems[0].BackColor = Color.Silver;

                page5Clear();
                lab_meta_channel.Text = selChannelMeta.cid + "-" + selChannelMeta.name;
                propertiesFile = envConfig.channels + selChannelMeta.cid + @"\" + selChannelMeta.cid + envConfig.configExt;

                params_Load();
                Metas_Load();
                //去掉选中项背景
                listView5.SelectedItems[0].Selected = false;// 会引发第二次 该方法的调用
                listView5.SelectedItems.Clear();
            }
        }

        private void page5Clear()
        {
            GC.Collect();
            flowLayoutParam.Controls.Clear();
            flowLayoutMeta.Controls.Clear();
            allProperties.Clear();
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        private void params_Load()
        {
            GC.Collect();
            // 加载游戏参数
            if (!FileUtil.checkFile(propertiesFile))
            {
                return;
            }
            PpHelper pptHelper = new PpHelper(propertiesFile);
            Dictionary<string, string> dic;
            try
            {
                dic = pptHelper.getAllProperties();
            }
            catch (Exception)
            {
                MessageBox.Show("获取gameConfig参数配置异常，请检查！");
                return;
            }

            if (dic != null)
            {
                foreach (KeyValuePair<string, string> p in dic)
                {
                    string name = p.Key;
                    string value = p.Value;
                    param_item_add(name, value);
                }
            }
            else
            {
                this.flowLayoutParam.Controls.Clear();

            }
        }
        private void btn_param_add_Click(object sender, EventArgs e)
        {
            param_item_add("", "");
        }
        private void param_item_add(string name, string value)
        {
            GC.Collect();
            TextBox param_name = new TextBox();
            TextBox param_value = new TextBox();
            Label param_ch = new Label();
            Button param_delete = new Button();
            Panel param_panel = new Panel();
            // 
            // param_name
            // 
            param_name.Location = new System.Drawing.Point(4, 4);
            param_name.Name = "param_name";
            param_name.Size = new System.Drawing.Size(100, 21);
            param_name.TabIndex = 0;
            param_name.Text = name;
            // 
            // param_value
            // 
            param_value.Location = new System.Drawing.Point(131, 5);
            param_value.Name = "param_value";
            param_value.Size = new System.Drawing.Size(220, 21);
            param_value.TabIndex = 1;
            param_value.Text = value;
            // 
            // param_delete
            // 
            param_delete.Location = new System.Drawing.Point(357, 4);
            param_delete.Name = "param_delete";
            param_delete.Size = new System.Drawing.Size(54, 23);
            param_delete.TabIndex = 2;
            param_delete.Text = "删除";
            param_delete.UseVisualStyleBackColor = true;
            param_delete.Click += new System.EventHandler(this.param_delete_Click);
            // 
            // param_ch
            // 
            param_ch.AutoSize = true;
            param_ch.Location = new System.Drawing.Point(110, 9);
            param_ch.Name = "param_ch";
            param_ch.Size = new System.Drawing.Size(11, 12);
            param_ch.TabIndex = 3;
            param_ch.Text = "=";
            // 
            // param_panel
            // 
            param_panel.Controls.Add(param_ch);
            param_panel.Controls.Add(param_delete);
            param_panel.Controls.Add(param_value);
            param_panel.Controls.Add(param_name);
            param_panel.Location = new System.Drawing.Point(3, 3);
            param_panel.Name = "param_panel";
            param_panel.Size = new System.Drawing.Size(414, 31);
            flowLayoutParam.Controls.Add(param_panel);
        }

        private void btn_meta_add_Click(object sender, EventArgs e)
        {
            meta_item_add("", "");
        }
        private void meta_item_add(string name, string value)
        {
            GC.Collect();
            TextBox meta_k = new TextBox();
            TextBox meta_v = new TextBox();
            Label meta_c = new Label();
            Button meta_delete = new Button();
            Panel panel_meta_item = new Panel();
            // 
            // meta_k
            // 
            meta_k.Location = new System.Drawing.Point(13, 6);
            meta_k.Name = "meta_k";
            meta_k.Size = new System.Drawing.Size(138, 21);
            meta_k.Text = name;
            // 
            // meta_v
            // 
            meta_v.Location = new System.Drawing.Point(203, 6);
            meta_v.Name = "meta_v";
            meta_v.Size = new System.Drawing.Size(133, 21);
            meta_v.Text = value;
            // 
            // meta_c
            // 
            meta_c.AutoSize = true;
            meta_c.Location = new System.Drawing.Point(171, 9);
            meta_c.Name = "meta_c";
            meta_c.Size = new System.Drawing.Size(11, 12);
            meta_c.Text = "=";
            //
            // meta_delete
            //
            meta_delete.Location = new System.Drawing.Point(345, 4);
            meta_delete.Name = "meta_delete";
            meta_delete.Size = new System.Drawing.Size(51, 23);
            meta_delete.Text = "删除";
            meta_delete.UseVisualStyleBackColor = true;
            meta_delete.Click += new System.EventHandler(this.meta_delete_Click);
            // 
            // panel_meta_item
            // 
            panel_meta_item.Controls.Add(meta_delete);
            panel_meta_item.Controls.Add(meta_c);
            panel_meta_item.Controls.Add(meta_v);
            panel_meta_item.Controls.Add(meta_k);

            panel_meta_item.Location = new System.Drawing.Point(3, 3);
            panel_meta_item.Name = "panel_meta_item";
            panel_meta_item.Size = new System.Drawing.Size(410, 34);

            flowLayoutMeta.Controls.Add(panel_meta_item);

        }

        private void meta_delete_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            flowLayoutMeta.Controls.Remove(b.Parent);
        }
        private void param_delete_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            this.flowLayoutParam.Controls.Remove(b.Parent);
        }


        private void btn_meta_autoadd_Click(object sender, EventArgs e)
        {
            GC.Collect();
            if (!FileUtil.checkFile(propertiesFile))
            {
                return;
            }
            flowLayoutMeta.Controls.Clear();
            PpHelper pptHelper = new PpHelper(propertiesFile);
            Dictionary<string, string> dic;
            try
            {
                dic = pptHelper.getAllProperties();
            }
            catch (Exception)
            {
                MessageBox.Show("获取gameConfig参数配置异常，请检查！");
                return;
            }

            foreach (KeyValuePair<string, string> p in dic)
            {
                string name = p.Key.ToUpper();
                string value = p.Key;
                meta_item_add(name, value);
            }
        }

        private void btn_saveParamMeta_click(object sender, EventArgs e)
        {
            GC.Collect();
            meatas.Clear();
            foreach (Control panel in flowLayoutMeta.Controls)
            {
                //  Panel panelMeta = (Panel)panel;
                string name = "";
                string value = "";
                foreach (Control ctrl in panel.Controls)
                {

                    if (ctrl is TextBox && ctrl.Name.Equals("meta_k"))
                    {
                        name = ctrl.Text;
                    }
                    if (ctrl is TextBox && ctrl.Name.Equals("meta_v"))
                    {
                        value = ctrl.Text;
                    }
                }
                if (name == "" || value == "")
                {
                    continue;
                }
                meatas.Add(name, value);
            }
            cManager.addChannelMeta(selChannelMeta, meatas);

            cparams.Clear();
            foreach (Control panel in this.flowLayoutParam.Controls)
            {
                //  Panel panelMeta = (Panel)panel;
                string name = "";
                string value = "";
                foreach (Control ctrl in panel.Controls)
                {

                    if (ctrl is TextBox && ctrl.Name.Equals("param_name"))
                    {
                        name = ctrl.Text;
                    }
                    if (ctrl is TextBox && ctrl.Name.Equals("param_value"))
                    {
                        value = ctrl.Text;
                    }
                }
                if (name == "" || value == "")
                {
                    continue;
                }
                cparams.Add(name, value);
            }

            cManager.setChannelProperty(selChannelMeta, cparams);

            MessageBox.Show("保存成功");
        }

        private void btn_delMeta_Click(object sender, EventArgs e)
        {
            GC.Collect();
            cManager.delChannelMeta(selChannelMeta.cid);
            MessageBox.Show("删除成功");
            Metas_Load();
        }

        private void Metas_Load()
        {
            GC.Collect();
            Dictionary<string, string> metas = cManager.getChannelMetas(selChannelMeta.cid);

            if (metas != null)
            {
                foreach (KeyValuePair<string, string> p in metas)
                {
                    string name = p.Key.ToUpper();
                    string value = p.Value;
                    meta_item_add(name, value);
                }
            }
            else
            {
                flowLayoutMeta.Controls.Clear();

            }

        }

        private void meta_keyword_TextChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (listView5.Items.Count > 0 && meta_keyword.Text.Trim() != string.Empty)
            {

                for (int i = 0; i < listView5.Items.Count; i++)
                {
                    if (listView5.Items[i].SubItems[1].Text.StartsWith(meta_keyword.Text.Trim()))
                    {
                        listView5.Items[i].Selected = true;
                        listView5.Items[i].EnsureVisible();
                        return;
                    }

                    if (listView5.Items[i].SubItems[0].Text.IndexOf(meta_keyword.Text.Trim())>=0)
                    {
                        listView5.Items[i].Selected = true;
                        listView5.Items[i].EnsureVisible();
                        return;
                    }

                }
            }
        }

        private void radio_meta_all_CheckedChanged(object sender, EventArgs e)
        {
            cManager.initChannelMetaedData(listView5, radio_metad.Checked);
            GC.Collect();
        }

        private void radio_skin_yes_CheckedChanged(object sender, EventArgs e)
        {
            skinUI1.Active = !skinUI1.Active;

        }

        private void btn_skinfile_Click(object sender, EventArgs e)
        {
            GC.Collect();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = envConfig.currenPath + @"\skins";
            dialog.Filter = "skin files (*.skn)|*.skn";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                skinUI1.LoadSkinFile(dialog.FileName);
                lab_skin.Text = Path.GetFileName(dialog.FileName);
                string strmainname = Path.GetFileNameWithoutExtension(dialog.FileName);
                compManager.setValue("skinfile", strmainname + ".skn");

            }
        }

        private void btn_clearCache_Click(object sender, EventArgs e)
        {
            clear_probar.Value = 0;
            btn_clearCache.Enabled = false;
            clearCacheThread cache = new clearCacheThread();
            cache.temp = checkBox_res.Checked;
            cache.unsignapk = checkBox_unsign.Checked;
            cache.signapk = checkBox_signed.Checked;
            cache.compile = checkBox_compile.Checked;
            cache.errlog = checkBox_errlog.Checked;

            cache.probar = clear_probar;
            cache.clear_lab = clear_lab;
            cache.callback = ClearCallBack;

            Thread tWorkingThread = new Thread(cache.doWork);
            tWorkingThread.Name = cache.name;
            tWorkingThread.SetApartmentState(ApartmentState.STA); // 单线程
            taskThreadList.Add(tWorkingThread);
            tWorkingThread.Start();
            GC.Collect();


        }

        /// <summary>
        /// 打包线程回调
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        private void ThreadCallBack(string key, string msg, Button sender)
        {
            GC.Collect();
            Thread tWorkingThread = getThread(key);
            deleteThread(key);
            if (msg == TaskThread.MSG_ABORT)
            {
                if (tWorkingThread != null)
                {
                    try
                    {
                        tWorkingThread.Abort();
                    }
                    catch (ThreadAbortException)
                    {
                        ////不作处理          
                        //printThreadState();
                    }
                    catch (Exception)
                    {
                        //不作处理        
                    }
                }
            }
            if (msg == TaskThread.MSG_DELETE)
            {
                flowLayoutTask.Controls.Remove(sender.Parent);
                checked_dic.Remove(key);
                if (tWorkingThread != null)
                {
                    try
                    {
                        tWorkingThread.Abort();
                    }
                    catch (ThreadAbortException)
                    {
                        ////不作处理          
                        //printThreadState();
                    }
                    catch (Exception)
                    {
                        //不作处理        
                    }
                }

            }
            if (msg == TaskThread.MSG_COMPLETE)
            {
                // deleteThread(key);
            }
            GC.Collect();
        }
        /// <summary>
        /// 移除线程
        /// </summary>
        public void deleteThread(string name)
        {
            GC.Collect();
            Thread tWorkingThread = getThread(name);
            if (tWorkingThread != null)
            {
                taskThreadList.Remove(tWorkingThread);
            }

        }
        /// <summary>
        /// 清理回调
        /// </summary>
        private void ClearCallBack(string key, string msg)
        {
            GC.Collect();
            deleteThread(key);
            btn_clearCache.Enabled = true;
            GC.Collect();
        }


        /// <summary>
        /// 解包回调
        /// </summary> 
        private void ChannelDeBack(string key, string msg)
        {
            GC.Collect();
            string path = key + @"smali\com\sandglass\game\model\SGConst.smali";
            string sdk = FileUtil.getSdkVersionBySmali(path);
            channel_sdk_version.Text = "sdk版本：" + sdk;
        }
        /// <summary>
        /// 解包回调
        /// </summary> 
        private void GameDeBack(string key, string msg)
        {
            GC.Collect();
            string path = key + @"smali\com\sandglass\game\model\SGConst.smali";
            string sdk = FileUtil.getSdkVersionBySmali(path);
            game_sg_version.Text = "sdk版本：" + sdk;

        }

        private void initSdkSet()
        {
            this.flowLayoutPanelSdk.Controls.Clear();
            string sdk = compManager.getSdk();

            RadioButton rbNo = new RadioButton();
            rbNo.AutoSize = true;
            rbNo.Location = new System.Drawing.Point(3, 3);
            rbNo.Name = "sdk_no";
            rbNo.Text = "否"; 
            rbNo.UseVisualStyleBackColor = true;
            rbNo.CheckedChanged += new System.EventHandler(this.radioButtonSdk_CheckedChanged);
            rbNo.Tag = "false";  
            if (sdk == "false")
            {
                rbNo.Checked = true;
            }

            this.flowLayoutPanelSdk.Controls.Add(rbNo);
 
            string path = envConfig.sdk;
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (FileInfo file in folder.GetFiles("*.dex"))
            {
                Console.WriteLine(file.FullName);
                string sdkname = Path.GetFileNameWithoutExtension(file.Name);
                RadioButton rb = new RadioButton();
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(3, 3);
                rb.Name = sdkname;
                rb.Text = sdkname;
                rb.UseVisualStyleBackColor = true;
                rb.CheckedChanged += new System.EventHandler(this.radioButtonSdk_CheckedChanged);
                rb.Tag = sdkname; 
                if (sdk == sdkname)
                {
                    rb.Checked = true;
                }
                this.flowLayoutPanelSdk.Controls.Add(rb); 
            } 
        }
        /// <summary>
        /// sdk 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_sdk_add_Click(object sender, EventArgs e)
        {
            GC.Collect(); 

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = envConfig.currenPath;
            dialog.Filter = "skin files (*.jar)|*.jar"; 
            if (dialog.ShowDialog() == DialogResult.OK)
            { 
                string dexname = envConfig.sdk + Path.GetFileNameWithoutExtension(dialog.FileName) + ".dex"; 
                Process makeDex = new Process();  
                makeDex.StartInfo.UseShellExecute = false;
                makeDex.StartInfo.CreateNoWindow = true;
                makeDex.StartInfo.FileName = envConfig.toolPath + @"\dx.bat";
                makeDex.StartInfo.RedirectStandardInput = true;
                makeDex.StartInfo.RedirectStandardOutput = true;
                makeDex.StartInfo.RedirectStandardError = true; 
                makeDex.StartInfo.Arguments = " --dex --output=" + dexname + " " + dialog.FileName;
                makeDex.Start();
                makeDex.WaitForExit(); 
                initSdkSet();
            }
        
        }
            
        private void radioButtonSdk_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if(rb.Checked){
                string sdk = (string)rb.Tag;
                compManager.setSdk(sdk);
                GC.Collect();
                 
            } 

        }
           
         




        /////////////////////////////////////////////////////废弃，备份代码////////////////////////////////////////////////////////////////////////////




        /////////////////////////

    }
}
