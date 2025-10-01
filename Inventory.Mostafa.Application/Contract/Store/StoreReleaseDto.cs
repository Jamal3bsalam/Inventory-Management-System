using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Store
{
    public class StoreReleaseDto
    {
        public int Id { get; set; }
        public string? UnitName { get; set; }
        public string? ReceiverName { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? StoreReleaseType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FileUrl { get; set; }
        public List<StoreReleaseItemResponseDto> Items { get; set; } = new();

    }
}
