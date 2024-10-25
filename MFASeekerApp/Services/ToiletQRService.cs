using Entities;
using MFASeeker.Model;
using QRCoder;
using System.Text.Json;


namespace MFASeeker.Services
{
    public static class ToiletQRService
    {
        public static ImageFile GenerateQRCode(Toilet toilet)
        {
            var tempToilet = new
            {
                Name = toilet.Name,
                Description = toilet.Description,
                Location = toilet.Location,
                Rating = toilet.Rating,
                CreatedDate = toilet.CreatedDate,
                UserName = toilet.User.UserName,
                Guid = toilet.Guid,
                Id = toilet.Id,
            };
            string toiletData = JsonSerializer.Serialize(tempToilet);
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(toiletData, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            return new ImageFile
            {
                ByteBase64 = Convert.ToBase64String(qrCodeImage),
                FileName = $"{toilet.Guid}_qrcode.png",
                ContentType = "png"
            };
        }
    }
}
