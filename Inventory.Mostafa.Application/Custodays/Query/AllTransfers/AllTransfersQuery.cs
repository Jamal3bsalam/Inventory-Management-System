using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Query.AllTransfers
{
    public class AllTransfersQuery:IRequest<Result<Pagination<IEnumerable<TransactionDto>>>>
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; } 
    }
}
