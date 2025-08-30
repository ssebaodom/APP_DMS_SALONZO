using System;
using System.IO;

namespace SSE.Common.Functions
{
    public class ImageProcessing
    {
        public static bool SaveImage(string path, string imageName, string imgBaseStr)
        {
            //Check if directory exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(imgBaseStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }
    }
}