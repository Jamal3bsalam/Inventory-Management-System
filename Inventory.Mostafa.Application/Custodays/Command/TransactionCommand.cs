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
    public class TransactionCommand:IRequest<Result<List<CustodayDto>>>
    {
        public int UnitId { get; set; }
        public int NewRecipints { get; set; }
        public ICollection<CustodayItemDto>? Items { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string TransactionHijriDate { get; set; }
        public string? FileName { get; set; }

    }
}
