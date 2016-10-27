using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

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
    public partial class Test : Form
    {
        private static XmlDocument xmlDoc = new XmlDocument();
        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            string path = @"E:\SVN\sandGlass-1.2.1-source\sandGlass\bin\Release\sdk";

            DirectoryInfo folder = new DirectoryInfo(path);



            foreach (FileInfo file in folder.GetFiles("*.dex"))
            {
                Console.WriteLine(file.FullName);
                string strmainname = Path.GetFileNameWithoutExtension(file.Name);
                RadioButton rb = new RadioButton(); 
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(3, 3);
                rb.Name = strmainname;
                rb.Text = strmainname;
                rb.UseVisualStyleBackColor = true;
                rb.CheckedChanged += new System.EventHandler(this.radioButtonSdk_CheckedChanged);
                rb.Tag = strmainname;
                this.flowLayoutPanelSdk.Controls.Add(rb);

            }



             
        }

        private void radioButtonSdk_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if(rb.Checked){
                string tag = (string)rb.Tag;
                sdk.Text = tag;
                Console.WriteLine(tag);

            } 

        }

        private void btn_sdk_add_Click(object sender, EventArgs e)
        {
            GC.Collect();
            string to = envConfig.currenPath + @"/sdk/jars/";

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = envConfig.currenPath;
            dialog.Filter = "skin files (*.jar)|*.jar";
            string name ="";
            if (dialog.ShowDialog() == DialogResult.OK)
            {  
                name = Path.GetFileNameWithoutExtension(dialog.FileName);
                label1.Text = dialog.FileName;
                //if (!Directory.Exists(to))
                //    Directory.CreateDirectory(to);
                //string tofile = to + Path.GetFileName(dialog.FileName); ;
                //File.Copy(dialog.FileName, tofile, true);

                Process makeDex = new Process();
                makeDex.StartInfo.FileName = envConfig.toolPath + @"\dx.bat"; 
                string dexname = envConfig.currenPath + @"\sdk\" + name + ".dex"; 

              //  dx --dex --output=1_4_1.dex sgframework_1.4.1_dev.jar

                makeDex.StartInfo.Arguments = " --dex --output=" + dexname + " " + dialog.FileName;
                makeDex.Start();
                makeDex.WaitForExit();

                this.flowLayoutPanelSdk.Controls.Clear();
                string path = @"E:\SVN\sandGlass-1.2.1-source\sandGlass\bin\Release\sdk";

                DirectoryInfo folder = new DirectoryInfo(path);



                foreach (FileInfo file in folder.GetFiles("*.dex"))
                {
                    Console.WriteLine(file.FullName);
                    string strmainname = Path.GetFileNameWithoutExtension(file.Name);
                    RadioButton rb = new RadioButton();
                    rb.AutoSize = true;
                    rb.Location = new System.Drawing.Point(3, 3);
                    rb.Name = strmainname;
                    rb.Text = strmainname;
                    rb.UseVisualStyleBackColor = true;
                    rb.CheckedChanged += new System.EventHandler(this.radioButtonSdk_CheckedChanged);
                    rb.Tag = strmainname;
                    this.flowLayoutPanelSdk.Controls.Add(rb);

                }
                 
            }

           





        }

        private void btn_fresh_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanelSdk.Controls.Clear();
            string path = @"E:\SVN\sandGlass-1.2.1-source\sandGlass\bin\Release\sdk";

            DirectoryInfo folder = new DirectoryInfo(path);



            foreach (FileInfo file in folder.GetFiles("*.dex"))
            {
                Console.WriteLine(file.FullName);
                string strmainname = Path.GetFileNameWithoutExtension(file.Name);
                RadioButton rb = new RadioButton();
                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(3, 3);
                rb.Name = strmainname;
                rb.Text = strmainname;
                rb.UseVisualStyleBackColor = true;
                rb.CheckedChanged += new System.EventHandler(this.radioButtonSdk_CheckedChanged);
                rb.Tag = strmainname;
                this.flowLayoutPanelSdk.Controls.Add(rb);

            }
        }
    }
}
