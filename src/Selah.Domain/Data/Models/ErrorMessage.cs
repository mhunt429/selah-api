using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selah.Domain.Data.Models
{
    public record ErrorMessage
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
