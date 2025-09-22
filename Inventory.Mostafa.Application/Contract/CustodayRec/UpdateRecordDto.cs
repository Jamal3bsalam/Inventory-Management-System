using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayRec
{
    public class UpdateRecordDto
    {
        public string? Notic { get; set; }
        public DateTime? Date { get; set; }
        public IFormFile? File { get; set; }
    }
}
