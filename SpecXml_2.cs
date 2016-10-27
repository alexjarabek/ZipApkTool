using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;


namespace sandGlass
{
    class SpecXml_2
    {
        public string game, channel, properties, gameXml, gameName, wxSmali;

        //YY
        public void YYxml()
        {
            //PpHelper pt = new PpHelper(properties);
            //string packageName = pt.GetPropertiesText("package");
            //string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            //string xmlTmpFile = gameXml + @".tmp";
            ////  MessageBox.Show(xmlTmpFile);
            //FileUtil.writeContent(xmlTmpFile, tmpXml);

            //Dictionary<string, string> attrsProvider = new Dictionary<string, string>();
            //attrsProvider.Add("android" + FileUtil.colon + "name", "com.yy.gamesdk.provider.YYDataProvider");
            //attrsProvider.Add("android" + FileUtil.colon + "authorities", packageName + ".yygamesdkprovider");
            //attrsProvider.Add("android" + FileUtil.colon + "exported", "true");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "provider", attrsProvider);   //添加 provider


            //Dictionary<string, string> attrsService = new Dictionary<string, string>();
            //attrsService.Add("android" + FileUtil.colon + "name", "com.tencent.android.tpush.rpc.XGRemoteService");
            //attrsService.Add("android" + FileUtil.colon + "exported", "true");
            //string addChildName = "intent-filter";
            //Dictionary<string, string> attrsAdd = new Dictionary<string, string>();
            //// attrsAdd.Add("hello", "world");
            //string addChildName1 = "action";
            //Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            //attrsAdd1.Add("android" + FileUtil.colon + "name", packageName + ".PUSH_ACTION");
            //string textTxt = null;
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "service", attrsService, addChildName, attrsAdd, addChildName1, attrsAdd1, textTxt);  //添加service

            //string tempXml = FileUtil.getContent(xmlTmpFile);
            //tempXml = FileUtil.xmlRestore(tempXml);
            //tmpXml = FileUtil.replaceXmlColon(tempXml);
            //FileUtil.writeContent(gameXml, tmpXml);
            //File.Delete(xmlTmpFile);
        }
        //值尚互动
        public void ZSHDxml()
        {

            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";

            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "fly.fish.aidl.MyRemoteService");
            attrsService.Add("android" + FileUtil.colon + "enabled", "true");
            string addChildName = "intent-filter";
            Dictionary<string, Dictionary<string, string>> attrsAdd = new Dictionary<string, Dictionary<string, string>>();

            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", packageName + ".fly.fish.aidl.IMyTaskBinder");

            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");

