using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Data;

namespace sandGlass
{

    /**/
    /// <summary>
    /// XML 操作基类
    /// </summary>
    public class XmlHelper
    {
 

        public static bool checkXmlExist(string fileName)
        {
            bool flag = false;
            if (File.Exists(fileName))
                flag = true;
            return flag;
        }
        /**/
        /// <summary>
        /// 读取Xml到DataSet中

        /// </summary>
        /// <param name="XmlPath">路径</param>
        /// <returns>结果集</returns>
        public static DataSet GetXml(string XmlPath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(@XmlPath);
            return ds;
        }
        public static bool checkNode(string xmlPath, string node)
        {
            bool flag = false;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNode rs = xmlDoc.SelectSingleNode(@"//" + node);
            if (rs != null)
                flag = true;
            return flag;
        }

        /**/
        /// <summary>
        /// 读取xml文档并返回一个节点:适用于一级节点

        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="NodeName">节点</param>
        /// <returns></returns>
        public static string ReadXmlReturnNode(string XmlPath, string Node)
        {
            XmlDocument docXml = new XmlDocument();

            docXml.Load(@XmlPath);
            XmlNodeList xn = docXml.GetElementsByTagName(Node);
            return xn.Item(0).InnerText.ToString();
        }
        public static string getChannelName(string XmlPath)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(@XmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"channel");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute("name");
        }
        public static string getChannelAtrr(string XmlPath, string attr)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(@XmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(@"channel");
            XmlElement xe = (XmlElement)nodePath;
            return xe.GetAttribute(attr);
        }
        /**/
        /// <summary>
        /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中

        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
        /// <returns></returns>
        public static DataSet GetXmlData(string xmlPath, string XmlPathNode)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            return ds;
        }

        /**/
        /// <summary>
        /// 替换Xml节点内容
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="Node">要替换内容的节点:节点路径 根节点/父节点/当前节点</param>
        /// <param name="Content">新的内容</param>
        public static void XmlNodeReplace(string xmlPath, string Node, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            objXmlDoc.SelectSingleNode(Node).InnerText = Content;
            objXmlDoc.Save(xmlPath);

        }

        /**/
        /// <summary>
        /// 更改节点的属性值

        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeAttribute1">要更改的节点属性的名称</param>
        /// <param name="NodeAttributeText">更改的属性值</param>
        public static void XmlAttributeEdit(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);
        }

        /**/
        /// <summary>
        /// 删除XML节点和此节点下的子节点

        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="Node">节点路径</param>
        public static void XmlNodeDelete(string xmlPath, string Node)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
            objXmlDoc.Save(xmlPath);
        }

        /**/
        /// <summary>
        /// 删除一个节点的属性

        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径（xpath）</param>
        /// <param name="NodeAttribute">属性名称</param>
        public static void xmlnNodeAttributeDel(string xmlPath, string NodePath, string NodeAttribute)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.RemoveAttribute(NodeAttribute);
            objXmlDoc.Save(xmlPath);
        }

        /**/
        /// <summary>
        /// 插入一个节点和此节点的子节点

        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="MailNode">当前节点路径</param>
        /// <param name="ChildNode">新插入节点</param>
        /// <param name="Element">插入节点的子节点</param>
        /// <param name="Content">子节点的内容</param>
        public static void XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        /**/
        /// <summary>
        /// 向一个节点添加属性

        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeAttribute1">要添加的节点属性的名称</param>
        /// <param name="NodeAttributeText">要添加属性的值</param>
        public static void AddAttribute(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {

            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);

            XmlAttribute nodeAttribute = objXmlDoc.CreateAttribute(NodeAttribute1);

            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            nodePath.Attributes.Append(nodeAttribute);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);
        }
        /**/
        /// <summary>
        /// 插入多个属性

        /// </summary>
        public static void addMultiAttribute(string xmlPath, string NodePath, Dictionary<string, string> attr, string innerTxt = null, string targetXml = null)
        {

            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);

            //    if (nodePath==null )
            //    {
            //        MessageBox.Show(xmlPath + "\r\n" + NodePath);
            //    XmlElement objElement = objXmlDoc.CreateElement(NodePath);
            // nodePath.AppendChild(objElement);
            //   }
            foreach (KeyValuePair<string, string> dic in attr)
            {
                XmlAttribute nodeAttribute = objXmlDoc.CreateAttribute(dic.Key);
                nodePath.Attributes.Append(nodeAttribute);
                XmlElement xe = (XmlElement)nodePath;
                xe.SetAttribute(dic.Key, dic.Value);
                if (innerTxt != null)
                    xe.InnerText = innerTxt;
            }

            if (targetXml != null)
            {
                MessageBox.Show(targetXml);
                objXmlDoc.Save(targetXml);
            }
            else
                objXmlDoc.Save(xmlPath);
        }

        /**/
        /// <summary>
        /// 插入一节点,带一属性

        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Attrib">属性名称</param>
        /// <param name="AttribContent">属性值</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content = null, string targetXml = null)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            if (targetXml != null)
                objXmlDoc.Save(targetXml);
            else
                objXmlDoc.Save(xmlPath);
        }
        /**/
        /// <summary>
        /// 插入一节点,带多个属性

        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="attr">属性名称</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertMultiElement(string xmlPath, string MainNode, string Element, Dictionary<string, string> attr, string Content = null, string targetXml = null)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string[] mains = MainNode.Split(new char[] { '/' });
            string pathTmp = null;
            string curNode = null;
            bool reloadXml = false;
            foreach (string s in mains)
            {
                curNode = s;
                if (pathTmp == null)
                {
                    pathTmp += s;
                    continue;
                }
                pathTmp += "/" + s;
                string ss = pathTmp;
                XmlNode nodePathTmp = objXmlDoc.SelectSingleNode(@"//" + pathTmp);
                if (nodePathTmp == null)
                {
                    string cNode = pathTmp.Replace("/" + curNode, "");
                    XmlInsertElement(xmlPath, cNode, curNode, null);
                    reloadXml = true;
                }

            }
            if (reloadXml == true)
                objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement sNode = objXmlDoc.CreateElement(Element);
            nodePath.AppendChild(sNode);

            foreach (KeyValuePair<string, string> dic in attr)
            {
                XmlElement xe = (XmlElement)sNode;
                xe.SetAttribute(dic.Key, dic.Value);
                if (Content != null)
                {
                    xe.InnerText = Content;
                }
            }
            if (targetXml != null)
                objXmlDoc.Save(targetXml);
            else
                objXmlDoc.Save(xmlPath);
        }
        public static void XmlInsertMultiElementForSpec(string xmlPath, string MainNode, string Element, Dictionary<string, string> attr, string childNode = null, Dictionary<string, string> childAttr = null, string childNode1 = null, Dictionary<string, string> childAttr1 = null, string child1Content = null)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string[] mains = MainNode.Split(new char[] { '/' });
            string pathTmp = null;
            string curNode = null;
            bool reloadXml = false;
            foreach (string s in mains)
            {
                curNode = s;
                if (pathTmp == null)
                {
                    pathTmp += s;
                    continue;
                }
                pathTmp += "/" + s;
                string ss = pathTmp;
                XmlNode nodePathTmp = objXmlDoc.SelectSingleNode(@"//" + pathTmp);
                if (nodePathTmp == null)
                {
                    string cNode = pathTmp.Replace("/" + curNode, "");
                    XmlInsertElement(xmlPath, cNode, curNode, null);
                    reloadXml = true;
                }

            }
            if (reloadXml == true)
                objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement sNode = objXmlDoc.CreateElement(Element);
            nodePath.AppendChild(sNode);

            foreach (KeyValuePair<string, string> dic in attr)
            {
                XmlElement xe = (XmlElement)sNode;
                xe.SetAttribute(dic.Key, dic.Value);
            }
            if (childNode != null)
            {
                XmlElement xe1 = objXmlDoc.CreateElement(childNode);
                foreach (KeyValuePair<string, string> dic in childAttr)
                {
                    XmlElement xe = (XmlElement)xe1;
                    xe.SetAttribute(dic.Key, dic.Value);
                }
                if (childNode1 != null)
                {

                    XmlElement c = objXmlDoc.CreateElement(childNode1);
                    foreach (KeyValuePair<string, string> dic in childAttr1)
                    {
                        XmlElement xe = (XmlElement)c;
                        xe.SetAttribute(dic.Key, dic.Value);
                    }
                    if (child1Content != null)
                        c.InnerText = child1Content;
                    xe1.AppendChild(c);
                }
                sNode.AppendChild(xe1);

            }
            objXmlDoc.Save(xmlPath);
        }

        public static void XmlInsertMultiElementForSpec1(string xmlPath, string MainNode, string Element, Dictionary<string, string> attr, string childNode = null, Dictionary<string, Dictionary<string, string>> childAttr = null)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string[] mains = MainNode.Split(new char[] { '/' });
            string pathTmp = null;
            string curNode = null;
            bool reloadXml = false;
            foreach (string s in mains)
            {
                curNode = s;
                if (pathTmp == null)
                {
                    pathTmp += s;
                    continue;
                }
                pathTmp += "/" + s;
                string ss = pathTmp;
                XmlNode nodePathTmp = objXmlDoc.SelectSingleNode(@"//" + pathTmp);
                if (nodePathTmp == null)
                {
                    string cNode = pathTmp.Replace("/" + curNode, "");
                    XmlInsertElement(xmlPath, cNode, curNode, null);
                    reloadXml = true;
                }

            }
            if (reloadXml == true)
                objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement sNode = objXmlDoc.CreateElement(Element);
            nodePath.AppendChild(sNode);

            foreach (KeyValuePair<string, string> dic in attr)
            {
                XmlElement xe = (XmlElement)sNode;
                xe.SetAttribute(dic.Key, dic.Value);
            }
            if (childNode != null)
            {
                XmlElement xe1 = objXmlDoc.CreateElement(childNode);
                foreach (KeyValuePair<string, Dictionary<string, string>> dic in childAttr)
                {
                    XmlElement c = objXmlDoc.CreateElement(dic.Key);
                    foreach (KeyValuePair<string, string> childs in dic.Value)
                    {
                        XmlElement xe = (XmlElement)c;
                        xe.SetAttribute(childs.Key, childs.Value);
                    }
                    xe1.AppendChild(c);
                }
                sNode.AppendChild(xe1);
            }
            objXmlDoc.Save(xmlPath);
        }
        /**/

        /// List 可以重复插入节点值 ，不受K-V 约束

        public static void XmlInsertMultiElementForSpec2(string xmlPath, string MainNode, string Element, Dictionary<string, string> attr, string childNode = null, List<Dictionary<string, Dictionary<string, string>>> childAttr = null)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string[] mains = MainNode.Split(new char[] { '/' });
            string pathTmp = null;
            string curNode = null;
            bool reloadXml = false;
            foreach (string s in mains)
            {
                curNode = s;
                if (pathTmp == null)
                {
                    pathTmp += s;
                    continue;
                }
                pathTmp += "/" + s;
                string ss = pathTmp;
                XmlNode nodePathTmp = objXmlDoc.SelectSingleNode(@"//" + pathTmp);
                if (nodePathTmp == null)
                {
                    string cNode = pathTmp.Replace("/" + curNode, "");
                    XmlInsertElement(xmlPath, cNode, curNode, null);
                    reloadXml = true;
                }

            }
            if (reloadXml == true)
                objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement sNode = objXmlDoc.CreateElement(Element);
            nodePath.AppendChild(sNode);

            foreach (KeyValuePair<string, string> dic in attr)
            {
                XmlElement xe = (XmlElement)sNode;
                xe.SetAttribute(dic.Key, dic.Value);
            }
            if (childNode != null)
            {
                XmlElement xe1 = objXmlDoc.CreateElement(childNode);


                foreach (Dictionary<string, Dictionary<string, string>> attr1 in childAttr)
                {

                    foreach (KeyValuePair<string, Dictionary<string, string>> dic in attr1)
                    {
                        XmlElement c = objXmlDoc.CreateElement(dic.Key);
                        foreach (KeyValuePair<string, string> childs in dic.Value)
                        {
                            XmlElement xe = (XmlElement)c;
                            xe.SetAttribute(childs.Key, childs.Value);
                        }
                        xe1.AppendChild(c);
                    }
                }
                sNode.AppendChild(xe1);
            }
            objXmlDoc.Save(xmlPath);
        }


        /**/
        /// <summary>
        /// 插入一节点,不带属性

        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        public static void XmlInsertGame(string xmlPath, string MainNode, string Element, string Content, Dictionary<string, string> attr)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            foreach (KeyValuePair<string, string> dic in attr)
            {
                objElement.SetAttribute(dic.Key, dic.Value);
            }
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        /**/
        /// <summary>
        /// 在根节点下添加父节点
        /// </summary>
        public static void AddParentNode(string xmlPath, string parentNode)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlPath);
            // 创建一个新的menber节点并将它添加到根节点下
            XmlElement Node = xdoc.CreateElement(parentNode);
            xdoc.DocumentElement.PrependChild(Node);
            xdoc.Save(xmlPath);
        }

        /**/
        /// <summary>
        /// 根据节点属性读取子节点值(较省资源模式)
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="FatherElement">父节点值</param>
        /// <param name="AttributeName">属性名称</param>
        /// <param name="AttributeValue">属性值</param>
        /// <param name="ArrayLength">返回的数组长度</param>
        /// <returns></returns>
        public static System.Collections.ArrayList GetSubElementByAttribute(string XmlPath, string FatherElement, string AttributeName, string AttributeValue, int ArrayLength)
        {
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            XmlDocument docXml = new XmlDocument();
            docXml.Load(@XmlPath);
            XmlNodeList xn;
            xn = docXml.DocumentElement.SelectNodes("//" + FatherElement + "[" + @AttributeName + "='" + AttributeValue + "']");
            XmlNodeList xx = xn.Item(0).ChildNodes;
            for (int i = 0; i < ArrayLength & i < xx.Count; i++)
            {

                al.Add(xx.Item(i).InnerText);
            }

            return al;
        }
        public static void createXml(string xmlPath, string rootNode)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement(rootNode);
            doc.AppendChild(root);
            doc.Save(@xmlPath);
            Console.Write(doc.OuterXml);
        }
        public static bool mergeXml(string path, string baseXml, string targetXml)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlDocument baseDoc = new XmlDocument();
                baseDoc.Load(baseXml);
                foreach (XmlNode item in xmlDoc.DocumentElement)
                {
                    try
                    {
                        XmlElement xe = (XmlElement)item;
                        string method = xe.GetAttribute("mergeMethod");
                        string xPath = xe.GetAttribute("mergePath");
                        if (method == "insert")
                        {
                            //  MessageBox.Show("//" + xPath);
                            baseDoc.DocumentElement.SelectSingleNode("//" + xPath).AppendChild(baseDoc.ImportNode(item, true));
                        }
                        else
                        {
                            Dictionary<string, string> attr = new Dictionary<string, string>();
                            for (int i = 0; i < item.Attributes.Count; i++)
                            {
                                attr.Add(xe.Attributes[i].Name, xe.Attributes[i].Value);
                            }
                            addMultiAttribute(baseXml, xPath, attr, null, null);
                            baseDoc.Load(baseXml);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message); 
                        return false;
                         
                    }

                }
                baseDoc.Save(baseXml);
            }
            catch (XmlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true ;
        }

        public static void mergeResXml(string targetXml, string baseXml, string finalXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(targetXml);
            XmlDocument baseDoc = new XmlDocument();
            baseDoc.Load(baseXml);
            foreach (XmlNode item in xmlDoc.DocumentElement)
            {
                //               XmlNode basePath = baseDoc.SelectSingleNode(@"//resources/" + item.Name);
                //               XmlElement baseXe = (XmlElement)basePath;
                XmlNodeList xList = baseDoc.SelectNodes(@"//resources/" + item.Name);
                List<string> baseAtrr = new List<string>();
                foreach (XmlNode list in xList)
                {
                    XmlElement tt = (XmlElement)list;
                    baseAtrr.Add(tt.GetAttribute("name"));
                }
                XmlElement xe = (XmlElement)item;
                string s = xe.GetAttribute("name");
                bool exists = baseAtrr.Contains(xe.GetAttribute("name"));
                if (!exists)
                {
                    baseDoc.DocumentElement.SelectSingleNode("//resources").AppendChild(baseDoc.ImportNode(item, true));
                }
            }
            // string s= item.ParentNode.Name.ToString();
            baseDoc.Save(finalXml);
        }
        public static Int64 getMaxId(XmlNodeList gameList)
        {
            Int64 maxId = 0;
            // Dictionary<string, Int64> maxManager = new Dictionary<string, Int64>();
            //   XmlDocument gameDoc = new XmlDocument();
            //    gameDoc.Load(gameXml);
            //    XmlNodeList gameList = gameDoc.SelectNodes(@"//resources/public");
            foreach (XmlNode list in gameList)
            {
                XmlElement gAttr = (XmlElement)list;
                string id = gAttr.GetAttribute("id");
                Int64 tempId = Convert.ToInt64(id, 16);
                if (tempId > maxId)
                {
                    maxId = tempId;
                }
            }
            return maxId;

        }
        public static void mergePublicXml(string sdkXml, string gameXml, string finalXml)
        {
            XmlDocument sdkDoc = new XmlDocument();
            sdkDoc.Load(sdkXml);
            XmlDocument gameDoc = new XmlDocument();
            gameDoc.Load(gameXml);
            string xmlType = "type";
            Dictionary<string, Int64> maxManager = new Dictionary<string, Int64>();
            Int64 max = 0;
            List<string> tmpTypes = new List<string>();
            XmlNodeList xnodes = gameDoc.SelectNodes(@"//resources/public");
            foreach (XmlNode item in sdkDoc.DocumentElement)
            {
                //  MessageBox.Show(item.Name);
                XmlNodeList xList = gameDoc.SelectNodes(@"//resources/" + item.Name);
                List<string> baseAtrr = new List<string>();
                foreach (XmlNode list in xList)
                {
                    XmlElement tt = (XmlElement)list;
                    baseAtrr.Add(tt.GetAttribute("name"));
                }
                XmlElement xe = (XmlElement)item;
                bool exists = baseAtrr.Contains(xe.GetAttribute("name"));
                if (!exists)
                {
                    Int64 tempId = 0;
                    string maxIdName = "";
                    xmlType = xe.GetAttribute("type");
                    if (!maxManager.Keys.Contains<string>(xmlType))
                    {
                        maxManager[xmlType] = 0;
                    }

                    XmlNodeList gameList = gameDoc.SelectNodes(@"//resources/public[@type='" + xmlType + "']");
                    foreach (XmlNode list in gameList)
                    {
                        XmlElement gAttr = (XmlElement)list;
                        string id = gAttr.GetAttribute("id");
                        tempId = Convert.ToInt64(id, 16);
                        if (tempId > maxManager[xmlType])
                        {
                            maxManager[xmlType] = tempId;
                        }
                        maxIdName = gAttr.GetAttribute("name");
                    }
                    XmlNode lastNode = gameDoc.SelectSingleNode(@"//resources/public[@type='" + xmlType + "'] [@name='" + maxIdName + "']");
                    string maxStr = null;
                    if (maxManager[xmlType] != 0)
                    {
                        maxManager[xmlType] += 1;
                        maxStr = "0x" + string.Format("{0:X}", maxManager[xmlType]).ToLower();

                    }
                    else
                    {
                        if (max == 0)
                        {
                            max = getMaxId(xnodes);
                        }
                        tempId = max;
                        tempId += 65536;
                        string tmpStr = "0x" + string.Format("{0:X}", tempId).ToLower();
                        tmpStr = tmpStr.Substring(0, 6) + "0000";

                        tempId = Convert.ToInt64(tmpStr, 16);
                        maxManager[xmlType] = tempId;
                        maxStr = "0x" + string.Format("{0:X}", maxManager[xmlType]).ToLower();

                        max = tempId;
                    }
                    xe.SetAttribute("id", maxStr);
                    gameDoc.DocumentElement.SelectSingleNode("//resources").AppendChild(gameDoc.ImportNode(item, true));
                }
            }
            gameDoc.Save(finalXml);
        }
        public static void mergePublicXmlNew(string sdkXml, string gameXml, string finalXml)
        {
            XmlDocument sdkDoc = new XmlDocument();
            sdkDoc.Load(sdkXml);
            XmlDocument gameDoc = new XmlDocument();
            gameDoc.Load(gameXml);
            Dictionary<string, Int64> maxManager = new Dictionary<string, Int64>();

            List<string> tmpTypes = new List<string>();
            XmlNodeList xnodes = gameDoc.SelectNodes(@"//resources/public");
            List<string> allAtrr = new List<string>();
            List<string> allDetail = new List<string>();
            List<string> allDetailLast = new List<string>();
            List<string> androidAttr = retunAttrs();
            List<string> finalAttr = new List<string>();
            foreach (XmlNode item in gameDoc.DocumentElement)
            {

                XmlNodeList xList = gameDoc.SelectNodes(@"//resources/" + item.Name);
                foreach (XmlNode list in xList)
                {
                    XmlElement tt = (XmlElement)list;
                    string tmpType = tt.GetAttribute("type");
                    string tmpName = tt.GetAttribute("name");
                    allAtrr.Add(tmpType);
                    allDetail.Add(tmpType + "%" + tmpName);
                }
            }
            foreach (XmlNode item in sdkDoc.DocumentElement)
            {

                XmlNodeList xList = sdkDoc.SelectNodes(@"//resources/" + item.Name);
                foreach (XmlNode list in xList)
                {
                    XmlElement tt = (XmlElement)list;
                    string tmpType = tt.GetAttribute("type");
                    string tmpName = tt.GetAttribute("name");
                    bool exists = allAtrr.Contains(tmpType);
                    if (!exists)
                    {
                        allAtrr.Add(tmpType);
                    }
                    allDetail.Add(tmpType + "%" + tmpName);
                }
            }
            allAtrr = allAtrr.Distinct<string>().ToList();
            allAtrr.Sort();
            allDetail = allDetail.Distinct<string>().ToList();
            //    allDetail.Sort();
            //sort by android asc
            List<string> allDetailLastTmp = new List<string>();

            foreach (string a in allDetail)
            {
                string[] attrArr = a.Split(new char[] { '%' });
                foreach (string andAttr in androidAttr)
                {
                    if (allDetail.Contains<string>(andAttr + "%" + attrArr[1]))
                    {
                        allDetailLast.Add(a);
                    }
                    else
                    {
                        allDetailLastTmp.Add(a);
                    }
                    break;
                }
            }


            foreach (string a in allDetailLastTmp)
            {
                allDetailLast.Add(a);
            }
            //  allDetailLast = allDetailLast.Distinct<string>().ToList();
            //  allDetail =  allDetailTmp;
            //sort by android asc
            //   allDetail.Sort();
            Int64 initId = Convert.ToInt64("0x7f020000", 16);
            if (allAtrr.Contains<string>("attr"))
                initId = Convert.ToInt64("0x7f010000", 16);

            foreach (string andAttr in androidAttr)
            {
                if (allAtrr.Contains<string>(andAttr))
                {
                    finalAttr.Add(andAttr);
                }
            }
            foreach (string a in allAtrr)
            {
                if (!finalAttr.Contains<string>(a))
                {
                    finalAttr.Add(a);
                }
            }
            foreach (string a in finalAttr)
            {
                if (!maxManager.Keys.Contains<string>(a))
                {
                    maxManager[a] = initId;
                    initId += 65536;
                }
                //    s += a.ToString()+"\r\n";
            }

            //   MessageBox.Show("所有属性:\r\n" + s);
            //          s = null;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration head = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(head);
            XmlElement root = doc.CreateElement("resources");
            doc.AppendChild(root);
            foreach (string a in allDetailLast)
            {
                // MessageBox.Show(a);
                string[] attrArr = a.Split(new char[] { '%' });
                XmlElement node = doc.CreateElement("public");
                Int64 tempId = maxManager[attrArr[0]];
                tempId = maxManager[attrArr[0]];
                node.SetAttribute("type", attrArr[0]);
                node.SetAttribute("name", attrArr[1]);
                node.SetAttribute("id", "0x" + string.Format("{0:X}", tempId).ToLower());
                maxManager[attrArr[0]] = tempId + 1;

                root.AppendChild(node);
                //      s += a.ToString() + "\r\n";
            }
            doc.Save(finalXml);
            return;
        }
        public static void mergePublicXml2(string sdkXml, string gameXml, string finalXml)
        {
            XmlDocument sdkDoc = new XmlDocument();
            sdkDoc.Load(sdkXml);
            XmlDocument gameDoc = new XmlDocument();
            gameDoc.Load(gameXml);
            string xmlType = "type";
            Dictionary<string, Int64> maxManager = new Dictionary<string, Int64>();
            Int64 max = 0;
            Int64 atrId = 0;
            List<string> tmpTypes = new List<string>();
            XmlNodeList xnodes = gameDoc.SelectNodes(@"//resources/public");
            foreach (XmlNode item in sdkDoc.DocumentElement)
            {
                XmlNodeList xList = gameDoc.SelectNodes(@"//resources/" + item.Name);
                List<string> baseAtrr = new List<string>();
                foreach (XmlNode list in xList)
                {
                    XmlElement tt = (XmlElement)list;
                    baseAtrr.Add(tt.GetAttribute("name") + "_" + tt.GetAttribute("type"));
                    if (!tmpTypes.Contains(tt.GetAttribute("type")))
                        tmpTypes.Add(tt.GetAttribute("type"));
                    if (tt.GetAttribute("name") == "attr")
                        atrId = 2130771968;
                }
                XmlElement xe = (XmlElement)item;
                bool exists = baseAtrr.Contains(xe.GetAttribute("name") + "_" + xe.GetAttribute("type"));
                if (!exists)
                {
                    Int64 tempId = 0;
                    string maxIdName = "";
                    xmlType = xe.GetAttribute("type");
                    if (!maxManager.Keys.Contains<string>(xmlType))
                    {
                        maxManager[xmlType] = 0;
                    }

                    XmlNodeList gameList = gameDoc.SelectNodes(@"//resources/public[@type='" + xmlType + "']");
                    foreach (XmlNode list in gameList)
                    {
                        XmlElement gAttr = (XmlElement)list;
                        string id = gAttr.GetAttribute("id");
                        tempId = Convert.ToInt64(id, 16);
                        if (tempId > maxManager[xmlType])
                        {
                            maxManager[xmlType] = tempId;
                        }
                        maxIdName = gAttr.GetAttribute("name");
                    }
                    XmlNode lastNode = gameDoc.SelectSingleNode(@"//resources/public[@type='" + xmlType + "'] [@name='" + maxIdName + "']");
                    string maxStr = null;
                    bool checkAttr = tmpTypes.Contains(xe.GetAttribute("type"));
                    if (xmlType == "attr" && !checkAttr)
                    {
                        if (atrId == 0)
                            atrId = 2130771968;   //0x7f010000
                        //  max = tempId - 65536;
                        maxStr = "0x" + string.Format("{0:X}", atrId).ToLower();
                        atrId += 1;
                    }
                    else
                    {

                        if (maxManager[xmlType] != 0)
                        {
                            maxManager[xmlType] += 1;
                            maxStr = "0x" + string.Format("{0:X}", maxManager[xmlType]).ToLower();

                        }
                        else
                        {
                            if (max == 0)
                            {
                                max = getMaxId(xnodes);
                            }
                            tempId = max;
                            tempId += 65536;

                            string tmpStr = "0x" + string.Format("{0:X}", tempId).ToLower();
                            tmpStr = tmpStr.Substring(0, 6) + "0000";

                            tempId = Convert.ToInt64(tmpStr, 16);
                            maxManager[xmlType] = tempId;
                            maxStr = "0x" + string.Format("{0:X}", maxManager[xmlType]).ToLower();

                            max = tempId;
                        }
                    }
                    xe.SetAttribute("id", maxStr);
                    gameDoc.DocumentElement.SelectSingleNode("//resources").AppendChild(gameDoc.ImportNode(item, true));
                }
            }
            gameDoc.Save(finalXml);
        }
        public static List<string> retunAttrs()
        {
            List<string> baseAtrr = new List<string>();
            baseAtrr.Add("attr");
            baseAtrr.Add("drawable");
            baseAtrr.Add("mipmap");
            baseAtrr.Add("layout");
            baseAtrr.Add("anim");
            baseAtrr.Add("xml");
            baseAtrr.Add("raw");
            baseAtrr.Add("array");
            baseAtrr.Add("id");
            baseAtrr.Add("color");
            baseAtrr.Add("bool");
            baseAtrr.Add("string");
            baseAtrr.Add("integer");
            baseAtrr.Add("dimen");
            baseAtrr.Add("style");
            baseAtrr.Add("plurals");
            baseAtrr.Add("menu");
            return baseAtrr;
        }
    }
}
