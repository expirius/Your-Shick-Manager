using MFASeeker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Services
{
    public class LocalImageService
    {
        /// <summary>
        /// Открывает Media Picker для выбора фото
        /// </summary>
        /// <returns></returns>
        public async Task<FileResult?> TakePhoto()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Загрузите фото" });

                if (result?.ContentType is "image/png" or "image/jpeg" or "image/jpg")
                {
                    return result;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Неверный тип изображения", "Попробуйты выбрать другое изображение", "ОК");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
       /// <summary>
       /// Конвертирует fileResult в поток (stream)
       /// </summary>
       /// <param name="fileResult"></param>
       /// <returns></returns>
        public async Task<Stream?> FileResultToStream(FileResult fileResult)
        {
            if (fileResult == null)
                return null;

            return await fileResult.OpenReadAsync();
        }

        /// <summary>
        /// Конвертирует bute[] в поток
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>Stream</returns>
        public Stream ByteArrayToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }
        /// <summary>
        /// Конвертирует string в поток
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public byte[] StringToByteBase64(string text)
        {
            return Convert.FromBase64String(text);
        }
        /// <summary>
        /// Конвертирует byte[] в поток
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string ByteBase64ToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Загрузка изображения
        /// </summary>
        /// <param name="fileResult"></param>
        /// <returns></returns>
        public async Task<ImageFile?> Upload(FileResult fileResult)
        {
            byte[] bytes;

            try
            {
                using (var ms = new MemoryStream())
                {
                    var stream = await FileResultToStream(fileResult);
                    stream?.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                return new ImageFile
                {
                    ByteBase64 = ByteBase64ToString(bytes),
                    ContentType = fileResult.ContentType,
                    FileName = fileResult.FileName
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
