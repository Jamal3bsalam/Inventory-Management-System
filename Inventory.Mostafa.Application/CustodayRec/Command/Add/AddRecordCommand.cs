using Inventory.Mostafa.Application.Contract.CustodayRec;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.CustodayRec.Command.Add
{
    public class AddRecordCommand:IRequest<Result<RecordDto>>
    {
        public string? Notic { get; set; }
        public DateTime? Date { get; set; }
        public IFormFile? File { get; set; }
    }
}
