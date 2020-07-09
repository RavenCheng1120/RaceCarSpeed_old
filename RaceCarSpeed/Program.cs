using System;
using System.Drawing;
using Tesseract;

namespace RaceCarSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            // image processing
            ImageProcess imageProcess = new ImageProcess("/rawImage.png");
            Bitmap finalImage = imageProcess.ReadImage();
            finalImage.Save(imageProcess.filePath + "/processedImage.png", System.Drawing.Imaging.ImageFormat.Png);

            // use Tesseract to recognize number
            try
            {
                TesseractEngine ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
                // Pix pixImage = PixConverter.ToPix(finalImage);
                Pix pixImage = Pix.LoadFromFile(imageProcess.filePath + "/processedImage.png");
                Page page = ocr.Process(pixImage);
                string str = page.GetText();//識別後的內容
                Console.WriteLine("Result: " + str);
                page.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error message");
                Console.WriteLine(ex);
            }



        }
    }
}
