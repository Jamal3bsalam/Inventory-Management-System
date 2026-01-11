using Inventory.Mostafa.Domain.Entities.Store;
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
        public string? OrderType { get; set; }  // تعميد - أمر شراء - دعم
        public string? SupplierName { get; set; }
        public string? RecipintName { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? Attachment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<OrderItems>? OrderItems { get; set; }
        public ICollection<StoreReleaseItem>? StoreReleaseItems { get; set; }
    }
}
