using System;
namespace Inventory.Mostafa.Application.Contract.Return
{
    public class ReturnDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public string? RecipintsName { get; set; }
        public string? DocumentUrl { get; set; }
        public string? Reason { get; set; }
        public List<ReturnItemResponseDto> ItemResponseDtos { get; set; }
    }
}
