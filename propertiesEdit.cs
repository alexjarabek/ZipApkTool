using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace sandGlass
{
    public partial class propertiesEdit : Form
    {
        public propertiesEdit()
        {
            InitializeComponent();
         //   this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ControlBox = false;
        }
        public string channelTxt = null;
        public string channelVal = null;
        public string game       = null;
        private string propertiesFile = null;
        public int dicNum = 0;
        public string gamePic = null;
        public int iHight = 532;
        public Dictionary<string, string> allProperties=new Dictionary<string,string>();
     
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            //MessageBox.Show(dicNum.ToString());          
            if (dicNum > 0)
            {         
                Dictionary<int, string> tmpDataK = new Dictionary<int, string>();
                Dictionary<int, string> tmpDataV = new Dictionary<int, string>();
                string writeFileStr = "";              
                for(int i=1;i<=dicNum;i++)
                {
                    foreach (Control c in pannelPts.Controls)
                    {
                        if (c.Name == "K_" + i)
                        {
                            TextBox k = (TextBox)c;
                            if(k.Text.ToString().Length>0)
                                 tmpDataK[i] = k.Text.ToString();
                        }
                        else if (c.Name == "V_" + i)
                        {
                            TextBox v = (TextBox)c;
                            if(v.Text.ToString().Length>0)
                                tmpDataV[i] = v.Text.ToString();
                        }
                        else
                        { 
                        }                                                
                    }
                    if (tmpDataK.Count() > 0 && tmpDataK[i].Trim().Length>0 && tmpDataV[i].Trim().Length>0)
                    {
                        string lineStr = tmpDataK[i] + "=" + tmpDataV[i] + "\r\n";
                        writeFileStr += lineStr;
                    }
                }
                FileUtil.writeContent(propertiesFile, writeFileStr);
            }
            else
            {
                MessageBox.Show("没有添加属性!");
            }
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            GC.Collect();
            if (screenHorizontal.Checked == true)
            {
                //横
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "horizontal");
            }
            if (screenResolute.Checked == true)
            {
                //坚
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "resolute");
            }
            if (screenAuto.Checked == true)
            {
                //坚
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "auto");
            }
            GC.Collect();
           this.DialogResult = DialogResult.OK;
        }

        private void propertiesEdit_Load(object sender, EventArgs e)
        {
            if (checkReadme())
            {
                iHight = 768;
                string readFile = envConfig.channelPath + @"/" + channelVal + @"/readme.txt";
                FileStream fs = new FileStream(readFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                richTxt.Text = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
          //  MessageBox.Show(gamePic);
            rbNo.Checked = true;
            this.Size = new System.Drawing.Size(454, iHight);
            pannalPic.Hide();
            if (game.Length <= 0)
                this.Close();
            lbchannelTxt.Text = channelTxt;
            lbchannel.Text = channelVal;
            lbgame.Text = game;

            // 加载参数
            propertiesFile = envConfig.gameConfig+game+@"\"+channelVal+envConfig.configExt;
            if (!Directory.Exists(envConfig.gameConfig + @"\" + game))
                Directory.CreateDirectory(envConfig.gameConfig + @"\" + game);
            if(!FileUtil.checkFile(propertiesFile))
            {
               // envConfig.channelPath + channelVal + @"\".;
                string tplPtFile = envConfig.channelPath + channelVal + @"\" + channelVal + envConfig.configExt;
              //  MessageBox.Show(tplPtFile);
                if (FileUtil.checkFile(tplPtFile))
                {               
                    File.Copy(tplPtFile,  propertiesFile, true);
                }
                else
                {
                     string lineStr ="id=0\r\n";
                     lineStr += "key=0\r\n";
                     lineStr += "channel="+channelVal.ToUpper()+"\r\n";
                     lineStr += "package=com." + game + "." + channelVal + "." + channelVal;
                     FileUtil.writeContent(propertiesFile, lineStr);
                }
            }

            readPt();
        }
        private void readPt()
        {
            PpHelper pptHelper = new PpHelper(propertiesFile);
            Dictionary<string, string> dic = pptHelper.getAllProperties();
            dicNum = dic.Count();
            int i = 1;
            foreach (KeyValuePair<string, string> p in dic)
            {
                this.allProperties.Add(p.Key, p.Value);
                TextBox txtBoxK = new TextBox();
                txtBoxK.Name = "K_" + i.ToString();
                txtBoxK.Text = p.Key;
                txtBoxK.Left = 0;
                txtBoxK.Top = i * 50;
                Label l1 = new Label();
                l1.Text = "=";
                txtBoxK.Location = new Point(pannelPts.Location.X - 20, i * 25);
                l1.Location = new Point(120, i * 25);
                l1.Size = new System.Drawing.Size(10, 23);
                txtBoxK.KeyUp += new KeyEventHandler(this.txt_Change);
                pannelPts.Controls.Add(txtBoxK);
                pannelPts.Controls.Add(l1);
                TextBox txtBoxV = new TextBox();
                txtBoxV.Name = "V_" + i.ToString();
                txtBoxV.Text = p.Value;
                txtBoxV.Location = new Point(130, i * 25);
                txtBoxV.Size = new System.Drawing.Size(230, 23);
                txtBoxV.Top = i * 25;
                txtBoxV.KeyUp += new KeyEventHandler(this.txt_Change);
                pannelPts.Controls.Add(txtBoxV);
                i++;
            }
        }
        private void addAtr_Click(object sender, EventArgs e)
        {
            dicNum  += 1;
           // int num = allProperties.Count();
            int i = dicNum;
            TextBox txtBoxK = new TextBox();
            txtBoxK.Name = "K_" + i.ToString();
            txtBoxK.Text ="";
            txtBoxK.Left = 0;
            txtBoxK.Top = i * 50;
            Label l1 = new Label();
            l1.Text = "=";
            txtBoxK.Location = new Point(pannelPts.Location.X - 20, i * 25);
            l1.Location = new Point(120, i * 25);
            l1.Size = new System.Drawing.Size(10, 23);
            txtBoxK.KeyUp += new KeyEventHandler(this.txt_Change);            
            pannelPts.Controls.Add(txtBoxK);
            pannelPts.Controls.Add(l1);
            TextBox txtBoxV = new TextBox();
            txtBoxV.Name = "V_" + i.ToString();
            txtBoxV.Text ="";
            txtBoxV.Location = new Point(130, i * 25);
            txtBoxV.Size = new System.Drawing.Size(230, 23);
            txtBoxV.Top = i * 25;
            txtBoxV.KeyUp += new KeyEventHandler(this.txt_Change);            
            pannelPts.Controls.Add(txtBoxV);         
            i++;
        }
        public void txt_Change(object sender,KeyEventArgs e)
        {
         //  TextBox txt = sender as TextBox;
         //   MessageBox.Show(txt.Text.ToString() + ",name:"+txt.Name);

         //   this.allProperties.Add(txt.Text.ToString());
        }
        private bool isDebug()
        {
            bool debug = false;
            PpHelper pt = new PpHelper(envConfig.currenPath + @"\env.properties");
            if (pt.GetPropertiesText("debug") == "1")
                debug = true;
            else
                debug = false;
            return debug;
        }
        private bool checkReadme()
        {
            bool flag = false;
            if(FileUtil.checkFile(envConfig.channelPath + @"/" + channelVal + @"/readme.txt"))
                flag = true;
            return flag; 
        }
        private void btnChannelApk_Click(object sender, EventArgs e)
        {
            GC.Collect();
            getChannelApk.InitialDirectory = envConfig.channelPath + channelVal;
            if (getChannelApk.ShowDialog() == DialogResult.OK)
            {
                lbUpChannel.Text = "渠道更新中，请勿关闭窗口";
                button1.Hide();
                string command = envConfig.currenPath + @"\apktool\apktool.bat";
                string decompilePath = envConfig.channelPath + channelVal + @"\apkBase\";
                if (Directory.Exists(decompilePath))
                    FileUtil.deleteFolder(decompilePath);
                //   Directory.CreateDirectory(decompilePath);
                Process p = new Process();
                string channelApk = null;
                if (getChannelApk.ShowDialog() == DialogResult.OK)
                {
                    channelApk = getChannelApk.FileName.ToString();
                }
                getChannelApk.Dispose();
                GC.Collect();
                if (this.isDebug() == false)
                {
                          p.StartInfo.UseShellExecute = false;
                        p.StartInfo.CreateNoWindow = true;
                }
                else
                {
                    command = envConfig.currenPath + @"\apktool\apktool-debug.bat";
                }
                p.StartInfo.FileName = command;
                p.StartInfo.Arguments = " d " + channelApk + "  -o " + decompilePath;
                p.Start();
                p.WaitForExit();
                button1.Show();
                lbUpChannel.Text = "渠道母包更新完成.";
                string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
                string node = "channel";
                string channelPackage = getPackName(decompilePath + @"\AndroidManifest.xml");
                File.Copy(decompilePath + @"\AndroidManifest.xml", envConfig.channelPath + channelVal + @"\AndroidManifest.xml", true);
                XmlHelper.XmlAttributeEdit(channelXml, node, "chanelPackage", channelPackage);
                GC.Collect();

                //  MessageBox.Show(channelApk+"渠道母包更新完成.");
            }
        }
        private string getPackName(string gameXml)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(@gameXml);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"manifest");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute("package");
        }
        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            GC.Collect();
            if (fbYes.Checked == true)
            {
                XmlHelper.XmlAttributeEdit(channelXml, node, "useWater", "yes");
                // 使用角标，默认平铺 ， 100%

                XmlHelper.XmlAttributeEdit(channelXml, node, "waterPos", "AllIn");
                XmlHelper.XmlAttributeEdit(channelXml, node, "waterAlpha", "1");
                getPic();

                this.Size = new System.Drawing.Size(878, iHight);
                pannalPic.Show();
            }
            else if (rbNo.Checked==true)
            {
                XmlHelper.XmlAttributeEdit(channelXml, node, "useWater","no");
                this.Size = new System.Drawing.Size(454, iHight);
                pannalPic.Hide();
            }            
            else
            {
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "auto");
            }
            GC.Collect();
        }
        private void getPic()
        {
            GC.Collect();
            string picPath = null;
            string channelPath = envConfig.channelPath + channelVal + @"\";
            string savePath = envConfig.channelPath + channelVal + @"\" + channelVal + ".png";
            if (!FileUtil.checkFile(savePath))
            {
                openChannelPic.InitialDirectory = envConfig.channelPath + channelVal;
                if (openChannelPic.ShowDialog() == DialogResult.OK)
                {
                    picPath = openChannelPic.FileName.ToString();
                    picShow.Image = Image.FromFile(picPath);
                    lbPicWork.Text = "各尺寸角标生成中.";
                    copyIco(picPath, channelPath);
                    lbPicWork.Text = "角标生成成功.";
                }
                openChannelPic.Dispose();
            }
            else
            {
                picShow.Image = Image.FromFile(savePath);
                copyIco(savePath, channelPath);
                lbPicWork.Text = "角标生成成功.";
            }

            GC.Collect();
            
        }
        private void cbPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            string pos  = ImageCut.ImagePosition.Center.ToString();
            string getPos = cbPos.SelectedIndex.ToString();
            switch (getPos)
            {
               case "1":
                    pos = ImageCut.ImagePosition.LeftTop.ToString();
                    break;
                case "2":
                    pos = ImageCut.ImagePosition.TopMiddle.ToString();
                    break;
                case "3":
                    pos = ImageCut.ImagePosition.RightTop.ToString();
                    break;
                case "4":
                    pos = ImageCut.ImagePosition.Center.ToString();
                    break;
                case "5":
                    pos = ImageCut.ImagePosition.LeftBottom.ToString();
                    break;
                case "6":
                    pos = ImageCut.ImagePosition.BottomMiddle.ToString();
                    break;
                case "7":
                    pos = ImageCut.ImagePosition.RigthBottom.ToString();
                    break;
                case "8":
                    pos = ImageCut.ImagePosition.AllIn.ToString();
                    break;
                default :
                    pos = ImageCut.ImagePosition.Center.ToString();
                    break;
                
            }
            XmlHelper.XmlAttributeEdit(channelXml, node, "waterPos", pos);

       //     MessageBox.Show(cbPos.SelectedIndex.ToString());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            string alpha = ((decimal)(comboBox1.SelectedIndex) / 10).ToString() ;
            XmlHelper.XmlAttributeEdit(channelXml, node, "waterAlpha", alpha);
        }
        private void copyIco(string picPath, string channelPath)
        {
            FileUtil.setImg(picPath, channelPath + channelVal+".png", 512,512);
            /*
            FileUtil.setImg(picPath, channelPath + "uc_d.png", 32 / 4, 32 / 4);
            FileUtil.setImg(picPath, channelPath + "uc_hd.png", 72 / 4, 72 / 4);
            FileUtil.setImg(picPath, channelPath + "uc_md.png", 48 / 4, 48 / 4);
            FileUtil.setImg(picPath, channelPath + "uc_xhd.png", 96 / 4, 96 / 4);
            FileUtil.setImg(picPath, channelPath + "uc_xxhd.png", 144 / 4, 144 / 4);
            */
        }

        private void btnGetPic_Click(object sender, EventArgs e)
        {
            GC.Collect();
            string picPath = null;
            openChannelPic.InitialDirectory = envConfig.channelPath + channelVal;
            if (openChannelPic.ShowDialog() == DialogResult.OK)
            {
                picPath = openChannelPic.FileName.ToString();
                picShow.Image = Image.FromFile(picPath);
                lbPicWork.Text = "各尺寸角标生成中.";
                 string channelPath = envConfig.channelPath + channelVal + @"\";
                copyIco(picPath,channelPath);
                lbPicWork.Text = "角标生成成功.";
            }
            openChannelPic.Dispose();
            GC.Collect();           
        }

        private void screenResolute_TextChanged(object sender, EventArgs e)
        {
  
        }

        private void screenAuto_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void screenHorizontal_TextChanged(object sender, EventArgs e)
        {

        }

        private void screenHorizontal_MouseCaptureChanged(object sender, EventArgs e)
        {
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            GC.Collect();
            if (screenHorizontal.Checked == true)
            {
                //横
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "horizontal");
            }
        }

        private void screenResolute_MouseCaptureChanged(object sender, EventArgs e)
        {
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            GC.Collect();
            if (screenResolute.Checked == true)
            {
                //坚
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "resolute");
            }
            //         else
            //        {
            //            XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "auto");
            //        }
            GC.Collect();
        }

        private void screenAuto_MouseCaptureChanged(object sender, EventArgs e)
        {
            string channelXml = envConfig.channelPath + channelVal + @"\channel.xml";
            string node = "channel";
            GC.Collect();
            if (screenAuto.Checked == true)
            {
                //坚
                XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "auto");
            }
            //    else
            //    {
            //        XmlHelper.XmlAttributeEdit(channelXml, node, "screen", "auto");
            //    }
        }
    }
}
