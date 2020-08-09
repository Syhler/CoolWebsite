using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries
{
    public class GetReceiptItemByReceiptIdQuery : IRequest<List<ReceiptItemDto>>
    {
        public string ReceiptId { get; set; } = null!;
    }
    
    public class GetReceiptItemByReceiptIdQueryHandler : IRequestHandler<GetReceiptItemByReceiptIdQuery,
        List<ReceiptItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;


        public GetReceiptItemByReceiptIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _context.UserId = currentUserService.UserId;
        }

        public Task<List<ReceiptItemDto>> Handle(GetReceiptItemByReceiptIdQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.ReceiptItems.Where(x => x.ReceiptId == request.ReceiptId);

            if ( entities == null || !entities.Any())
            {
                throw new NotFoundException(nameof(ReceiptItem), request.ReceiptId);
            }

            var mapped = entities.ProjectTo<ReceiptItemDto>(_mapper.ConfigurationProvider);

            return Task.FromResult(mapped.ToList());
        }
    }
}