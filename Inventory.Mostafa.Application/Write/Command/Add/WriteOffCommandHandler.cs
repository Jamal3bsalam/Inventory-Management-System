using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using NHibernate.Loader.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


namespace Inventory.Mostafa.Application.Write.Command.Add
{
    public class WriteOffCommandHandler : IRequestHandler<WriteOffCommand, Result<WriteOffDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<WriteOff, int> _fileServices;

        public WriteOffCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IFileServices<WriteOff, int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<WriteOffDto>> Handle(WriteOffCommand request, CancellationToken cancellationToken)
        {


            var returnSpec = new ReturnSpec(request.ReturnsId.Value);
            var returnEntity = await _unitOfWork.Repository<Returns, int>().GetWithSpecAsync(returnSpec);
            if (returnEntity == null)
                return Result<WriteOffDto>.Failure($"Return with Id: {request.ReturnsId} not found.");


            var unitSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(unitSpec);
            if (unit == null) return Result<WriteOffDto>.Failure($"Unit With Id: {request.UnitId} Not Fount");

            var recipintsSpec = new RecipintsSpec(request.RecipintsId.Value);
            var recipints = await _unitOfWork.Repository<Recipients, int>().GetWithSpecAsync(recipintsSpec);
            if (recipints == null) return Result<WriteOffDto>.Failure($"Recipints With Id: {request.RecipintsId} Not Fount");

            var storeItemSpec = new StoreItemSpec(returnEntity.storeReleaseItemId.Value, true);
            var storeItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetWithSpecAsync(storeItemSpec);
            if (storeItem == null)
                return Result<WriteOffDto>.Failure($"Store Item with Id: {returnEntity.storeReleaseItemId} not found.");

            if(request.Quantity > returnEntity.Quantity)
                return Result<WriteOffDto>.Failure($"WriteOff quantity cannot be greater than returned quantity (Available: {returnEntity.Quantity}, Requested: {request.Quantity}).");


            // 1️⃣ قلل الكمية في الـ Return (لأن جزء منها اتسقط)
            returnEntity.Quantity -= request.Quantity;
            _unitOfWork.Repository<Returns, int>().Update(returnEntity);

            // 2️⃣ زوّد ConsumedQuantity تاني (لأن التالف فعليًا مش بيرجع للمخزن)
            storeItem.OrderItem.ConsumedQuantity += request.Quantity;
            if (storeItem.OrderItem.ConsumedQuantity > storeItem.OrderItem.Quantity)
                return Result<WriteOffDto>.Failure("Consumed quantity cannot exceed ordered quantity");
            _unitOfWork.Repository<StoreReleaseItem, int>().Update(storeItem);

            var writeOff = new WriteOff()
            {
                UnitId = request.UnitId,
                RecipintsId = request.RecipintsId,
                ReturnId = request.ReturnsId,
                Quantity = request.Quantity,
                DocumentPath = request.Documet != null ? $"\\Files\\WriteOff\\{_fileServices.Upload(request.Documet)}" : null
            };

            await _unitOfWork.Repository<WriteOff, int>().AddAsync(writeOff);

            // 4️⃣ احفظ التغييرات
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return Result<WriteOffDto>.Failure("Failed to add WriteOff");

            var dto = new WriteOffDto
            {
                Id = writeOff.Id,
                UnitName = unit.UnitName,
                RecipintsName = recipints.Name,
                ItemName = storeItem.OrderItem.ItemName,
                Quantity = writeOff.Quantity,
                DocumetPath = writeOff.DocumentPath != null ? _configuration["BASEURL"] + writeOff.DocumentPath : null
            };

            return Result<WriteOffDto>.Success(dto, "WriteOff added successfully");
        }
    }
}
