using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using Microsoft.Extensions.Configuration;
namespace Inventory.Mostafa.Application.Custodays.Command
{
    public class TransactionCommandHandler : IRequestHandler<TransactionCommand, Result<List<CustodayDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public TransactionCommandHandler(IUnitOfWork unitOfWork , IConfiguration configuration,IFileServices<CustodayTransfers,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<List<CustodayDto>>> Handle(TransactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var resultDtos = new List<CustodayDto>();

                foreach (var transfer in request.Items)
                {

                    if(request.NewRecipints == transfer.CurrentRecipints) return Result<List<CustodayDto>>.Failure("❌ المستلم الجديد لا يمكن أن يكون نفس المستلم القديم.");

                    // ✅ Step 1: احضر العهدة القديمة
                    var oldSpec = new CustodaySpec(transfer.CurrentRecipints);
                    var oldCustody = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(oldSpec);

                    //.GetFirstOrDefaultAsync(
                    //    x => x.UnitId == transfer.UnitId && x.RecipientsId == transfer.FromRecipientId,
                    //    include: x => x.CustodyItems
                    //);

                    if (oldCustody == null)
                        continue; // مفيش عهدة للمستلم القديم

                    // ✅ Step 2: احضر العهدة للمستلم الجديد أو أنشئها
                    var newSpec = new CustodaySpec(request.NewRecipints);
                    var newCustody = await _unitOfWork.Repository<Custoday,int>().GetWithSpecAsync(newSpec);
                        
                    //.GetFirstOrDefaultAsync(
                    //        x => x.UnitId == transfer.UnitId && x.RecipientsId == transfer.ToRecipientId,
                    //        include: x => x.CustodyItems
                    //    );

                    if (newCustody == null)
                    {
                        newCustody = new Custoday
                        {
                            UnitId = request.UnitId,
                            RecipientsId = request.NewRecipints,
                            CreatedDate = DateTime.Now,
                            CustodyItems = new List<CustodyItem>()
                        };
                        await _unitOfWork.Repository<Custoday,int>().AddAsync(newCustody);
                        await _unitOfWork.CompleteAsync();
                    }

                    // ✅ Step 3: التعامل مع كل منتج منقول
                    var oldItem = oldCustody.CustodyItems.FirstOrDefault(x => x.ItemId == transfer.ItemId);
                        if (oldItem == null || oldItem.Quantity < transfer.Quantity)
                            return Result<List<CustodayDto>>.Failure($"❌ المستلم القديم لا يمتلك كمية كافية من المنتج {transfer.ItemId}");

                        // خصم الكمية من القديم
                        oldItem.Quantity -= transfer.Quantity;
                        if (oldItem.Quantity == 0)
                        {
                           oldItem.IsDeleted = true;
                           _unitOfWork.Repository<CustodyItem,int>().Update(oldItem);
                        }

                        // أضف أو حدث عند المستلم الجديد
                        var newItem = newCustody.CustodyItems?.FirstOrDefault(x => x.ItemId == transfer.ItemId && x.IsDeleted == false);
                        if (newItem != null)
                            newItem.Quantity += transfer.Quantity;
                        else
                        {
                            newItem = new CustodyItem
                            {
                                CustodyId = newCustody.Id,
                                ItemId = transfer.ItemId,
                                Quantity = transfer.Quantity
                            };
                            await _unitOfWork.Repository<CustodyItem,int>().AddAsync(newItem);
                            await _unitOfWork.CompleteAsync();

                            var oldItemUnitExpenses = oldItem.UnitExpenseLinks; // افتراضياً علاقة 1-N بين CustodyItem و CustodyItemUnitExpense
                            foreach (var oldCiu in oldItemUnitExpenses)
                            {
                                var newCiu = new CustodyItemUnitExpense
                                {
                                    CustodyItemId = newItem.Id,
                                    UnitExpenseItemId = oldCiu.UnitExpenseItemId,
                                    Quantity = oldCiu.Quantity,
                                    CreatedAt = DateTime.Now
                                };
                                await _unitOfWork.Repository<CustodyItemUnitExpense, int>().AddAsync(newCiu);
                            }
                            await _unitOfWork.CompleteAsync();

                        }

                    newCustody.TransactionDate = DateOnly.FromDateTime(DateTime.Today);
                    newCustody.DocumentPath = request.FileName != null ? $"\\Files\\CustodayTransfers\\{request.FileName}" : null;
                    _unitOfWork.Repository<Custoday, int>().Update(newCustody);
                    // ✅ Step 4: تحديث المصروفات الخاصة بالعناصر المنقولة فقط
                    await UpdateExpensesAfterTransferAsync(request.Items,request.NewRecipints,request.UnitId,transfer.CurrentRecipints);

                    var transactionDto = new CustodayDto
                    {
                        UnitName = newCustody.Unit?.UnitName,
                        OldRecipints = oldCustody.Recipients?.Name,
                        NewRecipints = newCustody?.Recipients?.Name,
                        ItemsDtos = newCustody.CustodyItems.Select(i => new CustodaysUnitsItemsDto
                        {
                            ItemId = i.Item.Id,
                            ItemName = i.Item.ItemsName,
                            Quantity = i.Quantity
                        }).ToList(),
                        TransactionDate = newCustody.TransactionDate,
                        TransactionHijriDate = request.TransactionHijriDate,
                        DocumentPath = newCustody.DocumentPath != null? _configuration["BASEURL"] + newCustody.DocumentPath : null
                    };

                    resultDtos.Add(transactionDto);



                    // Save transfer record
                    var transferRecord = new CustodayTransfers
                    {
                        UnitId = request.UnitId,
                        ItemId = transfer.ItemId,
                        OldRecipientId = oldCustody.RecipientsId,
                        NewRecipientId = newCustody.RecipientsId,
                        Quantity = transfer.Quantity,
                        TransactionHijriDate = request.TransactionHijriDate,
                        DocumentPath = newCustody.DocumentPath,
                    };
                    await _unitOfWork.Repository<CustodayTransfers, int>().AddAsync(transferRecord);

                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();

                

                return Result<List<CustodayDto>>.Success(resultDtos,"✅ تمت عملية المناقلة وتحديث المصروفات بنجاح لكل العهد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<List<CustodayDto>>.Failure($"حدث خطأ أثناء عملية المناقلة: {ex.Message}");
            }
        }

        private async Task UpdateExpensesAfterTransferAsync(ICollection<CustodayItemDto> transfer,int newRecipinst,int unitId,int currentRecipints)
        {
            // 🔹 أولًا: مصروفات الوحدة
            var expenseSpec = new UnitExpenseSpec(unitId, currentRecipints);
            var relatedExpenses = await _unitOfWork.Repository<UnitExpense, int>().GetAllWithSpecAsync(expenseSpec);

            var transferredItemIds = transfer.Select(i => i.ItemId).ToList();

            foreach (var oldExpense in relatedExpenses)
            {
                if (oldExpense.ExpenseItems == null || !oldExpense.ExpenseItems.Any())
                    continue;

                var transferredItems = oldExpense.ExpenseItems
                    .Where(item => transferredItemIds.Contains(item.ItemId ?? 0))
                    .ToList();

                if (!transferredItems.Any())
                    continue;

                var remainingItems = oldExpense.ExpenseItems
                    .Where(item => !transferredItemIds.Contains(item.ItemId ?? 0))
                    .ToList();

                if (remainingItems.Count == 0)
                {
                    // 🟩 كل العناصر اتنقلت → حدث المصروف القديم
                    oldExpense.RecipientsId = newRecipinst;
                    _unitOfWork.Repository<UnitExpense, int>().Update(oldExpense);
                }
                else
                {
                    // 🟨 جزئي → قسم المصروف
                    oldExpense.ExpenseItems = remainingItems;
                    _unitOfWork.Repository<UnitExpense, int>().Update(oldExpense);
                    _unitOfWork.Repository<UnitExpenseItems, int>().DeleteRange(transferredItems);

                    var newUnitExpense = new UnitExpense
                    {
                        UnitId = oldExpense.UnitId,
                        RecipientsId = newRecipinst,
                        ExpenseType = "مناقلة عهدة",
                        DocumentNumber = oldExpense.DocumentNumber,
                        AttachmentUrl = oldExpense.AttachmentUrl,
                        ExpenseDate = oldExpense.ExpenseDate,
                        StoreReleaseId = oldExpense.StoreReleaseId,
                        ExpenseItems = transferredItems.Select(i => new UnitExpenseItems
                        {
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            Quantity = i.Quantity,
                        }).ToList()
                    };
                    await _unitOfWork.Repository<UnitExpense, int>().AddAsync(newUnitExpense);

                }

                // 🔹 ثانيًا: الـ StoreRelease
                if (oldExpense.StoreReleaseId.HasValue)
                {
                    var storeReleaseSpec = new StoreReleaseSpec(oldExpense.StoreReleaseId.Value);
                    var oldRelease = await _unitOfWork.Repository<StoreRelease,int>().GetWithSpecAsync(storeReleaseSpec);

                    if (oldRelease != null)
                    {
                        if (oldRelease.StoreReleaseItems == null || !oldRelease.StoreReleaseItems.Any())
                            continue;

                        var transferredReleaseItems = oldRelease.StoreReleaseItems
                            .Where(item => transferredItemIds.Contains(item.ItemId ?? 0))
                            .ToList();

                        var remainingReleaseItems = oldRelease.StoreReleaseItems
                            .Where(item => !transferredItemIds.Contains(item.ItemId ?? 0))
                            .ToList();

                        if (transferredReleaseItems.Count == 0)
                            continue;

                        if (remainingReleaseItems.Count == 0)
                        {
                            // 🟩 كل العناصر في الـ StoreRelease اتنقلت → حدث المستلم
                            oldRelease.RecipientsId = newRecipinst;
                            _unitOfWork.Repository<StoreRelease, int>().Update(oldRelease);
                        }
                        else
                        {
                            // 🟨 جزء فقط → قسم الـ StoreRelease
                            oldRelease.StoreReleaseItems = remainingReleaseItems;
                            _unitOfWork.Repository<StoreRelease, int>().Update(oldRelease);
                            _unitOfWork.Repository<StoreReleaseItem, int>().DeleteRange(transferredReleaseItems);
                            var newRelease = new StoreRelease
                            {
                                UnitId = oldRelease.UnitId,
                                RecipientsId = newRecipinst,
                                ReleaseDate = oldRelease.ReleaseDate,
                                DocumentNumber = oldRelease.DocumentNumber,
                                DocumentPath = oldRelease.DocumentPath,
                                StoreReleaseItems = transferredReleaseItems.Select(i => new StoreReleaseItem
                                {
                                    ItemId = i.ItemId,
                                    Quantity = i.Quantity,
                                    OrderId = i.OrderId,
                                    OrderItemId = i.OrderItemId,
                                    SerialNumbers = i.SerialNumbers,
                                }).ToList()
                            };

                           await _unitOfWork.Repository<StoreRelease, int>().AddAsync(newRelease);
                        }

                        await _unitOfWork.CompleteAsync();
                    }
                }
            }
        }

    }


}



