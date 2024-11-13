using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.Model
{
    public class UserImageToilet
    {
        // Все фото по туалету от юзеров 
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
