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

namespace Inventory.Mostafa.Application.CustodayRec.Command.Update
{
    public class UpdateRecordCommandHandler : IRequestHandler<UpdateRecordCommand, Result<RecordDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<CustodayRecord, int> _fileServices;

        public UpdateRecordCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IFileServices<CustodayRecord, int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<RecordDto>> Handle(UpdateRecordCommand request, CancellationToken cancellationToken)
        {
            if(request.RecordId == 0 && request.RecordId == null) Result<RecordDto>.Failure("Please Enter A Valid Id");

            var record = await _unitOfWork.Repository<CustodayRecord, int>().GetByIdAsync(request.RecordId.Value);
            if (record == null) return Result<RecordDto>.Failure($"Custody Record For This Id:{request.RecordId} Not Found");

            if(!string.IsNullOrEmpty(request.Notic))
                record.Notic = request.Notic;

            if(request.Date != null)
                record.Date = request.Date;

            if (!string.IsNullOrEmpty(record.FileUrl))
                if (request.File != null)
                {
                    _fileServices.Delete(record.FileUrl);
                    var file = _fileServices.Upload(request.File);
                    record.FileUrl = $"\\Files\\CustodayRecord\\{file}";
                }

            _unitOfWork.Repository<CustodayRecord,int>().Update(record);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<RecordDto>.Failure("Faild To Update Record");

            var recordDto = new RecordDto()
            {
                Id = record.Id,
                Notic = record.Notic,
                Date = record.Date,
                FileUrl = _configuration["BASEURL"] + record.FileUrl
            };

            return Result<RecordDto>.Success(recordDto, "Record Update Successfully.");

        }
    }
}
