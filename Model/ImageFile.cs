using FFImageLoading.Maui;
using MFASeeker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Model
{
    public class ImageFile
    {
        public required string ByteBase64 { get; set; }
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
        public ImageSource Image => GetImageSource();
        private ImageSource GetImageSource()
        {
            LocalImageService imageService = new();
            return ImageSource.FromStream(()=> imageService.ByteArrayToStream(Convert.FromBase64String(ByteBase64)));
        }
    }
}
