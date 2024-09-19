using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Model
{
    public class UserToiletMarker : Toilet
    {
        public string? UserName {  get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string GetMarkerInfo()
        {
            return $"Name: {Name}, Location: {Location}, Description: {Description}, Rating: {Rating}, Created By: {UserId}, On: {CreatedDate}";
        }
    }
}
