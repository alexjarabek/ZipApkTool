using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sandGlass
{
    public delegate void ExportCallBackDelegate(string thread, string msg);
    class exportThread
    {
        public bool check = false;
        public string name = "Export_Thread";
        public string game_name = "";
        public string game_id = "";
        public string game_package = "";
        public ExportCallBackDelegate callback;
         
        public static string MSG_GAME_NULL = "GAME_NULL";
        public static string MSG_COMPLETE = "COMPLETE";

        public channelManager cManager = new channelManager();

        public string call_test = @"http://lijinling.g.linnyou.com/CallBack/index/"; // 测试回调
        public string call = @"http://g.linnyou.com/CallBack/index/";//正式回调



        public void doWork()
        {
            if (game_id == "")
            {
                callback(name, MSG_GAME_NULL);
                return;
            }
            if(!Directory.Exists(envConfig.export_call)){

                Directory.CreateDirectory(envConfig.export_call);
            }
             
           List<channelItem> channellist = new List<channelItem>();
            if (check)
            {
                channellist = cManager.GetItemsByGame(game_id);// 已发渠道               
            }
            else
            {
                channellist = cManager.GetItems(); // 所有渠道
            }
            string line = game_name + "-包名,支付回调---对应的包名仅供参考" + "\r\n";
            int i =0;
            foreach (channelItem item in channellist)
            {
                if (item.flag == "true")
                {
                    i++;
                    line += "\r\n";
                    line += i + "、  " + item.name + "  版本：" + item.version + "-----------------------------------------\r\n";
                    line += "参考包名：  " + game_package + "." + item.package + "\r\n";
                    line += "正式回调：  " + call + item.cid + @"/" + game_id + "\r\n";
                    line += "测试回调：  " + call_test + item.cid + @"/" + game_id + "\r\n";
                }
            }
            FileUtil.writeContent(envConfig.export_call+game_name+".text", line);
            callback(name, MSG_COMPLETE);

        }

    }
}
