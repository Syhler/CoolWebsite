using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetAllFinancialProjectQuery : IRequest<FinancialProjectsVm>
    {
        
    }

    public class GetAllFinancialProjectQueryHandler : IRequestHandler<GetAllFinancialProjectQuery,FinancialProjectsVm>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetAllFinancialProjectQueryHandler(IApplicationDbContext context, IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<FinancialProjectsVm> Handle(GetAllFinancialProjectQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.FinancialProjects
                //.Where(x => x.FinancialProjectApplicationUsers.Any(y => y.UserId != _currentUser.UserID))
                .Include(x => x.FinancialProjectApplicationUsers)
                .OrderByDescending(x => x.LastModified.HasValue)
                .ThenByDescending(x => x.Created)
                .ThenByDescending(x => x.LastModified)
                .Select(x => new
                {
                    x,
                    Receipts = x.Receipts.Where(x => x.Deleted == null)
                })
                .Select(x => x.x);


            var projects = entity.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).ToList();

            

            return new FinancialProjectsVm
            {
                FinancialProjects = projects
            }; 
        }
    }
}