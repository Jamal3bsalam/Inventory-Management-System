using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class UpdateFileDto
    {
        public ICollection<int>? IDs { get; set; }
        public IFormFile? File { get; set; }
    }
}
