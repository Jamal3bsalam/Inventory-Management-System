using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Query.Search
{
    public class SerialNumbersSearchQuery:IRequest<Result<SerialNumbersSearchDto>>
    {
        public string? SerialNumber { get; set; }
    }
}
