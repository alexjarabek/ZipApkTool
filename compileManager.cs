using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;

namespace sandGlass
{
    /// <summary>
    /// APK合并
    /// </summary>
    class compileManager
    {
        private static XmlDocument xmlDoc = new XmlDocument();
        private string file = envConfig.configXml;

        public static string ApkVersion1 = "2_0_0";
        public static string ApkVersion2 = "2_0_0_RC4";
        // 工具版本
        public static string Version1 = "2_0_0";
        public static string Version2 = "2_0_1";
        // SDK版本
        //public static string SDK_Version1 = "xqsmy";// 新秦
        //public static string SDK_Version2 = "1_4_1";// 更新
       

        public string getConfig(string key)
        {
            return getValue(key);
        }

        public string getApktool()
        {
            return getValue("apktool");
        }
        public string getVersion()
        {
            return getValue("version");
        }
        public string getDebug()
        {
            return getValue("debug");
        }
        /// <summary>
        /// 设置debug
        /// </summary>
        /// <param name="value">true false</param>
        public void setDebug(string value)
        {
            configItem item = new configItem();
            item.name = "debug";
            item.value = value;
            saveUpdateToXml(item);
            envConfig.debug = value;
        }
        /// <summary>
        /// 设置version
        /// </summary>
        /// <param name="value"></param>
        public void setVersion(string value)
        {
            configItem item = new configItem();
            item.name = "version";
            item.value = value;
            saveUpdateToXml(item);
            envConfig.version = value;

        }
        /// <summary>
        /// 设置sdkVersion
        /// </summary>
        /// <param name="value"></param>
        public void setSdk(string value)
        {
            configItem item = new configItem();
            item.name = "sdk";
            item.value = value;
            saveUpdateToXml(item);
        }
       
        public string getSdk()
        {
            return getValue("sdk");
        } 

        /// <summary>
        /// 设置value
        /// </summary>
        /// <param name="value"></param>
        public void setValue(string key, string value)
        {
            configItem item = new configItem();
            item.name = key;
            item.value = value;
            saveUpdateToXml(item);
        }
        public void setSelectGamePath(string value)
        {
            configItem item = new configItem();
            item.name = "gameSelectFolder";
            item.value = value;
            saveUpdateToXml(item);
            envConfig.gameSelectFolder = value;
        }
        public void setSelectChannelPath(string value)
        {
            configItem item = new configItem();
            item.name = "channelSelectFolder";
            item.value = value;
            saveUpdateToXml(item);
            envConfig.channelSelectFolder = value;
        }
        public void setApktool(string value)
        {
            configItem item = new configItem();
            item.name = "apktool";
            item.value = value;
            saveUpdateToXml(item);
            envConfig.apkVersion = value;
            envConfig.apkTool = envConfig.toolPath + @"\apktool" + @"_" + value + ".bat";
            envConfig.apkTool_debug = envConfig.toolPath + @"\apktool" + @"_" + value + "_debug.bat"; 
        }
        /// <summary>
        /// 获取图片 相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Image getImageByCurrentPath(string path)
        {
            if (!FileUtil.checkFile(envConfig.currenPath + path))
            {
                return null;
            }
            Image img = System.Drawing.Image.FromFile(envConfig.currenPath + path);
            Image bmp = new System.Drawing.Bitmap(img);
            img.Dispose();
            return bmp;
        }

        /// <summary>
        /// 获取图片 全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Image getImage(string path)
        {
            if (!FileUtil.checkFile(path))
            {
                return null;
            }
            Image img = System.Drawing.Image.FromFile(path);
            Image bmp = new System.Drawing.Bitmap(img);
            img.Dispose();
            return bmp;
        }

        /// <summary>
        /// 返回配置.xml文档的根节点
        /// </summary>
        /// <returns>根节点</returns>
        public XmlNode GetRootNode()
        {

            if (!FileUtil.checkFile(file))
            {
                CreateXmlConfigFile(file);
            }
            xmlDoc.Load(file);
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
        public class configItem
        {
            public string name;
            public string value;

        }

        /// <summary>
        /// 添加配置节点 index 0 递增
        /// </summary>
        /// <param name="item">游戏Item</param>
        public void AddXmlNode(configItem item)
        {
            if (item == null) return;
            XmlNode root = GetRootNode();
            //创建节点
            XmlElement element = xmlDoc.CreateElement("Item");
            //添加属性
            element.SetAttribute("name", item.name);
            element.SetAttribute("value", item.value);
            XmlNode xml = root.AppendChild(element);
            xmlDoc.Save(file); ;

        }
        /// <summary>
        /// 新增更新列表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="item"></param>
        public void saveUpdateToXml(configItem item)
        {
            if (item == null) return;
            XmlNode root = GetRootNode();

            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@name='" + item.name + "']");
            if (xFind == null)
            {
                //新增
                AddXmlNode(item);

            }
            else
            {
                UpdateXmlNode(item);

            }

        }
        /// <summary>
        /// 修改item节点
        /// </summary>
        /// <param name="item">配置Item</param>
        public void UpdateXmlNode(configItem item)
        {
            if (item == null) return;
            XmlNode root = GetRootNode();
            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@name='" + item.name + "']");
            if (xFind == null) return;
            XmlElement element = (XmlElement)xFind;
            //添加属性
            element.SetAttribute("name", item.name);
            element.SetAttribute("value", item.value);
            xmlDoc.Save(file); ;
        }
        public string getValue(string name)
        {
            XmlNode root = GetRootNode();
            XmlNode xFind = xmlDoc.SelectSingleNode("Root/Item[@name='" + name + "']");
            if (xFind == null) return "";
            XmlElement element = (XmlElement)xFind;
            return element.GetAttribute("value");

        }


        public  void updateChannelXml(string game,string channel,string gameXml,string properties ){

            Dictionary<string, string> meatas=new channelManager().getChannelMetas(channel);
            if (meatas == null) return;

            PpHelper pt = new PpHelper(properties); 
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            //  MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            //遍历添加meta-data
            foreach (KeyValuePair<string, string> pair in meatas)
            {
                addMeataData(pt, xmlTmpFile, pair.Key, pair.Value);
            }

            pt.Close();
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        } 
        private void addMeataData(PpHelper pt, string xmlTmpFile, string name, string valueKey)
        {
            string value = pt.GetPropertiesText(valueKey);
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("android" + FileUtil.colon + "name", name);
            meatas.Add("android" + FileUtil.colon + "value", value);
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", meatas);
        }


    }
}
