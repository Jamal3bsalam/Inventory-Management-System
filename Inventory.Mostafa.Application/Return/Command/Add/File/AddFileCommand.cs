using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace Inventory.Mostafa.Application.Return.Command.File.Add
{
    public class AddFileCommand : IRequest<Result<string>>
    {
        public IFormFile? File { get; set; }

    }
}
