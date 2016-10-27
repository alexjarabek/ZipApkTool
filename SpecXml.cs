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
    class SpecXml
    {
        public string game, channel, properties, gameXml, gameName,wxSmali;

        public List<string> needMergePublic = new List<string>() { "null" };// 弃用


        //YY
        public void YYxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            //  MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsProvider = new Dictionary<string, string>();
            attrsProvider.Add("android" + FileUtil.colon + "name", "com.yy.gamesdk.provider.YYDataProvider");
            attrsProvider.Add("android" + FileUtil.colon + "authorities", packageName + ".yygamesdkprovider");
            attrsProvider.Add("android" + FileUtil.colon + "exported", "true");
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "provider", attrsProvider);   //添加 provider


            Dictionary<string, string> attrsService = new Dictionary<string, string>();
            attrsService.Add("android" + FileUtil.colon + "name", "com.tencent.android.tpush.rpc.XGRemoteService");
            attrsService.Add("android" + FileUtil.colon + "exported", "true");
            string addChildName = "intent-filter";
            Dictionary<string, string> attrsAdd = new Dictionary<string, string>();
            // attrsAdd.Add("hello", "world");
            string addChildName1 = "action";
            Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
            attrsAdd1.Add("android" + FileUtil.colon + "name", packageName + ".PUSH_ACTION");
            string textTxt = null;
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "service", attrsService, addChildName, attrsAdd, addChildName1, attrsAdd1, textTxt);  //添加service

            //定义需要添加的id
            string accId = pt.GetPropertiesText("metaId");
            Dictionary<string, string> metaIds = new Dictionary<string, string>();
            metaIds.Add("android" + FileUtil.colon + "name", "XG_V2_ACCESS_ID");
            metaIds.Add("android" + FileUtil.colon + "value", accId);
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", metaIds);  //添加id meta

            //定义需要添加的key
            string accKey = pt.GetPropertiesText("metaKey");
            Dictionary<string, string> metaKeys = new Dictionary<string, string>();
            metaKeys.Add("android" + FileUtil.colon + "name", "XG_V2_ACCESS_KEY");
            metaKeys.Add("android" + FileUtil.colon + "value", accKey);
            XmlHelper.XmlInsertMultiElement(xmlTmpFile, "manifest/application", "meta-data", metaKeys);  //添加key meta

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
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

            //Dictionary<string, string> activity = new Dictionary<string, string>();
            //activity.Add("android" + FileUtil.colon + "name", packageName + ".wxapi.WXPayEntryActivity");
            //activity.Add("android" + FileUtil.colon + "configChanges", "keyboardHidden|orientation");
            //activity.Add("android" + FileUtil.colon + "screenOrientation", "portrait");
            //activity.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //activity.Add("android" + FileUtil.colon + "exported", "true");
            //activity.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Translucent");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", activity);

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
            //string readPath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + game + envConfig.configExt;
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\" + "passport" + envConfig.configExt;
            //PpHelper writeHelper = new PpHelper(writePath);
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
        //悠悠村
        public void PPSxml()
        {           
            //=============================修改Properties========================================           
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\zConfig\" + "pps_packetid" + envConfig.configExt;
            PpHelper readHlper = new PpHelper(properties);
            string channel_id = readHlper.GetPropertiesText("id");
            string contentStr = "qudaoId=" + channel_id + "";
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
            string contentStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"+"\r\n";
            contentStr += "<Cpid value=\"" + pt.GetPropertiesText("cpid") + "\"></Cpid>" + "\r\n";

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


            addMeataData(pt, xmlTmpFile, "QHOPENSDK_APPKEY", "appkey");
            addMeataData(pt, xmlTmpFile, "QHOPENSDK_APPID", "appid");

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
            MessageBox.Show(xmlTmpFile);
            FileUtil.writeContent(xmlTmpFile, tmpXml);


            addMeataData(pt, xmlTmpFile, "ANQUSDK_CPUIN", "cId");
            addMeataData(pt, xmlTmpFile, "ANQUSDK_APPID", "appId");
            addMeataData(pt, xmlTmpFile, "ANQUSDK_APPKEY", "appkey");

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

        //联想
        //public void LENOVOxml()
        //{
        //    PpHelper pt = new PpHelper(properties);
        //    string packageName = pt.GetPropertiesText("package");
        //    string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
        //    string xmlTmpFile = gameXml + @".tmp";
        //    MessageBox.Show(xmlTmpFile);
        //    FileUtil.writeContent(xmlTmpFile, tmpXml);

        //    //------------------------------------------------------------------------------------------
        //    Dictionary<string, string> attrsService = new Dictionary<string, string>();
        //    attrsService.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.gamesdk.receiver.GameSdkReceiver");
        //    attrsService.Add("android" + FileUtil.colon + "permission", "com.lenovo.lsf.device.permission.MESSAGE");
        //    string addChildName = "intent-filter";
        //    Dictionary<string, Dictionary<string, string>> attrsAdd = new Dictionary<string, Dictionary<string, string>>();

        //    string addChildName1 = "action";
        //    Dictionary<string, string> attrsAdd1 = new Dictionary<string, string>();
        //    attrsAdd1.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.gamesdk.receiver.GameSdkReceiver");

        //    string addChildName2 = "category";
        //    Dictionary<string, string> attrsAdd2 = new Dictionary<string, string>();
        //    attrsAdd2.Add("android" + FileUtil.colon + "name", "android.intent.category.DEFAULT");

        //    string addChildName3 = "action";
        //    Dictionary<string, string> attrsAdd3 = new Dictionary<string, string>();
        //    attrsAdd3.Add("android" + FileUtil.colon + "name", pt.GetPropertiesText("appId"));

        //    attrsAdd.Add(addChildName1, attrsAdd1);
        //    attrsAdd.Add(addChildName2, attrsAdd2);
        //    attrsAdd.Add(addChildName3, attrsAdd3);
        //    XmlHelper.XmlInsertMultiElementForSpec1(xmlTmpFile, "manifest/application", "receiver", attrsService, addChildName, attrsAdd);  //添加receiver
        //    //------------------------------------------------------------------------------------------
        //    Dictionary<string, string> attrsReceiver1 = new Dictionary<string, string>();
        //    attrsReceiver1.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.gamesdk.receiver.GameSdkAndroidLReceiver");
        //    string filterName1 = "intent-filter";
        //    Dictionary<string, Dictionary<string, string>> filterReceiver1 = new Dictionary<string, Dictionary<string, string>>();

        //    string filterChildName1 = "action";
        //    Dictionary<string, string> filterAdd1 = new Dictionary<string, string>();
        //    attrsAdd1.Add("android" + FileUtil.colon + "name", "com.lenovo.lsf.device.ANDROID_L_MSG");

        //    string filterChildName2 = "category";
        //    Dictionary<string, string> filterAdd2 = new Dictionary<string, string>();
        //    attrsAdd2.Add("android" + FileUtil.colon + "name", packageName);

        //    filterReceiver1.Add(filterChildName1, filterAdd1);
        //    filterReceiver1.Add(filterChildName2, filterAdd2);
        //    XmlHelper.XmlInsertMultiElementForSpec1(xmlTmpFile, "manifest/application", "receiver", attrsReceiver1, filterName1, filterReceiver1);  //添加receiver
        //    //------------------------------------------------------------------------------------------
        //    addMeataData(pt, xmlTmpFile, "lenovo.open.appid", "appId");

        //    string tempXml = FileUtil.getContent(xmlTmpFile);
        //    tempXml = FileUtil.xmlRestore(tempXml);
        //    tmpXml = FileUtil.replaceXmlColon(tempXml);
        //    FileUtil.writeContent(gameXml, tmpXml);
        //    File.Delete(xmlTmpFile);
        //}

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

        //泡椒
        public void PAOJIAOWANGxml()
        {
            PpHelper pt = new PpHelper(properties);
            string packageName = pt.GetPropertiesText("package");
            string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            string xmlTmpFile = gameXml + @".tmp";
            FileUtil.writeContent(xmlTmpFile, tmpXml);

            Dictionary<string, string> attrsPermission = new Dictionary<string, string>();
            attrsPermission.Add("android" + FileUtil.colon + "name", packageName + ".permission.JPUSH_MESSAGE");
            attrsPermission.Add("android" + FileUtil.colon + "protectionLevel", "signature");
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest", "permission", attrsPermission);

            Dictionary<string, string> userPermission = new Dictionary<string, string>();
            userPermission.Add("android" + FileUtil.colon + "name", packageName + ".permission.JPUSH_MESSAGE");
            XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest", "uses-permission", userPermission);

            addMeataData(pt, xmlTmpFile, "JPUSH_APPKEY", "appKey");

            //string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";
            //Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
            //screenOrientation1.Add("android" + FileUtil.colon + "name", "com.pipaw.pipawpay.PipawPayActivity");
            //screenOrientation1.Add("android" + FileUtil.colon + "configChanges", "keyboardHidden|orientation|screenSize");
            //screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation1.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar");
            //screenOrientation1.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustResize");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

            //Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
            //screenOrientation2.Add("android" + FileUtil.colon + "name", "com.pipaw.pipawpay.PipawUserActivity");
            //screenOrientation2.Add("android" + FileUtil.colon + "configChanges", "keyboardHidden|orientation|screenSize");
            //screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation2.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }

        //草花
        public void CAOHUAxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CH_AppID", "appId");
            meatas.Add("CH_AppKey", "appKey");
            insertAndroidManifest(meatas);

            //PpHelper pt = new PpHelper(properties);
            //string packageName = pt.GetPropertiesText("package");
            //string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            //string xmlTmpFile = gameXml + @".tmp";
            //FileUtil.writeContent(xmlTmpFile, tmpXml);

            //string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";

            //Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
            //screenOrientation1.Add("android" + FileUtil.colon + "name", "com.chsdk.view.payment.PayActivity");
            //screenOrientation1.Add("android" + FileUtil.colon + "configChanges", "orientation|screenSize");
            //screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation1.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Black.NoTitleBar");
            //screenOrientation1.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustPan");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

            //Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
            //screenOrientation2.Add("android" + FileUtil.colon + "name", "com.chsdk.view.usercenter.UserCenter");
            //screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation2.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Black.NoTitleBar");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

            //addMeataData(pt, xmlTmpFile, "CH_AppID", "appId");
            //addMeataData(pt, xmlTmpFile, "CH_AppKey", "appKey");

            //string tempXml = FileUtil.getContent(xmlTmpFile);
            //tempXml = FileUtil.xmlRestore(tempXml);
            //tmpXml = FileUtil.replaceXmlColon(tempXml);
            //FileUtil.writeContent(gameXml, tmpXml);
            //File.Delete(xmlTmpFile);
        }

        //数游
        public void SHUYOUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_PARTNERID", "partnrId");
            meatas.Add("CY_GAMEID", "gameId");
            insertAndroidManifest(meatas);

            //PpHelper pt = new PpHelper(properties);
            //string packageName = pt.GetPropertiesText("package");
            //string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            //string xmlTmpFile = gameXml + @".tmp";
            //FileUtil.writeContent(xmlTmpFile, tmpXml);

            //string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";

            //Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
            //screenOrientation1.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.FastRegister");
            //screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

            //Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
            //screenOrientation2.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.Login");
            //screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

            //Dictionary<string, string> screenOrientation3 = new Dictionary<string, string>();
            //screenOrientation3.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.GetPwd");
            //screenOrientation3.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation3);

            //Dictionary<string, string> screenOrientation4 = new Dictionary<string, string>();
            //screenOrientation4.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.Bound");
            //screenOrientation4.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation4);

            //Dictionary<string, string> screenOrientation5 = new Dictionary<string, string>();
            //screenOrientation5.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.PayActivity");
            //screenOrientation5.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation5);

            //Dictionary<string, string> screenOrientation6 = new Dictionary<string, string>();
            //screenOrientation6.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.PtbFinishActivity");
            //screenOrientation6.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation6);

            //Dictionary<string, string> screenOrientation7 = new Dictionary<string, string>();
            //screenOrientation7.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.ShengcardActivity");
            //screenOrientation7.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation7);

            //Dictionary<string, string> screenOrientation8 = new Dictionary<string, string>();
            //screenOrientation8.Add("android" + FileUtil.colon + "name", "sy07073.mobile.game.sdk.activity.LoadPage");
            //screenOrientation8.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation8);

            //addMeataData(pt, xmlTmpFile, "CY_PARTNERID", "partnrId");
            //addMeataData(pt, xmlTmpFile, "CY_GAMEID", "gameId");

            //string tempXml = FileUtil.getContent(xmlTmpFile);
            //tempXml = FileUtil.xmlRestore(tempXml);
            //tmpXml = FileUtil.replaceXmlColon(tempXml);
            //FileUtil.writeContent(gameXml, tmpXml);
            //File.Delete(xmlTmpFile);
        }

        //游龙
        public void YOULONGxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("PID", "id");
            meatas.Add("PKEY", "key");
            insertAndroidManifest(meatas);

            //PpHelper pt = new PpHelper(properties);
            //string packageName = pt.GetPropertiesText("package");
            //string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            //string xmlTmpFile = gameXml + @".tmp";
            //FileUtil.writeContent(xmlTmpFile, tmpXml);

            //string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";

            //Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
            //screenOrientation1.Add("android" + FileUtil.colon + "name", "com.yx19196.activity.YXLoginActivity");
            //screenOrientation1.Add("android" + FileUtil.colon + "background", "@color/transparent");
            //screenOrientation1.Add("android" + FileUtil.colon + "label", "@string/app_name");
            //screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation1.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Translucent.NoTitleBar");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

            //Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
            //screenOrientation2.Add("android" + FileUtil.colon + "name", "com.yx19196.activity.YXRegisterByPhoneActivity");
            //screenOrientation2.Add("android" + FileUtil.colon + "label", "@string/app_name");
            //screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation2.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Translucent.NoTitleBar");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

            //Dictionary<string, string> screenOrientation3 = new Dictionary<string, string>();
            //screenOrientation3.Add("android" + FileUtil.colon + "name", "com.yx19196.activity.YXOneKeyRegisterActivity");
            //screenOrientation3.Add("android" + FileUtil.colon + "label", "@string/app_name");
            //screenOrientation3.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Translucent.NoTitleBar");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation3);

            //addMeataData(pt, xmlTmpFile, "PID", "id");
            //addMeataData(pt, xmlTmpFile, "PKEY", "key");

            //string tempXml = FileUtil.getContent(xmlTmpFile);
            //tempXml = FileUtil.xmlRestore(tempXml);
            //tmpXml = FileUtil.replaceXmlColon(tempXml);
            //FileUtil.writeContent(gameXml, tmpXml);
            //File.Delete(xmlTmpFile);
        }
        //XY
        public void XYxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("XYSDK_APPKEY", "appKey");
            meatas.Add("XYSDK_APPID", "appId");
            insertAndroidManifest(meatas);

            //PpHelper pt = new PpHelper(properties);
            //string packageName = pt.GetPropertiesText("package");
            //string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            //string xmlTmpFile = gameXml + @".tmp";
            //FileUtil.writeContent(xmlTmpFile, tmpXml);

            //string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";

            //Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
            //screenOrientation1.Add("android" + FileUtil.colon + "name", "com.unionpay.uppay.PayActivity");
            //screenOrientation1.Add("android" + FileUtil.colon + "label", "@string/app_name");
            //screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation1.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|screenSize");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

            //Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
            //screenOrientation2.Add("android" + FileUtil.colon + "name", "com.kingnet.xyplatform.source.activity.XYLoginActivity");
            //screenOrientation2.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustUnspecified|stateHidden");
            //screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation2.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboard|keyboardHidden|screenSize");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

            //Dictionary<string, string> screenOrientation3 = new Dictionary<string, string>();
            //screenOrientation3.Add("android" + FileUtil.colon + "name", "com.alipay.sdk.app.H5PayActivity");
            //screenOrientation3.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|navigation");
            //screenOrientation3.Add("android" + FileUtil.colon + "exported", "false");
            //screenOrientation3.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustResize|stateHidden");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation3);

            //addMeataData(pt, xmlTmpFile, "XYSDK_APPKEY", "appKey");
            //addMeataData(pt, xmlTmpFile, "XYSDK_APPID", "appId");

            //string tempXml = FileUtil.getContent(xmlTmpFile);
            //tempXml = FileUtil.xmlRestore(tempXml);
            //tmpXml = FileUtil.replaceXmlColon(tempXml);
            //FileUtil.writeContent(gameXml, tmpXml);
            //File.Delete(xmlTmpFile);
        }
        //当乐
        //public void DANGLExml()
        //{
        //    PpHelper pt = new PpHelper(properties);
        //    string packageName = pt.GetPropertiesText("package");
        //    string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
        //    string xmlTmpFile = gameXml + @".tmp";
        //    FileUtil.writeContent(xmlTmpFile, tmpXml);

        //    string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";

        //    Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
        //    screenOrientation1.Add("android" + FileUtil.colon + "name", "com.downjoy.activity.SdkActivity");
        //    screenOrientation1.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.Translucent.NoTitleBar");
        //    screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
        //    screenOrientation1.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|screenSize");
        //    screenOrientation1.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustResize");
        //    XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

        //    Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
        //    screenOrientation2.Add("android" + FileUtil.colon + "name", "com.unionpay.uppay.PayActivity");
        //    screenOrientation2.Add("android" + FileUtil.colon + "label", "@string/app_name");
        //    screenOrientation2.Add("android" + FileUtil.colon + "excludeFromRecents", "true");
        //    screenOrientation2.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustResize");
        //    screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
        //    screenOrientation2.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|screenSize");
        //    XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

        //    string tempXml = FileUtil.getContent(xmlTmpFile);
        //    tempXml = FileUtil.xmlRestore(tempXml);
        //    tmpXml = FileUtil.replaceXmlColon(tempXml);
        //    FileUtil.writeContent(gameXml, tmpXml);
        //    File.Delete(xmlTmpFile);
        //}

        //搜狗
        public void SOUGOUxml()
        {
            //PpHelper pt = new PpHelper(properties);
            //string packageName = pt.GetPropertiesText("package");
            //string tmpXml = FileUtil.setXmlColon(FileUtil.getContent(gameXml));
            //string xmlTmpFile = gameXml + @".tmp";
            //FileUtil.writeContent(xmlTmpFile, tmpXml);

            //string screen = "1".Equals(pt.GetPropertiesText("screenOrientation")) ? "landscape" : "portrait";

            //Dictionary<string, string> screenOrientation1 = new Dictionary<string, string>();
            //screenOrientation1.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouProgressDialog");
            //screenOrientation1.Add("android" + FileUtil.colon + "theme", "@style/Activity_MyDialog");
            //screenOrientation1.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation1.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation1);

            //Dictionary<string, string> screenOrientation2 = new Dictionary<string, string>();
            //screenOrientation2.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouLoginActivity");
            //screenOrientation2.Add("android" + FileUtil.colon + "theme", "@style/Activity_MyDialog");
            //screenOrientation2.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation2.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation2.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation2);

            //Dictionary<string, string> screenOrientation3 = new Dictionary<string, string>();
            //screenOrientation3.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouRegActivity");
            //screenOrientation3.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation3.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation3.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustPan");
            //screenOrientation3.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation3);

            //Dictionary<string, string> screenOrientation4 = new Dictionary<string, string>();
            //screenOrientation4.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouPayActivity");
            //screenOrientation4.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation4.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation4.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "adjustPan");
            //screenOrientation4.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation4);

            //Dictionary<string, string> screenOrientation5 = new Dictionary<string, string>();
            //screenOrientation5.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouWebViewActivity");
            //screenOrientation5.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation5.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation5.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustUnspecified");
            //screenOrientation5.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation5);

            //Dictionary<string, string> screenOrientation6 = new Dictionary<string, string>();
            //screenOrientation6.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouThirdLoginActivity");
            //screenOrientation6.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation6.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation6.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustResize");
            //screenOrientation6.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation6);

            //Dictionary<string, string> screenOrientation7 = new Dictionary<string, string>();
            //screenOrientation7.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouAliwapActivity");
            //screenOrientation7.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation7.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation7.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustResize");
            //screenOrientation7.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation7);

            //Dictionary<string, string> screenOrientation8 = new Dictionary<string, string>();
            //screenOrientation8.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouPayRecordActivity");
            //screenOrientation8.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation8.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustResize");
            //screenOrientation8.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation8);

            //Dictionary<string, string> screenOrientation9 = new Dictionary<string, string>();
            //screenOrientation9.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouFeedBackActivity");
            //screenOrientation9.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation9.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation9.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustResize");
            //screenOrientation9.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation9);

            //Dictionary<string, string> screenOrientation10 = new Dictionary<string, string>();
            //screenOrientation10.Add("android" + FileUtil.colon + "name", "com.sogou.gamecenter.sdk.SogouFeedBackRecordActivity");
            //screenOrientation10.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
            //screenOrientation10.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            //screenOrientation10.Add("android" + FileUtil.colon + "screenOrientation", screen);
            //screenOrientation3.Add("android" + FileUtil.colon + "windowBackground", "@null");
            //screenOrientation3.Add("android" + FileUtil.colon + "windowSoftInputMode", "stateHidden|adjustResize");
            //screenOrientation10.Add("android" + FileUtil.colon + "configChanges", "mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|screenLayout|fontScale|orientation");
            //XmlHelper.XmlInsertMultiElementForSpec(xmlTmpFile, "manifest/application", "activity", screenOrientation10);

            //addMeataData(pt, xmlTmpFile, "appid", "appId");
            //addMeataData(pt, xmlTmpFile, "appkey", "appKey");

            //string tempXml = FileUtil.getContent(xmlTmpFile);
            //tempXml = FileUtil.xmlRestore(tempXml);
            //tmpXml = FileUtil.replaceXmlColon(tempXml);
            //FileUtil.writeContent(gameXml, tmpXml);
            //File.Delete(xmlTmpFile);

            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("appid", "appId");
            meatas.Add("appkey", "appKey");
            insertAndroidManifest(meatas);
        }

        //17wo
        public void PLAYGCxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("APP_ID", "appId");
            meatas.Add("APP_KEY", "appKey");
            meatas.Add("APP_SECRET", "appSecret");
            insertAndroidManifest(meatas);
        }

        //热酷
        public void REKOOxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("REKOO_GAMEKEY", "gameKey");
            meatas.Add("RK_APPID", "appId");
            meatas.Add("RK_GAMEID", "gameId");
            insertAndroidManifest(meatas);
        }

        //云点联动
        public void CLOUDPOINTxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CLOUDPOINT_APPID", "appId");
            meatas.Add("CLOUDPOINT_APPKEY", "appKey");
            insertAndroidManifest(meatas);
        }

        //飞流
        public void FEILIUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("FLGAMESDK_APP_ID", "appId");
            meatas.Add("FLGAMESDK_APP_KEY", "appKey");
            meatas.Add("FLGAMESDK_COMPANY_ID", "companyId");
            meatas.Add("FLGAMESDK_COOP_ID", "coopId");
            insertAndroidManifest(meatas);
        }

        //机锋
        public void GFANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("gfan_cpid", "cpId");
            meatas.Add("gfan_pay_appkey", "appKey");
            insertAndroidManifest(meatas);
        }

        //职内网络
        public void ZNNETxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("YYJIA_COOPID", "coopId");
            meatas.Add("YYJIA_APPID", "appId");
            insertAndroidManifest(meatas);
        }

        //点点玩
        public void DIANDIANWANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appId");
            meatas.Add("CY_GAMEID", "gameId");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
        }

        //猎宝
        public void LIEBAOxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("LB_APPID", "appId");
            meatas.Add("LB_GAMEID", "gameId");
            meatas.Add("LB_AGENT", "agent");
            insertAndroidManifest(meatas);
        }

        //绿壳
        public void LVKExml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appId");
            meatas.Add("CY_GAMEID", "gameId");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
        }

        //口袋
        public void KOUDAIxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appId");
            meatas.Add("CY_GAMEID", "gameId");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
        }

        //众合
        public void ZHONGHExml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appId");
            meatas.Add("CY_GAMEID", "gameId");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
        }

        //绿岸
        public void GREENCOASTxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("LVANSDK_APPID", "appId");
            meatas.Add("LVANSDK_APPKEY", "appKey");
            meatas.Add("LVANSDK_PRIVATEKEY", "privateKey");
            insertAndroidManifest(meatas);
        }

        //新浪
        public void SINAxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("sina_game_appkey", "appKey");
            meatas.Add("sina_game_redirect", "redirect");
            insertAndroidManifest(meatas);
        }

        //手盟
        public void SUNIONxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("UMENG_APPKEY", "appKey");
            meatas.Add("UMENG_CHANNEL", "umengChannel");
            meatas.Add("SHOUMENG_APP_ID", "appId");
            meatas.Add("SHOUMENG_GAME_ID", "gameId");
            meatas.Add("SHOUMENG_PACKET_ID", "packetId");
            meatas.Add("SHOUMENG_LOGIN_KEY", "loginKey");
            meatas.Add("SHOUMENG_PLATFORM_NAME", "platfromName");
            insertAndroidManifest(meatas);
        }

        //宝软
        public void BAORUANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("BAORUAN_APPID", "appId");
            meatas.Add("BAORUAN_UNIQUEKEY", "uniqueKey");
            meatas.Add("BAORUAN_CHANNEL", "baoruanChannel");
            meatas.Add("BAORUAN_CID", "cId");
            insertAndroidManifest(meatas);
        }

        //靠谱助手
        public void KAOPUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("KAOPU_APPID", "appId");
            meatas.Add("KAOPU_APPKEY", "appKey");
            meatas.Add("KAOPU_SECRETKEY", "secrtKey");
            meatas.Add("KAOPU_APPVERSION", "appVersion");
            insertAndroidManifest(meatas);

