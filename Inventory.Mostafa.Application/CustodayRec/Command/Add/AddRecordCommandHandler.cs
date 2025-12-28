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

namespace Inventory.Mostafa.Application.CustodayRec.Command.Add
{
    public class AddRecordCommandHandler : IRequestHandler<AddRecordCommand, Result<RecordDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<CustodayRecord, int> _fileServices;

        public AddRecordCommandHandler(IUnitOfWork unitOfWork , IConfiguration configuration , IFileServices<CustodayRecord,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<RecordDto>> Handle(AddRecordCommand request, CancellationToken cancellationToken)
        {
            if (request.Notic == null) return Result<RecordDto>.Failure("Please Enter A Valid Notice");
            if (request.File.Length == 0 && string.IsNullOrEmpty(request.File.Name)) return Result<RecordDto>.Failure("No File Uploaded");


            var file = _fileServices.Upload(request.File);
            

            var record = new CustodayRecord()
            {
                Notic = request.Notic,
                Date = request.Date,
                FileUrl = $"\\Files\\CustodayRecord\\{file}"
            };

            await _unitOfWork.Repository<CustodayRecord,int>().AddAsync(record);
            var result =  await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<RecordDto>.Failure("Faild To Add Record");

            var recordDto = new RecordDto()
            {
                Id = record.Id,
                Notic = record.Notic,
                Date = record.Date,
                FileUrl = _configuration["BASEURL"] + record.FileUrl
            };


            return Result<RecordDto>.Success(recordDto,"Record Add Successfully.");
        }
    }
}
