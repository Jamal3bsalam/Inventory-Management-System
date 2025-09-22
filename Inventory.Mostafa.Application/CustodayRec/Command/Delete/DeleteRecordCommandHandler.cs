using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.CustodayRec;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.CustodayRec.Command.Delete
{
    public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRecordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0 && request.Id == null) Result<string>.Failure("Please Enter A Valid Id");

            var record = await _unitOfWork.Repository<CustodayRecord, int>().GetByIdAsync(request.Id.Value);
            if (record == null) return Result<string>.Failure($"Custody Record For This Id:{request.Id} Not Found");

            _unitOfWork.Repository<CustodayRecord, int>().Delete(record);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<string>.Failure("Faild To Delete Record");

            return Result<string>.Success("Record Deleted Successfully.");

        }
    }
}
