using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sandGlass
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

          //   Application.Run(new Test());

            //// V2.0_beta  兼容老版本，老版本入口，可切换至 新版
            // if (new compileManager().getConfig("version") == compileManager.Version1)
            // {
            //     Application.Run(new compile());
            // }
            // else
            // {
            //     Application.Run(new Home());
            // }


              Application.Run(new ApkForm());
            //  Application.Run(new parametersEdit());

        }
    }
}
