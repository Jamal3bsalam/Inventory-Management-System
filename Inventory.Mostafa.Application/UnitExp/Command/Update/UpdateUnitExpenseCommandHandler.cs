using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Command.Update
{
    public class UpdateUnitExpenseCommandHandler : IRequestHandler<UpdateUnitExpenseCommand, Result<UnitExpensDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UpdateUnitExpenseCommandHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<UnitExpensDto>> Handle(UpdateUnitExpenseCommand request, CancellationToken cancellationToken)
        {
            if (request.UnitExpenseId.Value == null) return Result<UnitExpensDto>.Failure("Please Enter A Valid Id.");

            var spec = new UnitExpenseSpec(request.UnitExpenseId.Value);
            var unitExpense = await _unitOfWork.Repository<UnitExpense,int>().GetWithSpecAsync(spec);
            if (unitExpense == null)
                return Result<UnitExpensDto>.Failure($"UnitExpense with Id {request.UnitExpenseId} not found");

            if (request.UnitId.HasValue && request.UnitId != 0)
                unitExpense.UnitId = request.UnitId.Value;

            if (request.RecipientsId.HasValue && request.RecipientsId != 0)
                unitExpense.RecipientsId = request.RecipientsId.Value;

            if (!string.IsNullOrWhiteSpace(request.ExpenseType))
                unitExpense.ExpenseType = request.ExpenseType;

            if (!string.IsNullOrWhiteSpace(request.AttachmentUrl))
                unitExpense.AttachmentUrl = $"\\Files\\UnitExpense\\{request.AttachmentUrl}";

            if (!string.IsNullOrWhiteSpace(request.DocumentNumber))
                unitExpense.DocumentNumber = request.DocumentNumber;

            if (request.ExpenseDate.HasValue)
                unitExpense.ExpenseDate = request.ExpenseDate.Value;

            if (request.UnitExpenseItemsDtos != null && request.UnitExpenseItemsDtos.Any())
            {
                // ممكن تعمل مسح للقديمة وإضافة جديدة
                var existingItems = unitExpense.ExpenseItems.ToList();

                if (existingItems.Any())
                {
                    // 2️⃣ امسحهم كلهم من الريبو
                    foreach (var oldItem in existingItems)
                    {
                        _unitOfWork.Repository<UnitExpenseItems, int>().Delete(oldItem);
                    }
                }

                // 3️⃣ ضيف الجديد
                foreach (var itemDto in request.UnitExpenseItemsDtos)
                {
                    await _unitOfWork.Repository<UnitExpenseItems, int>().AddAsync(new UnitExpenseItems
                    {
                        UnitExpenseId = unitExpense.Id, // مهم عشان يحط الـ FK صح
                        ItemId = itemDto.ItemId,
                        Quantity = itemDto.Quantity
                    });
                }
            }

            _unitOfWork.Repository<UnitExpense, int>().Update(unitExpense);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<UnitExpensDto>.Failure("Faild To Update Unit Expense");

            var unitExpenseDto = new UnitExpensDto()
            {
                Id = unitExpense.Id,
                UnitName = unitExpense?.Unit?.UnitName,
                RecipientsName = unitExpense?.Recipients?.Name,
                ExpenseType = unitExpense?.ExpenseType,
                AttachmentUrl = _configuration["BASEURL"] + unitExpense?.AttachmentUrl,
                DocumentNumber = unitExpense?.DocumentNumber,
                ExpenseDate = unitExpense?.ExpenseDate,
                UnitExpenseItemsDtos = unitExpense?.ExpenseItems?.Select(u => new UnitExpensItemsDto() { ItemId = u.ItemId, ItemName = u.Item?.ItemsName, Quantity = u.Quantity }).ToList()
            };

            return Result<UnitExpensDto>.Success(unitExpenseDto, "Unit Expense Updated Successfully");

        }
    }
}
