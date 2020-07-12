using System;
using System.Drawing;
using Tesseract;

namespace RaceCarSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize Tesseract object
            TesseractEngine ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);

            // calculate total process time
            DateTime beforDT = System.DateTime.Now; 


            /*** image processing ***/
            // turning the speed image into negative image 
            ImageProcess imageProcess = new ImageProcess("/rawImage.png");
            Bitmap finalImage = imageProcess.ReadImage();

            //* save the processed image as png file
            // finalImage.Save(imageProcess.filePath + "/processedImage.png", System.Drawing.Imaging.ImageFormat.Png);
            /*** image processing ***/


            /*** use Tesseract to recognize number ***/
            try
            {
                Pix pixImage = PixConverter.ToPix(finalImage); //unable to work at Tesseract3.3.0
                //* read the processed image as Pix format
                // Pix pixImage = Pix.LoadFromFile(imageProcess.filePath + "/processedImage.png");
                Page page = ocr.Process(pixImage);
                string str = page.GetText();//識別後的內容
                Console.WriteLine("Result: " + str);
                page.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: ");
                Console.WriteLine(ex.Message);
            }
            /*** use Tesseract to recognize number ***/

            // calculate total process time
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("DateTime總共花費" + ts.TotalMilliseconds + "ms.");

        }
    }
}