            attrsAdd.Add(addChildName1, attrsAdd1);
            attrsAdd.Add(addChildName2, attrsAdd2);
            XmlHelper.XmlInsertMultiElementForSpec1(xmlTmpFile, "manifest/application", "service", attrsService, addChildName, attrsAdd);  //添加service

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);

            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "AsdkPublisher.txt";
            string contentStr = pt.GetPropertiesText("asdk");
            FileUtil.writeContent(writePath, contentStr);
        }
        //悠悠村
        public void UUxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            //  MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsProvider = new Dictionary<string, string>();
            attrsProvider.Add("android" + FileUtil.colon + "name", "com.logsdk.provider.CacheDataProvider");
            attrsProvider.Add("android" + FileUtil.colon + "authorities", packageName + ".log.cachedataprovider");
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "provider", attrsProvider);

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);

            //=============================修改Properties========================================

            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "passport" + envConfig.configExt;
            PpHelper readHlper = new PpHelper(properties);
            string channel_id = readHlper.GetPropertiesText("channelId");
            string contentStr = "channel_id=" + channel_id + "\r\n";
            contentStr += "apk_key=" + readHlper.GetPropertiesText("appkey") + "\r\n";
            contentStr += "game_type=" + readHlper.GetPropertiesText("gameType") + "\r\n";
            contentStr += "switch_user=" + readHlper.GetPropertiesText("switchUser");
            FileUtil.writeContent(writePath, contentStr);
            string uuChannelPath = envConfig.targetPath + @game + @"\" + channel + @"\assets\UU_ChannelId";
            FileUtil.writeContent(uuChannelPath, channel_id);
        }
        //PPS
        public void PPSxml()
        {
            //=============================修改Properties========================================           
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\zConfig\" + "pps_packetid" + envConfig.configExt;
            PpHelper readHlper = new PpHelper(properties);
            string channel_id = readHlper.GetPropertiesText("id");
            string contentStr = "qudaoId=" + channel_id + "";
            FileUtil.writeContent(writePath, contentStr);
        }
        //TT语音
        public void TTYYxml()
        {
            //=============================修改Properties========================================           
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "tt_game_sdk_opt" + envConfig.configExt;
            PpHelper readHlper = new PpHelper(properties);
            string gameId = readHlper.GetPropertiesText("gameId");
            string contentStr = "source = KaoPu" + "\r\n";
            contentStr += "gameId=" + gameId + "\r\n";
            FileUtil.writeContent(writePath, contentStr);
            writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\TTGameSDKConfig.cfg";
            FileUtil.writeContent(writePath, readHlper.GetPropertiesText("sdkcfig"));

        }
        /// <summary>
        /// 游艺春秋 icc
        /// </summary>
        public void ICCxml()
        { 
            //=============================修改Properties========================================           
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "ICC_SDK.conf";
            PpHelper readHlper = new PpHelper(properties);
            string gameId = readHlper.GetPropertiesText("game_id");
            string contentStr = "{'remoteUrl:'http://debug.sdk.m.iccgame.com/html5-v2/index.html?game_id='" + gameId + "'}"; 
            FileUtil.writeContent(writePath, contentStr);  
              
        }


        //唐腾
        public void MANGOxml()
        {
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\BNSDK_RES\config.properties";
            string contentStr = "ComeFromType=1" + "\r\n" + "ComeFrom=mgw" + "\r\n" + "WepayKey=MIICeAIBADANBgkqhkiG9w0BAQEFAASCAmIwggJeAgEAAoGBAL/Bbaw7xjeuNEULW8XK3wnbsyAQ6uNdvYDXu/jLQ2PK8B44kqq25+E0qXX1m1ZfVDztEYnFpmpGhUSiWktpA7w7agBpCGbLXlUzANJypEgV7EBFETBm8UVMGn/jCstrCd8C+jG/OhXGF9kCyzO8PeR/FYljbJ99gD0gMs8nUsuBAgMBAAECgYEAo8EsxpwZT22eR6RGkCgKxuFvHCA2Z0qjTcduSC5Vc4BYBIbtgYpHhYQyf4DqUh2A07LkelJnJnTo1E8Naz7YJ0Y8bFdhP4czLQK9QK8B83kakFDh01JMcJK7Eul+gfFLP8y8Rz31PvLVJ3FE8+4mVBYetgPU3USX6FTbOP4Csn0CQQD+wR2zOeRTx8bb/LnxWKW0OzwbXoQFXIRIlxQMRFS9v2ZBiJVWfmZUz+ljFMpUDGtdINLVnYrcMKg4cdw3tQovAkEAwLF0cVMkMBUJ6jJGLlrCPE1q2GkY/20Qm3Z6ZVNud58wSbFf1mVB9w4sOE2zMkEGLHfaRVLE59amvH5h5zcJTwJALdtrUjjIjgA3HqBWhEg1w8Sp5C9WSnTF5x8y36ZpLqLGcGN6plAocXnfhBNY/Foj9WaULRmnxk0H6ukr/+cZxwJBAK1wDArkdrIAfcgaMCkQ77svQ3g+QI3HMSd84HXLPqbU1bW+vTBQO6uPSiXzadNVy6TCy+eRtEGoZrNKXfyrAOkCQQD7+jFbW8i2oQJdNJarem0xkPg6dQOllFfEMVrL3+i2LDAaKDAiajVhsq9uo5pzT1Zmr0v16mQgQUkMK4Lix1Rk" + "\r\n"
                + "Account =http:///m.mgwyx.com/" + "\r\n" + "BBS =http:///m.mgwyx.com/sklr#taolun" + "\r\n" + "Help =http:///m.mgwyx.com/strategy/23172.html" + "\r\n" + "Gift =http:///m.mgwyx.com/lb/1_11.html" + "\r\n" + "Strategy =http:///m.mgwyx.com/strategy/6_1.html" + "\r\n";
            contentStr += "ChannelId=" + pt.GetPropertiesText("ChannelId") + "\r\n";
            contentStr += "GameId=" + pt.GetPropertiesText("GameId") + "\r\n";
            contentStr += "SKey=" + pt.GetPropertiesText("SKey") + "\r\n";

            FileUtil.writeContent(writePath, contentStr);
        }
        //巨人星云
        public void SCLOUDMxml()
        {
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\ztsdk_config.properties";
            string contentStr = "config.sdk.class=com.mztgame.mgcafe.ZTLibMgcafe" + "\r\n" + "config.domain.passport.legacy=http://passport.mztgame.com" + "\r\n" + "config.domain.passport=http://other.passport.mobileztgame.com" + "\r\n"
                + "config.domain.pay=http://pay.mobileztgame.com" + "\r\n"
                + "config.appid=" + pt.GetPropertiesText("appid") + "\r\n" + "config.appkey=" + pt.GetPropertiesText("appkey") + "\r\n";
            FileUtil.writeContent(writePath, contentStr);
        }

        //一起玩
        public void LEWANxml()
        {
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\sdk_info.xml";
            string contentStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n";
            contentStr += "<Cpid value=\"" + pt.GetPropertiesText("cpid") + "\"></Cpid>" + "\r\n";
            FileUtil.writeContent(writePath, contentStr);
        }

        //猎宝
        public void LIEBAOxml()
        {
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\lb_config.properties";
            string contentStr = "appID=" + pt.GetPropertiesText("appId") + "\r\n";
            contentStr += "gameID=" + pt.GetPropertiesText("gameId") + "\r\n";
            contentStr += "agent=" + pt.GetPropertiesText("agent") + "\r\n"; 
            FileUtil.writeContent(writePath, contentStr);
        }
        // modou 魔豆
        public void MODOUxml()
        {

            string url_release = @"http://api.mfsdk.com/";
            string url_debug = @"http://test.mfsdk.com/";

            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\mf_config.xml";
            string contentStr = "<mf-config>" + "\r\n";
            contentStr += "<appkey>" + pt.GetPropertiesText("appKey") + "</appkey>" + "\r\n";
            contentStr += "<privatekey>" + pt.GetPropertiesText("privateConfigKey") + "</privatekey>" + "\r\n";
            contentStr += "<sdk_debug>false</sdk_debug>" + "\r\n";
            contentStr += "<url_release>" + url_release + "</url_release>" + "\r\n";
            contentStr += "<url_debug>" + url_debug + "</url_release>" + "\r\n";
            contentStr += "</mf-config>" + "\r\n";
            FileUtil.writeContent(writePath, contentStr);
        }

        //神起
        public void SHENQIxml()
        {
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\SourceInfo.xml";
            string contentStr = "<SourceInfo>" + "\r\n";
            contentStr += "<UserID>" + pt.GetPropertiesText("uid") + "</UserID>" + "\r\n";
            contentStr += "<SourceID>" + pt.GetPropertiesText("sid") + "</SourceID>" + "\r\n";
            contentStr += "<Version>" + pt.GetPropertiesText("sversion") + "</Version>" + "\r\n";
            contentStr += "</SourceInfo>" + "\r\n";
            FileUtil.writeContent(writePath, contentStr);
        }

        //37玩
        public void SQWANxml()
        {
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\37wan_config.xml";
            string contentStr = "<config>" + "\r\n";
            contentStr += "<gameid>" + pt.GetPropertiesText("appId") + "</gameid>" + "\r\n";
            contentStr += "<partner>1</partner>" + "\r\n";
            contentStr += "<referer>sy00000_1</referer>" + "\r\n";
            contentStr += "</config>" + "\r\n";
            FileUtil.writeContent(writePath, contentStr);
        }
        //360
        public void QIHUxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("android" + FileUtil.colon + "name", "QHOPENSDK_PRIVATEKEY");
            meatas.Add("android" + FileUtil.colon + "value", FileUtil.md5(pt.GetPropertiesText("appsecret") + "#" + pt.GetPropertiesText("appkey")));
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", meatas);

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }
        //琵琶网（二狐）
        public void PIPAxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";

            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.tencent.android.tpush.rpc.XGRemoteService");
            attrsService.Add("android" + FileUtil.colon + "exported", "true");
            string addChildName = "intent-filter";
            Dictionary<string, Dictionary<string, string>> attrsAdd = new Dictionary<string, Dictionary<string, string>>();

            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", packageName + ".PUSH_ACTION");
            attrsAdd.Add(addChildName1, attrsAdd1);
            XmlHelper.XmlInsertMultiElementForSpec1(xmlTmpFile, "manifest/application", "service", attrsService, addChildName, attrsAdd);  //添加service

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }

        //安趣
        public void ANQUxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp"; 
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("android" + FileUtil.colon + "name", "ANQUSDK_PRIVATEKEY");
            meatas.Add("android" + FileUtil.colon + "value", FileUtil.md5(pt.GetPropertiesText("appSecret") + "Anqu" + pt.GetPropertiesText("appkey")));
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", meatas);

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }

        //玖游
        public void JIUYOUxml()
        {
            string path = envConfig.targetPath + @game + @"\" + channel + @"\res\raw\" + "configurations.xml";
            PpHelper pd = new PpHelper(properties);
            XmlHelper.XmlNodeReplace(path, @"//sdkconfig/config/clientid", pd.GetPropertiesText("clientId"));
            XmlHelper.XmlNodeReplace(path, @"//sdkconfig/config/channelid", pd.GetPropertiesText("channelId"));
            XmlHelper.XmlNodeReplace(path, @"//sdkconfig/config/key", pd.GetPropertiesText("key"));
            XmlHelper.XmlNodeReplace(path, @"//sdkconfig/config/voucher", pd.GetPropertiesText("voucher"));
        }

        //靠谱助手
        public void KAOPUxml()
        {
            //================================================写入json================================================
            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\kaopu_game_config.json";
            string contentStr = "{\"KP_Channel\":\"kaopu\",\"ChannelKey\":\"kaopu\",\"gameName\":\"";
            contentStr += pt.GetPropertiesText("gameName");
            contentStr += "\",\"screenType\":\"2\",\"fullScreen\":\"true\",\"param\":\"\"}";
            pt.Close();
            FileUtil.writeContent(writePath, contentStr);
        }
        /// <summary>
        /// 新浪手助 sinasz
        /// </summary>
        public void SINASZxml()
        {
            //=============================json========================================

            //{"SG_Channel":"cyjh","ChannelKey":"cyjh","gameName":"魔灵幻想","screenType":"2","fullScreen":"true","param":""}

            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "sguo_game_config.json";
            PpHelper pt = new PpHelper(properties);

            string contentStr = "{\"SG_Channel\":\"cyjh\",\"ChannelKey\":\"cyjh\",\"gameName\":\"";
            contentStr += pt.GetPropertiesText("gameName");
            contentStr += "\",\"screenType\":\"";
            contentStr += pt.GetPropertiesText("screenType");
            contentStr += "\",\"fullScreen\":\"true\",\"param\":\"\"}";
            pt.Close();
            FileUtil.writeContent(writePath, contentStr);

        }

        //联旭科技
        public void LIANXUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("BaiduMobAd_CHANNEL", "lxChannel");
            meatas.Add("BaiduMobAd_STAT_ID", "statId");
            insertAndroidManifest(meatas);
        }

        //7k7k
        public void QKQKxml()
        {

            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("android" + FileUtil.colon + "name", "QIKESDK_PRIVATEKEY");
            meatas.Add("android" + FileUtil.colon + "value", FileUtil.md5(pt.GetPropertiesText("appSecret") + "7k7k" + pt.GetPropertiesText("appKey")));
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", meatas);

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);

        }


        //卓易
        public void ZHUOYIxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsPermission = new Dictionary<string, string>();
            attrsPermission.Add("android" + FileUtil.colon + "name", "com.zhuoyi.system.promotion.provider.PromWebContentProvider");
            attrsPermission.Add("android" + FileUtil.colon + "authorities", packageName);
            attrsPermission.Add("android" + FileUtil.colon + "exported", "false");
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "provider", attrsPermission);

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }

        //uc
        public void UCxml()
        {
            string tmp = FileUtil.getContent(gameXml);
            string tmpXml = FileUtil.replaceContent(tmp, "android:launchMode='singleTask':", "android:launchMode='standard':");
            FileUtil.writeContent(gameXml, tmpXml);
        }

        //91玩
        public void JYWANxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.weedong.gamesdk.ui.WdPaymentActivity");
            attrsService.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|navigation|screenSize");
            attrsService.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            attrsService.Add("android" + FileUtil.colon + "theme", "@style/WdPayTheme");
            attrsService.Add("android" + FileUtil.colon + "windowBackground", "@null");
            attrsService.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustPan");
            string addChildName = "intent-filter";
            Dictionary<string, Dictionary<string, string>> attrsAdd = new Dictionary<string, Dictionary<string, string>>();
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "android.intent.action.VIEW");
            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
            string addChildName3 = "data";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "scheme", "tencent" + packageName);
            attrsAdd.Add(addChildName1, attrsAdd1);
            attrsAdd.Add(addChildName2, attrsAdd2);
            attrsAdd.Add(addChildName3, attrsAdd3);
            XmlHelper.XmlInsertMultiElementForSpec1(xmlTmpFile, "manifest/application", "activity", attrsService, addChildName, attrsAdd);  //添加activity

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }


        public void MEITUxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.weedong.gamesdk.ui.WdPaymentActivity");
            attrsService.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|navigation|screenSize");
            attrsService.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            attrsService.Add("android" + FileUtil.colon + "theme", "@style/WdPayTheme");
            attrsService.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustPan");
            string addChildName = "intent-filter";
            Dictionary<string, Dictionary<string, string>> attrsAdd = new Dictionary<string, Dictionary<string, string>>();
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "android.intent.action.VIEW");
            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
            string addChildName3 = "data";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "scheme", "tencent" + packageName);
            attrsAdd.Add(addChildName1, attrsAdd1);
            attrsAdd.Add(addChildName2, attrsAdd2);
            attrsAdd.Add(addChildName3, attrsAdd3);
            XmlHelper.XmlInsertMultiElementForSpec1(xmlTmpFile, "manifest/application", "activity", attrsService, addChildName, attrsAdd);  //添加activity

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);

        }




        //酷狗
        public void KUGOUxml()
        {
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);
            XmlDocument dDoc = new XmlDocument();
            dDoc.Load(xmlTmpFile);
            XmlNode dpath = dDoc.SelectSingleNode(@"//manifest/application");
            XmlElement dXe = (XmlElement)dpath;
            //    dXe.GetAttribute("android" + FileUtil.colon + "debuggable");
            dXe.SetAttribute("android" + FileUtil.colon + "hardwareAccelerated", "true");
            dDoc.Save(xmlTmpFile);
            tmpXml = FileUtil.replaceXmlColon(FileUtil.getContent(xmlTmpFile));
            tmpXml = FileUtil.xmlRestore(tmpXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }
        /// <summary>
        /// 手游天下 mzuser
        /// </summary>
        public void MZUSERxml()
        {
            //=============================修改Properties========================================

            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "mzdata" + envConfig.configExt;
            PpHelper readHlper = new PpHelper(properties);
            string contentStr = "cpid=" + readHlper.GetPropertiesText("cpid") + "\r\n";
            contentStr += "gameid=" + readHlper.GetPropertiesText("gameid") + "\r\n";
            contentStr += "channel=" + readHlper.GetPropertiesText("channelid");
            FileUtil.writeContent(writePath, contentStr);

        }

        //豌豆荚  
        public void WDJxml()
        {
            PpHelper pt = new PpHelper(properties);
            string data = "Wandoujia-PaySdk-" + pt.GetPropertiesText("appKey");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);
            Dictionary<string, string> attrsService = new Dictionary<string, string>();

            attrsService.Add("android" + FileUtil.colon + "name", "com.wandoujia.oakenshield.activity.OakenshieldActivity");
            attrsService.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|screenSize");
            attrsService.Add("android" + FileUtil.colon + "theme", @"@android:style/Theme.Translucent.NoTitleBar");
            attrsService.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustResize");

            string addChildName = "intent-filter";

            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "com.wandoujia.oakenshield");

            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");

            string addChildName3 = "data";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "scheme", data);

            List<Dictionary<string, Dictionary<string, string>>> list = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list3 = new Dictionary<string, Dictionary<string, string>>();

            list1.Add(addChildName1, attrsAdd1);
            list2.Add(addChildName2, attrsAdd2);
            list3.Add(addChildName3, attrsAdd3);

            list.Add(list1);
            list.Add(list2);
            list.Add(list3);

            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "activity", attrsService, addChildName, list);  //添加activity
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }



        //唱吧
        public void CHANGBAxml()
        {
            PpHelper pt = new PpHelper(properties);
            string data = pt.GetPropertiesText("data");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);
            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.changba.activity.OAuthActivity");
            attrsService.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            attrsService.Add("android" + FileUtil.colon + "noHistory", "true");
            string addChildName = "intent-filter";

            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "android.intent.action.VIEW");
            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.BROWSABLE");
            string addChildName3 = "category";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
            string addChildName4 = "data";
            Dictionary<string, string> attrsAdd4 = new Dictionary<string, string>();
            attrsAdd4.Add("android" + FileUtil.colon + "scheme", data);
            List<Dictionary<string, Dictionary<string, string>>> list = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list3 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list4 = new Dictionary<string, Dictionary<string, string>>();
            list1.Add(addChildName1, attrsAdd1);
            list2.Add(addChildName2, attrsAdd2);
            list3.Add(addChildName3, attrsAdd3);
            list4.Add(addChildName4, attrsAdd4);
            list.Add(list1);
            list.Add(list3);
            list.Add(list2);
            list.Add(list4);
            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "activity", attrsService, addChildName, list);  //添加activity
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }
        //百度
        public void BAIDUxml()
        {

            PpHelper pt = new PpHelper(properties);
            string data = "qwallet" + pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);
            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.baidu.platformsdk.pay.channel.qqwallet.QQPayActivity");
            attrsService.Add("android" + FileUtil.colon + "launchMode", "singleTop");
            attrsService.Add("android" + FileUtil.colon + "exported", "true");
            attrsService.Add("android" + FileUtil.colon + "configChanges", "orientation|navigation|screenSize|keyboard|keyboardHidden");
            attrsService.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Translucent.NoTitleBar");
            string addChildName = "intent-filter";
            Dictionary<string, Dictionary<string, string>> attrsAdd = new Dictionary<string, Dictionary<string, string>>();
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "android.intent.action.VIEW");
            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.BROWSABLE");
            string addChildName3 = "category";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
            string addChildName4 = "data";
            Dictionary<string, string> attrsAdd4 = new Dictionary<string, string>();
            attrsAdd4.Add("android" + FileUtil.colon + "scheme", data);
            List<Dictionary<string, Dictionary<string, string>>> list = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list3 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list4 = new Dictionary<string, Dictionary<string, string>>();
            list1.Add(addChildName1, attrsAdd1);
            list2.Add(addChildName2, attrsAdd2);
            list3.Add(addChildName3, attrsAdd3);
            list4.Add(addChildName4, attrsAdd4);
            list.Add(list1);
            list.Add(list3);
            list.Add(list2);
            list.Add(list4);
            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "activity", attrsService, addChildName, list);  //添加activity
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }
        //
        public void CHONGCHONGxml()
        {


            //     <receiver android:name="com.sandglass.game.LoginOutBroadcastReceiver" mergePath="manifest/application"  mergeMethod="insert">
            //    <intent-filter>
            //        <action android:name="CCPAY_LOGINOUT_ACTION" />
            //        <data android:scheme="102067" />
            //    </intent-filter>
            //</receiver>
            PpHelper pt = new PpHelper(properties);
            string data = pt.GetPropertiesText("app_id");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);
            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.sandglass.game.LoginOutBroadcastReceiver");
            string addChildName = "intent-filter";
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "CCPAY_LOGINOUT_ACTION");
            string addChildName2 = "data";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "scheme", data);
            List<Dictionary<string, Dictionary<string, string>>> list = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2 = new Dictionary<string, Dictionary<string, string>>();
            list1.Add(addChildName1, attrsAdd1);
            list2.Add(addChildName2, attrsAdd2);
            list.Add(list1);
            list.Add(list2);

            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "receiver", attrsService, addChildName, list);  //添加activity
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);


        }


        //联想
        public void LENOVOxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            //------------------------------------------------------------------------------------------
            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.gamesdk.receiver.GameSdkReceiver");
            attrsService.Add("android" + FileUtil.colon + "permission", "com.lenovo.lsf.device.permission.MESSAGE"); 
            string addChildName = "intent-filter";
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.gamesdk.receiver.GameSdkReceiver");
            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", pt.GetPropertiesText("appId"));
            string addChildName3 = "action";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "name", packageName);
            List<Dictionary<string, Dictionary<string, string>>> list = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list3 = new Dictionary<string, Dictionary<string, string>>();
            
            list1.Add(addChildName1, attrsAdd1);
            list2.Add(addChildName2, attrsAdd2);
            list3.Add(addChildName3, attrsAdd3);            
            list.Add(list1);
            list.Add(list2);
            list.Add(list3);

            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "receiver", attrsService, addChildName, list);  //添加activity

            //------------------------------------------------------------------------------------------ 
            Dictionary<string, string> attrsReceiver = new Dictionary<string, string>();
            attrsReceiver.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.gamesdk.receiver.GameSdkAndroidLReceiver");
            string filterName = "intent-filter";
            string filterChildName1 = "action";
            Dictionary<string, string> filterAdd1 = new Dictionary<string, string>();
            filterAdd1.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.device.ANDROID_L_MSG");
            string filterChildName2 = "category";
            Dictionary<string, string> filterAdd2 = new Dictionary<string, string>();
            filterAdd2.Add("android" + FileUtil.colon + "name", packageName);
            List<Dictionary<string, Dictionary<string, string>>> filterlist = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> filterlist1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> filterlist2 = new Dictionary<string, Dictionary<string, string>>();
            filterlist1.Add(filterChildName1, filterAdd1);
            filterlist2.Add(filterChildName2, filterAdd2); 
            filterlist.Add(filterlist1);
            filterlist.Add(filterlist2);
            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "receiver", attrsReceiver, filterName, filterlist);  //添加activity
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlTmpFile);
            string launcher = "android.intent.category.LAUNCHER";
            XmlNode xFind = objXmlDoc.SelectSingleNode("manifest/application/activity/intent-filter/category[@android" + FileUtil.colon + "name='" + launcher + "']");

            if (xFind != null)
            {
                XmlNode parent = xFind.ParentNode;
                parent.RemoveAll();
                //创建节点
                XmlElement element = objXmlDoc.CreateElement("action");
                element.SetAttribute("android" + FileUtil.colon + "name", "lenovoid.MAIN"); 
                XmlElement element2 = objXmlDoc.CreateElement("category");
                element2.SetAttribute("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
                parent.AppendChild(element);
                parent.AppendChild(element2);
                objXmlDoc.Save(xmlTmpFile);
            }            
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }


        //腾讯 应用宝
        public void TENCENTxml()
        {

            PpHelper pt = new PpHelper(properties);
            string data = "tencent" + pt.GetPropertiesText("qqAppId");
            string wxAppId = pt.GetPropertiesText("wxAppId");
            string package = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            //          SDK接入 QQ接入配置 START
            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.tencent.tauth.AuthActivity");
            attrsService.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            attrsService.Add("android" + FileUtil.colon + "noHistory", "true");
            string addChildName = "intent-filter";
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", "android.intent.action.VIEW");
            string addChildName2 = "category";
            Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
            attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.BROWSABLE");
            string addChildName3 = "category";
            Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
            attrsAdd3.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
            string addChildName4 = "data";
            Dictionary<string, string> attrsAdd4 = new Dictionary<string, string>();
            attrsAdd4.Add("android" + FileUtil.colon + "scheme", data);
            List<Dictionary<string, Dictionary<string, string>>> list = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list3 = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list4 = new Dictionary<string, Dictionary<string, string>>();
            list1.Add(addChildName1, attrsAdd1);
            list2.Add(addChildName2, attrsAdd2);
            list3.Add(addChildName3, attrsAdd3);
            list4.Add(addChildName4, attrsAdd4);
            list.Add(list1);
            list.Add(list2);
            list.Add(list3);
            list.Add(list4);
            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "activity", attrsService, addChildName, list);  //添加activity

            //           TODO SDK接入 微信接入配置 START
            Dictionary<string, string> attrsService2 = new Dictionary<string, string>();
            attrsService2.Add("android" + FileUtil.colon + "name", package + ".wxapi.WXEntryActivity");
            attrsService2.Add("android" + FileUtil.colon + "excludeFromRecents", "true");
            attrsService2.Add("android" + FileUtil.colon + "exported", "true");
            attrsService2.Add("android" + FileUtil.colon + "label", "WXEntryActivity");
            attrsService2.Add("android" + FileUtil.colon + "launchMode", "singleTop");
            attrsService2.Add("android" + FileUtil.colon + "taskAffinity", package + ".diff");
            string addChildName_ = "intent-filter";
            string addChildName1_ = "action";
            Dictionary<string, string> attrsAdd1_ = new Dictionary<string, string>();
            attrsAdd1_.Add("android" + FileUtil.colon + "name", "android.intent.action.VIEW");
            string addChildName2_ = "category";
            Dictionary<string, string> attrsAdd2_ = new Dictionary<string, string>();
            attrsAdd2_.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");
            string addChildName3_ = "data";
            Dictionary<string, string> attrsAdd3_ = new Dictionary<string, string>();
            attrsAdd3_.Add("android" + FileUtil.colon + "scheme", wxAppId);

            List<Dictionary<string, Dictionary<string, string>>> list_ = new List<Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> list1_ = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list2_ = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> list3_ = new Dictionary<string, Dictionary<string, string>>();
            list1_.Add(addChildName1_, attrsAdd1_);
            list2_.Add(addChildName2_, attrsAdd2_);
            list3_.Add(addChildName3_, attrsAdd3_);

            list_.Add(list1_);
            list_.Add(list2_);
            list_.Add(list3_);

            XmlHelper.XmlInsertMultiElementForSpec2(xmlTmpFile, "manifest/application", "activity", attrsService2, addChildName_, list_);  //添加activity

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
            // 微信 smali 植入
            wxSmali = envConfig.targetPath + game + @"\" + channel + @"\smali\com\sandglass\game\WXEntryActivity.smali";
            string tempContent = FileUtil.replaceContent(FileUtil.getContent(wxSmali), @"com/sandglass/game", package.Replace(".", @"/") + @"/wxapi");
            string targetFolder = envConfig.targetPath + game + @"\" + channel + @"\smali\" + package.Replace(".", @"\") + @"\wxapi\";
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            string targetPath = targetFolder + @"WXEntryActivity.smali";
            FileUtil.writeContent(targetPath, tempContent);

        }

        //VIVO
        public void VIVOxml()
        {

            PpHelper pt = new PpHelper(properties); 
            string package = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);
 
            //           TODO SDK接入 微信接入配置 START
            Dictionary<string, string> attrsService2 = new Dictionary<string, string>();
            attrsService2.Add("android" + FileUtil.colon + "name", package + ".wxapi.WXEntryActivity"); 
            attrsService2.Add("android" + FileUtil.colon + "exported", "true"); 
            attrsService2.Add("android" + FileUtil.colon + "launchMode", "singleTop");
            attrsService2.Add("android" + FileUtil.colon + "theme", "@style/pop_view");
              attrsService2.Add("android" + FileUtil.colon + " android:windowSoftInputMode", "stateAlwaysHidden"); 
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", attrsService2);  //添加activity
              
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
              
            WXEntrySmali(package);
        }

     


        /// <summary>
        /// 微信 smali 植入
        /// </summary>
        /// <param name="package">植入包名</param>
        public void WXEntrySmali(string package) {
              
            wxSmali = envConfig.targetPath + game + @"\" + channel + @"\smali\com\sandglass\game\WXEntryActivity.smali";
            string tempContent = FileUtil.replaceContent(FileUtil.getContent(wxSmali), @"com/sandglass/game", package.Replace(".", @"/") + @"/wxapi");
            string targetFolder = envConfig.targetPath + game + @"\" + channel + @"\smali" + package.Replace(".", @"\") + @"\wxapi\";
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            string targetPath = targetFolder + @"WXEntryActivity.smali";
            FileUtil.writeContent(targetPath, tempContent);
        
        }


        private void insertAndroidManifest(Dictionary<string, string> notes)
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            //  MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            //遍历添加meta-data
            foreach (KeyValuePair<string, string> pair in notes)
            {
                addMeataData(pt, xmlTmpFile, pair.Key, pair.Value);
            }

            pt.Close();
            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }
        private void addMeataData(PpHelper pt, string xmlTmpFile, string name, string valueKey)
        {
            string value = pt.GetPropertiesText(valueKey);
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("android" + FileUtil.colon + "name", name);
            meatas.Add("android" + FileUtil.colon + "value", value);
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", meatas);
        }

    }
}
