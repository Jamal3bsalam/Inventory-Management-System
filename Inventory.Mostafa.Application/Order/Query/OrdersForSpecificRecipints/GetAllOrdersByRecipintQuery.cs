using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Shared;
using MediatR;

namespace Inventory.Mostafa.Application.Order.Query.OrdersForSpecificRecipints
{
    public class GetAllOrdersByRecipintQuery:IRequest<Result<OrdersForRecipientResponseDto>>
    {
        public string RecipintName { get; set; }
    }
}
