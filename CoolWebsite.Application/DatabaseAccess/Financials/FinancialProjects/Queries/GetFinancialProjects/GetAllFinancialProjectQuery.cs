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
                .Include(x => x.FinancialProjectApplicationUsers)
                .Include(x => x.Receipts)
                .OrderByDescending(x => x.LastModified.HasValue)
                .ThenByDescending(x => x.Created)
                .ThenByDescending(x => x.LastModified);



            return new FinancialProjectsVm
            {
                FinancialProjects = entity.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).ToList()
            }; 
        }
    }
}