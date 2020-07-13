using System;
using System.Drawing;
using Tesseract;

namespace RaceCarSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            /* initialize Tesseract object */
            TesseractEngine ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
            /* declare variables */
            string speedStr;
            int speed = 0;

            getImage projectCars2 = new getImage();
            projectCars2.LoadVideo();


            /* calculate total process time */
            DateTime beforDT = System.DateTime.Now;


            /* image processing */
            /* turn the dashboard image into negative image */
            ImageProcess imageProcess = new ImageProcess("/cropImage.png");
            Bitmap finalImage = imageProcess.ReadImage();


            /* use Tesseract to recognize number */
            try
            {
                Pix pixImage = PixConverter.ToPix(finalImage); //unable to work at Tesseract 3.3.0
                Page page = ocr.Process(pixImage);
                speedStr = page.GetText();//識別後的內容
                Console.WriteLine("String result: " + speedStr);
                page.Dispose();

                /* Parse str to int */
                bool isParsable = Int32.TryParse(speedStr, out speed);
                if (!isParsable)
                    Console.WriteLine("Could not be parsed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: " + ex.Message);
            }


            Console.WriteLine("Speed right now is at " + speed + "\n");


            /* calculate total process time */
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("DateTime總共花費" + ts.TotalMilliseconds + "ms.");

        }
    }
}
