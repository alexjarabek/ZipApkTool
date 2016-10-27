using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandGlass
{
    public class channelItem
    {
        public int index=-1;
        public string cid="";
        public string name="";   
        public string version="";
        public string isR = "false";
        public string isfoot = "false";
        public string foot="";
 //       public string apk="";
        public string flag = "true";
        public string package="";//打包时 渠道的包名后缀
        public string sdkPackage = "";// 获取替换资源的包名，sdk 包名
        public string isOwnKey = "false";// 渠道是否用自己的签名
        public string des = "渠道描述信息";


    }
}
