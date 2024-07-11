
using ImageMagick;
using System.Text;

namespace img2ascii.Services
{
    public interface IMainService
    {
        Task<byte[]> ConvertToAscii(IFormFile file);
    }

    public class MainService : IMainService
    {
        public async Task<byte[]> ConvertToAscii(IFormFile file)
        {
            using var img = new MagickImage(file.OpenReadStream());

            img.Scale(img.Width / 8, img.Height / 8);
            img.Grayscale();
            img.ColorSpace = ColorSpace.XYZ;
            var pixels = img.GetPixels();

            var lumas = new List<double>();
            foreach (var pixel in pixels)
            {
                var color = pixel.ToColor();
                if (color == null) continue;
                var luma = 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B;
                byte quantizedLuma = Convert.ToByte(Math.Floor(luma / 255));
                lumas.Add(luma);
            }

            var min = lumas.Min();
            var max = lumas.Max();

            /*for (int height = 0; height < img.Height; height++)
            {
                for (int width = 0; width < img.Width; width++)
                {
                    
                }
            }*/

            return img.ToByteArray();
        }
    }
}
