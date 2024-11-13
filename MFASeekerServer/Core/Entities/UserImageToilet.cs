using System.ComponentModel.DataAnnotations;

namespace MFASeekerServer.Core.Entities
{
    public class UserImageToilet
    {
        // Все фото по туалету от юзеров 
        // Прикинул примерно как будет выглядить
        // Id // User   // Toilet.Adress // Toilet.Id // Image
        // 1  // Vlados // Думская 1     // 1         // Картинка 1
        // 2  // Vlados // Думская 1     // 1         // Картинка 2
        // 3  // Vlados // Кировская 2к3 // 5         // Картинка 122
        // 4  // System // Думская 2     // 6         // Картинка 6122
        // 5  // System // Думская 1     // 1         // Картинка 3
        public int Id { get; set; }
        public int UserID { get; set; }
        public int ImageID { get; set; }
        public int ToiletID { get; set; }
        //
        public User? User { get; set; }
        public ImageFile? ImageFile { get; set; }
        public Toilet? Toilet { get; set; }
    }
}
