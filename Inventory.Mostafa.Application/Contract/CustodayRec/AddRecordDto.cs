using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayRec
{
    public class AddRecordDto
    {
        [Required(ErrorMessage = "Notice Is Required")]
        public string? Notic { get; set; }
        [Required(ErrorMessage = "Date Is Required")]
        public DateOnly? Date { get; set; }
        public IFormFile? File { get; set; }
    }
}
