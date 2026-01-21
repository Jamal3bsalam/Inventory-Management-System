//using Inventory.Mostafa.Application.Abstraction.Files;
//using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
//using Inventory.Mostafa.Application.Contract.Return;
//using Inventory.Mostafa.Domain.Entities;
//using Inventory.Mostafa.Domain.Entities.AssetsReturns;
//using Inventory.Mostafa.Domain.Entities.CustodayTables;
//using Inventory.Mostafa.Domain.Entities.Identity;
//using Inventory.Mostafa.Domain.Entities.Store;
//using Inventory.Mostafa.Domain.Entities.UnitEx;
//using Inventory.Mostafa.Domain.Shared;
//using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
//using Inventory.Mostafa.Domain.Specification.Return;
//using Inventory.Mostafa.Domain.Specification.Store;
//using Inventory.Mostafa.Domain.Specification.UnitExp;
//using Inventory.Mostafa.Domain.Specification.UnitSpecification;
//using MediatR;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


//namespace Inventory.Mostafa.Application.Return.Command.Update
//{
//    internal class UpdateReturnCommandHandler : IRequestHandler<UpdateReturnCommand, Result<ReturnDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IConfiguration _configuration;
//        private readonly IFileServices<Returns, int> _fileServices;

//        public UpdateReturnCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IFileServices<Returns, int> fileServices)
//        {
//            _unitOfWork = unitOfWork;
//            _configuration = configuration;
//            _fileServices = fileServices;
//        }
//        public async Task<Result<ReturnDto>> Handle(UpdateReturnCommand request, CancellationToken cancellationToken)
//        {
//            if (request.ReturnId == 0 || request.ReturnId == null) return Result<ReturnDto>.Failure("Please Enter Valid Id.");
//            var returnSpec = new ReturnSpec(request.ReturnId.Value);
//            var returns = await _unitOfWork.Repository<Returns,int>().GetWithSpecAsync(returnSpec);
//            if (returns == null) return Result<ReturnDto>.Failure($"Return With This Id: {request.ReturnId} Not Found");

//            var recipintsSpec = new RecipintsSpec(returns.RecipientsId.Value);
//            var recipints = await _unitOfWork.Repository<Recipients, int>().GetWithSpecAsync(recipintsSpec);

//            var unitSpec = new UnitSpec(returns.UnitId.Value);
//            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(unitSpec);

//            var custodaySpec = new CustodaySpec(returns.RecipientsId.Value);
//            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);

//            var unitExpenseSpec = new UnitExpenseSpec(returns.ExpenseId.Value);
//            var unitExpense = await _unitOfWork.Repository<UnitExpense, int>().GetWithSpecAsync(unitExpenseSpec);

//            var expenseItemsSpec = new UnitExpenseItemSpec(unitExpense.Id);
//            var expenseItems = await _unitOfWork.Repository<UnitExpenseItems, int>().GetAllWithSpecAsync(expenseItemsSpec);

//            if (expenseItems == null) return Result<ReturnDto>.Failure($"Unit Expense Item For Unit Expense With Id: {returns.ExpenseId} Not Fount");


//            var selectedItem = expenseItems.FirstOrDefault(i => i.ItemId == returns.ItemId);

//            var storeItemsSpec = new StoreItemSpec(unitExpense.StoreReleaseId.Value, true);
//            var storeItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetAllWithSpecAsync(storeItemsSpec);
//            var selectedStoreItem = storeItem.FirstOrDefault(i => i.ItemId == returns.ItemId);


//            if (request.Quantity > selectedItem.Quantity) return Result<ReturnDto>.Failure($"Return quantity cannot be greater than the released quantity(Available: {selectedStoreItem.Quantity}, Requested: {request.Quantity}).");


//            if (!string.IsNullOrEmpty(request.Reason))
//                returns.Reason = request.Reason;

//            if (request.File != null && !string.IsNullOrEmpty(request.File.FileName))
//            {
//                if (!string.IsNullOrEmpty(returns.DocumentPath))
//                {
//                    _fileServices.Delete(returns.DocumentPath);
//                }

//                var fileName = _fileServices.Upload(request.File);
//                returns.DocumentPath = $"\\Files\\Returns\\{fileName}";
//            }


//            if (request.Quantity.HasValue && request.Quantity > 0)
//            {

//                //var old = returns.Quantity;
//                //var newQ = request.Quantity;

//                //var diff = newQ - old;
//                if (diff != 0)
//                {
//                    returns.Quantity = newQ;
//                    var custodayItems = custoday?.CustodyItems?.FirstOrDefault(c => c.CustodyId == custoday.Id && c.ItemId == selectedStoreItem.ItemId);
//                    if (custodayItems != null)
//                    {
//                        custodayItems.Quantity -= diff;
//                        if (custodayItems.Quantity < 0)
//                            return Result<ReturnDto>.Failure("Custody quantity cannot be negative");

//                        _unitOfWork.Repository<CustodyItem, int>().Update(custodayItems);
//                    }
//                    selectedStoreItem.Quantity -= diff;
//                    if (selectedStoreItem.Quantity < 0)
//                        return Result<ReturnDto>.Failure("quantity cannot be negative");
//                    selectedItem.Quantity -= diff;
//                    if(selectedItem.Quantity < 0)
//                        return Result<ReturnDto>.Failure("quantity cannot be negative");

//                    _unitOfWork.Repository<StoreReleaseItem, int>().Update(selectedStoreItem);
//                    _unitOfWork.Repository<UnitExpenseItems, int>().Update(selectedItem);
//                }

//            }

//            var result = await _unitOfWork.CompleteAsync();
//            if (result <= 0) return Result<ReturnDto>.Failure("Faild To Update Returns");

//            var returnDto = new ReturnDto()
//            {
//                Id = returns.Id,
//                UnitName = unit.UnitName,
//                RecipintsName = recipints.Name,
//                //ItemName = selectedStoreItem.OrderItem.ItemName,
//                DocumentUrl = returns.DocumentPath != null ? _configuration["BASEURL"] + returns.DocumentPath : null,
//                //Quantity = returns.Quantity,
//                Reason = returns.Reason,
//            };

//            return Result<ReturnDto>.Success(returnDto, "Returns Updated Successfully");


//        }
//    }
//}
