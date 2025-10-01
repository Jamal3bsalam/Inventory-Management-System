using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Add
{
    public class AddStoreReleaseCommand:IRequest<Result<StoreReleaseDto>>
    {
        public int UnitId { get; set; }             // الوحدة اللي بيتم الصرف ليها
        public int ReceiverId { get; set; }         // المستلم
        public DateOnly ReleaseDate { get; set; }   // تاريخ الصرف
        public string? DocumentNumber { get; set; } 
        public string? DocumentPath { get; set; }
        public List<CreateStoreReleaseItemDto> Items { get; set; } = new();
    }
}
