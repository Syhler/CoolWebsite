using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetFinancialProjectsByUserIdQuery : IRequest<FinancialProjectsVm>
    {
        public string UserId { get; set; }
    }

    public class GetFinancialProjectsByUserIdQueryHandler : IRequestHandler<GetFinancialProjectsByUserIdQuery, FinancialProjectsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFinancialProjectsByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<FinancialProjectsVm> Handle(GetFinancialProjectsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var projects = _context.FinancialProjects.Where(x =>
                    x.FinancialProjectApplicationUsers.Any(user => user.UserId == request.UserId))
                .Include(x => x.Receipts)
                .Include(x => x.FinancialProjectApplicationUsers);

            if (projects == null || projects.ToList().Count <= 0)
            {
                throw new NotFoundException(nameof(Domain.Entities.Financial.FinancialProject), request.UserId);
            }
            
            
            return new FinancialProjectsVm
            {
                FinancialProjects = projects.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).ToList()
            };
        }
    }
}