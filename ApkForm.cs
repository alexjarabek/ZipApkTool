using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using System.Threading;//
namespace sandGlass
{
    public partial class ApkForm : Form
    {
        string file = "";
        public ApkForm()
        {
            InitializeComponent();
        }
        private void ApkForm_Load(object sender, EventArgs e)
        {

            CheckForIllegalCrossThreadCalls = false;// 控件，其他线程调用
        }

        private void but_getapk_Click(object sender, EventArgs e)
        {
            getApkShow();
        }

        public void getApkShow()
        {
            GC.Collect();
            if (getApk.ShowDialog() == DialogResult.OK)
            {
                but_getapk.Enabled = false;
                file = getApk.FileName.ToString();
                  
                DeBackDelegate deCallBack = callBack;
                Thread t = new Thread(decompile);
                t.Start(deCallBack);
                 
            }
            getApk.Dispose();
        }

        public delegate void DeBackDelegate(string msg); 
        public void decompile(object o)
        {
            string to = envConfig.temp_decompile;
            bool ret = apkTool.d(this.glog, file, to);
            DeBackDelegate callback = o as DeBackDelegate;
            if (ret)
            {
                callback("success");
            }
            else
            {
                callback("error");
            }

        }
        /// <summary>
        /// 解包回调
        /// </summary> 
        private void callBack(string msg)
        {
            MessageBox.Show(msg);
            but_ok.Enabled = true;
            GC.Collect();

        }
        string new_package = "";
        private void but_ok_Click(object sender, EventArgs e)
        {
             
            new_package = text_new_package.Text;
            BackCompielDelegate deCallBack = callBack;
            if (new_package != "" && File.Exists(envConfig.temp_decompile_manifest))
            { 
                XmlHelper.XmlAttributeEdit(envConfig.temp_decompile_manifest, "manifest", "package", new_package);
                backcompile();
            }
        }
        public delegate void BackCompielDelegate(string msg);
        /// <summary>
        /// 解包回调
        /// </summary> 
        private void compilelBack(string msg)
        {
            MessageBox.Show(msg);
            but_ok.Enabled = true;
            GC.Collect();

        }
        public void backcompile()
        {

            makeRes();
            makeDex();
            makeSmali();
            makeUnsignApk();
            makeSignApk();
        }
        /// <summary>
        /// 生成r.java r.class
        /// </summary>
        private void makeRes()
        {
            

            if (Directory.Exists(envConfig.temp_gen))
                FileUtil.deleteFolder(envConfig.temp_gen);
            Directory.CreateDirectory(envConfig.temp_gen);
            if (Directory.Exists(envConfig.temp_classes))
                FileUtil.deleteFolder(envConfig.temp_classes);
            Directory.CreateDirectory(envConfig.temp_classes);

            string packagePath = new_package.Replace(".", @"\");
            string gameFilePath = envConfig.temp_gen + packagePath; 
            Process makeR = new Process();
            makeR.StartInfo.FileName = envConfig.toolPath + @"\makeR-debug.bat";

            makeR.StartInfo.Arguments =  envConfig.temp_decompile_res + " " + envConfig.temp_decompile_manifest + " " + envConfig.temp_gen + "  " + envConfig.temp_classes + " " + gameFilePath;
            makeR.Start();
            makeR.WaitForExit(); 
             
        }
        /// <summary>
        /// 生成DEX
        /// </summary>
        public void makeDex()
        {

            Process makeDex = new Process();
            makeDex.StartInfo.FileName = envConfig.toolPath + @"\dx.bat";
            makeDex.StartInfo.Arguments = "   --dex --output=" + envConfig.temp_dex + "   " + envConfig.temp_classes;
            makeDex.Start();
            makeDex.WaitForExit(); 
        }
        /// <summary>
        /// 生成smali
        /// </summary>
        public void makeSmali()
        { 
            Process makeSmali = new Process();
            makeSmali.StartInfo.FileName = envConfig.toolPath + @"\getSmali-debug.bat";
            makeSmali.StartInfo.Arguments = envConfig.temp_decompile_smali+"  " + envConfig.temp_dex;
            makeSmali.Start();
            makeSmali.WaitForExit(); 

        }


        /// <summary>
        /// 生成apk 未签名
        /// </summary>
        public void makeUnsignApk()
        { 

            string unSignApk = envConfig.temp+"unsign.apk";
            Process p = new Process();
            p.StartInfo.FileName = envConfig.apkTool_debug;
            p.StartInfo.Arguments = "  b  " + envConfig.temp_decompile + "  -o  " + unSignApk;
            p.Start(); 
            p.WaitForExit();
           
        }
        /// <summary>
        /// 签名
        /// </summary>
        public void makeSignApk()
        {
            string signPwd = "lyhd2015";
            string keyStrorePwd = "lyhd2015";
            string keyStore = envConfig.temp_keystory;
            string alias = "lyhd"; 
            string releaseApk = envConfig.temp+"sign.apk"; 
            Process makeSign = new Process(); 
           
            makeSign.StartInfo.FileName = envConfig.toolPath + @"\sign.bat"; 
            string unSignApkPath = envConfig.temp+"unsign.apk";
           

            makeSign.StartInfo.Arguments = keyStore + " " + keyStrorePwd + " " + signPwd + " " + releaseApk + " " + unSignApkPath + " " + alias;
            makeSign.Start();
            makeSign.WaitForExit(); 
        }


    }
}
