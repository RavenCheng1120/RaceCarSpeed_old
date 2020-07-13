﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using Tesseract;

namespace RaceCarSpeed
{
    /* load in video, crop out the dashboard image */
    class Program
    {
        static void Main(string[] args)
        {
            /* initialize Tesseract object */
            TesseractEngine ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);


            /* declare variables for Tesseract */
            string speedStr;
            int speed = 0;
            int preSpeed = 0; // previous speed

            /* declare variables for video input */
            VideoCapture capture;
            string videoName = Directory.GetCurrentDirectory() + "/ProjectCARS_short.mp4";
            int frameCount;
            int totalFrame;
            Bitmap BitmapFrame;
            Rectangle cropArea;

            /* declare variables for testing error rate */
            int wrong = 0;


            if (!File.Exists(videoName))
            {
                throw new FileNotFoundException(videoName);
            }
            capture = new VideoCapture(videoName);
            frameCount = 0;
            cropArea = new Rectangle(1545, 865, 60, 38);
            totalFrame = (int)capture.GetCaptureProperty(CapProp.FrameCount); // 影片中的影格總數

            Console.WriteLine("testnum: " + frameCount + "totalFrame: " + totalFrame);
            

            /* get each frame from the video */
            while (frameCount < totalFrame)
            {
                /* calculate total process time */
                DateTime beforDT = System.DateTime.Now;

                /* Crop area
                 * x:1545px  y:865px  boxsize: 60x38*/
                Mat pFrame = Crop_frame(capture.QueryFrame(), cropArea);
                BitmapFrame = pFrame.ToBitmap();
                frameCount += 1;


                /* image processing */
                /* turn the dashboard image into negative image */
                ImageProcess imageProcess = new ImageProcess();
                BitmapFrame = imageProcess.NegativePicture(BitmapFrame);

                /* use Tesseract to recognize number */
                try
                {
                    Pix pixImage = PixConverter.ToPix(BitmapFrame); //unable to work at Tesseract 3.3.0
                    Page page = ocr.Process(pixImage);
                    speedStr = page.GetText(); // 識別後的內容
                    // Console.WriteLine("String result: " + speedStr);
                    page.Dispose();

                    /* Parse str to int */
                    bool isParsable = Int32.TryParse(speedStr, out speed);
                    if (!isParsable)
                    {
                        Console.WriteLine("Could not be parsed.");
                        wrong += 1;
                        speed = preSpeed; // 如果偵測不到錯誤，使用前一個速度值
                    }
                        
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error message: " + ex.Message);
                }

                Console.WriteLine("  Speed right now is at " + speed);
                preSpeed = speed;

                /* calculate total process time */
                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                Console.WriteLine("DateTime總共花費" + ts.TotalMilliseconds + "ms.\n");
            }

            Console.WriteLine("錯誤數量: " + wrong + "全部frame數量: " + totalFrame);
            Console.WriteLine("正確率: " + (double)(totalFrame - wrong) / totalFrame);
        }



        /* Crop out a square area */
        static Mat Crop_frame(Mat input, Rectangle crop_region)
        {
            Image<Bgr, Byte> buffer_im = input.ToImage<Bgr, Byte>();
            buffer_im.ROI = crop_region;

            Image<Bgr, Byte> cropped_im = buffer_im.Copy();

            return cropped_im.Mat;
        }

    }
}
