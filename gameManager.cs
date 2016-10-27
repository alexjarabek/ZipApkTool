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
 
using System.Xml.Serialization;
namespace sandGlass
{
    public delegate void GameDeBackDelegate(string thread, string msg);
    class gameManager
    {

        private static XmlDocument xmlDoc = new XmlDocument();
        private static List<gameItem> itemlist = new List<gameItem>();
        public GameDeBackDelegate deCallBack;
        public static string MSG_COMPLETE = "COMPLETE";
        public static string MSG_ERROR = "ERROR";
        /// <summary>
        /// 返回games.xml文档的根节点
        /// </summary>
        /// <returns>根节点</returns>
        public XmlNode GetRootNode()
        {
            //xmlDoc = new XmlDocument();

            if (!FileUtil.checkFile(envConfig.gamesXml))
            {
                CreateXmlConfigFile(envConfig.gamesXml);
            }

            xmlDoc.Load(envConfig.gamesXml);
            XmlNode root = xmlDoc.DocumentElement;
            return root;

        }

        public void CreateXmlConfigFile(string fileName)
        {

            XmlTextWriter xmlWriter;
            xmlWriter = new XmlTextWriter(fileName, Encoding.UTF8);//creat ;
            xmlWriter.Formatting = Formatting.Indented;//自动缩进格式

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Root");
            xmlWriter.WriteEndElement();
            xmlWriter.Close();

            xmlWriter = null;
            GC.Collect();
        }

        public void initialListView(ListView listview, ImageList imagelist)
        {
            listview.Clear();
            listview.LargeImageList = imagelist;
            //    imagelist.ImageSize = new Size(25, 25);// 设置行高 20 //分别是宽和高  
            GC.Collect();
        }
        /// <summary>
        /// 重新生成 gamexml
        /// </summary>
        public void reCreatGameXml()
        {
            FileUtil.deleteFile(envConfig.gamesXml);
            List<string> folders = FileUtil.getFolders(envConfig.games);
            foreach (string folder in folders)
            {
                string game = new DirectoryInfo(folder).Name;
                if (game == ".svn")
                {
                    continue;
                }
                string gameProperties = envConfig.games + game + @"\" + game + envConfig.configExt;
                gameItem item = getItem(game);
                AddXmlNode(item);
            }

            itemlist.Clear();
            xmlSave();

        }



        /// <summary>
        /// 添加game节点 属性占一个节点
        /// </summary>
        /// <param name="item">游戏Item</param>
        public void reCreatGameXml(string xxx)
        {

            List<string> folders = FileUtil.getFolders(envConfig.games);
            List<gameItem> lst = new List<gameItem>();

            foreach (string folder in folders)
            {
                string game = new DirectoryInfo(folder).Name;
                if (game == ".svn")
                {
                    continue;
                }
                string gameProperties = envConfig.games + game + @"\" + game + envConfig.configExt;
                gameItem item = getItem(game);
                lst.Add(item);
            }

            itemlist.Clear();


            XmlDocument xd = new XmlDocument();
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(lst.GetType());
                xz.Serialize(sw, lst);
                Console.WriteLine(sw.ToString());
                xd.LoadXml(sw.ToString());
                xd.Save("E:\\1.xml");
            }
           
        }


        public void xmlToList() {

            List<gameItem> lst2 = new List<gameItem>();
            using (XmlReader reader = XmlReader.Create("c:\\1.xml"))
            {
                XmlSerializer xz = new XmlSerializer(lst2.GetType());
                lst2 = (List<gameItem>)xz.Deserialize(reader);
                Console.WriteLine(reader.ToString());
            }

        }



        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="listView"></param>
        /// <param name="imagelist"></param>
        public void initGameData(ListView listView, ImageList imagelist)
        {
            GC.Collect();

            string icon = null;

            XmlNode root = GetRootNode();
            listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大
            listView.Clear();
            imagelist.Images.Clear();
            listView.Columns.Add("1", 120, HorizontalAlignment.Center); //一步添加  
            listView.Columns.Add("2", 120, HorizontalAlignment.Center); //一步添加  
            listView.Columns.Add("3", 120, HorizontalAlignment.Center); //一步添加  
            //    listView.Columns.Add("序号", 50, HorizontalAlignment.Left); 
            
            itemlist = GetItems();

            foreach (gameItem gameitem in itemlist)
            {
                icon = envConfig.games + gameitem.gid + @"\" + gameitem.gid + @".png";

                if (!FileUtil.checkFile(icon))
                {
                    icon = envConfig.images + @"\default_icon.png";
                }

                ListViewItem item = new ListViewItem();
                item.Tag = gameitem;//-----------------
                item.Text = gameitem.name;
                item.ImageIndex = gameitem.index;
                item.SubItems.Add(gameitem.gid);
                item.SubItems.Add(gameitem.version);
                imagelist.Images.Add("" + gameitem.index, getImage(icon));//
                listView.Items.Add(item);

            }
            listView.EndUpdate();  //结束数据处理，UI界面一次性绘制
            GC.Collect();
        }

