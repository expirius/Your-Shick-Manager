using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Model
{
    public class Toilet
    {
        public int Id;
        public string? Name;
        public string? Location;
        public string? Description;
        public short Raiting;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Location);
        }
    }
}
