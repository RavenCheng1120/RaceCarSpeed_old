using Emgu.CV;
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
        VideoCapture capture;
        ImageViewer viewer = new ImageViewer();
        string videoName = Directory.GetCurrentDirectory() + "/ProjectCARS_short.mp4";
        int testnum = 1;
        Bitmap BitmapFrame;

        public getImage()
        {
            if (!File.Exists(this.videoName))
            {
                throw new FileNotFoundException(videoName);
            }
            capture = new VideoCapture(videoName);
        }

        public Bitmap LoadVideo()
        {
            Console.WriteLine("Frame rate = " + capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {
                //獲得的圖像
                viewer.Image = capture.QueryFrame();
                if(testnum == 1)
                {
                    Rectangle cropArea = new Rectangle(1545, 865, 60, 38);
                    Mat pFrame = crop_color_frame(capture.QueryFrame(), cropArea);
                    BitmapFrame = pFrame.ToBitmap();
                    BitmapFrame.Save(Directory.GetCurrentDirectory()+"/cropImage.png", ImageFormat.Png);
                    testnum = 0;
                }
                
            });

            viewer.ShowDialog();
            return BitmapFrame;
        }

        Mat crop_color_frame(Mat input, Rectangle crop_region)
        {
            Image<Bgr, Byte> buffer_im = input.ToImage<Bgr, Byte>();
            buffer_im.ROI = crop_region;

            Image<Bgr, Byte> cropped_im = buffer_im.Copy();

            return cropped_im.Mat;
        }


    }
}
