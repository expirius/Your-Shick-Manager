using Entities;
using System.ComponentModel.DataAnnotations;

namespace MFASeekerServer.Core.Entities
{
    public class Toilet : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        public override Guid Guid { get; set; }
        public string? Location { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Rating { get; set; } = 0;
        public bool IsPrivate { get; set; } = true;
        public int UserID { get; set; }
        public User User { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<UserImageToilet> UserImageToilets { get; set; }
        //private Guid CreateGuid() => new(Guid.NewGuid().ToString().GetHashCode().ToString("x"));

    }
}
