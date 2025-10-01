using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.writeOff
{
    public class CreateWriteOff
    {
        [Required(ErrorMessage = "Unit Id Is Required")]
        public int? UnitId { get; set; }
        [Required(ErrorMessage = "Recipints Id Is Required")]
        public int? RecipintsId { get; set; }
        [Required(ErrorMessage = "Return Id Is Required")]
        public int? ReturnId { get; set; }
        [Required(ErrorMessage = "Document Id Is Required")]
        public IFormFile? Document { get; set; }
        [Required(ErrorMessage = "Quantity Id Is Required")]
        public int? Quantity { get; set; }
    }
}
