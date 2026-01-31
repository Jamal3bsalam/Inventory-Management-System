using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
namespace Inventory.Mostafa.Application.Store.Command.Add.NewRecipints
{
    public class AddNewRecipintsCommand:IRequest<Result<RecipintsDto>>
    {
        public int? UnitId { get; set; }
        public string? RecipintsName { get; set; }
    }
}
