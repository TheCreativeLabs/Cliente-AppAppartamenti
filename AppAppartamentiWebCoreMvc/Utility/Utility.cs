using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AppAppartamentiWebCoreMvc.Utility
{
    public static class Utility
    {
        public enum AnnunciOrder
        {
            PRICE_ASC = 0,
            PRICE_DESC = 1,
            SIZE_ASC = 2,
            SIZE_DESC = 3,
            CREATION_DATE_ASC = 4,
            CREATION_DATE_DESC = 5
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        { 
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public static byte[] Compress(byte[] data)
        {
            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            using (var inStream = new MemoryStream(data))
            using (var outStream = new MemoryStream())
            {
                var image = Image.FromStream(inStream);

                // if we aren't able to retrieve our encoder
                // we should just save the current image and
                // return to prevent any exceptions from happening
                if (jpgEncoder == null)
                {
                    image.Save(outStream, ImageFormat.Jpeg);
                }
                else
                {
                    var qualityEncoder = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, 25L);
                    image.Save(outStream, jpgEncoder, encoderParameters);
                }

                return outStream.ToArray();
            }
        }
    }

   
}


