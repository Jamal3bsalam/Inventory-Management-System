using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Order
{
    public class Orders:BaseEntity<int>
    {
        public string? OrderNumber { get; set; }
        [Column(TypeName = "nvarchar(50)")]  // يخزن الاسم مش الرقم
        public OrderType? OrderType { get; set; }  // تعميد - أمر شراء - دعم
        public string? SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Attachment { get; set; }

        public ICollection<OrderItems>? OrderItems { get; set; }
    }
}
