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
        private string? CachePath {  get; set; }
        //public ImageSource FirstImage => GetImageSource();
        //// Возвращаем изображение из файла, хранящегося в памяти
        //private string GetImageCachePath()
        //{
        //    var cahePath = Path.Combine(FileSystem.CacheDirectory, FileName);
        //    if (!File.Exists(cahePath))
        //    {
        //        LocalImageService imageService = new();
        //        return Task.Run(async () =>
        //        {
        //            return await imageService.SaveByteArrayToFile(Convert.FromBase64String(ByteBase64), FileName);
        //        }).GetAwaiter().GetResult();
        //    }
        //    else
        //        return cahePath;
        //}
        //private ImageSource GetImageSource()
        //{
        //    //LocalImageService imageService = new();
        //    //return ImageSource.FromStream(()=> imageService.ByteArrayToStream(Convert.FromBase64String(ByteBase64)));
        //    CachePath = GetImageCachePath();
        //    return ImageSource.FromFile(CachePath);
        //}
    }
}
