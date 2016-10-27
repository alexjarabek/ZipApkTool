using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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
    public delegate void ThreadCallBackDelegate(string thread, string msg,Button sender);
    class TaskThread
    {
        public Button start;
        public Button stop;
        public Button delete;
        public Label title;
        public Label time;
        public Label percent;
        public Label log;
        public ProgressBar probar;
        public ThreadCallBackDelegate callback;
        public gameItem gameItem;
        public channelItem channelItem;

        private string fromPath, toPath;// 
        private string game, channel;
        private int fileNum;
        private int getNum;
        private string showPer;
        private string gameXml, packageName;
        private string propertiesFile;
        private string unSignApk, unSignApkName, pkgTime;
        private string tmpR = "";
        private string tmpCls = "";

        public static string MSG_ABORT = "ABORT";
        public static string MSG_DELETE = "DELETE";
        public static string MSG_COMPLETE = "COMPLETE";
        public Process currentProcess;

        public bool mIsReInforce = true;
        public string applicationName = "";
        public string mainActivityName = "";

        private bool isDebug()
        {
            bool debug = false;
            if (envConfig.debug == "true")
            {
                debug = true;
            }
            return debug;
        }
        public void Abort()
        {
            if (currentProcess != null)
            {
                try
                {
                    currentProcess.Kill();
                    currentProcess.Close();
                }
                catch (Exception)
                {
                    currentProcess = null; 
                }
               
            }
            currentProcess = null;
            callback(game + "_" + channel, MSG_ABORT, this.stop);
            GC.Collect();
 
        }
        public void Delete()
        {
            if (currentProcess != null)
            {
                try
                {
                    currentProcess.Kill();
                    currentProcess.Close();
                }
                catch (Exception)
                {
                    currentProcess = null;
                }
            }
            currentProcess = null;
            callback(game + "_" + channel, MSG_DELETE,this.delete);
           
        }
        public void Complete()
        {
            callback(game + "_" + channel, MSG_COMPLETE, start);
           
        }

        public void TaskStart()
        {
            DateTime start_time = DateTime.Now;
            start.Enabled = false;
            ClearEvent(delete, "Click");
            stop.Click += new System.EventHandler(this.stop_Click);
            delete.Click += new System.EventHandler(this.delete_Click);
            game = gameItem.gid;
            channel = channelItem.cid;
            if(channel==null||channel==""){
                MessageBox.Show("渠道标识错误！请更新渠道信息！");
                log.Text = "渠道标识错误！任务终止！";
                this.Abort();
                return;
            }

            tmpR = envConfig.tmpFolder + game + @"\" + channel + @"\R\";
            tmpCls = envConfig.tmpFolder + game + @"\" + channel + @"\class\";
            fromPath = envConfig.deCompilePath + game + @"\";
            toPath = envConfig.targetPath + game + @"\" + channel + @"\";
            log.Text = "开始复制游戏资源..";

            /// 拷贝  游戏 到compile 对应目录  25%
            getNum = FileUtil.getFilesNum(fromPath);
            showPer = (getNum / 25).ToString();
            copyFolder(fromPath, toPath, game);
            log.Text = "开始合并渠道资源..";
            GC.Collect(); 
            mergeApkBase();
            log.Text = "开始分析游戏支持cpu类型..";
            deleteFolder();
            GC.Collect();
            fileNum = 0;
            gameXml = envConfig.targetPath + @game + @"\" + channel + @"\AndroidManifest.xml";
            log.Text = "开始合并资源id..";
            // 合并 AndroidManifest.xml
            this.mergeXml(gameXml, channel);
            probar.Value = 60;
            percent.Text = "60%";
            //整合 AndroidManifest  icon

            GC.Collect();
            log.Text = "开始合并 AndroidManifest  游戏icon..";
            mergeManifest();
            probar.Value = 61;
            percent.Text = "61%";
            //合并public       
            log.Text = "开始合并public.xml文件..";
            mergeRes(channel, game);
            probar.Value = 65;
            percent.Text = "65%";
            // 生成R  class 文件
            if (new compileManager().getValue("isStart") == "true")
            {               
                DialogResult dr = MessageBox.Show(game + "-" + channel + "合并完成,是否需要打开合并目录！", "Begin提示！", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //return;
                    if (Directory.Exists(toPath))
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                        psi.Arguments = "/e,/open, " + toPath;
                        System.Diagnostics.Process.Start(psi);
                    }
                    MessageBox.Show("继续");
                }                
            }
            log.Text = "开始生成R、class 文件";
            makeRes();
            GC.Collect();
            probar.Value = 70;
            percent.Text = "70%";
            // 生成Dex 文件

            log.Text = "开始生成Dex文件";
            makeDex();
            // smali
            log.Text = "开始生成smali文件";
            makeSmali();
            probar.Value = 75;
            percent.Text = "75%";
            // sdk版本控制
            checkSmali();

            // 清除打包缓存
            log.Text = "清除打包缓存";
            FileUtil.deleteFolder(tmpR);
            FileUtil.deleteFolder(tmpCls);
            File.Delete(envConfig.tmpFolder + game + @"\" + channel + @"\temp.dex");
            GC.Collect();
            log.Text = "开始打包生成APK..";

            pkgTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (mIsReInforce) {
                log.Text = "加固处理...";
                inforce();
            }

            makeUnsignApk();
            if (!FileUtil.checkFile(unSignApk))
            {
                this.Abort();
                return;
            }
            probar.Value = 80;
            percent.Text = "80%";
            log.Text = "生成APK成功，正在签名..";
            if (mIsReInforce)
            {
                makeInforce();
            }

            makeSignApk();

            string releasePath = envConfig.releasePkg + game + @"\";
            string releaseApk = releasePath + game + "_" + unSignApkName;
            if (!FileUtil.checkFile(releaseApk))
            {
                MessageBox.Show("签名失败!");
                log.Text = "签名失败.异常终止.";
                this.Abort();
               
            }
            GC.Collect();
            log.Text = "打包完成";
            probar.Value = 100;
            percent.Text = "100%";
            DateTime end = DateTime.Now;
            TimeSpan ts = end - start_time;
            int Seconds = (int)ts.TotalSeconds;
            time.Text = "耗：" + Seconds + "秒";
            start.Text = "打开文件目录";
            start.Enabled = true;
            // 清除 start 打包事件
            ClearEvent(start, "Click");
            start.Click += new System.EventHandler(this.openFile_Click);
            this.Complete();
            GC.Collect();
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_Click(object sender, EventArgs e)
        {
            this.Abort();

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, EventArgs e)
        {
            this.Delete();

        }
        /// <summary>
        /// 打开文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFile_Click(object sender, EventArgs e)
        {
            string releasePath = envConfig.releasePkg + game + @"\";
            string releaseApk = releasePath + game + "_" + unSignApkName;
            if (FileUtil.checkFile(releaseApk))
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = "/e,/open, " + envConfig.releasePkg + game + @"\";
                System.Diagnostics.Process.Start(psi);
            }

        }
        void ClearEvent(Control control, string eventname)
        {
            if (control == null) return;
            if (string.IsNullOrEmpty(eventname)) return;

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            Type controlType = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(control, null);
            FieldInfo fieldInfo = (typeof(Control)).GetField("Event" + eventname, mFieldFlags);
            Delegate d = eventHandlerList[fieldInfo.GetValue(control)];

            if (d == null) return;
            EventInfo eventInfo = controlType.GetEvent(eventname);

            foreach (Delegate dx in d.GetInvocationList())
                eventInfo.RemoveEventHandler(control, dx);

        }
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="pro"></param>
        /// <param name="lb"></param>
        /// <param name="game">文件名字</param>
        public void copyFolder(string from, string to, string name)
        {
            if (!Directory.Exists(from))
            {

                this.Abort();
                MessageBox.Show("不存在" + from + " 文件");
                log.Text = "不存在" + from + " 文件,异常终止";
                return;
            }
            String[] files = Directory.GetFiles(from);
            FileUtil.deleteFolder(to);
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            foreach (string sub in Directory.GetDirectories(from))
                mergeFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");
            foreach (string file in Directory.GetFiles(from))
            {
                File.Copy(file, to + Path.GetFileName(file), true);
                fileNum += 1;

                if ((fileNum % (int)Convert.ToDecimal(showPer)) == 0)
                {
                    probar.Value += 1;
                    percent.Text = probar.Value + "%";
                }

            }

        }
        public void delGameSdk()
        {
            string targetPath = envConfig.targetPath + game + @"\" + channel + @"\";
            string sandglass = targetPath + @"smali\com\sandglass\";
            FileUtil.deleteFolder(sandglass);

        }


        /// <summary>
        /// 合并文件夹 - NO--   FileUtil.deleteFolder(to);
        /// </summary>
        /// <param name="from">apkbase</param>
        /// <param name="to">compile</param>

        public void mergeFolder(string from, string to)
        {
            if (!Directory.Exists(from))
            {
                this.Abort();
                MessageBox.Show("不存在" + from + " 文件");
                log.Text = "不存在" + from + " 文件,异常终止";
                return;
               
            }
            String[] files = Directory.GetFiles(from);

            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            foreach (string sub in Directory.GetDirectories(from))
                mergeFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");
            foreach (string file in Directory.GetFiles(from))
            {
                File.Copy(file, to + Path.GetFileName(file), true);
                fileNum += 1;

                if ((fileNum % (int)Convert.ToDecimal(showPer)) == 0)
                {
                    probar.Value += 1;
                    percent.Text = probar.Value + "%";
                }

            }

        }
        /// <summary>
        /// 合并  apkbase 到compile 对应目录  25%
        /// </summary>
        public void mergeApkBase()
        {
            // 保存参数文件--assets
            propertiesFile = envConfig.gameConfig + game + @"\" + channel + envConfig.configExt;// 游戏-渠道的参数文件
            if (!Directory.Exists(envConfig.targetPath + @game + @"\" + channel + @"\assets\"))
                Directory.CreateDirectory(envConfig.targetPath + @game + @"\" + channel + @"\assets\");

            File.Copy(propertiesFile, envConfig.targetPath + @game + @"\" + channel + @"\assets\" + game + envConfig.configExt, true);
            //合并文件
            string apkBase = envConfig.channels + channel + @"\apkBase\";
            if (!Directory.Exists(apkBase))
            {
                MessageBox.Show("不存在sdk母包文件");
                log.Text = "不存在sdk母包文件,异常终止";
                this.Abort();
                return;
            }

            if (File.Exists(apkBase + @"AndroidManifest.xml"))
                File.Delete(apkBase + @"AndroidManifest.xml");
            if (File.Exists(apkBase + @"apktool.yml"))
                File.Delete(apkBase + @"apktool.yml");
             
            getNum = FileUtil.getFilesNum(apkBase);
            showPer = (getNum / 25).ToString();
            mergeFolder(apkBase, envConfig.targetPath + @game + @"\" + channel + @"\");

        }

        /// <summary>
        /// 获取目录文件夹下的所有子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filePattern"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory, int maxCount)
        {
            List<string> subDirectories = new List<string>();
            if (string.IsNullOrEmpty(directory))
            {
                return subDirectories;
            }
            if (maxCount <= 0)
            {
                return subDirectories;
            }
            string[] directories = Directory.GetDirectories(directory);
            foreach (string subDirectory in directories)
            {
                if (subDirectories.Count == maxCount)
                {
                    break;
                }
                subDirectories.Add(subDirectory);
            }
            return subDirectories;
        }
        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public  string getDirectoryName(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return string.Empty;// DirectoryHelper.CreateDirectory(directory);
            }
            return new DirectoryInfo(directory).Name;
        }
        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public string getFolderName(string path)
        {
            if (path.Contains("\\"))
            {
                string[] arr = path.Split('\\');
                return arr[arr.Length - 1];
            }
            else
            {
                string[] arr = path.Split('/');
                return arr[arr.Length - 1];
            }

        }
        /// <summary>
        /// 删除不用的文件夹
        /// </summary>
        public void deleteFolder()
        {
            //string xxxPath = envConfig.targetPath + @game + @"\" + channel + @"\res\drawable-xxxhdpi\";
            //if (Directory.Exists(xxxPath))
            //    FileUtil.deleteFolder(xxxPath);

            //xxxPath = envConfig.targetPath + @game + @"\" + channel + @"\res\values-xxxhdpi\";
            //if (Directory.Exists(xxxPath))
            //    FileUtil.deleteFolder(xxxPath);
            
         // delArmeabi(envConfig.targetPath + @game + @"\" + channel + @"\lib");
            string decompile = envConfig.deCompilePath + @game + @"\lib";
            string compile = envConfig.targetPath + @game + @"\" + channel + @"\lib";

            if (!Directory.Exists(decompile)) return ;
            List<string> decomplieLibs = new List<string>();
            List<string> compileLibs = new List<string>();

            List<string>  decompileSublist = Directory.GetDirectories(decompile).ToList<string>();//获取子文件夹列表
            List<string>  compileSublist = Directory.GetDirectories(compile).ToList<string>();//获取子文件夹列表
      
            foreach (string name in decompileSublist)
            {
                decomplieLibs.Add(getDirectoryName(name));                
            }
            foreach (string name in compileSublist)
            {
                compileLibs.Add(getDirectoryName(name));
            }

            foreach (string lib in compileLibs)
            {
                bool flag = false;
                foreach (string delib in decomplieLibs)
                {
                    if (lib==delib)
                    {// 游戏支持
                        Console.WriteLine("===========游戏支持=======" + lib);
                        log.Text = "游戏支持ABI：" + lib;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {// 游戏不支持
                    Console.WriteLine("===========游戏不支持=======" + lib);
                 
                    FileUtil.deleteFolder(compile+@"\"+lib);
                }

            }
        }
  
        /// <summary>
        /// 合并资源 public.xml
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="game"></param>

        private void mergeRes(string channel, string game)
        {
            string path = envConfig.channelPath + channel + @"\apkBase\res\";
            if (!Directory.Exists(path)) {
                return;
            }

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
                                XmlHelper.mergePublicXml2(channelXmlFile, gameBaseXmlFile, channelTargetXml);  // channel->gameBase : public.xml 渠道合并进游戏xml                                                               
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
        /// <summary>
        /// 合并 AndroidManifest.xml
        /// </summary>
        /// <param name="gameXmlPath"></param>
        /// <param name="channel"></param>
        private void mergeXml(string gameXmlPath, string channel)
        {
            string channelConfigPath = envConfig.channelPath + channel + @"\config.xml";
            string screen = gameItem.orientation;
            if (screen == "portrait")
            {
                if (FileUtil.checkFile(envConfig.channelPath + channel + @"\config_portrait.xml"))
                {
                    channelConfigPath =envConfig.channelPath + channel + @"\config_portrait.xml";
                }               
            }
            else if (screen == "landscape")
            {
                if (FileUtil.checkFile(envConfig.channelPath + channel + @"\config_landscape.xml"))
                {
                    channelConfigPath = envConfig.channelPath + channel + @"\config_landscape.xml";
                }
                 
            }
            if (!FileUtil.checkFile(channelConfigPath))
            {
                MessageBox.Show("不存在" + channelConfigPath + " 文件!");
                log.Text = "不存在渠道" + channel + "config文件!异常终止";
                this.Abort();
                return;
            }
           
            string gameTmpPath = gameXmlPath + @".tmp";
            string channelTmpPath = channelConfigPath + @".tmp";

            string tmpGameXmlContent = FileUtil.getContent(gameXmlPath);
            string tmpChannelXmlContent = FileUtil.getContent(channelConfigPath);
            string tmpGameXml = FileUtil.setXmlColon(tmpGameXmlContent);
            string tmpChannelXml = FileUtil.setXmlColon(tmpChannelXmlContent);

            FileUtil.writeContent(gameTmpPath, tmpGameXml);
            FileUtil.writeContent(channelTmpPath, tmpChannelXml);
           
            if (!XmlHelper.mergeXml(channelTmpPath, gameTmpPath, gameXmlPath))
            {
                FileUtil.deleteFile(gameTmpPath);
                FileUtil.deleteFile(channelTmpPath);
                MessageBox.Show("请检查：" + channelConfigPath);
                log.Text = channelConfigPath + " xml格式错误,异常终止";
                this.Abort();
                return;
            }

            FileUtil.writeContent(gameXmlPath, FileUtil.replaceXmlColon(FileUtil.getContent(gameTmpPath)));
            FileUtil.deleteFile(gameTmpPath);
            FileUtil.deleteFile(channelTmpPath);

        }

        /// <summary>
        /// 整合 AndroidManifest  icon 
        /// </summary>
        public void mergeManifest()
        {
            string oldName = getPackName(gameXml);
            applicationName = getApplicationName(gameXml);
            mainActivityName = getMainActivityName(gameXml);

            PpHelper pt = new PpHelper(propertiesFile);
            packageName = pt.GetPropertiesText("package");
            string reinfoce = pt.GetPropertiesText("reinforce");
            mIsReInforce = reinfoce == "1" ? true : false;

            // 特殊游戏处理 
            this.replaceSpecGameXml(game, gameXml, packageName, oldName);
            // 替换包名
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

            dXe.SetAttribute("android" + FileUtil.colon + "debuggable", "false");
            dDoc.Save(xmlTmpFile1);
            tmpXml1 = FileUtil.replaceXmlColon(FileUtil.getContent(xmlTmpFile1));
            tmpXml1 = FileUtil.xmlRestore(tmpXml1);
            FileUtil.writeContent(gameXml, tmpXml1);
            File.Delete(xmlTmpFile1);

            // 整合icon
            if (gameItem.icon.Length > 0)
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
                copyIco(envConfig.currenPath + gameItem.icon, envConfig.targetPath + @game + @"\" + channel + @"\res\", icoName + ".png", channel);
                tmpXml = FileUtil.replaceXmlColon(FileUtil.getContent(xmlTmpFile));
                tmpXml = FileUtil.xmlRestore(tmpXml);
                FileUtil.writeContent(gameXml, tmpXml);
                File.Delete(xmlTmpFile);
            }
            // 反射处理特殊渠道
            try
            {
                updateChannelXml(game, channel, gameXml, propertiesFile);

            }
            catch (Exception EE)
            {
                   MessageBox.Show(EE.ToString());
            }

        }

        /// <summary>
        /// 反射方式,整合特殊 xml
        /// </summary>
        /// <param name="game"></param>
        /// <param name="channel"></param>
        /// <param name="gameXml"></param>
        /// <param name="perpties"></param>
        private void updateChannelXml(string game, string channel, string gameXml, string perpties)
        {
            //老版本
            if (envConfig.version == compileManager.Version1)
            {
                SpecXml specXml = new SpecXml();
                specXml.game = game;
                specXml.channel = channel;
                specXml.gameXml = gameXml;
                specXml.properties = perpties;
                specXml.gameName = gameItem.gid;
                Type t = specXml.GetType();
                List<string> funcs = new List<string>();
                foreach (MethodInfo mi in t.GetMethods())
                {
                    funcs.Add(mi.Name.ToString());
                }
                string channelMethod = channel.ToUpper() + "xml";
                bool exists = funcs.Contains(channelMethod);
                MethodInfo hello = t.GetMethod(channelMethod);
                if (hello == null) return;
                hello.Invoke(specXml, null);
            }
            else
            {// 新版本
                new compileManager().updateChannelXml(game, channel, gameXml, propertiesFile);
                SpecXml_2 specXml = new SpecXml_2();
                specXml.game = game;
                specXml.channel = channel;
                specXml.gameXml = gameXml;
                specXml.properties = perpties;
                specXml.gameName = gameItem.gid;
                Type t = specXml.GetType();
                List<string> funcs = new List<string>();
                foreach (MethodInfo mi in t.GetMethods())
                {
                    funcs.Add(mi.Name.ToString());
                }
                string channelMethod = channel.ToUpper() + "xml";
                bool exists = funcs.Contains(channelMethod);
                MethodInfo hello = t.GetMethod(channelMethod);
                if (hello == null) return;
                hello.Invoke(specXml, null);
            }
            return;
        }
        /// <summary>
        /// icon 处理
        /// </summary>
        /// <param name="cpIco">替换用的icon</param>
        /// <param name="compilePath">目标路径</param>
        /// <param name="fileName">游戏原来icon</param>
        /// <param name="channel"></param>
        private void copyIco(string cpIco, string compilePath, string fileName, string channel)
        { 
            string useWater = channelItem.isfoot;
            if (useWater == "true")
            {
                int waterSize = 512;
                // 透明度
                float alpha = 1;
                ImageCut.ImagePosition pos;
                // 平铺
                pos = ImageCut.ImagePosition.AllIn;
                string waterPath = envConfig.channelPath + channel + @"\" + channel + "_water.png";// 略缩图
                FileUtil.setImg(envConfig.channelPath + channel + @"\" + channel + ".png", waterPath, waterSize, waterSize);
                string cpIcoOk = envConfig.channelPath + channel + @"\" + channel + "_water_ok.png";// ok
                ImageCut.DrawImage(cpIco, waterPath, alpha, pos, cpIcoOk);
                cpIco = cpIcoOk;
            }

            string drawablePath = compilePath + @"drawable\";
            string drawableHdPath = compilePath + @"drawable-hdpi\";
            string drawableMdPath = compilePath + @"drawable-mdpi\";
            string drawableXhPath = compilePath + @"drawable-xhdpi\";
            string drawableXxPath = compilePath + @"drawable-xxhdpi\";
            string drawableXxxPath = compilePath + @"drawable-xxxhdpi\";

            ImageCut.GenThumbnail(cpIco, drawablePath + fileName, 32, 32);
            ImageCut.GenThumbnail(cpIco, drawableHdPath + fileName, 72, 72);
            ImageCut.GenThumbnail(cpIco, drawableMdPath + fileName, 48, 48);
            ImageCut.GenThumbnail(cpIco, drawableXhPath + fileName, 96, 96);
            ImageCut.GenThumbnail(cpIco, drawableXxPath + fileName, 144, 144);
            ImageCut.GenThumbnail(cpIco, drawableXxxPath + fileName, 192, 192);


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
        /// <summary>
        /// 获取游戏包名
        /// </summary>
        /// <param name="gameXml"></param>
        /// <returns></returns>
        private string getPackName(string gameXml)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(@gameXml);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"manifest");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute("package");
        }

        private string getApplicationName(string gameXml)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(gameXml);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"//manifest/application");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute("android:name");
        }

        private string getMainActivityName(string gameXml)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(gameXml);
            XmlNodeList xnlist = objXmlDoc.SelectNodes(@"//manifest/application/activity");
            string mainActivityName = "";
            foreach (XmlNode xn in xnlist)
            {
                XmlNode nodePath = xn.SelectSingleNode(@"intent-filter/category");
                if (nodePath == null) {
                    continue;
                }

                XmlElement xe = (XmlElement)nodePath;
                if (xe == null)
                {
                    continue;
                }

                string an = xe.GetAttribute("android:name");
                if (an == "android.intent.category.LAUNCHER")
                {
                    mainActivityName = xn.Attributes["android:name"].Value;;
                }

                if (mainActivityName != "")
                {
                    break;
                }
                Console.WriteLine("Student:" + xn.Attributes["android:name"].Value);
            }

            return mainActivityName;
        }

        /////


        /// <summary>
        /// 生成 R 文件
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="pro"></param>
        /// <param name="lb"></param>
        private void makeRes()
        {
            string channelMainXml = envConfig.channels + channel + @"\AndroidManifest.xml";
            string sdkPackage = channelItem.sdkPackage;// 渠道包名
            string targetPath = envConfig.targetPath + game + @"\" + channel + @"\";
            string gameMainXml = targetPath + @"AndroidManifest.xml";
            string resPath = targetPath + "res";

            if (Directory.Exists(tmpR))
                FileUtil.deleteFolder(tmpR);
            Directory.CreateDirectory(tmpR);
            if (Directory.Exists(tmpCls))
                FileUtil.deleteFolder(tmpCls);
            Directory.CreateDirectory(tmpCls);

            string packagePath = packageName.Replace(".", @"\");
            string sdkPackagePath = sdkPackage.Replace(".", @"\");

            string gameFilePath = tmpR + packagePath;
            string channelFilePath = tmpR + sdkPackagePath;
            Process makeR = new Process();
            currentProcess = makeR;
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
            makeR.StartInfo.Arguments = " " + resPath + " " + gameMainXml + " " + tmpR + "  " + tmpCls + " " + gameFilePath + " " + envConfig.targetPath + @game + @"\" + channel + @"\assets\";
            makeR.Start();

            bool flag = true;
            string err_log = "";
            if (envConfig.debug == "false")
            {
                StreamReader error_reader = makeR.StandardError;//截取输出流             
                int i = 0;
                while (!error_reader.EndOfStream)
                {
                    i++;
                    err_log += error_reader.ReadLine() + "\r\n";
                     
                    flag = false;
                }
                error_reader.Close();
            }
            makeR.WaitForExit();
            currentProcess = null;
            string logpath = "";
            if (!flag & envConfig.debug == "false")
            {
                string writePath = envConfig.logs + game + @"\";
                if (!Directory.Exists(writePath))
                {
                    Directory.CreateDirectory(writePath);
                }
                logpath = writePath + "log_" + channel + "_ " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                FileUtil.writeContent(logpath, err_log);
                log.Text = "异常日志:" + logpath;
                this.Abort();
                return;
            }
  
            string Rfile = tmpR + packagePath + @"\R.java";

            if (!FileUtil.checkFile(Rfile))
            {
                MessageBox.Show("生成R文件失败");
                log.Text = "生成R文件失败,异常终止";
                this.Abort();
                return;
            }
            if (channelItem.isR == "true")
            {
                if (!FileUtil.checkFile(channelMainXml))
                {
                    MessageBox.Show(channelMainXml + "文件未找到，重新生成渠道R文件失败");
                    log.Text = "重新生成渠道R文件失败,异常终止";
                    this.Abort();
                    return;
                }
                makeR.StartInfo.Arguments = " " + resPath + " " + channelMainXml + " " + tmpR + "  " + tmpCls + " " + channelFilePath + " " + envConfig.targetPath + @game + @"\" + channel + @"\assets\";
                currentProcess = makeR;
                makeR.Start();


                 flag = true;
                 err_log = "";
                if (envConfig.debug == "false")
                {
                    StreamReader error_reader = makeR.StandardError;//截取输出流             
                    int i = 0;
                    while (!error_reader.EndOfStream)
                    {
                        i++;
                        err_log += error_reader.ReadLine() + "\r\n";

                        flag = false;
                    }
                    error_reader.Close();
                }
                makeR.WaitForExit();
                currentProcess = null;
                logpath = "";
                if (!flag & envConfig.debug == "false")
                {
                    string writePath = envConfig.logs + game + @"\";
                    if (!Directory.Exists(writePath))
                    {
                        Directory.CreateDirectory(writePath);
                    }
                    logpath = writePath + "log_" + channel + "_ " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    FileUtil.writeContent(logpath, err_log);
                    log.Text = "生成R异常:" + logpath;
                    this.Abort();
                    return;
                }

                Rfile = tmpR + sdkPackagePath + @"\R.java";
                if (!FileUtil.checkFile(Rfile))
                {
                    MessageBox.Show("生成R文件失败");
                    log.Text = "重新生成渠道R文件失败,异常终止";
                    this.Abort();
                    return;
                }
            }
        }
        /// <summary>
        /// 签名
        /// </summary>
        public void makeSignApk()
        {
            string signPwd = gameItem.signPwd;
            string keyStrorePwd = gameItem.keyPwd;
            string keyStore = envConfig.currenPath + gameItem.keyPath;
            string alias = gameItem.alias;

            if (channelItem.isOwnKey == "true")
            {
                this.log.Text = "正在使用渠道方签名文件..";
                keyStore = envConfig.channels + this.channelItem.cid + @"\" + this.channelItem.cid + ".keystore";
                string signfile = envConfig.channels + this.channelItem.cid + @"\sign" + envConfig.configExt;
                if (!FileUtil.checkFile(signfile))
                {
                    this.Abort();
                    this.log.Text = "渠道签名不存在！异常终止";
                    MessageBox.Show("渠道签名不存在！");
                    return;
                }
                if (!FileUtil.checkFile(keyStore))
                {
                    this.Abort();
                    this.log.Text = "渠道keystore不存在！异常终止";
                    MessageBox.Show("渠道keystore不存在！");
                    return;
                }

                PpHelper helper = new PpHelper(signfile);
                alias = helper.GetPropertiesText("alias");
                keyStrorePwd = helper.GetPropertiesText("keyPwd");
                signPwd = helper.GetPropertiesText("signPwd");

                if (alias == "" || keyStrorePwd == "" || signPwd == "")
                {
                    this.Abort();

                    this.log.Text = "渠道签名信息不完整！异常终止";
                    MessageBox.Show("渠道签名信息不完整！");
                    return;
                }
            }
            string releasePath = envConfig.releasePkg + game + @"\";
            if (!Directory.Exists(releasePath))
                Directory.CreateDirectory(releasePath);
            string releaseApk = releasePath + game + "_" + unSignApkName;

            Process makeSign = new Process();
            currentProcess = makeSign;
            if (!this.isDebug())
            {  // 签名-隐藏命令行输出
                makeSign.StartInfo.UseShellExecute = false;
                makeSign.StartInfo.CreateNoWindow = true;
                makeSign.StartInfo.RedirectStandardInput = false;
                makeSign.StartInfo.RedirectStandardOutput = false;// 不输出，，，影响效率
                makeSign.StartInfo.RedirectStandardError = false;
            } 
          
            makeSign.StartInfo.FileName = envConfig.toolPath + @"\sign.bat";

            string unSignApkPath = unSignApk;
            if (mIsReInforce)
            {
                string compileFolder = envConfig.compileFolder + game + @"\";
                string newUnSignApkName = channel + "_" + pkgTime + "_new.apk";
                unSignApkPath = compileFolder + newUnSignApkName;
            }

            makeSign.StartInfo.Arguments = " " + keyStore + " " + keyStrorePwd + " " + signPwd + " " + releaseApk + " " + unSignApkPath + " " + alias;
            makeSign.Start();
            makeSign.WaitForExit();
            currentProcess = null;
        }

        /// <summary>
        /// 生成apk 未签名
        /// </summary>
        public void makeUnsignApk()
        {
            string packageFolder = envConfig.targetPath + @game + @"\" + channel + @"\";
            string compileFolder = envConfig.compileFolder + game + @"\";


            unSignApkName = channel + "_" + pkgTime + ".apk";
            unSignApk = compileFolder + unSignApkName;
            if (!Directory.Exists(packageFolder))
            {
                return ;
            }

            string command = envConfig.apkTool;
            Process p = new Process();
            currentProcess = p;
            if (envConfig.debug == "false")
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
            }
            else
            {
                command = envConfig.apkTool_debug;

            }
            p.StartInfo.FileName = command;
            p.StartInfo.Arguments = "  b  " + packageFolder + "  -o  " + unSignApk;
            p.Start();

            bool flag = true;
            string err_log = "";
            if (envConfig.debug == "false")
            {
                StreamReader error_reader = p.StandardError;//截取输出流             
                int i = 0;
                while (!error_reader.EndOfStream)
                {
                    i++;
                    err_log += error_reader.ReadLine() + "\r\n";
                    //if (!err_log.StartsWith("Warning:") && !err_log.StartsWith("W:"))
                    //{
                    //    flag = false;
                    //}
                    flag = false;
                }
                error_reader.Close();
            }
            p.WaitForExit();
            currentProcess = null;
            string logpath="";
            if (!flag & envConfig.debug == "false")
            {
                string writePath = envConfig.logs + game + @"\";
                if (!Directory.Exists(writePath))
                {
                    Directory.CreateDirectory(writePath);
                }
                logpath = writePath + "log_" +channel+"_ "+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                FileUtil.writeContent(logpath, err_log); 
                log.Text = "异常日志:" + logpath;
            }
        }

        public void inforce()
        {
            string compileFolder = envConfig.compileFolder + game + @"\";
            unSignApkName = channel + "_" + pkgTime + ".apk";
            unSignApk = compileFolder + unSignApkName;
            
            string newReInforceApk = compileFolder + channel + "_" + pkgTime + "_new.apk";

            string command = envConfig.apkTool;
            Process p = new Process();
            currentProcess = p;
            if (envConfig.debug == "false")
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
            }
            else
            {
                command = envConfig.apkTool_debug;
            }

            p.StartInfo.FileName = envConfig.toolPath + @"\preReInforce.bat";
            p.StartInfo.Arguments = command + " " + envConfig.currenPath+@"\" + " " + game + " " +channel + " " + pkgTime
                + " "+ applicationName +" " + mainActivityName + " " + packageName;

            p.Start();

            bool flag = true;
            string err_log = "";
            if (envConfig.debug == "false")
            {
                StreamReader error_reader = p.StandardOutput;//截取输出流      

//                StreamWriter sw = File.AppendText(envConfig.logs + game + @"\log_1.txt");
//                int i = 0;
                while (!error_reader.EndOfStream)
                {
//                    i++;
//                    err_log += error_reader.ReadLine() + "\r\n";
                    string str = error_reader.ReadLine();
//                    sw.WriteLine(str + "\r\n");
                    if (str == "exit")
                    {
                        break;
                    }
                }
//                sw.Flush();
 //               sw.Close();
                error_reader.Close();

                p.StandardInput.WriteLine("exit");
            }
            p.WaitForExit();
            currentProcess = null;
            string logpath = "";
            if (!flag & envConfig.debug == "false")
            {
                string writePath = envConfig.logs + game + @"\";
                if (!Directory.Exists(writePath))
                {
                    Directory.CreateDirectory(writePath);
                }
                logpath = writePath + "log_" + channel + "_ " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                FileUtil.writeContent(logpath, err_log);
                log.Text = "异常日志:" + logpath;
            }
        }

        public void makeInforce()
        {
            string compileFolder = envConfig.compileFolder + game + @"\";
            string dcfpath = compileFolder + channel + "_" + pkgTime + @"\";
            
            Process p = new Process();
            currentProcess = p;
            if (envConfig.debug == "false")
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
            }

            p.StartInfo.FileName = envConfig.toolPath + @"\reInforce.bat";
            p.StartInfo.Arguments = envConfig.currenPath+@"\" + " " + game + " " + channel + " " + pkgTime;
            p.Start();

            p.WaitForExit();
            currentProcess = null;
        }


        /// <summary>
        /// 生成smali
        /// </summary>
        public void makeSmali()
        {
            string targetPath = envConfig.targetPath + game + @"\" + channel + @"\";

            Process makeSmali = new Process();
            currentProcess = makeSmali;
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
              
            }
            
            makeSmali.StartInfo.Arguments = targetPath + @"smali\   " + envConfig.tmpFolder + game + @"\" + channel + @"\temp.dex  ";
            makeSmali.Start();
            makeSmali.WaitForExit();
            currentProcess = null;

        }

        /// <summary>
        /// sdk版本控制smali
        /// </summary>
        public void checkSmali()
        { 
            string sdk =new compileManager().getSdk();
            if (sdk == "false" || sdk=="")
            {
                log.Text = "检测sdk版本控制:false";
                return;
            }
            log.Text = "检测sdk版本控制:"+sdk;
            string sdkdex = envConfig.sdk + sdk + @".dex";

            if (!FileUtil.checkFile(sdkdex))
            {
                MessageBox.Show(sdkdex + "文件不存在");
                this.Abort();
                return;
            }
            string targetPath = envConfig.targetPath + game + @"\" + channel + @"\";
            // 删除游戏sdk smali文件
            //delGameSdk();
 
            Process makeSmali = new Process();
            currentProcess = makeSmali;
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

            }
           makeSmali.StartInfo.Arguments = targetPath + @"smali\   " + sdkdex;
           //   makeSmali.StartInfo.Arguments = envConfig.currenPath + @"\smali\   " + sdkdex;

            makeSmali.Start();
            makeSmali.WaitForExit();
            currentProcess = null;

        }


        /// <summary>
        /// 生成DEX
        /// </summary>
        public void makeDex()
        {

            Process makeDex = new Process();
            currentProcess = makeDex;
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

            makeDex.StartInfo.Arguments = "   --dex --output=" + envConfig.tmpFolder + game + @"\" + channel + @"\temp.dex   " + tmpCls;
            makeDex.Start();
            makeDex.WaitForExit();
            currentProcess = null;
        }
        /////

    }
}
