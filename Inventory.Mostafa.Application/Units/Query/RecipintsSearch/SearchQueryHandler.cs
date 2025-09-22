using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Query.RecipintsSearch
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, Result<IEnumerable<RecipintsDtos>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<RecipintsDtos>>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Search)) return Result<IEnumerable<RecipintsDtos>>.Failure("Please Enter A Valid Name.");

            var spec = new RecipintsSpec(request.Search);
            var recipints = await _unitOfWork.Repository<Recipients,int>().GetAllWithSpecAsync(spec);

            if(recipints == null) return Result<IEnumerable<RecipintsDtos>>.Failure("No Recipints With This Name.");

            var recipintsDto = recipints.Select(R => new RecipintsDtos()
            {
                Id = R.Id,
                Name = R.Name,
                UnitName = R.Unit?.UnitName
            });

            return Result<IEnumerable<RecipintsDtos>>.Success(recipintsDto, "Recipints Retrived Successfully");

        }
    }
}
