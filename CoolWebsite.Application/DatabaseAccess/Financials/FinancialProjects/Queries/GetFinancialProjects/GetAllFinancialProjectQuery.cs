using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
            ICurrentUserService currentUserService, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<FinancialProjectsVm> Handle(GetAllFinancialProjectQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Where(x => x.FinancialProjectApplicationUsers.Any(x => x.UserId == _currentUser.UserID))
                .Where(x => x.Deleted == null)
                .OrderByDescending(x => x.LastModified.HasValue)
                .ThenByDescending(x => x.Created)
                .ThenByDescending(x => x.LastModified);


            var projects = entity.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).ToList();

            RemoveDeletedReceiptFromDto(projects);
            

            return new FinancialProjectsVm
            {
                FinancialProjects = projects
            }; 
        }

        private void RemoveDeletedReceiptFromDto(List<FinancialProjectDto> projects)
        {
            foreach (var financialProjectDto in projects)
            {
                financialProjectDto.Receipts = financialProjectDto.Receipts.Where(x => x.Deleted == null).ToList();
            }
        }
    }
}