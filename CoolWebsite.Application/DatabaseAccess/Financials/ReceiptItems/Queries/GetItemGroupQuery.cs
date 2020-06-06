using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Domain.Entities.Enums;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries
{
    public class GetItemGroupQuery : IRequest<List<ItemGroupDto>>
    {
        
    }

    public class GetItemGroupQueryHandler : IRequestHandler<GetItemGroupQuery, List<ItemGroupDto>>
    {
        public async Task<List<ItemGroupDto>> Handle(GetItemGroupQuery request, CancellationToken cancellationToken)
        {
            return Enum.GetValues(typeof(ItemGroup))
                .Cast<ItemGroup>()
                .Select(x => new ItemGroupDto {Value = (int) x, Name = x.ToString()})
                .ToList();
        }
    }
}