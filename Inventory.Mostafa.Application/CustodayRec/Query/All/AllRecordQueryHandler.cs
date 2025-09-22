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

namespace Inventory.Mostafa.Application.CustodayRec.Query.All
{
    public class AllRecordQueryHandler : IRequestHandler<AllRecordQuery, Result<IEnumerable<RecordDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AllRecordQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<IEnumerable<RecordDto>>> Handle(AllRecordQuery request, CancellationToken cancellationToken)
        {
            var records = await _unitOfWork.Repository<CustodayRecord, int>().GetAllAsync();
            if (records.Count() == 0) return Result<IEnumerable<RecordDto>>.Failure("No Records Fonded");

            var recordsDto = records.Select(r => new RecordDto()
            {
                Id = r.Id,
                Notic = r.Notic,
                Date = r.Date,
                FileUrl = _configuration["BASEURL"] + r.FileUrl,
            });

            return Result<IEnumerable<RecordDto>>.Success(recordsDto,"All Records Retrived Successfully");
        }
    }
}
