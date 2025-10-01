using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Command
{
    public class TransactionCommand:IRequest<Result<CustodayDto>>
    {
        public int? CustodayId { get; set; }
        public int? UnitId { get; set; }
        public int? ItemId { get; set; }
        public int? NewRecipints { get; set; }
        public int? Quantity { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public IFormFile? File { get; set; }

    }
}
