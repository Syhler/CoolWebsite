using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries
{
    public class GetReceivedTransactionByProjectQuery : IRequest<List<TransactionDto>>
    {
        public string FinancialProjectId { get; set; } = null!;
    }

    public class GetReceivedTransactionByProjectQueryHandler : IRequestHandler<GetReceivedTransactionByProjectQuery, List<TransactionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetReceivedTransactionByProjectQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public Task<List<TransactionDto>> Handle(GetReceivedTransactionByProjectQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.Transactions
                .Where(x => x.FinancialProjectId == request.FinancialProjectId &&
                            x.ToUserId == _currentUserService.UserId);

            var mapped = entity.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider).ToList();

            return Task.FromResult(mapped);
        }
    }
    
    
}