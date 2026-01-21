using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Return
{
    public class AllReturnDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public string? RecipintsName { get; set; }
        public string? ReturnDocUrl { get; set; }
        public string? Reason { get; set; }
        public List<ReturnItemResponseDto>? ReturnItems { get; set; }
    }
}
