using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Write.Command.Delete
{
    public class DeleteWriteOffCommandHandler : IRequestHandler<DeleteWriteOffCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWriteOffCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteWriteOffCommand request, CancellationToken cancellationToken)
        {
            var writeOffSpce = new WriteOffSpec(request.Id.Value);
            var writeOff = await _unitOfWork.Repository<WriteOff, int>().GetWithSpecAsync(writeOffSpce);
            if (writeOff == null) return Result<string>.Failure("WriteOff not found");

            //if (writeOff.IsDeleted.Value) return Result<string>.Failure("WriteOff already deleted");

            var returnSpec = new ReturnSpec(writeOff.ReturnId.Value);
            var returns = await _unitOfWork.Repository<Returns, int>().GetWithSpecAsync(returnSpec);
            if (returns == null) return Result<string>.Failure("Related Return not found");

            var spec = new StoreItemSpec(returns.storeReleaseItemId.Value,true);
            var storeItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetWithSpecAsync(spec);
            if (storeItem == null) return Result<string>.Failure("StoreReleaseItem not found");

            var orderItem = storeItem.OrderItem;
            if (orderItem == null) return Result<string>.Failure("OrderItem not found");

            int qtyToRestore = writeOff.Quantity.Value;

            // 4. integrity check: نتأكد إننا نقدر نقلل الـ ConsumedQuantity من غير ما يبقى سالب
            if (orderItem.ConsumedQuantity < qtyToRestore)
            {
                return Result<string>.Failure(
                    $"Inconsistent data: ConsumedQuantity ({orderItem.ConsumedQuantity}) is less than write-off qty ({qtyToRestore}). Please fix data before deleting write-off.");
            }

            returns.Quantity += qtyToRestore;
            orderItem.ConsumedQuantity -= qtyToRestore;

            writeOff.IsDeleted = true;
            writeOff.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Returns, int>().Update(returns);
            _unitOfWork.Repository<StoreReleaseItem, int>().Update(storeItem);
            _unitOfWork.Repository<WriteOff, int>().Update(writeOff);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<string>.Failure("Failed to delete write-off");

            return Result<string>.Success("WriteOff Deleted and quantities restored successfully");
        }
    }
}
