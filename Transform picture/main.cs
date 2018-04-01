using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace ProgramsExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            Image image = Image.FromFile("bitmap.bmp");
            var bitmap = new Bitmap(image);
            MakeGray(bitmap);
            bitmap.Save("bitmapResult.bmp");
        }

        private static void MakeGray(Bitmap bitmap)
        {
            // Задання формату пікселя
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
            // Отримання розмірів зображення
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            // Блокуємо інформацію про зображення в память
            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, pixelFormat);
            // Отримуємо адресу першої лінії
            IntPtr intPtr = bitmapData.Scan0;
            // Стіорюємо масив byte і поміщаємо в нього набір даних
            int numBytes = bitmapData.Stride * bitmap.Height;
            int widthBytes = bitmapData.Stride;
            byte[] rgbValues = new byte[numBytes];
            // Копіюємо значення в масив
            Marshal.Copy(intPtr, rgbValues, 0, numBytes);
            // Перебираємо пікселі по 3 байта на кожний і змінюємо їх значення
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {
                int value = rgbValues[counter] + rgbValues[counter + 1] + rgbValues[counter + 2];
                byte color = Convert.ToByte(value / 3);
                rgbValues[counter] = color;
                rgbValues[counter + 1] = color;
                rgbValues[counter + 2] = color;
            }
            // Копіюємо набір даних назад в зображення
            Marshal.Copy(rgbValues, 0, intPtr, numBytes);
            // Розблоковуємо набір даних в пам'яті назад в зображення 
            bitmap.UnlockBits(bitmapData);
        }  
    }
}
