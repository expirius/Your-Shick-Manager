using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace MFASeeker.Model
{
    public class ToiletQR : Toilet
    {
        public ImageFile QRCode { get; set; }
    }
}
