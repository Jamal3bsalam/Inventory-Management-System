using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Command
{
    public class TransactionCommandHandler : IRequestHandler<TransactionCommand, Result<CustodayDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<CustodayTransfers, int> _fileServices;

        public TransactionCommandHandler(IUnitOfWork unitOfWork , IConfiguration configuration,IFileServices<CustodayTransfers,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<CustodayDto>> Handle(TransactionCommand request, CancellationToken cancellationToken)
        {
            if (request.CustodayId == null || request.NewRecipints == null || request.UnitId == null) return Result<CustodayDto>.Failure("Please Enter Valid Data.");

            var custodaySpec = new CustodaySpec(request.CustodayId.Value,true,true);
            var currentCustoday = await _unitOfWork.Repository<Custoday,int>().GetWithSpecAsync(custodaySpec);

            if (currentCustoday == null) return Result<CustodayDto>.Failure($"Custoday With This Id: {request.CustodayId} Not Found.");

            var newRecipintsCustodaySpec = new CustodaySpec(request.NewRecipints.Value);
            var newRecipintsCustoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(newRecipintsCustodaySpec);

            if(newRecipintsCustoday == null)
            {
                newRecipintsCustoday = new Custoday()
                {
                    UnitId = currentCustoday.UnitId,
                    RecipientsId = request.NewRecipints.Value,
                    CreatedDate = DateTime.Now,
                    CustodyItems = new List<CustodyItem>()
                };
                
                await _unitOfWork.Repository<Custoday,int>().AddAsync(newRecipintsCustoday);
            }

            // جلب العنصر المحدد فقط
            var item = currentCustoday.CustodyItems.FirstOrDefault(c => c.ItemId == request.ItemId);
            if (item == null)
                return Result<CustodayDto>.Failure("Item not found in the current custoday.");


            if (request.Quantity <= 0) return Result<CustodayDto>.Failure("Quantity must be greater than zero.");

                if(request.Quantity > item.Quantity) return Result<CustodayDto>.Failure($"Requested quantity ({request.Quantity}) Greater than available quantity ({item.Quantity}) for item {item.Item.ItemsName}.");

                var quantityToTransfer = request.Quantity;

                var newRecipintsCustodayItem = newRecipintsCustoday.CustodyItems.FirstOrDefault(i => i.ItemId == item.ItemId);
                if (newRecipintsCustodayItem != null)
                    newRecipintsCustodayItem.Quantity += quantityToTransfer;
                else
                    newRecipintsCustoday.CustodyItems.Add(new CustodyItem()
                    {
                        CustodyId = newRecipintsCustoday.Id,
                        ItemId = item.ItemId,
                        Quantity = quantityToTransfer,
                    });
                item.Quantity -= quantityToTransfer;

                if (item.Quantity == 0)
                {
                    currentCustoday.CustodyItems.Remove(item);
                    _unitOfWork.Repository<CustodyItem,int>().Delete(item);
                }

            newRecipintsCustoday.TransactionDate = request.TransactionDate.Value;
            newRecipintsCustoday.DocumentPath = request.File != null ? $"\\Files\\CustodayTransfers\\{_fileServices.Upload(request.File)}" : null;

            _unitOfWork.Repository<Custoday, int>().Update(currentCustoday);
            _unitOfWork.Repository<Custoday, int>().Update(newRecipintsCustoday);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<CustodayDto>.Failure("Faild To Transfer Custoday");

            var custodayTransfer = new CustodayTransfers() 
            {
                ItemId = request.ItemId.Value,UnitId = request.UnitId.Value ,OldRecipientId = currentCustoday.RecipientsId,
                NewRecipientId = newRecipintsCustoday.RecipientsId, Quantity = request.Quantity.Value,TransactionDate = request.TransactionDate.Value,
                DocumentPath = newRecipintsCustoday.DocumentPath
            };

            await _unitOfWork.Repository<CustodayTransfers, int>().AddAsync(custodayTransfer);
            await _unitOfWork.CompleteAsync();

            var transactionDto = new CustodayDto()
            {
                UnitName = newRecipintsCustoday.Unit.UnitName,
                OldRecipints = currentCustoday.Recipients.Name,
                NewRecipints = newRecipintsCustoday.Recipients.Name,
                ItemsDtos = newRecipintsCustoday.CustodyItems.Select(i => new CustodaysUnitsItemsDto()
                {
                    ItemId = i.Item.Id,
                    ItemName = i.Item.ItemsName,
                    Quantity = i.Quantity
                }).ToList(),
                TransactionDate = request.TransactionDate.Value,
                DocumentPath = newRecipintsCustoday.DocumentPath != null ? _configuration["BASEURL"] + newRecipintsCustoday.DocumentPath : null
            };

            return Result<CustodayDto>.Success(transactionDto, "Custoday Transfered Successfully.");
        }

    }
}
