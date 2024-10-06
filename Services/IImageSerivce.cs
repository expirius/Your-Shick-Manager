using MFASeeker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Services
{
    public interface IToiletImageSerivce
    {
        Image GetImage(int Id);
        List<Image> GetImageList(int Id);
        void SetImage(int Id, Image image);
    }
}
