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
    public delegate void ChannelDeBackDelegate(string thread, string msg);
    class channelManager
    {       
        private static XmlDocument xmlDoc = new XmlDocument();
        private static XmlDocument metaXmlDoc = new XmlDocument();
        private static List<channelItem> itemlist = new List<channelItem>();
        public ChannelDeBackDelegate deCallBack;
        public static string MSG_COMPLETE = "COMPLETE";
        public static string MSG_ERROR = "ERROR";
        /// <summary>
        /// 返回channels.xml文档的根节点
        /// </summary>
        /// <returns>根节点</returns>
        public XmlNode GetRootNode()
        {

            if (!FileUtil.checkFile(envConfig.channelsXml))
            {
                CreateXmlConfigFile(envConfig.channelsXml);
            }
            xmlDoc.Load(envConfig.channelsXml);
            XmlNode root = xmlDoc.DocumentElement;
            return root;

        }
        /// <summary>
        /// 返回meta_datas.xml文档的根节点
        /// </summary>
        /// <returns>根节点</returns>
        public XmlNode GetMetaRootNode()
        {

            if (!FileUtil.checkFile(envConfig.meta_datasXml))
            {
                CreateXmlConfigFile(envConfig.meta_datasXml);
            }
            metaXmlDoc.Load(envConfig.meta_datasXml);
            XmlNode root = metaXmlDoc.DocumentElement;
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
        public void initialListView(ListView listview)
        {
            listview.Clear();
            GC.Collect();
        }


        /// <summary>
        /// 新增/更新渠道信息
        /// </summary>
        /// <param name="item"></param>
        public int saveUpdate(channelItem item)
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
        /// 保存渠道配置--暂时做保存备份
        /// </summary>
        /// <param name="item"></param>
        public void saveItem(channelItem item)
        {
            string lineStr = "";
            //    lineStr = "index=" + item.index + "\r\n";
            lineStr = "name=" + item.name + "\r\n";
            lineStr += "cid=" + item.cid + "\r\n";
            lineStr += "package=" + item.package + "\r\n";
            lineStr += "version=" + item.version + "\r\n";
            lineStr += "foot=" + item.foot + "\r\n";
          //  lineStr += "apk=" + item.apk + "\r\n";
            lineStr += "isfoot=" + item.isfoot + "\r\n";
            lineStr += "isR=" + item.isR + "\r\n";
            lineStr += "sdkPackage=" + item.sdkPackage + "\r\n";
            lineStr += "isOwnKey=" + item.isOwnKey + "\r\n";
            lineStr += "flag=" + item.flag + "\r\n";
            lineStr += "des=" + item.des.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "") + "\r\n";
            FileUtil.writeContent(envConfig.channels + item.cid + @"\" + item.cid + "_info" + envConfig.configExt, lineStr);
        }
        /// <summary>
        /// 保存 property 文件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="metas"></param>
        public void setChannelProperty(channelItem item, Dictionary<string, string> metas)
        {
            if (metas == null) return;
            string   writeFileStr = ""; 
            foreach (KeyValuePair<string, string> p in metas)
                {
                    string name = p.Key;
                    string value = p.Value;
                    string   lineStr = name + "=" + value + "\r\n";
                    writeFileStr += lineStr;
                }
             
            // 当前到channels 下
            string tempFile = envConfig.channels + item.cid + @"\" + item.cid + envConfig.configExt;
            FileUtil.writeContent(tempFile, writeFileStr);

        }
        /// <summary>
        /// 生成渠道 channel.xml
        /// </summary>
        /// <param name="file"></param>
        public void createChannelxml(string file)
        {
            if (!FileUtil.checkFile(file))
            {
                XmlTextWriter xmlWriter;
                xmlWriter = new XmlTextWriter(file, Encoding.UTF8);//creat ;
                xmlWriter.Formatting = Formatting.Indented;//自动缩进格式           
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("channel");
                xmlWriter.WriteEndElement();
                xmlWriter.Close();

                xmlWriter = null;
                GC.Collect();
            }

        }

        /// <summary>
        /// 新增更新列表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="item"></param>
        public int saveUpdateToXml(channelItem item)
        {
            if (item == null) return -2;
            XmlNode root = GetRootNode();

            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@index='" + item.index + "']");
            if (xFind == null)
            {
                //新增
                xFind = xmlDoc.SelectSingleNode("Root/Item[@cid='" + item.cid + "']");
                if (xFind != null)
                {
                    DialogResult dr = MessageBox.Show("已经存在标识为：" + item.cid + "的渠道信息", "确认是否覆盖", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

        /// <summary>
        /// 添加节点 index 0 递增
        /// </summary>
        /// <param name="item">游戏Item</param>
        public int AddXmlNode(channelItem item)
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
        public int UpdateXmlNode(channelItem item)
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
        public void setElement(XmlElement element, channelItem item)
        {

            element.SetAttribute("cid", item.cid);
            element.SetAttribute("name", item.name);
            element.SetAttribute("version", item.version);
            element.SetAttribute("isR", item.isR);
            element.SetAttribute("isfoot", item.isfoot);
            element.SetAttribute("foot", item.foot);
          //  element.SetAttribute("apk", item.apk);
            element.SetAttribute("flag", item.flag);
            element.SetAttribute("package", item.package);
            element.SetAttribute("sdkPackage", item.sdkPackage);
            element.SetAttribute("isOwnKey", item.isOwnKey);
            element.SetAttribute("des", item.des);

        }

        /// <summary>
        /// 读取渠道xml文件返回对象 
        /// </summary>
        public channelItem getItemByXml(string cid)
        {
            XmlNode root = GetRootNode();
            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@cid='" + cid + "']");
            channelItem item = null;
            if (xFind != null)
            {
                item = new channelItem();
                XmlElement element = (XmlElement)xFind;
                item = getItemByElement(element);
                return item;

            }
            return item;

        }

        public void xmlSave()
        {
            xmlDoc.Save(envConfig.channelsXml);
        }

        public void copy(string from, string to)
        {
            if (!File.Exists(from))
            {
                return;
            }
            if (from.Equals(to))
            {
                return;
            }
            File.Copy(from, to, true);

        }

        /// <summary>
        /// 重新生成 channelsxml
        /// </summary>
        public void reCreatChannelsXml()
        {
            FileUtil.deleteFile(envConfig.channelsXml);
            List<string> folders = FileUtil.getFolders(envConfig.channels);
            foreach (string folder in folders)
            {
                string id = new DirectoryInfo(folder).Name;
                if (id == ".svn")
                {
                    continue;
                }
                channelItem item = null;
                string fileName = envConfig.channels + id + @"\" + id + "_info" + envConfig.configExt;
                if (FileUtil.checkFile(fileName))
                {
                    item = getItem(id);
                }
                else
                {
                    item = new channelItem();
                    // 读取channel.xml
                    string channelXml = envConfig.channels + id + @"\channel.xml";
                    if (FileUtil.checkFile(channelXml))
                    {
                        string name = XmlHelper.getChannelAtrr(channelXml, "name");
                        string useWater = XmlHelper.getChannelAtrr(channelXml, "useWater");
                        if (useWater == "yes") { item.isfoot = "true"; }
                        item.name = name;
                    }
                    string readFile = envConfig.channels + id + @"\readme.txt";
                    if (FileUtil.checkFile(readFile))
                    {
                        FileStream fs = new FileStream(readFile, FileMode.Open, FileAccess.Read);
                        StreamReader sr = new StreamReader(fs, Encoding.Default);
                        string readme = sr.ReadToEnd();
                        item.des = readme.Replace("\r\n\r\n", "");
                        sr.Close();
                        fs.Close();
                    }

                    item.cid = id;
                    item.flag = "false";
                }
                AddXmlNode(item);
            }
            itemlist.Clear();
            xmlSave();

        }

        /// <summary>
        /// 初始化渠道列表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="listView"></param>
        /// <param name="imagelist"></param>
        public void initChannelData(ListView listView)
        {
            GC.Collect();
            XmlNode root = GetRootNode();
            listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大
            listView.Clear();
            listView.Columns.Add("名称", 120, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("标识", 100, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("版本", 90, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("状态", 90, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("序号", 50, HorizontalAlignment.Left); //一步添加  

            if (itemlist.Count <= 0)
                itemlist = GetItems();
            foreach (channelItem citem in itemlist)
            {
                ListViewItem item = new ListViewItem();

                item.Tag = citem;//-----------------
                item.Text = citem.name;
                item.ImageIndex = citem.index;
                item.SubItems.Add(citem.cid);
                item.SubItems.Add(citem.version);
                if (citem.flag == "true")
                {
                    item.SubItems.Add("完成");
                }
                else
                {
                    item.SubItems.Add("待续");
                    item.ForeColor = Color.Red;

                }

                item.SubItems.Add("" + citem.index);
                listView.Items.Add(item);
            }
            listView.EndUpdate();  //结束数据处理，UI界面一次性绘制
            GC.Collect();
        }
        /// <summary>
        /// 初始化 渠道选择列表 checkData
        /// </summary>
        public void initChannelCheckData(ListView listView, string game, bool check)
        {
            GC.Collect();
            XmlNode root = GetRootNode();
            listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大
            listView.Clear();
            listView.Columns.Add("名称", 120, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("标识", 100, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("版本", 90, HorizontalAlignment.Left); //一步添加              
            listView.Columns.Add("序号", 50, HorizontalAlignment.Left); //一步添加  

            List<channelItem> channellist = new List<channelItem>();
            if (check)
            {
                channellist = GetItemsByGame(game);// 已发渠道               
            }
            else
            {
                channellist = GetItems(); // 所有渠道
            }
            int i = 0;
            foreach (channelItem citem in channellist)
            {
                ListViewItem item = new ListViewItem();
                if (citem.flag == "true")
                {
                    i++;
                    item.Tag = citem;//-----------------
                    item.Text = citem.name;
                    item.ImageIndex = citem.index;
                    item.SubItems.Add(citem.cid);
                    item.SubItems.Add(citem.version);
                    item.SubItems.Add("" + i);// 计数用的序号
                    listView.Items.Add(item);
                }

            }
            listView.EndUpdate();  //结束数据处理，UI界面一次性绘制
            GC.Collect();

        }
        /// <summary>
        /// 初始化 渠道已经录入的列表
        /// </summary>
        public void initChannelMetaedData(ListView listView,  bool check)
        {
            GC.Collect();
            XmlNode root = GetRootNode();
            listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大
            listView.Clear();
            listView.Columns.Add("名称", 120, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("标识", 100, HorizontalAlignment.Left); //一步添加  
            listView.Columns.Add("版本", 90, HorizontalAlignment.Left); //一步添加              
            listView.Columns.Add("序号", 50, HorizontalAlignment.Left); //一步添加  

           

            List<channelItem> channellist = new List<channelItem>();
            if (check)
            {
                channellist = getChannelMetaed();// 已经录入渠道               
            }
            else
            {
                channellist = GetItems(); // 所有渠道
            }
            int i = 0;
            foreach (channelItem citem in channellist)
            {
                i++;
                ListViewItem item = new ListViewItem();
                if (citem.flag == "true")
                {
                    item.Tag = citem;//-----------------
                    item.Text = citem.name;
                    item.ImageIndex = citem.index;
                    item.SubItems.Add(citem.cid);
                    item.SubItems.Add(citem.version);
                    item.SubItems.Add("" + i); //  计数用的 序号
                    listView.Items.Add(item);
                }

            }
           
       //     listView.Sorting = SortOrder.Ascending;
       //     listView.Sort();
           

            listView.EndUpdate();  //结束数据处理，UI界面一次性绘制
            GC.Collect();

        }
        /// <summary>
        /// element to  channelItem
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public channelItem getItemByElement(XmlElement element)
        {
            channelItem item = new channelItem();
            item.name = element.GetAttribute("name");
            item.cid = element.GetAttribute("cid");
            item.package = element.GetAttribute("package");
            item.version = element.GetAttribute("version");
            item.foot = element.GetAttribute("foot");
            item.des = element.GetAttribute("des");
         //   item.apk = element.GetAttribute("apk");
            item.isfoot = element.GetAttribute("isfoot");
            item.isR = element.GetAttribute("isR");
            item.sdkPackage = element.GetAttribute("sdkPackage");
            item.isOwnKey = element.GetAttribute("isOwnKey");
            item.flag = element.GetAttribute("flag");
            try
            {
                item.index = Convert.ToInt32(element.GetAttribute("index"));
            }
            catch (Exception)
            {
                item.index =-1;
            }
            return item;
        }



        /// <summary>
        /// 根据游戏获取已打过的Items
        /// </summary>
        /// <param name="item">游戏Item</param>
        public List<channelItem> GetItemsByGame(string name)
        {

            string from = envConfig.gameConfig + name;
            List<channelItem> itemlistByGame = new List<channelItem>();
            if (!Directory.Exists(from))
            {
                return itemlistByGame;

            }

            List<string> channels = new List<string>();
            DirectoryInfo layerDir = new DirectoryInfo(from);
            foreach (FileInfo layerFile in layerDir.GetFiles("*.properties"))
            {
                string strmainname = Path.GetFileNameWithoutExtension(layerFile.Name);
                //               Console.WriteLine(strmainname);
                if (strmainname == "sign") continue;// 签名文件
                channels.Add(strmainname);
                channelItem item = getItemByXml(strmainname);
                if (item != null)
                {
                    itemlistByGame.Add(item);

                }

            }
            return itemlistByGame;
        }
        /// <summary>
        /// 获取Items
        /// </summary>
        /// <param name="item">游戏Item</param>
        public List<channelItem> GetItems()
        {
            XmlNode root = GetRootNode();
            XmlNodeList list = root.ChildNodes;
            List<channelItem> itemlist = new List<channelItem>();

            foreach (XmlElement element in list)
            {

                channelItem item = getItemByElement(element);
              

                itemlist.Add(item);
            }
            return itemlist;
        }
        /// <summary>
        /// 读取渠道property文件返回 渠道对象 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public channelItem getItem(string cid)
        {
            string fileName = envConfig.channels + cid + @"\" + cid + "_info" + envConfig.configExt;
            GC.Collect();
            channelItem item = new channelItem();
            if (FileUtil.checkFile(fileName))
            {

                PpHelper helper = new PpHelper(fileName);
                item.name = helper.GetPropertiesText("name");
                item.cid = helper.GetPropertiesText("cid");
                item.package = helper.GetPropertiesText("package");
                item.version = helper.GetPropertiesText("version");
                item.foot = helper.GetPropertiesText("foot");
                item.des = helper.GetPropertiesText("des");
            //    item.apk = helper.GetPropertiesText("apk");
                item.isfoot = helper.GetPropertiesText("isfoot");
                item.isR = helper.GetPropertiesText("isR");
                item.sdkPackage = helper.GetPropertiesText("sdkPackage");
                item.isOwnKey = helper.GetPropertiesText("isOwnKey");
                item.flag = helper.GetPropertiesText("flag");
            }
            return item;
        }
        public bool checkApkBase(string id)
        {

            string path = envConfig.channels + id + @"\apkBase\";
            return Directory.Exists(path);

        }
        private channelItem item;
        private string from;
        private string to;
        private RichTextBox clog;
        private Button btnGetApk;
        private Button btnSave;

        /// <summary>
        /// 解压母包
        /// </summary>
        /// <param name="richTxtInfo"></param>
        /// <param name="from"></param>
        /// <param name="name"></param>
        public void deCompile(channelItem item,string from, RichTextBox clog, Button btnGetApk, Button btnSave = null)
        {
           //from = item.apk;
            if (from == null || from.Equals(""))
            {
                return;
            }
            if (!FileUtil.checkFile(from))
            {
                MessageBox.Show("不存在APK文件,母包更新失败");
                return;
            }
            this.item = item;
            this.from = from;
            this.to = envConfig.channels + item.cid + @"\apkBase\";
            this.clog = clog;
            this.btnGetApk = btnGetApk;
            this.btnSave = btnSave;

            if (btnSave != null) btnSave.Enabled = false;
            btnGetApk.Enabled = false;
            clog.Text = "";
            Thread t = new Thread(d);
            t.Start();

        }
     
        public void d()
        {
            bool ret=apkTool.d(clog, from, to);
            if (FileUtil.checkFile(to + @"\AndroidManifest.xml") &&ret)
            {
                // 重新获取资源，所需
                File.Copy(to + @"\AndroidManifest.xml", envConfig.channels + item.cid + @"\AndroidManifest.xml", true);
                item.sdkPackage = getPackName(to + @"\AndroidManifest.xml");
                // 更新  sdkPackage
                saveUpdate(item);
                btnGetApk.Enabled = true;
                if (btnSave != null) btnSave.Enabled = true;
                if (deCallBack != null)
                {
                    deCallBack(to, MSG_COMPLETE);
                }
                return;
            }
            else
            {
                if (deCallBack != null)
                {
                    deCallBack(to, MSG_ERROR);
                }

            }

          
        }

        private string getPackName(string xml)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(@xml);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"manifest");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute("package");
        }
        /// <summary>
        /// 添加渠道配置
        /// </summary>
        /// <param name="selItem">渠道</param>
        /// <param name="meatas">参数</param>

        public void addChannelMeta(channelItem selItem, Dictionary<string, string> meatas)
        {

            if (selItem == null || meatas.Count <= 0) return;

            XmlNode root = GetMetaRootNode();
            XmlNode xFind = metaXmlDoc.SelectSingleNode("Root/channel[@cid='" + selItem.cid + "']");
            //  if (xFind != null) delChannelMeta(channel);
            if (xFind != null)
                root.RemoveChild(xFind);
            //创建节点
            XmlElement cElement = metaXmlDoc.CreateElement("channel");
            setElement(cElement, selItem);

            foreach (KeyValuePair<string, string> meata in meatas)
            {
                string android_name = meata.Key;
                string android_value = meata.Value;

                XmlElement sub = metaXmlDoc.CreateElement("meta");
                sub.SetAttribute("name", android_name);
                sub.SetAttribute("value", android_value);
                cElement.AppendChild(sub);

            }
            root.AppendChild(cElement);
            metaXmlDoc.Save(envConfig.meta_datasXml);

        }

        public void delChannelMeta(string channel)
        {
            XmlNode root = GetMetaRootNode();
            XmlNode xFind = metaXmlDoc.SelectSingleNode("Root/channel[@cid='" + channel + "']");
            if (xFind == null) return;
            root.RemoveChild(xFind);
            metaXmlDoc.Save(envConfig.meta_datasXml);

        }

        public Dictionary<string, string> getChannelMetas(string channel)
        {

            XmlNode root = GetMetaRootNode();
            XmlNode xFind = metaXmlDoc.SelectSingleNode("Root/channel[@cid='" + channel + "']");
            if (xFind == null) return null;

            XmlNodeList list = xFind.ChildNodes;
            Dictionary<string, string> metas = new Dictionary<string, string>();
            foreach (XmlNode node in list)
            {
                XmlElement meta = (XmlElement)node;
                metas.Add(meta.GetAttribute("name"), meta.GetAttribute("value"));

            }

            return metas;

        }
        /// <summary>
        /// 获取已经录入的渠道配置
        /// </summary>
        /// <returns></returns>
        public List<channelItem> getChannelMetaed()
        {

            XmlNode root = GetMetaRootNode();
            XmlNodeList list = metaXmlDoc.GetElementsByTagName("channel");
            List<channelItem> channellist = new List<channelItem>();
            channelItem item = null;
            foreach (XmlNode node in list)
            {
                XmlElement element = (XmlElement)node;
                item = getItemByElement(element);
                channellist.Add(item);
            }

            return channellist;

        }

        


        //////////////////////////

        //////////////////////////

    }
}
