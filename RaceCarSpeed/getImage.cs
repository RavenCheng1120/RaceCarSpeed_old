using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace RaceCarSpeed
{
    class getImage
    {
        VideoCapture capture;
        ImageViewer viewer = new ImageViewer();
        string videoName = Directory.GetCurrentDirectory() + "/projectCars2.mp4";

        public getImage()
        {
            if (!File.Exists(this.videoName))
            {
                throw new FileNotFoundException(videoName);
            }
            capture = new VideoCapture(videoName);
        }

        public void LoadVideo()
        {
            Console.WriteLine("Frame rate = " + capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {
                //獲得的圖像
                viewer.Image = capture.QueryFrame();
            });
            viewer.ShowDialog();
        }
        
        
    }
}
