using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Utility
{
    public class ImageResizer
    {
        private int allowedFileSizeInByte;
        private string sourcePath;
        private string destinationPath;

        public ImageResizer(int allowedSize, string destinationPath)
        {
            allowedFileSizeInByte = allowedSize;
            this.destinationPath = destinationPath;
        }

        public ImageResizer(int allowedSize, string sourcePath, string destinationPath)
        {
            allowedFileSizeInByte = allowedSize;
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
        }

        public Bitmap ScaleImage(Bitmap image, double scale)
        {
            int newWidth = (int)(image.Width * scale);
            int newHeight = (int)(image.Height * scale);

            Bitmap result = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        public void ScaleImageAsJPG(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SaveTemporary(bmp, ms, 100, ImageCodecInfo.GetImageEncoders()[0]);

                while (ms.Length > allowedFileSizeInByte)
                {
                    double scale = Math.Sqrt
                    ((double)allowedFileSizeInByte / (double)ms.Length);
                    ms.SetLength(0);
                    bmp = ScaleImage(bmp, scale);
                    SaveTemporary(bmp, ms, 100, ImageCodecInfo.GetImageEncoders()[0]);
                }

                if (bmp != null)
                    bmp.Dispose();
                SaveImageToFile(ms);
            }
        }

        public void ScaleImage()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(sourcePath, FileMode.Open))
                {
                    Bitmap bmp = (Bitmap)Image.FromStream(fs);
                    SaveTemporary(bmp, ms, 100);

                    while (ms.Length > allowedFileSizeInByte)
                    {
                        double scale = Math.Sqrt
                        ((double)allowedFileSizeInByte / (double)ms.Length);
                        ms.SetLength(0);
                        bmp = ScaleImage(bmp, scale);
                        SaveTemporary(bmp, ms, 100);
                    }

                    if (bmp != null)
                        bmp.Dispose();
                    SaveImageToFile(ms);
                }
            }
        }
        private void SaveTemporary(Bitmap bmp, MemoryStream ms, int quality, ImageCodecInfo codec)
        {
            EncoderParameter qualityParam = new EncoderParameter
                (System.Drawing.Imaging.Encoder.Quality, quality);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            bmp.Save(ms, codec, encoderParams);
        }

        private void SaveTemporary(Bitmap bmp, MemoryStream ms, int quality)
        {
            EncoderParameter qualityParam = new EncoderParameter
                (System.Drawing.Imaging.Encoder.Quality, quality);
            var codec = GetImageCodecInfo();
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            bmp.Save(ms, codec, encoderParams);
        }

        private void SaveImageToFile(MemoryStream ms)
        {
            byte[] data = ms.ToArray();

            using (FileStream fs = new FileStream(destinationPath, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }
        }

        private ImageCodecInfo GetImageCodecInfo()
        {
            FileInfo fi = new FileInfo(sourcePath);

            switch (fi.Extension)
            {
                case ".bmp": return ImageCodecInfo.GetImageEncoders()[0];
                case ".jpg":
                case ".jpeg": return ImageCodecInfo.GetImageEncoders()[1];
                case ".gif": return ImageCodecInfo.GetImageEncoders()[2];
                case ".tiff": return ImageCodecInfo.GetImageEncoders()[3];
                case ".png": return ImageCodecInfo.GetImageEncoders()[4];
                default: return null;
            }
        }
    }
}
