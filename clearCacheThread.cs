using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms; 
namespace sandGlass
{
    public delegate void ClearCallBackDelegate(string thread, string msg);
    class clearCacheThread
    {
        public bool temp=false;
        public bool unsignapk=false;
        public bool signapk=false;
        public bool compile = false;
        public bool errlog = false;

        public ProgressBar probar;
        public Label clear_lab;

        public string name = "Clear_Cache_Thread";
        public ClearCallBackDelegate callback;

        public static string MSG_ABORT = "ABORT";
        public static string MSG_COMPLETE = "COMPLETE";

        public void doWork() {
            probar.Value = 5;
            clear_lab.Text = "";
        if(temp){
            string path = envConfig.upPath;
            deleteFolder(path);
        }
        probar.Value = 25;
        if (unsignapk)
        {
            string path = envConfig.compileFolder;
            deleteFolder(path, "releasePkg");
        }
        probar.Value = 50;
        if (errlog)
        {
            string path = envConfig.logs;
            deleteFolder(path);
        }
        probar.Value = 55;
        if (signapk)
        {
            string path = envConfig.releasePkg;
             deleteFolder(path);
        } 
            probar.Value = 70;


        if (compile)
        {
            string path = envConfig.targetPath;
            deleteFolder(path);
        }
        probar.Value = 100;
        clear_lab.Text = "清理完毕";
        callback(name, MSG_COMPLETE);
        }

        public  void deleteFolder(string path,string ext="")
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                DirectoryInfo[] childs = folder.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    if (child.Name == ext)
                    {
                        continue;
                    }
                  //  Console.WriteLine(child.Name);
                    try
                    {
                        clear_lab.Text = "清理目录" + child.Name+"..";
                        deleteFolder(child.FullName);
                        child.Delete(true);
                    }
                    catch (Exception e)
                    {

                        MessageBox.Show(e.Message);
                    }
                    
                }
              //  folder.Delete(true);
            }
        }
    }
}
