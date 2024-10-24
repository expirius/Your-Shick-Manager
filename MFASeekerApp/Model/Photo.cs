using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Model
{
    public class Photo
    {
        public string FilePath { get; set; }  // Локальный путь к файлу
        public string Url { get; set; }        // URL-адрес на сервере
        public bool IsFromServer { get; set; } // Откуда фото
    }
}
