using MFASeekerApp.Model;
using MFASeekerApp.Model;
using QRCoder;
using System.Text.Json;


namespace MFASeekerApp.Services
{
    public static class ToiletQRService
    {
        public static ImageFile GenerateQRCode(Toilet toilet)
        {
            var tempToilet = new
            {
                toilet.Name,
                toilet.Description,
                toilet.Location,
                toilet.Rating,
                toilet.CreatedDate,
                toilet.User.UserName,
                toilet.Guid,
                toilet.Id,
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
