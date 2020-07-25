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
    public class GetPayedTransactionByProjectQuery : IRequest<List<TransactionDto>>
    {
        public string FinancialProjectId { get; set; } = null!;
    }

    public class GetPayedTransactionByProjectQueryHandler
        : IRequestHandler<GetPayedTransactionByProjectQuery, List<TransactionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;


        public GetPayedTransactionByProjectQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public Task<List<TransactionDto>> Handle(GetPayedTransactionByProjectQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.Transactions
                .Where(x => x.FromUserId == _currentUserService.UserId &&
                            x.FinancialProjectId == request.FinancialProjectId);

            var mapped = entity.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider).ToList();

            return Task.FromResult(mapped);
        }
    }
    
    
}