        private Image getImage(string path)
        {
            Image img = System.Drawing.Image.FromFile(path);
            Image bmp = new System.Drawing.Bitmap(img);
            img.Dispose();
            return bmp;
        }


        /// <summary>
        /// 新增/更新游戏信息
        /// </summary>
        /// <param name="item"></param>
        public int saveUpdate(gameItem item)
        {
            int index = saveUpdateToXml(item);

            if (index < 0)
            {
                return -1;

            }

            saveItem(item);
            itemlist = GetItems();
            return index;
        }

        /// <summary>
        /// 新增更新列表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="item"></param>
        public int saveUpdateToXml(gameItem item)
        {
            if (item == null) return -2;
            XmlNode root = GetRootNode();
            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@index='" + item.index + "']");

            if (xFind == null)
            {
                //新增
                xFind = xmlDoc.SelectSingleNode("Root/Item[@gid='" + item.gid + "']");
                if (xFind != null)
                {
                    DialogResult dr = MessageBox.Show("已经存在标识为：" + item.gid + "的游戏", "确认是否覆盖", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        XmlElement element = (XmlElement)xFind;
                        item.index = Convert.ToInt32(element.GetAttribute("index"));
                        return UpdateXmlNode(item);
                    }
                    else
                    {
                        return -1000;

                    }
                }

                return AddXmlNode(item);
            }
            else
            {
                return UpdateXmlNode(item);

            }

        }
        public bool checkApkBase(string id)
        {
            string path = envConfig.deCompilePath + id + @"\";
            return Directory.Exists(path);

        }

        /// <summary>
        /// 添加game节点 index 0 递增
        /// </summary>
        /// <param name="item">游戏Item</param>
        public int AddXmlNode(gameItem item)
        {

            if (item == null) return -2;
            XmlNode root = GetRootNode();
            XmlElement lastElement = (XmlElement)root.LastChild;

            int index = 0;
            if (lastElement != null)
            {
                string strIndex = lastElement.GetAttribute("index");
                index = Convert.ToInt32(strIndex) + 1;
            }
            //创建节点
            XmlElement element = xmlDoc.CreateElement("Item");
            //添加属性
            setElement(element, item);
            element.SetAttribute("index", "" + index);
            item.index = index;
            //将节点加入到指定的节点下
            XmlNode xml = root.AppendChild(element);
            xmlSave();
            return index;
        }




        /// <summary>
        /// 修改item节点
        /// </summary>
        /// <param name="item">渠道Item</param>
        public int UpdateXmlNode(gameItem item)
        {
            if (item == null) return -2;
            XmlNode root = GetRootNode();
            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@index='" + item.index + "']");
            XmlElement element = (XmlElement)xFind;
            setElement(element, item);
            //  element.SetAttribute("index", index);
            xmlSave();

            return item.index;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        public void setElement(XmlElement element, gameItem item)
        {

            element.SetAttribute("index", "" + item.index);
            element.SetAttribute("name", item.name);
            element.SetAttribute("version", item.version);
            element.SetAttribute("orientation", item.orientation);
            element.SetAttribute("gid", item.gid);
            element.SetAttribute("icon", item.icon);
            element.SetAttribute("key", item.key);
        //    element.SetAttribute("apk", item.apk);
            element.SetAttribute("flag", item.flag);
            element.SetAttribute("package", item.package);
            element.SetAttribute("des", item.des);
            element.SetAttribute("keyPwd", item.keyPwd);
            element.SetAttribute("signPwd", item.signPwd);
            element.SetAttribute("alias", item.alias);
            element.SetAttribute("keyPath", item.keyPath);
            //     element.SetAttribute("apktool", item.apktool);

        }

        /// <summary>
        /// 保存游戏信息
        /// </summary>
        /// <param name="item"></param>
        public void saveItem(gameItem item)
        {
            string lineStr = "";
            //     lineStr = "index=" + item.index + "\r\n";
            lineStr = "name=" + item.name + "\r\n";
            lineStr += "gid=" + item.gid + "\r\n";
            lineStr += "package=" + item.package + "\r\n";
            lineStr += "version=" + item.version + "\r\n";
            lineStr += "icon=" + item.icon + "\r\n";

          //  lineStr += "apk=" + item.apk + "\r\n";
            lineStr += "orientation=" + item.orientation + "\r\n";
            lineStr += "key=" + item.key + "\r\n";
            lineStr += "alias=" + item.alias + "\r\n";
            lineStr += "keyPwd=" + item.keyPwd + "\r\n";
            lineStr += "keyPath=" + item.keyPath + "\r\n";
            lineStr += "signPwd=" + item.signPwd + "\r\n";
            //       lineStr += "apktool=" + item.apktool + "\r\n";
            lineStr += "flag=" + item.flag + "\r\n";
            lineStr += "des=" + item.des.Replace("\n", "").Replace(" ","").Replace("\t","").Replace("\r","") + "\r\n";
            FileUtil.writeContent(envConfig.games + item.gid + @"\" + item.gid + envConfig.configExt, lineStr);

        }


        public void xmlSave()
        {
            xmlDoc.Save(envConfig.gamesXml);
        }

        /// <summary>
        /// 获取Items
        /// </summary>
        /// <param name="item">游戏Item</param>
        public List<gameItem> GetItems()
        {

            XmlNode root = xmlDoc.DocumentElement;
            XmlNodeList list = root.ChildNodes;
            List<gameItem> itemlist = new List<gameItem>();

            foreach (XmlElement element in list)
            {
                gameItem item = new gameItem();
                item.name = element.GetAttribute("name");
                item.gid = element.GetAttribute("gid");
                item.package = element.GetAttribute("package");
                item.version = element.GetAttribute("version");
                item.icon = element.GetAttribute("icon");
                item.des = element.GetAttribute("des");
            //    item.apk = element.GetAttribute("apk");
                item.orientation = element.GetAttribute("orientation");
                item.key = element.GetAttribute("key");
                item.flag = element.GetAttribute("flag");
                item.alias = element.GetAttribute("alias");
                item.keyPwd = element.GetAttribute("keyPwd");
                item.keyPath = element.GetAttribute("keyPath");
                item.signPwd = element.GetAttribute("signPwd");
                //       item.apktool = element.GetAttribute("apktool");

                try
                {
                    item.index = Convert.ToInt32(element.GetAttribute("index"));
                }
                catch (Exception)
                {
                    continue;
                }

                itemlist.Add(item);
            }
            return itemlist;
        }
        /// <summary>
        /// 读取渠道property文件返回 游戏对象 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public gameItem getItem(string gid)
        {
            string fileName = envConfig.games + gid + @"\" + gid + envConfig.configExt;
            GC.Collect();
            gameItem item = new gameItem();
            if (FileUtil.checkFile(fileName))
            {
                // 读取游戏信息
                PpHelper helper = new PpHelper(fileName);
                item.name = helper.GetPropertiesText("name");
                item.gid = helper.GetPropertiesText("gid");
                item.package = helper.GetPropertiesText("package");
                item.version = helper.GetPropertiesText("version");
                item.icon = helper.GetPropertiesText("icon");
                item.des = helper.GetPropertiesText("des");
      //          item.apk = helper.GetPropertiesText("apk");
                item.key = helper.GetPropertiesText("key");
                item.orientation = helper.GetPropertiesText("orientation");
                item.flag = helper.GetPropertiesText("flag");
                item.alias = helper.GetPropertiesText("alias");
                item.keyPwd = helper.GetPropertiesText("keyPwd");
                item.keyPath = helper.GetPropertiesText("keyPath");
                item.signPwd = helper.GetPropertiesText("signPwd");

            }

            return item;

        }
        /// <summary>
        /// 读取渠道xml文件返回对象 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public gameItem getItemByXml(string gid)
        {
            XmlNode root = GetRootNode();
            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@gid='" + gid + "']");
            gameItem item = null;
            if (xFind != null)
            {
                item = new gameItem();
                XmlElement element = (XmlElement)xFind;
                item.name = element.GetAttribute("name");
                item.gid = element.GetAttribute("gid");
                item.version = element.GetAttribute("version");
                item.package = element.GetAttribute("package");
                item.icon = element.GetAttribute("icon");
                item.key = element.GetAttribute("key");
                item.orientation = element.GetAttribute("orientation");
     //           item.apk = element.GetAttribute("apk");
                item.des = element.GetAttribute("des");
                item.signPwd = element.GetAttribute("signPwd");
                item.alias = element.GetAttribute("alias");
                item.keyPwd = element.GetAttribute("keyPwd");
                item.keyPath = element.GetAttribute("keyPath");
                return item;

            }
            return item;

        }
        private string from;
        private string to;
        private RichTextBox richTextBox;
        private Button getApkbtn, btnSave;
        /// <summary>
        /// 解压母包
        /// </summary>
        /// <param name="richTxtInfo"></param>
        /// <param name="from"></param>
        /// <param name="name"></param>
        public void deCompile(string from, string name, RichTextBox richTextBox, Button getApkbtn, Button btnSave = null)
        {

            if (from.Equals(""))
            {
                return;
            }
            if (!FileUtil.checkFile(from))
            {
                MessageBox.Show("不存在APK文件,母包更新失败");
                return;
            }
            richTextBox.Text = "";

            if (btnSave != null) btnSave.Enabled = false;
            getApkbtn.Enabled = false;
            string to = envConfig.deCompilePath + name + @"\";
            this.from = from;
            this.to = to;
            this.richTextBox = richTextBox;
            this.getApkbtn = getApkbtn;
            this.btnSave = btnSave;
            Thread t = new Thread(d);
            t.Start();
        }

        public void d()
        {
            bool ret = apkTool.d(richTextBox, from, to);
            getApkbtn.Enabled = true;
            if (btnSave != null) btnSave.Enabled = true;
            if (deCallBack == null) return;
            if (ret)
            {  
                deCallBack(to, MSG_COMPLETE);
            }
            else
            {
                deCallBack(to,MSG_ERROR);
            }


           
        }


    }
}
