using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace sandGlass
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void updateChannelXml(string game, string channel, string gameXml, string perpties)
        {
            SpecXml specXml = new SpecXml();
            specXml.game = game;
            specXml.channel = channel;
            specXml.gameXml = gameXml;
            specXml.properties = perpties;
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
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SpecXml spXml = new SpecXml();
            string game = "yiwan";
            if (spXml.needMergePublic.Contains<string>(game))
            {
                MessageBox.Show("new func");
            }
            else
            {
                MessageBox.Show("old");
            }
            return;
            XmlHelper.mergePublicXmlNew(@"H:\sandGlass-new\sandGlass-new\sandGlass\sandGlass\bin\Debug\channels\yiwan\apkBase\res\values\public.xml", @"H:\sandGlass-new\sandGlass-new\sandGlass\sandGlass\bin\Debug\deCompile\hjtkfb\res\values\public.xml", @"h:\test.xml");
            MessageBox.Show("hello");
            return;
            string str = "0x7f03004f";
            str = textBox1.Text.ToString();
            Int64 ii = Convert.ToInt64(str, 16);
            Int64 addI = ii + 65536;

            Int64  pre = ii - 65536;
            string strs = string.Format("{0:X}", addI);
            string preStr = string.Format("{0:X}", pre);

            richTextBox1.Text += str + ":" + ii.ToString() +"pre:("+pre.ToString()+":"+preStr+") \t "+ "next:(" + addI.ToString() + ":"+strs+ ")\r\n";
           // richTextBox1.Text += ii.ToString() + ": " + strs + "\r\n";
            return;
            try
            {
                updateChannelXml("hjtkfb", "i360", @"H:\sandGlass-new\sandGlass-new\sandGlass\sandGlass\bin\Debug\compile\hjtkfb\yy\AndroidManifest.xml", @"H:\sandGlass-new\sandGlass-new\sandGlass\sandGlass\bin\Debug\compile\hjtkfb\yy\assets\hjtkfb.properties");
            }catch(Exception EE)
            {
             //   MessageBox.Show(EE.ToString());
            }
            return;
/*
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            string game = "ttpkq";
            psi.Arguments = "/e,/select, " + envConfig.releasePkg +game;
            System.Diagnostics.Process.Start(psi);
            return;
          //  MessageBox.Show((32 / 4).ToString());
            string sourcePic=@"C:\UC\111.png";
            string wPic = @"C:\UC\w.png";
            string targetPic = @"C:\UC\222.png";
            ImageCut.DrawImage(sourcePic, wPic, 0.5f, ImageCut.ImagePosition.LeftBottom, targetPic);
            return;
*/
          
            string sdkXml = @"F:\客户端\sandGlass\sandGlass\bin\\Debug\channels\uc\apkBase\res\values\public.xml";
            string gameXml = @"F:\客户端\sandGlass\sandGlass\bin\Debug\deCompile\ttpkq\res\values\public.xml";
            string s = @"c:\22222222222222222222.xml";
              XmlDocument sdkDoc = new XmlDocument();
              sdkDoc.Load(sdkXml);
            XmlDocument gameDoc = new XmlDocument();
            gameDoc.Load(gameXml);
            string xmlType = "type";
            Dictionary<string, Int64> maxManager = new Dictionary<string, Int64>();
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
                    if( ! maxManager.Keys.Contains<string>(xmlType))
                    {
                      //  MessageBox.Show(xmlType.ToString()+"不存在");
                        maxManager[xmlType] = 0;
                    }
                   
                    XmlNodeList gameList = gameDoc.SelectNodes(@"//resources/public[@type='" + xmlType + "']");
                    foreach (XmlNode list in gameList)
                    {
                        //list.LastChild.InsertAfter
                        XmlElement gAttr = (XmlElement)list;
                        richTextBox1.Text += "type=" + gAttr.GetAttribute("type") + "\t\t";
                        richTextBox1.Text += "name=" + gAttr.GetAttribute("name") + "\t\t";
                        string id = gAttr.GetAttribute("id");
                        tempId = Convert.ToInt64(id, 16);
                        richTextBox1.Text += "id=" + id + ",转换后：id=" + tempId + "\r\n";
                        if (tempId > maxManager[xmlType])
                        {
                                maxManager[xmlType] = tempId;                           
                        }
                        maxIdName = gAttr.GetAttribute("name");
                    }
                    XmlNode lastNode = gameDoc.SelectSingleNode(@"//resources/public[@type='" + xmlType + "'] [@name='" + maxIdName + "']");
                  //  lastNode.NextSibling = item;                
                    maxManager[xmlType] += 1;
                    MessageBox.Show(maxManager[xmlType].ToString());
                    string maxStr = "0x" + string.Format("{0:X}", maxManager[xmlType]).ToLower();
                    xe.SetAttribute("id", maxStr);
                  //  lastNode.AppendChild(gameDoc.ImportNode(item,true));
                  //  lastNode.AppendChild(item);
                    richTextBox1.Text += "\r\n\r\n";
                    richTextBox1.Text += "插入行 "+item.OuterXml.ToString() +"\r\n\r\n";
                    gameDoc.DocumentElement.SelectSingleNode("//resources").AppendChild(gameDoc.ImportNode(item, true));
                }
            }
            gameDoc.Save(@"c:\ffffffffffff.xml");
            /*
            XmlNodeList gameList = gameDoc.SelectNodes(@"//resources/public[@type='" + xmlType + "']");
            foreach (XmlNode list in gameList)
            {
                //list.LastChild.InsertAfter
                XmlElement gAttr = (XmlElement)list;
                richTextBox1.Text += "type=" + gAttr.GetAttribute("type") + "\t\t";
                richTextBox1.Text += "name=" + gAttr.GetAttribute("name") + "\t\t";
                string id = gAttr.GetAttribute("id");
                Int64 tempId = Convert.ToInt64(id, 16);
                richTextBox1.Text += "id=" + id + ",转换后：id=" + tempId + "\r\n";
                if (tempId > maxId)
                {
                    maxId = tempId;
                    maxIdName = gAttr.GetAttribute("name");
                }
            }
            XmlNode lastNode = gameDoc.SelectSingleNode(@"//resources/public[@type='"+xmlType+"'] [@name='"+maxIdName+"']"); 

            maxId+=1;
            richTextBox1.Text += "\n\n+1后最大ID为：" + maxId.ToString() + "\t";

            string maxStr = "0x" + string.Format("{0:X}", maxId).ToLower();
            richTextBox1.Text += "转换后为：" + maxStr.ToString() + "\t 验证int:" + Convert.ToInt64(maxStr, 16).ToString();
            XmlElement element = (XmlElement)lastNode;
            richTextBox1.Text += "得到最后一个节点" + lastNode.OuterXml.ToString();
            return;
           */
            /*   
             XmlNode strNode = gameDoc.SelectNodes(@"//resources/public[@type='dimen']");  //找到最后一个节点后，插入对应值。
          XmlNode xn =  strNode.LastChild;
            XmlElement xe = (XmlElement)xn;
            string name = xe.GetAttribute("name");
            string id = xe.GetAttribute("id");
            richTextBox1.Text = "name=" + name + " id=" + id;
           foreach (XmlNode item in xmlDoc.DocumentElement)
           {
               XmlElement xe = (XmlElement)item;
               string s = xe.GetAttribute("name");
           }
           return;
           
           string gameXml = @"F:\客户端\sandGlass\sandGlass\bin\Debug\deCompile\ttpkq\AndroidManifest.xml";
           string sdkXml = @"F:\客户端\sandGlass\sandGlass\bin\Debug\channels\uc\config.xml";
           DataSet  ds =  XmlHelper.GetXml(gameXml);
           DataTable dt = ds.Tables[0];
           foreach (DataRow dr in dt.Rows)
           {
               for (int i = 0; i < dt.Columns.Count; i++)
               {
                //   dr[i].ToString();
                   richTextBox1.Text += dr[i].ToString()+"\r\n";
               }
           }
       */
        }
    }
}
