using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Store
{
    public class CreateStoreReleaseItemDto
    {
        public int OrderId { get; set; }            // رقم الأمر اللي هنخصم منه
        public int OrderItemId { get; set; }        // الصنف المحدد داخل الأمر
        public int Quantity { get; set; }           // الكمية اللي هتصرف
        public List<string>? SerialNumbers { get; set; }  // لو فيه أرقام سيريال
    }
}
