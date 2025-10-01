using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Return
{
    public class UpdateReturnDto
    {
        public IFormFile? Document { get; set; }
        public int? Quantity { get; set; }
        public string? Reason { get; set; }

    }
}
