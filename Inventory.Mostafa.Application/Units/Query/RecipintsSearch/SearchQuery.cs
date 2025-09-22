using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Query.RecipintsSearch
{
    public class SearchQuery:IRequest<Result<IEnumerable<RecipintsDtos>>>
    {
        public string? Search { get; set; }
    }
}
