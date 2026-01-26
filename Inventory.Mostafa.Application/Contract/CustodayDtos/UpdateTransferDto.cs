using Microsoft.AspNetCore.Http;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class UpdateTransferDto
    {
        public string? HijriDate { get; set; }
        public IFormFile? File { get; set; }
    }
}
