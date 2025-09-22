using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Opening
{
    public class UpdateOpeningDto
    {
        public int? NewQuantity { get; set; }
        public ICollection<string>? NewSerialNumbers { get; set; }
    }
}
