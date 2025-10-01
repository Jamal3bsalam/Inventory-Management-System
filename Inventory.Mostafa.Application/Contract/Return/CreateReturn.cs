﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Return
{
    public class CreateReturn
    {
        [Required(ErrorMessage = "Unit Id Is Required")]
        public int? UnitId { get; set; }
        [Required(ErrorMessage = "Recipints Id Is Required")]
        public int? RecipintsId { get; set; }
        public IFormFile? Document { get; set; }
        [Required(ErrorMessage = "Store Release Item Id Is Required")]
        public int? StoreReleaseItemId { get; set; }
        [Required(ErrorMessage = "Quantity Id Is Required")]
        public int? Quantity { get; set; }
        [Required(ErrorMessage = "Reason Id Is Required")]
        public string? Reason { get; set; }
    }
}
