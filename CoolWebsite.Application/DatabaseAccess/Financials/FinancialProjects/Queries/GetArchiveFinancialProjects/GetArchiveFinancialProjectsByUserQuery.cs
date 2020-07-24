using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore.Internal;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetArchiveFinancialProjects
{
    public class GetArchiveFinancialProjectsByUserQuery : IRequest<List<FinancialProjectDto>>
    {
        
    }

    public class GetArchiveFinancialProjectsByUserHandler 
        : IRequestHandler<GetArchiveFinancialProjectsByUserQuery, List<FinancialProjectDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;


        public GetArchiveFinancialProjectsByUserHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context.UserId = currentUserService.UserId;
        }

        public Task<List<FinancialProjectDto>> Handle(GetArchiveFinancialProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.FinancialProjects
                .Include(x => x.FinancialProjectApplicationUsers)
                .Where(x => x.Deleted != null && x.DeletedByUserId == _currentUserService.UserId);

            var mapped = entities.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).ToList();

            return Task.FromResult(mapped);
        }
    }
}