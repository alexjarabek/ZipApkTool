using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;   


namespace sandGlass
{
    class ImageCut
    {
        public enum ImagePosition
        {
            LeftTop,        //左上    1
            LeftBottom,    //左下  5 
            RightTop,       //右上  3 
            RigthBottom,  //右下   7
            TopMiddle,     //顶部居中 2 
            BottomMiddle, //底部居中   6
            Center,           //中心   4
            AllIn  //平铺
        }
        /// <summary>   
        /// 添加图片水印   
        /// </summary>   
        /// <param name="sourcePicture">源图片文件名</param>   
        /// <param name="waterImage">水印图片文件名</param>   
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>   
        /// <param name="position">位置</param>   
        /// <param name="PicturePath" >图片的路径</param>   
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>   
        public static string DrawImage(string sourcePicture,
                                          string waterImage,
                                          float alpha,
                                          ImagePosition position,
                                          string targetImg,int tWidth=512,int tHeight=512)
        {           
            //   
            // 判断参数是否有效   
            //   
            if (sourcePicture == string.Empty || waterImage == string.Empty || alpha == 0.0 || targetImg == string.Empty)
            {
                MessageBox.Show("不存在文件");
               return sourcePicture;
            }

            //   
            // 源图片，水印图片全路径   
            //   
            string sourcePictureName = sourcePicture;
            string waterPictureName = waterImage;
            string fileSourceExtension = System.IO.Path.GetExtension(sourcePictureName).ToLower();
            string fileWaterExtension = System.IO.Path.GetExtension(waterPictureName).ToLower();
            //   
            // 判断文件是否存在,以及类型是否正确   
            //   
            if (System.IO.File.Exists(sourcePictureName) == false ||
                System.IO.File.Exists(waterPictureName) == false || (
                fileSourceExtension != ".gif" &&
                fileSourceExtension != ".jpg" &&
                fileSourceExtension != ".png") || (
                fileWaterExtension != ".gif" &&
                fileWaterExtension != ".jpg" &&
                fileWaterExtension != ".png")
                )
            {
                return sourcePicture;
            }


            //   
            // 将需要加上水印的图片装载到Image对象中   
            //   
            Image imgPhoto = Image.FromFile(sourcePictureName);
            
            //   
            // 确定其长宽   
            //   
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //   
            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。   
            //   
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //   
            // 设定分辨率   
            //    
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            bmPhoto.MakeTransparent();
            //   
            // 定义一个绘图画面用来装载位图   
            //   
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            
            
            //   
            //于水印是图片，需要定义一个Image来装载它   
            //   
            Image imgWatermark = new Bitmap(waterPictureName);
            //   
            // 获取水印图片的高度和宽度   
            //   
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。   
            // 成员名称   说明    
            // AntiAlias      指定消除锯齿的呈现。     
            // Default        指定不消除锯齿。     
            // HighQuality  指定高质量、低速度呈现。     
            // HighSpeed   指定高速度、低质量呈现。     
            // Invalid        指定一个无效模式。     
            // None          指定不消除锯齿。    
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
     //       grPhoto.Clear(Color.White); 
            //   
            // 第一次描绘,底图描绘在绘图画面上   
            //   
            grPhoto.DrawImage(imgPhoto,
                                        new Rectangle(0, 0, phWidth, phHeight),
                                        0,
                                        0,
                                        phWidth,
                                        phHeight,
                                        GraphicsUnit.Pixel);

            //   
            // 装载水印图片。并设定其分辨率   
            //   
            Bitmap bmWatermark = new Bitmap(bmPhoto);
             bmWatermark.MakeTransparent();
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //   
            // 继续，将水印图片装载到一个绘图画面grWatermark   
            //   
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //   
            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。   
            //          
            ImageAttributes imageAttributes = new ImageAttributes();

            //   
            //Colormap: 定义转换颜色的映射   
            //   
            ColorMap colorMap = new ColorMap();
 
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {    
           new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色   
           new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色   
           new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色          
           new float[] {0.0f,  0.0f,  0.0f,  alpha, 0.0f}, //透明度        
           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//   

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。   
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。   
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.SkipGrays,
             ColorAdjustType.Bitmap);
  
            //设置位置   

            int xPosOfWm;
            int yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.AllIn:
                    xPosOfWm = 0;
                    yPosOfWm = 0;
                    wmWidth = 512;
                    wmHeight = 512;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }
            //   
            // 第二次绘图，把水印印上去   
            //   
            grWatermark.DrawImage(imgWatermark,
             new Rectangle(xPosOfWm,
                                 yPosOfWm,
                                 tWidth,
                                 tHeight),
                                 0,
                                 0,
                                 wmWidth,
                                 wmHeight,
                                 GraphicsUnit.Pixel,
                                 imageAttributes);

            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            if (System.IO.File.Exists(targetImg))
                System.IO.File.Delete(targetImg);
            imgPhoto.Save(targetImg, ImageFormat.Png);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            return null;
        }

        public static Image CombinImage(string sourceImg, string destImg)
        {
            Image imgBack = System.Drawing.Image.FromFile(sourceImg);      
            Image img = System.Drawing.Image.FromFile(destImg);       


            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics        
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, 0, 0);      // g.DrawImage(imgBack, 0, 0, 相框宽, 相框高); 
            g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框



            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            g.DrawImage(img, 1, 1, 1, 1);
            GC.Collect();
            return imgBack;
        }
        /**/
        /// <summary> 
        ///  生成缩略图  
        /// </summary> 
        /// <param name="pathImageFrom"> 源图的路径(含文件名及扩展名) </param> 
        /// <param name="pathImageTo"> 生成的缩略图所保存的路径(含文件名及扩展名) 
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值) </param> 
        /// <param name="height"> 欲生成的缩略图 "画布" 的高度(像素值) </param> 
        public static void GenThumbnail(string pathImageFrom, string pathImageTo, int width, int height)
        {
            Image imageFrom = null;
            try
            {
                imageFrom = Image.FromFile(pathImageFrom);
            }
            catch
            {
                //throw; 
            }
            if (imageFrom == null)
            {
                return;
            }
            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            ImageAttributes imageAttributes = new ImageAttributes();

            //   
            //Colormap: 定义转换颜色的映射   
            //   
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

                      ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {    
           new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色   
           new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色   
           new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色          
           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 0.0f}, //透明度        
           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//   

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。   
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。   
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);

            Bitmap bmp = new Bitmap((int)width, (int)height);
          //  Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
          //  bmp.SetResolution(imageFrom.HorizontalResolution, imageFrom.VerticalResolution);
        //    bmp.MakeTransparent(Color.White);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //    g.Clear(Color.White); 
           // g.DrawImage(imageFrom,new Rectangle(0, 0,bmp.Width,bmp.Height),0,0,0,0,GraphicsUnit.Pixel,imageAttributes);
            g.DrawImage(imageFrom, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
            try
            {
                bmp.Save(pathImageTo, ImageFormat.Png);
            }
            catch
            {
            }
            finally
            {
                //显示释放资源 
                imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
        } 
    }
}
