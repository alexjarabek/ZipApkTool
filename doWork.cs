using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace sandGlass
{
    public partial class doWork : Form
    {
        public StreamWriter sIn = null;
        public StreamReader sOut = null;
        public int fileNum;
        public int getNum;
        public string showPer, thGame, thChannel, picString;
        public ProgressBar thPro;
        public Label thLb;
        public Dictionary<string, string> channels = new Dictionary<string, string>();
        public BackgroundWorker worker = new BackgroundWorker();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        delegate void RunWorker(int i, Panel plChannel, int rowHeight, KeyValuePair<string, string> val);
        private bool isDebug()
        {
            bool debug = false;
            PpHelper pt = new PpHelper(envConfig.currenPath + @"\env.properties");
            if (pt.GetPropertiesText("debug") == "1")
                debug = true;
            else
                debug = false;
            pt.Close();
            return debug;
        }
        public doWork()
        {

            InitializeComponent();
            //compile sC = new compile();
            //sC.Close();

        }
        public string game, gamePath, gameName, pwdFilePath, fromPath;
        private void getGame_Click(object sender, EventArgs e)
        {

            getGameFile.InitialDirectory = envConfig.currenPath + @"\gameApk\" + game;

            if (getGameFile.ShowDialog() == DialogResult.OK)
            {
                gamePath = getGameFile.FileName.ToString();
                lbGameApk.Text = gamePath;
            }
            getGameFile.Dispose();
            GC.Collect();
        }
        private void setChannel()
        {
            int i = 1;
            foreach (KeyValuePair<string, string> val in channels)
            {
                Label lb = new Label();
                lb.Top = 150 + i * 30;
                lb.Size = new System.Drawing.Size(200, 20);
                this.Controls.Add(lb);
                lb.Text = "渠道名：" + val.Value + " 渠道缩写：" + val.Key;
                i++;
                ProgressBar pro = new ProgressBar();
                pro.Maximum = getNum;
                pro.Left = 200 + i * 30;
                this.Controls.Add(pro);
            }

        }

        private void deleGateDecomple(string apkPath, string decompilePath)
        {
            Func<string, string, string> func = this.decompileGame;
            IAsyncResult ar = func.BeginInvoke(apkPath, decompilePath, null, null);
            string result = func.EndInvoke(ar);
            if (this.isDebug() == false)
            {
                richTxtInfo.AppendText(result);
            }
        }
        //unzip apk package
        private string decompileGame(string apkPath, string decompilePath)
        {
            //  if (Directory.Exists(decompilePath))
            //
            bool isDecopile = Directory.Exists(decompilePath);
            if (isDecopile)
            {
                DialogResult dr = MessageBox.Show("已存在反编译目录,是否需要更新", "确认是否更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr != DialogResult.OK)
                {
                    fromPath = envConfig.deCompilePath + @game + @"\";
                    getNum = FileUtil.getFilesNum(fromPath);
                    showPer = (getNum / 25).ToString();
                    curEvent.Text = "不更新游戏反编译目录.";
                    return "ok";
                }
                else
                {
                    FileUtil.deleteFolder(decompilePath);
                }
            }
            richTxtInfo.AppendText("开始解压游戏包\r\n");
            string command = envConfig.currenPath + @"\apktool\apktool.bat";
            Process p = new Process();

            if (this.isDebug() == false)
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
            }
            else
            {
                command = envConfig.currenPath + @"\apktool\apktool-debug.bat";
            }



            p.StartInfo.FileName = command;
            p.StartInfo.Arguments = " d \"" + apkPath + "\"  -o " + decompilePath;
            p.Start();
            string line = null;
            if (this.isDebug() == false)
            {
                StreamReader reader = p.StandardOutput;//截取输出流
                line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine() + "\r\n";
                    line += "\r\n" + line;
                    richTxtInfo.AppendText(line);
                    // richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
                    // richTxtInfo.ScrollToCaret();
                }
                reader.Close();
            }
            p.WaitForExit();
            curEvent.Text = "母包反编译完成.";
            fromPath = envConfig.deCompilePath + @game + @"\";
            getNum = FileUtil.getFilesNum(fromPath);
            showPer = (getNum / 25).ToString();
            richTxtInfo.AppendText("\r\n" + curEvent.Text + "\r\n");
            return line;
            //Process.Start(command, " d "+apkPath+" "+decompilePath);
        }
        private void copyIco(string cpIco, string compilePath, string fileName, string channel)
        {
            string channelXml = envConfig.channelPath + channel + @"\channel.xml";
            string useWater = XmlHelper.getChannelAtrr(channelXml, "useWater");
            if (useWater == "yes")
            {
                int waterSize = 512 / 4;
                float alpha = float.Parse(XmlHelper.getChannelAtrr(channelXml, "waterAlpha"));
                ImageCut.ImagePosition pos;
                string posStr = XmlHelper.getChannelAtrr(channelXml, "waterPos");
                pos = ImageCut.ImagePosition.AllIn;
                switch (posStr)
                {
                    case "LeftTop":
                        pos = ImageCut.ImagePosition.LeftTop;
                        break;
                    case "LeftBottom":
                        pos = ImageCut.ImagePosition.LeftBottom;
                        break;
                    case "Center":
                        pos = ImageCut.ImagePosition.Center;
                        break;
                    case "RightTop":
                        pos = ImageCut.ImagePosition.RightTop;
                        break;
                    case "RigthBottom":
                        pos = ImageCut.ImagePosition.RigthBottom;
                        break;
                    case "TopMiddle":
                        pos = ImageCut.ImagePosition.TopMiddle;
                        break;
                    case "BottomMiddle":
                        pos = ImageCut.ImagePosition.BottomMiddle;
                        break;
                    case "AllIn":
                        pos = ImageCut.ImagePosition.AllIn;
                        waterSize = 512;
                        break;
                    default:
                        pos = ImageCut.ImagePosition.Center;
                        break;
                }

                string waterPath = envConfig.channelPath + channel + @"\" + channel + "_water.png";
                FileUtil.setImg(envConfig.channelPath + channel + @"\" + channel + ".png", waterPath, waterSize, waterSize);
                string cpIcoOk = envConfig.channelPath + channel + @"\" + channel + "_water_ok.png";
                ImageCut.DrawImage(cpIco, waterPath, alpha, pos, cpIcoOk);
                cpIco = cpIcoOk;
            }

            string drawablePath = compilePath + @"drawable\";
            string drawableHdPath = compilePath + @"drawable-hdpi\";
            string drawableMdPath = compilePath + @"drawable-mdpi\";
            string drawableXhPath = compilePath + @"drawable-xhdpi\";
            string drawableXxPath = compilePath + @"drawable-xxhdpi\";

            ImageCut.GenThumbnail(cpIco, drawablePath + fileName, 32, 32);
            ImageCut.GenThumbnail(cpIco, drawableHdPath + fileName, 72, 72);
            ImageCut.GenThumbnail(cpIco, drawableMdPath + fileName, 48, 48);
            ImageCut.GenThumbnail(cpIco, drawableXhPath + fileName, 96, 96);
            ImageCut.GenThumbnail(cpIco, drawableXxPath + fileName, 144, 144);
           
            /*
            ImageCut.DrawImage(cpIco, cpIco, 1, ImageCut.ImagePosition.AllIn, drawablePath + fileName,32,32);
            ImageCut.DrawImage(cpIco, cpIco, 1, ImageCut.ImagePosition.AllIn, drawableHdPath + fileName,72,72);
            ImageCut.DrawImage(cpIco, cpIco, 1, ImageCut.ImagePosition.AllIn, drawableMdPath + fileName,48,48);
            ImageCut.DrawImage(cpIco, cpIco, 1, ImageCut.ImagePosition.AllIn, drawableXhPath + fileName,96,96);
            ImageCut.DrawImage(cpIco, cpIco, 1, ImageCut.ImagePosition.AllIn, drawableXxPath + fileName,144,144);
            */



        }
        //打包主流程
        private void copyDecompileFile(string game, string channel, ProgressBar pro, Label lb)
        {
            // 拷贝游戏
            copyFolder(fromPath, envConfig.targetPath + @game + @"\" + channel + @"\", pro, lb, game);
          
            // 保存参数文件--assets
            string propertiesFile = envConfig.gameConfig + game + @"\" + channel + envConfig.configExt;
            if (!Directory.Exists(envConfig.targetPath + @game + @"\" + channel + @"\assets\"))
                Directory.CreateDirectory(envConfig.targetPath + @game + @"\" + channel + @"\assets\");
            if (getNum == fileNum)
            {
                File.Copy(propertiesFile, envConfig.targetPath + @game + @"\" + channel + @"\assets\" + game + envConfig.configExt, true);
                curEvent.Text = "copy  " + channel + "  完成.";
                pbT.Value = 20;
            }
            //合并文件
            string apkBase = envConfig.channelPath + channel + @"\apkBase\";
            if (!Directory.Exists(apkBase))
            {
                MessageBox.Show("不存在sdk母包文件");
                return;
            }
            File.Delete(apkBase + @"AndroidManifest.xml");
            File.Delete(apkBase + @"apktool.yml");
            mergeFolder(envConfig.targetPath + @game + @"\" + channel + @"\", apkBase, pro, lb, channel);
            // propertiesFile = envConfig.gameConfig + game + @"\" + channel + envConfig.configExt;

            File.Copy(propertiesFile, envConfig.targetPath + @game + @"\" + channel + @"\assets\" + game + envConfig.configExt, true);
            //检测游戏母包是否还有其他文件操作
            delArmeabi(game, envConfig.targetPath + @game + @"\" + channel + @"\lib");
            fileNum = 0;
            //todo merge xml 
            string gameXml = envConfig.targetPath + @game + @"\" + channel + @"\AndroidManifest.xml";
            string xxxPath = envConfig.targetPath + @game + @"\" + channel + @"\res\drawable-xxxhdpi\";
            if (Directory.Exists(xxxPath))
                FileUtil.deleteFolder(xxxPath);
            curEvent.Text = "合并" + gameXml;
            this.mergeXml(gameXml, channel);
            curEvent.Text = "xml合并完成";

            pro.Value = 60;
            pbT.Value = 60;
            lb.Text = "60%";

            //todo replace packageName
            string oldName = getPackName(gameXml);
            PpHelper pt = new PpHelper(propertiesFile);
            string packageName = pt.GetPropertiesText("package");
            this.replaceSpecGameXml(game, gameXml, packageName, oldName);
            //todo 特殊游戏包名
            if (packageName.Length > 0)
            {
                XmlHelper.XmlAttributeEdit(gameXml, "manifest", "package", packageName);
            }
            string tmpXml1 = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile1 = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile1, tmpXml1);
            XmlDocument dDoc = new XmlDocument();
            dDoc.Load(xmlTmpFile1);
            XmlNode dpath = dDoc.SelectSingleNode(@"//manifest/application");
            XmlElement dXe = (XmlElement)dpath;
            //    dXe.GetAttribute("android" + FileUtil.colon + "debuggable");
            dXe.SetAttribute("android" + FileUtil.colon + "debuggable", "false");
            dDoc.Save(xmlTmpFile1);
            tmpXml1 = FileUtil.replaceXmlColon(FileUtil.getContent(xmlTmpFile1));
            tmpXml1 = FileUtil.xmlRestore(tmpXml1);
            FileUtil.writeContent(gameXml, tmpXml1);
            File.Delete(xmlTmpFile1);





            //  string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            pbT.Value = 61;
            pro.Value = 61;
            lb.Text = "61%";
            curEvent.Text = "包名替换完成";

            //todo copy ico
            if (picString.Length > 0)
            {
                string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
                string xmlTmpFile = gameXml + @".tmp";
                FileUtil.writeContent(xmlTmpFile, tmpXml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlTmpFile);
                XmlNode basePath = xmlDoc.SelectSingleNode(@"//manifest/application");
                XmlElement baseXe = (XmlElement)basePath;
                string icoName = baseXe.GetAttribute("android" + FileUtil.colon + "icon");
                icoName = icoName.Substring(icoName.IndexOf('/') + 1);
                copyIco(picString, envConfig.targetPath + @game + @"\" + channel + @"\res\", icoName + ".png", channel);
                tmpXml = FileUtil.replaceXmlColon(FileUtil.getContent(xmlTmpFile));
                tmpXml = FileUtil.xmlRestore(tmpXml);
                FileUtil.writeContent(gameXml, tmpXml);
                File.Delete(xmlTmpFile);
            }

            //todo 检测渠道XML是否有特殊操作          
            try
            {
                updateChannelXml(game, channel, gameXml, propertiesFile);
            }
            catch (Exception EE)
            {
                    MessageBox.Show(EE.ToString());
            }
            curEvent.Text = "特殊渠道xml处理完成";

            //todo merge res *.xml

            this.mergeRes(channel, game);
            pbT.Value = 65;
            pro.Value = 65;
            lb.Text = "65%";
            curEvent.Text = "res xml 合并完成";
            //todo make R.java
            //todo compile class file
            //todo make dex 
            //todo make smali
            //todo merge smali          
            string targetPath = envConfig.targetPath + game + @"\" + channel + @"\";
            string resPath = targetPath + "res";
            string tmpFolder = envConfig.upPath + @"temp\";
            string tmpCls = tmpFolder + @"class\";
            string tmpR = tmpFolder + @"R\";
            if (Directory.Exists(tmpR))
                FileUtil.deleteFolder(tmpR);
            Directory.CreateDirectory(tmpR);
            if (Directory.Exists(tmpCls))
                FileUtil.deleteFolder(tmpCls);
            Directory.CreateDirectory(tmpCls);
            string packagePath = packageName.Replace(".", @"\");
            string javaFilePath = tmpR + packagePath;
   //            MessageBox.Show("java file:"+javaFilePath);
            curEvent.Text = "开始生成R文件";
            Process makeR = new Process();
            if (this.isDebug() == false)
            {
                makeR.StartInfo.UseShellExecute = false;
                makeR.StartInfo.CreateNoWindow = true;
                makeR.StartInfo.FileName = envConfig.toolPath + @"\makeR.bat";
                makeR.StartInfo.RedirectStandardInput = true;
                makeR.StartInfo.RedirectStandardOutput = true;
                makeR.StartInfo.RedirectStandardError = true;
            }
            else
            {
                makeR.StartInfo.FileName = envConfig.toolPath + @"\makeR-debug.bat";
            }
            makeR.StartInfo.Arguments = " " + resPath + " " + targetPath + "AndroidManifest.xml" + " " + tmpR + "  " + tmpCls + " " + javaFilePath + " " + envConfig.targetPath + @game + @"\" + channel + @"\assets\";

            makeR.Start();
            string line = "\r\n";
            StreamReader reader = null;
            if (this.isDebug() == false)
            {
                reader = makeR.StandardOutput;//截取输出流
                line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    line += "\r\n" + line;
                    richTxtInfo.AppendText(line);
                    richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
                    richTxtInfo.ScrollToCaret();
                }
            }
            makeR.WaitForExit();
            pbT.Value = 70;
            pro.Value = 70;
            lb.Text = "70%";
            curEvent.Text = "R文件生成完成";
            richTxtInfo.AppendText("\r\n R文件生成完成\r\n");




            if (makeRYes.Checked == true)
            {
                string channelXml = envConfig.channelPath + channel + @"\channel.xml";
                string chanelPackage = XmlHelper.getChannelAtrr(channelXml, "chanelPackage");
                MessageBox.Show(chanelPackage);
                makeChannelRes(channel, pro, lb);
            }

            curEvent.Text = "生成class文件";
            Process makeDex = new Process();
            if (this.isDebug() == false)
            {
                makeDex.StartInfo.UseShellExecute = false;
                makeDex.StartInfo.CreateNoWindow = true;
                makeDex.StartInfo.FileName = envConfig.toolPath + @"\dx.bat";
                makeDex.StartInfo.RedirectStandardInput = true;
                makeDex.StartInfo.RedirectStandardOutput = true;
                makeDex.StartInfo.RedirectStandardError = true;
            }
            else
            {
                makeDex.StartInfo.FileName = envConfig.toolPath + @"\dx.bat";
            }
            makeDex.StartInfo.Arguments = "   --dex --output=" + tmpFolder + "temp.dex  " + tmpCls;

            makeDex.Start();           
            line = "\r\n";
            if (this.isDebug() == false)
            {
                reader = makeDex.StandardOutput;//截取输出流
                line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    line += "\r\n" + line;
                    richTxtInfo.AppendText(line);
                    richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
                    richTxtInfo.ScrollToCaret();
                }
            }
            makeDex.WaitForExit();
            richTxtInfo.AppendText("\r\n  class文件生成完成\r\n");


            curEvent.Text = "生成smali";
            Process makeSmali = new Process();
            if (this.isDebug() == false)
            {
                makeSmali.StartInfo.UseShellExecute = false;
                makeSmali.StartInfo.CreateNoWindow = true;
                makeSmali.StartInfo.FileName = envConfig.toolPath + @"\getSmali.bat";
                makeSmali.StartInfo.RedirectStandardInput = true;
                makeSmali.StartInfo.RedirectStandardOutput = true;
                makeSmali.StartInfo.RedirectStandardError = true;
            }
            else
            {
                makeSmali.StartInfo.FileName = envConfig.toolPath + @"\getSmali-debug.bat";
                MessageBox.Show("begin");
            }
            makeSmali.StartInfo.Arguments = targetPath + @"smali\   " + tmpFolder + "temp.dex  ";

            makeSmali.Start();            
            line = "\r\n";
            if (this.isDebug() == false)
            {
                reader = makeSmali.StandardOutput;//截取输出流
                line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    line += "\r\n" + line;
                    richTxtInfo.AppendText(line);
                    richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
                    richTxtInfo.ScrollToCaret();
                }
            }
            makeSmali.WaitForExit();
            pbT.Value = 75;
            pro.Value = 75;
            lb.Text = "75%";
            curEvent.Text = "smali  合并完成";
            richTxtInfo.AppendText("\r\n" + "smali  合并完成\r\n");
            FileUtil.deleteFolder(tmpR);
            FileUtil.deleteFolder(tmpCls);
            File.Delete(tmpFolder + "temp.dex");

            string packageFolder = envConfig.targetPath + @game + @"\" + channel + @"\";
            string compileFolder = envConfig.compileFolder + game + @"\";
            string mkTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string compileApkName = channel + "_" + mkTime + ".apk";

            Process p = new Process();
            if (this.isDebug() == false)
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = envConfig.apkToolCommand;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
            }
            else
            {
                p.StartInfo.FileName = envConfig.currenPath + @"\apktool\apktool-debug.bat";
            }
            string compileApk = compileFolder + compileApkName;
            p.StartInfo.Arguments = "  b  " + packageFolder + "  -o  " + compileApk;

            p.Start();            
            line = "\r\n";
            if (this.isDebug() == false)
            {
                reader = p.StandardOutput;//截取输出流
                line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {

                    line = reader.ReadLine();
                    line += "\r\n" + line;
                    richTxtInfo.AppendText(line);
                    richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
                    richTxtInfo.ScrollToCaret();
                }
            }
            p.WaitForExit();
            pbT.Value = 80;
            pro.Value = 80;
            lb.Text = "80%";
            curEvent.Text = channel + " 打包完成";
            richTxtInfo.AppendText("\r\n" + curEvent.Text + "\r\n");

            Process makeSign = new Process();
 
            // 签名-调用命令行输出
            //makeSign.StartInfo.UseShellExecute = false;
            //makeSign.StartInfo.CreateNoWindow = true;
            //makeSign.StartInfo.RedirectStandardInput = true;
            //makeSign.StartInfo.RedirectStandardOutput = true;
            //makeSign.StartInfo.RedirectStandardError = true;

 
            string signPwd = txtSignPwd.Text.ToString();
            string keyStrorePwd = txtKeyPwd.Text.ToString();
            string keyStore = lbPwdFile.Text.ToString();
            string alias = txtAlias.Text.ToString();
            string releasePath = envConfig.releasePkg + game + @"\";
            if (!Directory.Exists(releasePath))
                Directory.CreateDirectory(releasePath);
            string releaseApk = releasePath + game + "_" + compileApkName;
            //  makeSign.StartInfo.FileName = envConfig.signPath + "jarsigner";
            makeSign.StartInfo.FileName = "jarsigner";
            //  compileApk = @"C:\Users\win8\Desktop\123.apk";
            makeSign.StartInfo.Arguments = "  -digestalg SHA1 -sigalg MD5withRSA -verbose -keystore " + keyStore + " -storepass " + keyStrorePwd + " -keypass " + signPwd + " -signedjar  " + releaseApk + "  " + compileApk + "  " + alias;
 

            Clipboard.SetText(makeSign.StartInfo.FileName + " " + makeSign.StartInfo.Arguments);
       //     MessageBox.Show("beginSign");

            makeSign.Start();
           
            //reader = makeSign.StandardOutput;//截取输出流
            //line = "\r\n";
            //line = reader.ReadLine();//每次读取一行
            //while (!reader.EndOfStream)
            //{
            //    line = reader.ReadLine();
            //    line += "\r\n" + line;
            //    richTxtInfo.AppendText(line);
            //    richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
            //    richTxtInfo.ScrollToCaret();
            //}
          
            makeSign.WaitForExit();

            pbT.Value = 100;
            pro.Value = 100;
            lb.Text = "100%";
            curEvent.Text = channel + "签名完成";
            richTxtInfo.AppendText("\r\n" + curEvent.Text + "\r\n");


            //todo 加固
        }

        private void makeChannelRes(string channel, ProgressBar pro, Label lb)
        {
            if (makeRYes.Checked == false)
            {
                return;
               
            }
            string channelXml = envConfig.channelPath + channel + @"\channel.xml";
            string channelMainXml = envConfig.channelPath + channel+@"\";
            string packageName = XmlHelper.getChannelAtrr(channelXml, "chanelPackage");
          //  MessageBox.Show("reMake"+packageName);
            string targetPath = envConfig.targetPath + game + @"\" + channel + @"\";
            string resPath = targetPath + "res";
            string tmpFolder = envConfig.upPath + @"temp\";
            string tmpCls = tmpFolder + @"class\";
            string tmpR = tmpFolder + @"R\";
         //   if (Directory.Exists(tmpR))
         //       FileUtil.deleteFolder(tmpR);
         //   Directory.CreateDirectory(tmpR);
         //   if (Directory.Exists(tmpCls))
         //       FileUtil.deleteFolder(tmpCls);
            Directory.CreateDirectory(tmpCls);
            string packagePath = packageName.Replace(".", @"\");
            string javaFilePath = tmpR + packagePath;
   //          MessageBox.Show("new java file:"+javaFilePath);
            curEvent.Text = "开始生成R文件";
            Process makeR = new Process();
            if (this.isDebug() == false)
            {
                makeR.StartInfo.UseShellExecute = false;
                makeR.StartInfo.CreateNoWindow = true;
                makeR.StartInfo.FileName = envConfig.toolPath + @"\makeR.bat";
                makeR.StartInfo.RedirectStandardInput = true;
                makeR.StartInfo.RedirectStandardOutput = true;
                makeR.StartInfo.RedirectStandardError = true;
            }
            else
            {
                makeR.StartInfo.FileName = envConfig.toolPath + @"\makeR-debug.bat";
            }
            makeR.StartInfo.Arguments = " " + resPath + " " + channelMainXml + "AndroidManifest.xml" + " " + tmpR + "  " + tmpCls + " " + javaFilePath + " " + envConfig.targetPath + @game + @"\" + channel + @"\assets\";

            makeR.Start();
            string line = "\r\n";
            StreamReader reader = null;
            if (this.isDebug() == false)
            {
                reader = makeR.StandardOutput;//截取输出流
                line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    line += "\r\n" + line;
                    richTxtInfo.AppendText(line);
                    richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
                    richTxtInfo.ScrollToCaret();
                }
            }
            makeR.WaitForExit();
            pbT.Value = 70;
            pro.Value = 70;
            lb.Text = "70%";
            curEvent.Text = "R文件生成完成";
            richTxtInfo.AppendText("\r\n 渠道R文件生成完成\r\n");           
        }
        private void mergeXml(string gameXmlPath, string channel)
        {
            string channelConfigPath = envConfig.channelPath + channel + @"\channel.xml";
            string screen = XmlHelper.getChannelAtrr(channelConfigPath, "screen");
            if (screen == "resolute")
            {
                channelConfigPath = envConfig.channelPath + channel + @"\config_portrait.xml";
            }
            else if (screen == "horizontal")
            {
                channelConfigPath = envConfig.channelPath + channel + @"\config_landscape.xml";
            }
            else
            {
                channelConfigPath = envConfig.channelPath + channel + @"\config.xml";
            }
            if (!FileUtil.checkFile(channelConfigPath))
            {
                MessageBox.Show("不存在渠道" + channel + "config文件,使用默认config");
                channelConfigPath = envConfig.channelPath + channel + @"\config.xml";
                //  return;
            }
            //string channelConfigPath = envConfig.channelPath + channel + @"\config.xml";
            string gameTmpPath = gameXmlPath + @".tmp";
            string channelTmpPath = channelConfigPath + @".tmp";

            string tmpGameXmlContent = FileUtil.getContent(gameXmlPath);
            string tmpChannelXmlContent = FileUtil.getContent(channelConfigPath);

            string tmpGameXml = FileUtil.setXmlColon(tmpGameXmlContent);
            string tmpChannelXml = FileUtil.setXmlColon(tmpChannelXmlContent);

            FileUtil.writeContent(gameTmpPath, tmpGameXml);
            FileUtil.writeContent(channelTmpPath, tmpChannelXml);
            XmlHelper.mergeXml(channelTmpPath, gameTmpPath, gameXmlPath);
            FileUtil.writeContent(gameXmlPath, FileUtil.replaceXmlColon(FileUtil.getContent(gameTmpPath)));
            FileUtil.deleteFile(gameTmpPath);
            FileUtil.deleteFile(channelTmpPath);
        }
        private void mergeFolder(string compilePath, string baseSdk, ProgressBar pro, Label lb, string channel)
        {
            getNum = FileUtil.getFilesNum(baseSdk);
            showPer = (getNum / 25).ToString();
            this.mergeBegin(baseSdk, compilePath, pro, lb, channel);
            if (getNum == fileNum)
            {
                curEvent.Text = "merge  " + channel + "  完成.";
            }
        }
        public void mergeBegin(string from, string to, ProgressBar pro, Label lb, string channel = null)
        {
            String[] files = Directory.GetFiles(from);

            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);
            foreach (string sub in Directory.GetDirectories(from))
                mergeBegin(sub + "\\", to + Path.GetFileName(sub) + "\\", pro, lb, game);
            foreach (string file in Directory.GetFiles(from))
            {
                File.Copy(file, to + Path.GetFileName(file), true);
                curEvent.Text = "copy :" + file;
                fileNum += 1;
                if ((fileNum % (int)Convert.ToDecimal(showPer)) == 0)
                {
                    pro.Value += 1;
                }
                // Double p = Convert.ToDouble(fileNum) / Convert.ToDouble(getNum);
                //   per.Text = string.Format("{0:0.00%}", p);
                //   pro.Increment(fileNum);
                if (channel != null)
                    curEvent.Text = "curren progress " + channel + " merge ...";
            }
            curEvent.Text = "curren progress " + channel + " merge ok";
            pro.Value = 50;
            lb.Text = "50%";
        }
        public void copyFolder(string from, string to, ProgressBar pro, Label lb, string game = null)
        {
            String[] files = Directory.GetFiles(from);
            FileUtil.deleteFolder(to);
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);
            foreach (string sub in Directory.GetDirectories(from))
                copyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\", pro, lb, game);
            foreach (string file in Directory.GetFiles(from))
            {
                File.Copy(file, to + Path.GetFileName(file), true);
                curEvent.Text = "copy :" + file;
                fileNum += 1;
                if ((fileNum % (int)Convert.ToDecimal(showPer)) == 0)
                {
                    if (pro.Value < 100)
                        pro.Value += 1;
                }
                if (game != null)
                    curEvent.Text = "curren progress " + game + " copy ...";
            }
            pbT.Value = 25;
            pro.Value = 25;
            lb.Text = "25%";
        }
        private bool getChannel(bool showPro = false)
        {

            int i = 1;
            int rowHeight = 25;
            foreach (KeyValuePair<string, string> val in channels)
            {
                Label lbChannel = new Label();   //channel name
                lbChannel.Text = val.Value;
                //  MessageBox.Show("key:"+val.Key+",val:"+val.Value);
                lbChannel.Name = "clbChannel" + i.ToString();
                lbChannel.Location = new Point(0, i * rowHeight);
                lbChannel.Size = new System.Drawing.Size(80, 20);

                if (showPro == true)
                {
                    pbT.Maximum = 100;
                    pbT.Value = 0;
                    pbT.Step = 1;
                    //          BackgroundWorker bkWorker = new BackgroundWorker();
                    //          bkWorker.WorkerReportsProgress = true;
                    //          bkWorker.WorkerSupportsCancellation = true;
                    this.syncPro(i, plChannel, rowHeight, val);
                }
                plChannel.Controls.Add(lbChannel);
                i++;
            }
            return true;
        }

        string syncPro(int i, Panel plChannel, int rowHeight, KeyValuePair<string, string> val)
        {
            ProgressBar pbChannel = new ProgressBar(); //channel progress 
            pbChannel.Name = "pbChannel" + i.ToString();
            pbChannel.Location = new Point(30, i * rowHeight);
            pbChannel.Size = new System.Drawing.Size(250, 15);
            pbChannel.Maximum = 100;
            pbChannel.Value = 0;
            pbChannel.Step = 1;
            Label lbP = new Label();
            lbP.Text = "0%";
            lbP.Name = "lbP" + i.ToString();
            lbP.Location = new Point(300, i * rowHeight);
            lbP.Size = new System.Drawing.Size(250, 15);
            pbChannel.BackColor = Color.Green;
            plChannel.Controls.Add(pbChannel);
            plChannel.Controls.Add(lbP);
            richTxtInfo.AppendText("合并代码" + val.Key);
            this.copyDecompileFile(game, val.Key, pbChannel, lbP);
            return "ok";
        }

        private void doWork_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            /*
             foreach (KeyValuePair<string, string> channel in channels)
             {
                 string txt = channel.Key;
                 string val = channel.Value;
             }
            */
            lbGame.Text = game;
            lbName.Text = gameName;
            string lastGame = envConfig.currenPath + @"\lastGame" + envConfig.configExt;
            string lineStr = "";
            lineStr = "game=" + game + "\r\n";
            FileUtil.writeContent(lastGame, lineStr);
            string signCfg = envConfig.gameConfig + game + @"\sign" + envConfig.configExt;
            PpHelper pptHelper = new PpHelper(signCfg);
            txtSignPwd.Text = pptHelper.GetPropertiesText("signPwd");
            txtAlias.Text = pptHelper.GetPropertiesText("alias");
            txtKeyPwd.Text = pptHelper.GetPropertiesText("keyPwd");
            if (!FileUtil.checkFile(envConfig.gameConfig + game + @"\" + game + ".keystore"))
                MessageBox.Show("请重新选择签名文件");
            else
                lbPwdFile.Text = envConfig.gameConfig + game + @"\" + game + ".keystore";
            getChannel();
        }

        private void getGameFile_FileOk(object sender, CancelEventArgs e)
        {
            //   MessageBox.Show("get game ok");
        }

        private void btnGetPwdFile_Click(object sender, EventArgs e)
        {
            if (getPwdFile.ShowDialog() == DialogResult.OK)
            {
                pwdFilePath = getPwdFile.FileName.ToString();
                lbPwdFile.Text = pwdFilePath;
            }
            getPwdFile.Dispose();
        }

        private void btnPackage_Click(object sender, EventArgs e)
        {
            curEvent.Text = game + " 开始解压";
            getChannel();
            string decompilePath = envConfig.deCompilePath + @game + @"\";
            deleGateDecomple(gamePath, decompilePath);
            // this.decompileGame(gamePath, decompilePath);
            // RunTaskDelegate runTask = new RunTaskDelegate(getChannel);
            // runTask.BeginInvoke(true, null, null);
            getChannel(true);
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/open, " + envConfig.releasePkg + game + @"\";
            System.Diagnostics.Process.Start(psi);
        }
        private void deleGateSync()
        {
            Func<bool, bool> func = this.getChannel;
            IAsyncResult ar = func.BeginInvoke(true, null, null);
            bool result = func.EndInvoke(ar);
            richTxtInfo.AppendText(result.ToString());
        }

        private void mergeRes(string channel, string game)
        {
            string path = envConfig.channelPath + channel + @"\apkBase\res\";
            string[] folders = Directory.GetDirectories(path, "values*");//get sdk base folders
            string dePath = envConfig.deCompilePath + game + @"\res\";
            foreach (string s in folders)
            {
                DirectoryInfo f = new DirectoryInfo(s);
                string folderName = f.Name;
                string gPath = dePath + folderName + @"\"; //get game base package folders
                if (Directory.Exists(gPath))
                {
                    string channelTarget = envConfig.targetPath + game + @"\" + channel + @"\res\";
                    FileInfo[] allFile = f.GetFiles();
                    foreach (FileInfo fi in allFile)
                    {
                        string channelXmlFile = fi.FullName;
                        string channelXmlName = fi.Name;
                        string gameBaseXmlFile = gPath + channelXmlName;
                        string channelTargetXml = channelTarget + folderName + @"\" + channelXmlName;
                        //  MessageBox.Show("channelXmlName:" + channelXmlName + ", channelXmlFile:" + channelXmlFile + ",gameXmlFile:" + gameBaseXmlFile + ",targetXml:" + channelTargetXml);
                        if (FileUtil.checkFile(gameBaseXmlFile))
                        {
                            //根据不同场景，选择合并xml的方式，1.根据GAME为base合并channel的xml进public.xml，2.根据channel为基础合并game的XML文件。
                            //   MessageBox.Show(game);
                            if (channelXmlName == "public.xml")
                            {
                                SpecXml spXml = new SpecXml();
                                if (spXml.needMergePublic.Contains<string>(channel))
                                {
                                    // MessageBox.Show("new");
                                    // XmlHelper.mergePublicXmlNew(channelXmlFile, gameBaseXmlFile, channelTargetXml);
                                    XmlHelper.mergePublicXml2(gameBaseXmlFile, channelXmlFile, channelTargetXml);  // gameBase->channel : public.xml游戏合并进渠道 xml
                                }
                                else
                                {
                                    XmlHelper.mergePublicXml2(channelXmlFile, gameBaseXmlFile, channelTargetXml);  // channel->gameBase : public.xml 渠道合并进游戏xml
                                    //  XmlHelper.mergePublicXml(gameBaseXmlFile, channelXmlFile, channelTargetXml);  // gameBase->channel : public.xml游戏合并进渠道 xml
                                }
                            }
                            else
                            {
                                XmlHelper.mergeResXml(channelXmlFile, gameBaseXmlFile, channelTargetXml);
                            }
                        }
                    }
                }
            }
        }
        private void delArmeabi(string game, string compileGame)
        {
            // return;
            if (game == "hjtkfb")
            {
                FileUtil.deleteFolder(compileGame + @"\armeabi-v7a");
                FileUtil.deleteFolder(compileGame + @"\x86");
            }
        }
        private void updateChannelXml(string game, string channel, string gameXml, string perpties)
        {
            SpecXml specXml = new SpecXml();
            specXml.game = game;
            specXml.channel = channel;
            specXml.gameXml = gameXml;
            specXml.properties = perpties;
            specXml.gameName = lbName.Text;
            Type t = specXml.GetType();


            List<string> funcs = new List<string>();
            foreach (MethodInfo mi in t.GetMethods())
            {
                funcs.Add(mi.Name.ToString());
            }
            string channelMethod = channel.ToUpper() + "xml";
            bool exists = funcs.Contains(channelMethod);
            MethodInfo hello = t.GetMethod(channelMethod);
            hello.Invoke(specXml, null);
            return;
        }
        private string getPackName(string gameXml)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(@gameXml);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"manifest");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute("package");
        }
        public void replaceSpecGameXml(string game, string gameXml, string newName, string oldName)
        {
            if (game == "hjtkfb")
            {
                string tmp = FileUtil.getContent(gameXml);
                string tmpXml = FileUtil.replaceContent(tmp, oldName, newName);
                FileUtil.writeContent(gameXml, tmpXml);
            }
        }
        void p_OutputDataReceived(Object sender, DataReceivedEventArgs e)
        {
            richTxtInfo.AppendText(e.Data);
        }

        void p_ErrorDataReceived(Object sender, DataReceivedEventArgs e)
        {
            richTxtInfo.AppendText(e.Data);
        }

        void p_Exited(Object sender, EventArgs e)
        {
            richTxtInfo.AppendText("完成");
        }

        private void richTxtInfo_TextChanged(object sender, EventArgs e)
        {
            richTxtInfo.SelectionStart = richTxtInfo.Text.Length;
            richTxtInfo.ScrollToCaret();
        }

        private void doWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;  //点击OK  
            }
            else
            {
                e.Cancel = true;
            }    
        }

    }
}
