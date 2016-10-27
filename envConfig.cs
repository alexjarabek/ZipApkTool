using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandGlass
{
    class envConfig
    {

        public static string romdisk = @"Z:";


        public static string currenPath  = Application.StartupPath;
        public static string gamePath    = currenPath+@"\game\";
        public static string channelPath = currenPath+@"\channels\";
        public static string gameXmlRoot = "gameConfig";
        public static string gameList    = "gameList";
        public static string gameXmlPath = gameXmlRoot + "/" + gameList;
        public static string upPath      = Application.StartupPath + @"\res\";


        public static string targetPath = Application.StartupPath + @"\compile\";
        public static string deCompilePath = Application.StartupPath + @"\deCompile\";
        public static string compileFolder = currenPath + @"\pkgs\";

        //public static string targetPath = romdisk + @"\compile\";
        //public static string deCompilePath = romdisk + @"\deCompile\";
        //public static string compileFolder = romdisk + @"\pkgs\";


        public static string gameConfig  = Application.StartupPath + @"\gameConfig\";      
        public static string releasePkg = compileFolder + @"releasePkg\";
        public static string configExt   = ".properties";
        public static string toolPath     = envConfig.currenPath + @"\apktool";
        public static string apkToolCommand = envConfig.toolPath + @"\apktool.bat";
//     sandglass -2.0
        public static string version = "2.0.0";
        public static string debug = "false";// true 弹cmd，false 不弹
        public static string configXml = Application.StartupPath + @"\config.xml";
        public static string apkVersion="2_0_0";
        public static string apkTool = envConfig.toolPath + @"\apktool" + @"_" + apkVersion + ".bat";
        public static string apkTool_debug = envConfig.toolPath + @"\apktool" + @"_" + apkVersion + "_debug.bat";           
        public static string images = Application.StartupPath + @"\images\";
        public static string gameApks = Application.StartupPath + @"\gameApks\";
        public static string games = currenPath + @"\games\";
        public static string gamesXml = games + "games.xml";   
        public static string channels = currenPath + @"\channels\";
        public static string channelsXml = channels+"channels.xml";
        public static string tmpFolder = currenPath + @"\res\temp\";
        public static string meta_datasXml = channels + "meta_datas.xml";

        public static string gameSelectFolder = currenPath;
        public static string channelSelectFolder = currenPath;
        public static string logs = currenPath + @"\logs\";
        public static string sdk = currenPath + @"\sdk\";
        public static string lastSdk = "1.4.1";

        public static string export_call = Application.StartupPath + @"\export\";
         

        //public static string tmpCls = tmpFolder + @"class\";
        //public static string tmpR = tmpFolder + @"R\";


         public static string temp = currenPath +@"\temp\";
         public static string temp_decompile = temp + @"decompile\"; 
         public static string temp_decompile_manifest = temp + @"decompile\AndroidManifest.xml";
         public static string temp_decompile_res = temp + @"decompile\res";
         public static string temp_decompile_smali = temp + @"decompile\smali";


         public static string temp_gen = temp + @"gen\";
         public static string temp_classes = temp + @"class\";
         public static string temp_dex = temp + @"dex\classes.dex";

         public static string temp_keystory = currenPath + @"\temp\linyou.keystore";

    }
}
