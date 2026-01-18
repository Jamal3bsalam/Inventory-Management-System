using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


namespace Inventory.Mostafa.Application.UnitExp.Command.Add
{
    public class AddUnitExpenseCommandHandler : IRequestHandler<AddUnitExpenseCommand, Result<UnitExpensDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AddUnitExpenseCommandHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<UnitExpensDto>> Handle(AddUnitExpenseCommand request, CancellationToken cancellationToken)
        {
            if(request.UnitId == 0 || request.RecipientsId == 0 || request.ExpenseType == null || request.ExpenseDate == null || string.IsNullOrEmpty(request.AttachmentUrl) || request.UnitExpenseItemsDtos.Count == 0)
            { return Result<UnitExpensDto>.Failure("Please Enter The Fileds With The Right Data"); }

            var uSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(uSpec);
            if (unit == null)
                return Result<UnitExpensDto>.Failure($"Unit with Id {request.UnitId} not found");
            var recipints = unit.Recipients.FirstOrDefault(R => R.Id == request.RecipientsId);
            if (recipints == null) return Result<UnitExpensDto>.Failure($"Recipint With This Id: {request.RecipientsId} not found");

            var itemsDict = new Dictionary<int, string>();

            foreach (var item in request.UnitExpenseItemsDtos)
            {
                var itemSpec = new ItemSpec(item.ItemId.Value);
                var items = await _unitOfWork.Repository<Items, int>().GetWithSpecAsync(itemSpec);
                if (items == null)
                    return Result<UnitExpensDto>.Failure($"Order With This Id: {item.ItemId} not found");
                itemsDict[item.ItemId.Value] = items.ItemsName;

            }


            var unitExpense = new UnitExpense()
            {
                UnitId = request.UnitId,
                RecipientsId = request.RecipientsId,
                ExpenseType = request.ExpenseType,
                ExpenseDate = request.ExpenseDate,
                DocumentNumber = request.DocumentNumber != null ? request.DocumentNumber : null,
                AttachmentUrl = request.AttachmentUrl != null ? $"\\Files\\UnitExpense\\{request.AttachmentUrl}" : null,
                ExpenseItems = request.UnitExpenseItemsDtos.Select(u => new UnitExpenseItems() {ItemId = u.ItemId,Quantity = u.Quantity,ItemName = itemsDict[u.ItemId.Value]}).ToList()
            };

            await _unitOfWork.Repository<UnitExpense,int>().AddAsync(unitExpense);
            var result = await _unitOfWork.CompleteAsync();

            var spec = new UnitExpenseSpec(unitExpense.Id);
            var unitExp = await _unitOfWork.Repository<UnitExpense,int>().GetWithSpecAsync(spec);
            if (unitExp == null) return Result<UnitExpensDto>.Failure("Faild To Save This Unit Expense");

            var custSpec = new CustodaySpec(request.RecipientsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custSpec);

            if (custoday == null)
            {
                custoday = new Custoday()
                {
                    RecipientsId = request.RecipientsId.Value,
                    UnitId = request.UnitId.Value,
                    CreatedDate = DateTime.UtcNow,
                    CustodyItems = new List<CustodyItem>()
                };
                await _unitOfWork.Repository<Custoday, int>().AddAsync(custoday);
                await _unitOfWork.CompleteAsync();
            }
            else if (custoday.CustodyItems == null)
            {
                custoday.CustodyItems = new List<CustodyItem>();
            }

            foreach (var item in request.UnitExpenseItemsDtos)
            {
                var custodyItem = custoday?.CustodyItems?.FirstOrDefault(ci => ci.ItemId == item.ItemId && ci.IsDeleted == false);
                if (custodyItem == null)
                {
                    custodyItem = new CustodyItem
                    {
                        CustodyId = custoday.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity
                    };
                    await _unitOfWork.Repository<CustodyItem, int>().AddAsync(custodyItem);
                    await _unitOfWork.CompleteAsync();
                }
                else
                {
                    custodyItem.Quantity += item.Quantity;
                    _unitOfWork.Repository<CustodyItem, int>().Update(custodyItem);
                }

                var expenseItem = unitExp.ExpenseItems.FirstOrDefault(e => e.ItemId == item.ItemId);

                var custodyItemUnitExpense = new CustodyItemUnitExpense
                {
                    CustodyItemId = custodyItem.Id,
                    UnitExpenseItemId = expenseItem.Id,
                    Quantity = (int)item.Quantity
                };

                await _unitOfWork.Repository<CustodyItemUnitExpense, int>().AddAsync(custodyItemUnitExpense);
            }

            await _unitOfWork.CompleteAsync();

            var unitExpenseDto = new UnitExpensDto()
            {
                Id = unitExp.Id,
                UnitName = unitExp?.Unit?.UnitName,
                RecipientsName = unitExp?.Recipients?.Name,
                ExpenseType = unitExpense?.ExpenseType,
                AttachmentUrl = _configuration["BASEURL"] + unitExpense?.AttachmentUrl,
                DocumentNumber = unitExpense?.DocumentNumber,
                ExpenseDate = unitExpense?.ExpenseDate,
                UnitExpenseItemsDtos = unitExp?.ExpenseItems?.Select(u => new UnitExpensItemsDto() { ItemId = u.ItemId, ItemName = u.Item?.ItemsName ,Quantity = u.Quantity }).ToList()
            };

            if (result <= 0) return Result<UnitExpensDto>.Failure("Faild To Save This Unit Expense");

            return Result<UnitExpensDto>.Success(unitExpenseDto, "Unit Expense Saved Successfully");
             
        }
    }
}
