using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries
{
    public class GetArchiveReceiptsByUserQuery : IRequest<List<ReceiptDto>>
    {
        
    }

    public class GetArchiveReceiptsByUserQueryHandler : IRequestHandler<GetArchiveReceiptsByUserQuery, List<ReceiptDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetArchiveReceiptsByUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public Task<List<ReceiptDto>> Handle(GetArchiveReceiptsByUserQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.Receipts
                .Where(x => x.Deleted != null && x.DeletedByUserId == _currentUserService.UserId);

            var mapped = entities.ProjectTo<ReceiptDto>(_mapper.ConfigurationProvider).ToList();

            foreach (var receiptDto in mapped)
            {
                if (_currentUserService.UserId != null)
                {
                    receiptDto.CurrentUserId = _currentUserService.UserId;
                }
            }

            return Task.FromResult(mapped);
        }
    }
}