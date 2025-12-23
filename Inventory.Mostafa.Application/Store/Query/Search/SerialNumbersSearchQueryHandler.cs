using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Query.Search
{
    public class SerialNumbersSearchQueryHandler : IRequestHandler<SerialNumbersSearchQuery, Result<SerialNumbersSearchDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SerialNumbersSearchQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<SerialNumbersSearchDto>> Handle(SerialNumbersSearchQuery request, CancellationToken cancellationToken)
        {
            if (request == null) return Result<SerialNumbersSearchDto>.Failure("Please Enter Valid Serial Number For Search.");

            var spec = new StoreReleaseItemSerialSpec(request.SerialNumber);
            var serialNumber = await _unitOfWork.Repository<ReleaseItemSerialNumber,int>().GetWithSpecAsync(spec);
            if (serialNumber == null) Result<SerialNumbersSearchDto>.Failure($"There Is No Item For This Serial Number: {request.SerialNumber}.");

            var storeItemSpec = new StoreItemSpec(serialNumber.StoreReleaseItemId,true);
            var storeItem = await _unitOfWork.Repository<StoreReleaseItem,int>().GetWithSpecAsync(storeItemSpec);   
            if (storeItem == null) Result<SerialNumbersSearchDto>.Failure($"There Is No Store Release Item For This Serial Number: {request.SerialNumber}.");

            var storeSpec = new StoreReleaseSpec(storeItem.StoreReleaseId);
            var store = await _unitOfWork.Repository<StoreRelease, int>().GetWithSpecAsync(storeSpec);
            if (store == null) Result<SerialNumbersSearchDto>.Failure($"There Is No Store Release For This Serial Number: {request.SerialNumber}.");

            var result = new SerialNumbersSearchDto()
            {
                UnitName = store.Unit.UnitName,
                ItemName = storeItem.Items.ItemsName,
                DocumentNumber = store.DocumentNumber,
                OrderNumber = storeItem.Order.OrderNumber,
                SerialNumber = request.SerialNumber
            };

            return Result<SerialNumbersSearchDto>.Success(result, "Serial Number Details Retreived Succssefully.");
        }
    }
}
