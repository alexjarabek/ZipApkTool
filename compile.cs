using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sandGlass
{
    public partial class compile : Form
    {
        public compile()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent; 
        }
        public string currenChannel = null;
        public string game
        {
            get { return txtGame.Text.ToString().Trim(); }
        }
        public string channel
        {
            get { return currenChannel; }
        }
        public string fileName = "";


        private void getStoreFile_Click(object sender, EventArgs e)
        {
            string storePath = "";
            if (getFile.ShowDialog() == DialogResult.OK)
            {
                storePath  = getFile.FileName.ToString();
          //      lbKeyStore.Text = storePath;
            }
        }

        private void compile_Load(object sender, EventArgs e)
        {
            string lastGame = envConfig.currenPath + @"\lastGame" + envConfig.configExt;
            if (FileUtil.checkFile(lastGame))
            {
                PpHelper pptHelper1 = new PpHelper(lastGame);
                txtGame.Text = pptHelper1.GetPropertiesText("game");
                string signCfg = envConfig.gameConfig + game + @"\sign" + envConfig.configExt;
                if (File.Exists(signCfg))
                {
                    PpHelper pptHelper = new PpHelper(signCfg);
                    txtSignPwd.Text = pptHelper.GetPropertiesText("signPwd");
                    txtAlias.Text = pptHelper.GetPropertiesText("alias");
                    txtKeyPwd.Text = pptHelper.GetPropertiesText("keyPwd");
                    if (!FileUtil.checkFile(envConfig.gameConfig + game + @"\" + game + ".keystore"))
                        txtKeyPwd.Text = "";
                    else
                        lbPwdFile.Text = "sign.keystore";
                    cfgLb.BackColor = Color.Green;
                    cfgLb.Text = " 检测到" + game + "的配置文件";
                }
            }

            // this.StartPosition = FormStartPosition.CenterScreen;
            channelBoxs.CheckOnClick = true;
            List<string> folders = FileUtil.getFolders(envConfig.channelPath);
            foreach (string folder in folders)
            {
                string channel = new DirectoryInfo(folder).Name;
                if (channel == ".svn") continue;

                string channelXml = envConfig.channelPath + channel + @"\channel.xml";
                try
                {
                    string channelName = XmlHelper.getChannelName(channelXml);
                    channelBoxs.Items.Add(new check(channel, channelName));
                }
                catch (Exception EE)
                {

                    MessageBox.Show(EE.Message+channelXml + " 不存在，或者有错误");
                }

               
               
            }
        }

        private void compile_Click(object sender, EventArgs e)
        {
        }
        public class check
        {
            String m_Text = "";
            String m_Value = "";

            public check(String Text)
            {
                m_Text = Text;
            }
            public check(String Text, String Value)
            {
                m_Text = Text;
                m_Value = Value;
            }

            public String Text
            {
                get { return m_Text; }
                set { m_Text = value; }
            }
            public String Value
            {
                get { return m_Value; }
                set { m_Value = value; }
            }

            public override string ToString()
            {
                return this.Text;
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string game = txtGame.Text.ToString().Trim();
            string signCfg = envConfig.gameConfig + game + @"\sign" + envConfig.configExt;
            if (!FileUtil.checkFile(envConfig.gameConfig + game + @"\"+game+".keystore"))
            {
                MessageBox.Show("请选择签名文件");
                return;
            }
            string lineStr = "";  
            lineStr = "signPwd=" + txtSignPwd.Text.ToString().Trim()+ "\r\n";
            lineStr += "alias=" + txtAlias.Text.ToString().Trim() + "\r\n";
            lineStr += "keyPwd=" + txtKeyPwd.Text.ToString().Trim() + "\r\n";
            FileUtil.writeContent(signCfg, lineStr);
            GC.Collect();
            if (channelBoxs.SelectedItems.Count <= 0)
                return;
            //todo get all checked channels
          //  Dictionary<string, string> dic = new Dictionary<string, string>();          
            doWork deCompile = new doWork();
            deCompile.game = this.game;
            deCompile.gameName = txtGameName.Text.ToString();
            deCompile.picString = lbIco.Text.ToString();
            foreach (check channel in channelBoxs.CheckedItems)
            {
                string txt = channel.Text;
                string val = channel.Value;
                deCompile.channels.Add(txt, val);
            }
     //       this.Hide();
            deCompile.StartPosition = FormStartPosition.CenterScreen;
            deCompile.ShowDialog();
      //      Application.Exit();    
        }

        private void channelBoxs_MouseUp(object sender, MouseEventArgs e)
        {
            GC.Collect();
            if (channelBoxs.SelectedItems.Count <= 0)
                return;
            string channelTxt = ((check)channelBoxs.SelectedItem).Value;
            string channelVal = ((check)channelBoxs.SelectedItem).Text;
            bool flag = false;
            foreach (check channel in channelBoxs.CheckedItems)
            {
                string txt = channel.Text;
                string val = channel.Value;
                if (txt == channelVal || val == channelVal)
                {
                    flag = true;
                }
            }
            if (flag == true)
            {
                propertiesEdit ppt = new propertiesEdit();
                ppt.channelTxt = channelTxt;
                ppt.channelVal = channelVal;
                ppt.game = this.game;
                ppt.StartPosition = FormStartPosition.CenterScreen;
                ppt.gamePic = lbIco.Text;
                ppt.ShowDialog();
                if (ppt.DialogResult == DialogResult.OK)
                {
                    //  MessageBox.Show("已确认");
                    GC.Collect();
                }
                else
                {
                    //  MessageBox.Show("未确认！");
                }
                GC.Collect();
            }
            else
            {
                //  MessageBox.Show("false");
            }
        }

        private void btnPic_Click(object sender, EventArgs e)
        {
            GC.Collect();
            string picCfg = envConfig.gameConfig + game + @"\" + this.game + ".png";

            if (!FileUtil.checkFile(picCfg))
            {
                if (getPicFile.ShowDialog() == DialogResult.OK)
                {
                    lbIco.Text = getPicFile.FileName.ToString();
                    picBoxIco.Image = Image.FromFile(lbIco.Text);

                    File.Copy(lbIco.Text, envConfig.gameConfig + game + @"\" + this.game + ".png", true);
                }
                getPicFile.Dispose();
            }
            else
            {
                lbIco.Text = picCfg.ToString();
                picBoxIco.Image = Image.FromFile(picCfg);

            }

           
            //if (getPicFile.ShowDialog() == DialogResult.OK)
            //{               
            //    lbIco.Text = getPicFile.FileName.ToString();
            //    picBoxIco.Image = Image.FromFile(lbIco.Text);               
            //}
            //getPicFile.Dispose();


            GC.Collect();
        }

        private void txtGame_KeyUp(object sender, KeyEventArgs e)
        {
            cfgLb.BackColor = Color.Red;
            string game = txtGame.Text.ToString().Trim();
            string signCfg = envConfig.gameConfig + game + @"\sign" + envConfig.configExt;
            if (!FileUtil.checkFile(signCfg))
            {
                txtSignPwd.Text = "lyhd2015";
                txtAlias.Text = "lyhd";
                txtKeyPwd.Text = "lyhd2015";
                lbPwdFile.Text = "please chose sign file";
                cfgLb.Text = "没有检测"+game+"的配置文件";
            }
            else
            {
                PpHelper pptHelper = new PpHelper(signCfg);
                txtSignPwd.Text = pptHelper.GetPropertiesText("signPwd");
                txtAlias.Text = pptHelper.GetPropertiesText("alias");
                txtKeyPwd.Text = pptHelper.GetPropertiesText("keyPwd");
                if (!FileUtil.checkFile(envConfig.gameConfig + game + @"\"+game+".keystore"))
                    txtKeyPwd.Text = "";
                else
                    lbPwdFile.Text = "sign.keystore";
                cfgLb.BackColor = Color.Green;
                cfgLb.Text = " 检测到"+game+"的配置文件";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  string signCfg = envConfig.gameConfig + this.game + @"\sign" + envConfig.configExt;
            if (getPwdFile.ShowDialog() == DialogResult.OK)
            {
                lbPwdFile.Text = getPwdFile.FileName.ToString().Trim();
                if (!Directory.Exists(envConfig.gameConfig + game))
                    Directory.CreateDirectory(envConfig.gameConfig + game);
                File.Copy(getPwdFile.FileName.ToString().Trim(), envConfig.gameConfig + game + @"\"+this.game+".keystore", true);
            }
            getPwdFile.Dispose();
        }
        /// <summary>
        /// 打包工具新版入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewtool_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.StartPosition = FormStartPosition.CenterScreen;
            home.ShowDialog();
            envConfig.version = compileManager.Version2;
            Application.Exit();   
        }

            
    }
}
