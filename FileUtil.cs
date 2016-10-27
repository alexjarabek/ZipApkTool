using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace sandGlass
{
    class FileUtil
    {

        public static string colon = "SPRcolonSPR";
        public static string getPropertiesFile(string channel, string game)
        {
            return envConfig.gameConfig + channel + "_" + game + envConfig.configExt;
        }

        public static string getContent(string path)
        {
            GC.Collect();
            //   return File.ReadAllText(path,Encoding.UTF8);
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string txtCnt = null;
            txtCnt = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return txtCnt;
        }
        public static void writeContent(string path, string content)
        {
            GC.Collect();
            // File.WriteAllText(path, content,Encoding.UTF8);

            if (!File.Exists(path))
                File.Create(path).Close();
            File.SetAttributes(path, FileAttributes.Normal);
            FileStream _file = new FileStream(@path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (_file == null)
            {
                MessageBox.Show("文件写入失败，请重试！");
                return;
            }
            using (StreamWriter writer1 = new StreamWriter(_file))
            {
                writer1.Write(content);
                writer1.Flush();
                writer1.Close();
                _file.Close();
            }

        }
        public static string getSdkVersionBySmali(string path)
        {         
            GC.Collect();
            if (!File.Exists(path))
            {
               return "未检测到";
            }

            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string flag = "";
            string version = "";

            while (!sr.EndOfStream)
            {
                string line =sr.ReadLine();
                if (line.Contains("SDK_FLAG"))
                {
                    flag = line.Substring(line.IndexOf('=') + 1).Replace("\"", ""); 
                }

                if (line.Contains("SDK_VERSIOND"))
                {
                    sr.Close();
                    fs.Close();
                    version = line.Substring(line.IndexOf('=') + 1).Replace("\"", "");
                    if(flag!="")
                    return version + " -" + flag;
                    return version;
                }

               
            }

            return "未检测到";
        }
        public static string replaceContent(string str, string oldStr, string newStr)
        {
            str = str.Replace(oldStr, newStr);
            return str;
        }
        public static string replaceXmlColon(string str)
        {
            return FileUtil.replaceContent(str, FileUtil.colon, ":");
        }
        public static string setXmlColon(string str)
        {
            return FileUtil.replaceContent(str, ":", FileUtil.colon);
        }
        public static string xmlRestore(string str)
        {
            str = FileUtil.replaceContent(str, "mergePath=\"manifest/application\"", "");
            str = FileUtil.replaceContent(str, "mergePath = \"manifest/application\"", "");
            str = FileUtil.replaceContent(str, "mergePath = \"manifest/application\"", "");
            str = FileUtil.replaceContent(str, "mergePath=\"manifest\" mergeMethod=\"insert\"", "");
            str = FileUtil.replaceContent(str, "mergeMethod=\"update\"", "");
            return FileUtil.replaceContent(str, "mergeMethod=\"insert\"", "");
        }
        public static void mkdirDecompileGame(string game)
        {
            if (!Directory.Exists(game))
            {
                Directory.CreateDirectory(game);
            }
        }
        public static void deleteGame(string game)
        {
            DirectoryInfo di = new DirectoryInfo(game);
            di.Delete(true);
        }
        public static void deleteFile(string path)
        {
            try
            {
                @File.Delete(@path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public static bool checkFile(string fileName)
        {
            bool flag = false;
            if (File.Exists(fileName))
                flag = true;
            return flag;
        }
        public static string uploadFile(string fileName, string safeName)
        {
            string upPath = envConfig.upPath;
            if (!Directory.Exists(upPath))
            {
                Directory.CreateDirectory(upPath);
            }
            string upTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string setFilePath = upPath + upTime + "_" + safeName;
            File.Copy(fileName, setFilePath);
            return setFilePath;
        }

        public static void copy(string from, string to)
        {
            if(!File.Exists(from)){
                return;
            }
            if(from.Equals(to)){
                return;
            }
            File.Copy(from, to, true);
            
        }

        public static void copyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);
            foreach (string sub in Directory.GetDirectories(from))
                copyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");
            foreach (string file in Directory.GetFiles(from))
                File.Copy(file, to + Path.GetFileName(file), true);
        }
        public static int getFilesNum(string path)
        {
            int count = 0;
            String[] sub = Directory.GetDirectories(path);
            for (int i = 0; i < sub.Length; i++)
            {
                count += getFilesNum(sub[i]);
            }
            String[] files = Directory.GetFiles(path);
            count += files.Length;
            return count;
        }
        public static List<string> getFolders(string directory)
        {
            List<string> subDirectories = new List<string>();
            if (string.IsNullOrEmpty(directory))
            {
                return subDirectories;
            }

            string[] directories = Directory.GetDirectories(directory);
            foreach (string subDirectory in directories)
            {
                subDirectories.Add(subDirectory);
            }
            return subDirectories;
        }
        public static void deleteFolder(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                DirectoryInfo[] childs = folder.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    try
                    {
                        child.Delete(true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                try
                {
                    folder.Delete(true);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

              
            }
        }
        public static void getAllFile(string path)
        {
            //  string path = envConfig.channelPath + @"\apkBase\res\values*";
            DirectoryInfo folders = new DirectoryInfo(path);
            List<string> FileList = new List<string>();
            FileInfo[] allFile = folders.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                FileList.Add(fi.Name);
            }
            DirectoryInfo[] allDir = folders.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                //  getAllFile(d);
            }
            //  return FileList;
        }
        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <param name="postedfile">原图</param>
        /// <param name="savepath">缩略图存放地址</param>
        /// <param name="targetwidth">指定的最大宽度</param>
        /// <param name="targetheight">指定的最大高度</param>
        public static void setImg(string initpath, string savepath, double targetwidth, double targetheight)
        {
            string dir = Path.GetDirectoryName(savepath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            Image initimage = Image.FromFile(initpath);
            if (initimage.Width <= targetwidth && initimage.Height <= targetheight)
            {

                if (!savepath.Equals(initpath))
                {
                    initimage.Save(savepath, System.Drawing.Imaging.ImageFormat.Png);
                }
                              
            }
            else
            {
                double newwidth = initimage.Width;
                double newheight = initimage.Height;

                if (initimage.Width > initimage.Height || initimage.Width == initimage.Height)
                {
                    if (initimage.Width > targetwidth)
                    {
                        newwidth = targetwidth;
                        newheight = initimage.Height * (targetwidth / initimage.Width);
                    }
                }
                else
                {
                    if (initimage.Height > targetheight)
                    {
                        newheight = targetheight;
                        newwidth = initimage.Width * (targetheight / initimage.Height);
                    }
                }
                Image newimage = new Bitmap((int)newwidth, (int)newheight);
                Graphics newg = Graphics.FromImage(newimage);
                newg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                newg.Clear(Color.White);
                newg.DrawImage(initimage, new Rectangle(0, 0, newimage.Width, newimage.Height), new Rectangle(0, 0, initimage.Width, initimage.Height), GraphicsUnit.Pixel);
                newimage.Save(savepath, System.Drawing.Imaging.ImageFormat.Png);
                //释放资源
                newg.Dispose();
                newimage.Dispose();
                initimage.Dispose();
            }
        }

        public static string md5(string str)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}
