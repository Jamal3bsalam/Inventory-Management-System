using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Command.Update
{
    public class UpdateTransferCommand : IRequest<Result<string>>
    {
        public int? Id { get; set; }
        public string? HijriDate { get; set; }
        public IFormFile? File { get; set; }
    }
}
