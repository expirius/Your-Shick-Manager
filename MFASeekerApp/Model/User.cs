using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class User : BaseEntity
    {
        public virtual string UserName { get; set; } = string.Empty;
        public virtual string DeviceInfo { get; set; } = string.Empty;
        //public virtual string Password {  get; set; } = string.Empty;
    }
}