//            {"KP_Channel":"kaopu","ChannelKey":"kpcs","gameName":"cs-游戏1","screenType":"2","fullScreen":"true","param":""}

            PpHelper pt = new PpHelper(properties);
            string writePath = envConfig.targetPath + @game + @"\" + channel + @"\assets\kaopu_game_config.json";
            string contentStr = "{\"KP_Channel\":\"kaopu\",\"ChannelKey\":\"kaopu\",\"gameName\":\"";
            contentStr += pt.GetPropertiesText("gameName");
            contentStr += "\",\"screenType\":\"2\",\"fullScreen\":\"true\",\"param\":\"\"}";
            pt.Close();
            FileUtil.writeContent(writePath, contentStr);
        }

        //酷我
        public void KUWOxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("gid", "gid");
            insertAndroidManifest(meatas);
        }
        //联旭科技
        public void LIANXUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("BaiduMobAd_CHANNEL", "lxChannel");
            meatas.Add("BaiduMobAd_STAT_ID", "statId");
            insertAndroidManifest(meatas);
        }
        //嗨游
        public void HIYOxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("IHIYO_APP_ID", "appId");
            meatas.Add("IHIYO_APP_KEY", "appKey");
            meatas.Add("RONG_CLOUD_APP_KEY", "rongKey");
            meatas.Add("IHIYO_CHANNEL_ID", "channelId");
            insertAndroidManifest(meatas);
        }
        //海马
        public void HAIMAxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("HMKey", "key");
            insertAndroidManifest(meatas);
        }
        //游戏群
        public void XMWxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("XMWAPPID", "appId");
            meatas.Add("XMWVERSION", "xmwVersion");
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


            addMeataData(pt, xmlTmpFile, "QIKESDK_CPUID", "cpuId");
            addMeataData(pt, xmlTmpFile, "QIKESDK_APPID", "appId");
            addMeataData(pt, xmlTmpFile, "QIKESDK_APPKEY", "appKey");

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
        //卓动
        public void JODOxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("JODO_PAYSDK_CPID", "cpId");
            meatas.Add("JODO_PAYSDK_GAMEID", "gameId");
            meatas.Add("JODO_PAYSDK_EXNET", "exnet");
            meatas.Add("JODO_PAYSDK_CHANNEL", "jodoChannel");
            insertAndroidManifest(meatas);
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

            addMeataData(pt, xmlTmpFile, "zy_app_id", "appId");
            addMeataData(pt, xmlTmpFile, "zy_app_key", "appKey");
            addMeataData(pt, xmlTmpFile, "zy_channel", "zyChannel");

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }

        //优酷
        public void YOUKUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("YKGAME_APPID", "appId");
            meatas.Add("YKGAME_APPNAME", "appName");
            meatas.Add("YKGAME_APPKEY", "appKey");
            meatas.Add("YKGAME_PRIVATEKEY", "privateKey");
            insertAndroidManifest(meatas);
 
        }

        //uc
        public void UCxml()
        {      
            string tmp = FileUtil.getContent(gameXml);
            string tmpXml = FileUtil.replaceContent(tmp, "android:launchMode='singleTask':", "android:launchMode='standard':");
            FileUtil.writeContent(gameXml, tmpXml);
        }
       
        //乐非凡
        public void HUCNxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("appid", "appId");
            insertAndroidManifest(meatas);
        }
        //拇指玩
        public void MUZHIWANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("MZWAPPKEY", "appKey");
            meatas.Add("DEBUG", "debug");
            insertAndroidManifest(meatas);
        }
        //万普
        public void WANPUxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("APP_ID", "appId");
            meatas.Add("APP_PID", "appPid");
            insertAndroidManifest(meatas);
        }
        //笨手机
        public void BENSHOUJIxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("FULIBAO_APPID", "fulibao_appid");
            meatas.Add("FULIBAO_APPKEY", "fulibao_appkey");
            insertAndroidManifest(meatas);
        }
        //找乐助手
        public void ZHAOLEZSxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("PRIVATE_IGKEY_ID", "igkey_id");
            meatas.Add("PRIVATE_IGKEY_CHANNELID", "igkey_channelid");
            meatas.Add("PRIVATE_IGKEY_VERSION", "igkey_version");
            insertAndroidManifest(meatas);
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
            attrsService.Add("android" + FileUtil.colon + "name", "com.weedong.gamesdkplatform.activity.ControlCenterActivity");
            attrsService.Add("android" + FileUtil.colon + "configChanges", "orientation|keyboardHidden|navigation|screenSize");
            attrsService.Add("android" + FileUtil.colon + "launchMode", "singleTask");
            attrsService.Add("android" + FileUtil.colon + "theme", "@android:style/Theme.NoTitleBar.Fullscreen");
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

            addMeataData(pt, xmlTmpFile, "WeeDong_APP_ID", "appId");

            string tempXml = FileUtil.getContent(xmlTmpFile);
            tempXml = FileUtil.xmlRestore(tempXml);
            tmpXml = FileUtil.replaceXmlColon(tempXml);
            FileUtil.writeContent(gameXml, tmpXml);
            File.Delete(xmlTmpFile);
        }

        //奇天乐地
        public void C1WANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("BaiduMobAd_STAT_ID", "statId");
            insertAndroidManifest(meatas);
        }

        public void DOUWANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("dw_appid", "dw_appid");
            meatas.Add("dw_channelid", "dw_channelid");
            meatas.Add("dw_serverid", "dw_serverid");
            meatas.Add("dw_coop", "dw_coop");
            insertAndroidManifest(meatas);
        }
        //快发
        public void KUAIFAxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("HJR_GAMEKEY", "gameKey");
            insertAndroidManifest(meatas);
        }
        //魔萍
        public void MOPINGxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("com.snowfish.appid", "appid");
            meatas.Add("com.snowfish.channelid", "channelid");
            insertAndroidManifest(meatas);
        }
        //安锋
        public void ANFANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("productid", "id");
            insertAndroidManifest(meatas);
        }
        //乐视
        public void LESHIxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("lepay_appid", "appId");
            meatas.Add("lepay_appkey", "appKey");          
            insertAndroidManifest(meatas);
        }

        //虫虫
        public void CHONGCHONGxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("developer_key", "developer_key");
            meatas.Add("app_id", "app_id");
            meatas.Add("DC_APPID", "dc_appid");
            insertAndroidManifest(meatas);
        }
        //齐齐乐
        public void QIQILExml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("FFSDK_CHANNEL", "appid");
            insertAndroidManifest(meatas);
        }
        //YY玩
        public void YAYAWANxml()
        {

            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("yayawan_game_id", "game_id");
            meatas.Add("yayawan_game_key", "game_key");
            meatas.Add("yayawan_game_secret", "game_secret");
            meatas.Add("yayawan_source_id", "source_id");
            insertAndroidManifest(meatas);
        }
        //任心游
        public void RXYxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appid");
            meatas.Add("CY_GAMEID", "gameid");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
        }
        //上海娱嘉
        public void SHYJxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appid");
            meatas.Add("CY_GAMEID", "gameid");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
        }
        //快用
        public void M7659xml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("HI_GAMEID", "gameid");
            meatas.Add("HI_GAMEKEY", "gamekey");
            meatas.Add("GAMESCREEN", "gamescreen");
            insertAndroidManifest(meatas);
        }
        //点点
        public void DDIANxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CY_APPID", "appid");
            meatas.Add("CY_GAMEID", "gameid");
            meatas.Add("CY_AGENT", "agent");
            insertAndroidManifest(meatas);
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

        //古芳
        public void GUFANGxml()
        {
            Dictionary<string, string> meatas = new Dictionary<string, string>();
            meatas.Add("CUUDOO_CHANNEL_CODE", "channel_id");
            meatas.Add("CUUDOO_GAME_CODE", "game_code");
            meatas.Add("CUUDOO_NOTIFY_URL", "notif_url ");
            insertAndroidManifest(meatas);
        }

        //豌豆荚  
        public void WDJxml()
        {
            PpHelper pt = new PpHelper(properties);
            string data = "Wandoujia-PaySdk-"+pt.GetPropertiesText("appKey");
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
            string data = "qwallet"+pt.GetPropertiesText("package");
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

        //腾讯
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
            string tempContent = FileUtil.replaceContent(FileUtil.getContent(wxSmali), @"com/sandglass/game", package.Replace(".", @"/"));
            string targetFolder = envConfig.targetPath + game + @"\" + channel  + @"\" + package.Replace(".", @"\") + @"\";
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            string    targetPath =targetFolder + @"WXEntryActivity.smali";
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
