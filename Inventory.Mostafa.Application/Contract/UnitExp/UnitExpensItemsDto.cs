﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.UnitExp
{
    public class UnitExpensItemsDto
    {
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
    }
}
