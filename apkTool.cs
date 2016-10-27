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

    class apkTool
    {
        public static bool d(RichTextBox richTxtInfo, string apkPath, string decompilePath)
        {

            if (Directory.Exists(decompilePath))
            {
                FileUtil.deleteFolder(decompilePath);

            }
            string command = envConfig.apkTool;

            richTxtInfo.AppendText(command + "\r\n");
            richTxtInfo.AppendText("开始解压母包\r\n");

            Process p = new Process();
            string line = null;
            if (envConfig.debug == "false")
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;

            }
            else
            {
                command = envConfig.apkTool_debug;
            }
            p.StartInfo.FileName = command;
            p.StartInfo.Arguments = " -f d  \"" + apkPath + "\"  -o " + decompilePath;
            // p.StartInfo.Arguments = " -f d  " + apkPath + "  -o " + decompilePath;  // 文件名空格需要转义
            p.Start();

            bool flag = true;
            if (envConfig.debug == "false")
            {
                StreamReader reader = p.StandardOutput;//截取输出流
                StreamReader error_reader = p.StandardError;//截取输出流
                richTxtInfo.AppendText("正在解压请稍等...\r\n");
                int i = 0;
                while (!error_reader.EndOfStream)
                {
                    i++;
                    line = error_reader.ReadLine() + "\r\n";
                    if (line.StartsWith("W") || line.StartsWith("Ignoring"))
                    {
                        if (i == 1) { richTxtInfo.AppendText("\r\n" + "--------------警告信息--------------" + "\r\n"); }
                    }
                    else
                    {
                        if (i == 1) { richTxtInfo.AppendText("\r\n" + "--------------错误信息--------------" + "\r\n"); }
                        flag = false;
                    }
                    richTxtInfo.AppendText(line);
                }
                error_reader.Close();

                //while (!reader.EndOfStream)
                //{
                //    line = reader.ReadLine() + "\r\n";
                //    richTxtInfo.AppendText(line);
                //}
                //reader.Close();

               
               

            }
            p.WaitForExit();
            if (flag)
            {
                richTxtInfo.AppendText("\r\n" + "------------恭喜你！母包更新完毕！------------" + "\r\n");
            }
            else
            {
                richTxtInfo.AppendText("\r\n" + "-----------抱歉母包更新失败-------------！" + "\r\n");
                richTxtInfo.AppendText("\r\n" + @"step1  先清除apktool缓存目录,C:\Users\win8\apktool\framework." + "\r\n");
                richTxtInfo.AppendText("\r\n" + "step2  step1继续报错,则请更换设置apktool版本." + "\r\n");

            }
            return flag;
        }



        public static bool b(string game, string packageFolder, string unSignApk)
        {
            if (!Directory.Exists(packageFolder))
            {
                return false;
            }
            string command = envConfig.apkTool;
            Process p = new Process();

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
            string log = "";
            if (envConfig.debug == "false")
            {
                StreamReader error_reader = p.StandardError;//截取输出流             
                int i = 0;
                while (!error_reader.EndOfStream)
                {
                    i++;
                    log += error_reader.ReadLine() + "\r\n";
                    if (log.StartsWith("W:"))
                    {
                    }
                    else
                    {
                        flag = false;
                    }
                }
                error_reader.Close();
            }
            p.WaitForExit();
            if (!flag & envConfig.debug == "false")
            {
                string writePath = envConfig.logs + game + @"\";
                if (!Directory.Exists(writePath))
                {
                    Directory.CreateDirectory(writePath);
                }
                FileUtil.writeContent(writePath + "err_log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", log);
            }

            return flag;
        }




    }
}
