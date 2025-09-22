using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Command.Delete
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices<Orders, int> _fileServices;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork, IFileServices<Orders, int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
        }
        public async Task<Result<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) return Result<string>.Failure("Please Enter A Valid Id.");

            var spec = new OrderSpec(request.Id.Value);
            var order = await _unitOfWork.Repository<Orders, int>().GetWithSpecAsync(spec);

            if (order == null) return Result<string>.Failure("No Order With This Id To Delete.");
            var hasReleaseItems = order.StoreReleaseItems.Any();
            if (hasReleaseItems)
            {
                order.IsDeleted = true;
                order.DeletedAt = DateTime.UtcNow;
                foreach (var item in order.OrderItems)
                {
                    item.IsDeleted = true;
                    item.DeletedAt = DateTime.UtcNow;
                }

                _unitOfWork.Repository<Orders, int>().Update(order);
            }
            else
            {
                _unitOfWork.Repository<Orders, int>().Delete(order);
            }
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<string>.Failure("Faild To Delete Order");

            return Result<string>.Success("Order Deleted Sucessfully.");

        }
    }
}
