using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MFASeekerServer.Core.Entities;

namespace Entities
{
    public class User : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        public string UserName { get; set; }
        public string? DeviceInfo { get; set; }
        //public virtual string Password {  get; set; } = string.Empty;

        // Навигация.
        // 1. У юзера может быть несколько туалетов 
        // 2. У юзера может быть несколько фото для туалетов
        public ICollection<Toilet> Toilets { get; set; }
        public ICollection<UserImageToilet> UserImageToilets { get; set; }
    }
}
