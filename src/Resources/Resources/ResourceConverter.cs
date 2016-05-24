using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Hasseware.Resources
{
    internal sealed class ResourceConverter : IResourceConverter
    {
        public object ConvertFromStore(object value, string mimeType)
        {
            switch (mimeType)
            {
                case "image/bmp":
                case "image/gif":
                case "image/jpeg":
                case "image/png":
                case "image/tiff":
                    using (var stream = CreateStreamFrom(value))
                        return Image.FromStream(stream);

                case "image/x-icon":
                    using (var stream = CreateStreamFrom(value))
                        return new Icon(stream);

                case "application/octet-stream":
                default:
                    return value;
            }
        }

        public object ConvertToStore(object value, out string mimeType)
        {
            mimeType = null;
            if (value == null || value is string)
                return value;

            using (var stream = new MemoryStream())
            {
                if (value is Image)
                {
                    if (!TryGetMimeType((Image)value, out mimeType))
                        throw new OperationCanceledException(Properties.Resources.Unsupported_image_resource);
                    ((Image)value).Save(stream, ((Image)value).RawFormat);
                }
                else if (value is Icon)
                {
                    mimeType = "image/x-icon";
                    ((Icon)value).Save(stream);
                }
                else if (value is Stream)
                {
                    mimeType = "application/octet-stream";
                    ((Stream)value).CopyTo(stream);
                }
                else
                {
                    throw new OperationCanceledException(String.Format(
                        Properties.Resources.Unsupported_resource_type, value.GetType()));
                }
                return stream.GetBuffer();
            }
        }

        private static Stream CreateStreamFrom(object value)
        {
            if (value is string)
                return new MemoryStream(Encoding.Default.GetBytes((string)value));

            return new MemoryStream((byte[])value);
        }

        private static bool TryGetMimeType(Image value, out string mimeType)
        {
            mimeType = null;
            if (value.RawFormat == ImageFormat.Bmp)
            {
                mimeType = "image/bmp";
                return true;
            }
            else if (value.RawFormat == ImageFormat.Gif)
            {
                mimeType = "image/gif";
                return true;
            }
            else if (value.RawFormat == ImageFormat.Jpeg)
            {
                mimeType = "image/jpeg";
                return true;
            }
            else if (value.RawFormat == ImageFormat.Png)
            {
                mimeType = "image/png";
                return true;
            }
            else if (value.RawFormat == ImageFormat.Tiff)
            {
                mimeType = "image/tiff";
                return true;
            }
            else if (value.RawFormat == ImageFormat.Icon)
            {
                mimeType = "image/x-icon";
                return true;
            }
            return false;
        }
    }
}
