using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Store
{
    public class UpdateStoreReleaseDto
    {
        [Required(ErrorMessage = "UnitId Is Required")]
        public int UnitId { get; set; }             // الوحدة اللي بيتم الصرف ليها
        [Required(ErrorMessage = "RecipintsId Is Required")]
        public int ReceiverId { get; set; }         // المستلم
        [Required(ErrorMessage = "Date Is Required")]
        public DateOnly ReleaseDate { get; set; }   // تاريخ الصرف
        public int? DocumentNumber { get; set; }
        [Required(ErrorMessage = "Document Is Required")]
        public string? DocumentPath { get; set; }
        [Required(ErrorMessage = "Items Is Required")]
        public List<CreateStoreReleaseItemDto> Items { get; set; } = new();
    }
}
