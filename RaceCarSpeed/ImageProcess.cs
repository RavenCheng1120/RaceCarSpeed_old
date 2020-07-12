using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace RaceCarSpeed
{
    public class ImageProcess
    {
        public readonly string filePath;
        private readonly string filename;
        public Bitmap processedImage;

        public ImageProcess(string imageName)
        {
            // 取得測試圖片的path
            // Debug狀態，image檔案位於/Users/patty/Projects/RaceCarSpeed/RaceCarSpeed/bin/Debug/netcoreapp3.1
            filePath = Directory.GetCurrentDirectory();
            filename = filePath + imageName;
        }

        public Bitmap ReadImage()
        {
            // 檢查檔案是否存在
            if (!File.Exists(this.filename))
            {
                throw new FileNotFoundException(filename);
            }

            // 讀取圖片
            processedImage = NegativePicture(new Bitmap(filename));

            return processedImage;
        }

        // 將圖片轉換成負片效果
        private static Bitmap NegativePicture(Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, w, h),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            image.UnlockBits(srcData);
            int cChannels = 3;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int current = y * srcData.Stride + x * 4;
                    for (int c = 0; c < cChannels; c++)
                    {
                        result[current + c] = (byte)(255 - buffer[current + c]);
                    }
                    result[current + 3] = 255;
                }
            }

            Bitmap resImg = new Bitmap(w, h);
            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, w, h),
            ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            return resImg;
        }
    }
}

