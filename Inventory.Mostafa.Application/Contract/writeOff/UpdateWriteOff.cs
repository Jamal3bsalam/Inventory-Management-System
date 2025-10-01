using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.writeOff
{
    public class UpdateWriteOff
    {
        public int? Quantity { get; set; }
        public IFormFile? File { get; set; }
    }
}
