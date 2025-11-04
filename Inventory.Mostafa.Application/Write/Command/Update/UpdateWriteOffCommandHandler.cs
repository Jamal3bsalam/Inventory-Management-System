using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Write.Command.Update
{
    public class UpdateWriteOffCommandHandler : IRequestHandler<UpdateWriteOffCommand, Result<WriteOffDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<WriteOff, int> _fileServices;

        public UpdateWriteOffCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IFileServices<WriteOff, int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<WriteOffDto>> Handle(UpdateWriteOffCommand request, CancellationToken cancellationToken)
        {
            // 1- هات الـ WriteOff القديم
            var writeOffSpce = new WriteOffSpec(request.Id.Value);
            var writeOff = await _unitOfWork.Repository<WriteOff, int>().GetWithSpecAsync(writeOffSpce);
            if (writeOff == null) return Result<WriteOffDto>.Failure("WriteOff not found");

            // 2- هات الـ Return اللي معمول منه WriteOff
            var returnSpec = new ReturnSpec(writeOff.ReturnId.Value);
            var returns = await _unitOfWork.Repository<Returns, int>().GetWithSpecAsync(returnSpec);
            if (returns == null) return Result<WriteOffDto>.Failure("Return not found");

            // 3- هات الـ StoreItem عشان تعدل في الـ ConsumedQuantity
            var expenseItemsSpec = new UnitExpenseItemSpec(returns.Expense.Id);
            var expenseItems = await _unitOfWork.Repository<UnitExpenseItems, int>().GetWithSpecAsync(expenseItemsSpec);

            // 4- احسب الفرق بين الكمية القديمة والجديدة
            int oldQty = writeOff.Quantity.Value;
            int newQty = request.Quantity.Value ;
            int diff = newQty - oldQty;

            if (diff > 0)
            {
                // المستخدم زوّد الكمية التالفة → المستهلك يزيد
                if (returns.Quantity < diff)
                    return Result<WriteOffDto>.Failure("Not enough quantity in return to write off");

                returns.Quantity -= diff;  // التالف زاد، المترجع يقل
                returns.WriteOfQuantity = newQty;
            }
            else if (diff < 0)
            {
                // المستخدم قلل الكمية التالفة → المخزن يستعيدها
                int restore = Math.Abs(diff);

                returns.Quantity += restore;                    // بيرجع للمترجع
                returns.WriteOfQuantity = newQty;
            }

            // 5- عدل بيانات الـ WriteOff نفسه
            writeOff.Quantity = newQty;

            if (request.File != null && !string.IsNullOrEmpty(request.File.FileName))
            {
                if (!string.IsNullOrEmpty(writeOff.DocumentPath))
                {
                    _fileServices.Delete(writeOff.DocumentPath);
                }

                var fileName = _fileServices.Upload(request.File);
                returns.DocumentPath = $"\\Files\\WriteOff\\{fileName}";
            }


            _unitOfWork.Repository<WriteOff, int>().Update(writeOff);
            _unitOfWork.Repository<Returns, int>().Update(returns);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<WriteOffDto>.Failure("Failed to update WriteOff");

            // 6- رجّع DTO
            var dto = new WriteOffDto
            {
                Id = writeOff.Id,
                UnitName = writeOff.Unit.UnitName,
                RecipintsName = writeOff.Recipients.Name,
                ItemName = expenseItems.Item.ItemsName,
                Quantity = writeOff.Quantity,
                DocumetPath = writeOff.DocumentPath != null ? _configuration["BASEURL"] + writeOff.DocumentPath : null
            };

            return Result<WriteOffDto>.Success(dto, "WriteOff updated successfully");
        }
    }
}
