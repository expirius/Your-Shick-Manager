using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.Model
{
    public class ImageFile : BaseEntity
    {
        public required string ByteBase64 { get; set; }
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
        public string? Path { get; set; }

    }
}
