﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace RaceCarSpeed
{
    /* capture image from mp4 video */
    class getImage
    {
        /* declare variables */
        VideoCapture capture;
        string videoName = Directory.GetCurrentDirectory() + "/ProjectCARS_short.mp4";
        int testnum;
        Bitmap BitmapFrame;


        /* Constructor: load in video file */
        public getImage()
        {
            if (!File.Exists(this.videoName))
            {
                throw new FileNotFoundException(videoName);
            }
            capture = new VideoCapture(videoName);
            testnum = 0;
        }


        /* get each frame from the video */
        public Bitmap LoadVideo()
        {
            //Console.WriteLine("Frame rate = " + capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));

            /* repeat cropping image until all the frames runs out*/
            while (testnum < capture.GetCaptureProperty(CapProp.FrameCount))
            {
                /* Crop area
                 * x:1545px  y:865px  boxsize: 60x38*/
                Rectangle cropArea = new Rectangle(1545, 865, 60, 38);
                Mat pFrame = Crop_frame(capture.QueryFrame(), cropArea);
                BitmapFrame = pFrame.ToBitmap();
                //BitmapFrame.Save(Directory.GetCurrentDirectory() + "/speedImage/cropImage_" + testnum + ".png", ImageFormat.Png);
                testnum += 1;
                Console.Write(testnum + " ");
            }
            
            return BitmapFrame;
        }


        /* Crop out a square area */
        Mat Crop_frame(Mat input, Rectangle crop_region)
        {
            Image<Bgr, Byte> buffer_im = input.ToImage<Bgr, Byte>();
            buffer_im.ROI = crop_region;

            Image<Bgr, Byte> cropped_im = buffer_im.Copy();

            return cropped_im.Mat;
        }

    }
}
