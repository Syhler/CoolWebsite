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
    public class GetPayedTransactionQuery : IRequest<List<TransactionDto>>
    {
    }

    public class GetPayedTransactionQueryHandler
        : IRequestHandler<GetPayedTransactionQuery, List<TransactionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetPayedTransactionQueryHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public Task<List<TransactionDto>> Handle(GetPayedTransactionQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.Transactions.Where(x => x.FromUserId == _currentUserService.UserId);

            var mapped = entities.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider).ToList();

            return Task.FromResult(mapped);
        }
    }
}