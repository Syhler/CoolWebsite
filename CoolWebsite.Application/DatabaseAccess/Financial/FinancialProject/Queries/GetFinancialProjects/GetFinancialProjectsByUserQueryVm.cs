using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects
{
    public class GetFinancialProjectsByUserQuery : IRequest<FinancialProjectsVm>
    {
        public string UserId { get; set; }
    }

    public class GetFinancialProjectsByUserQueryHandler : IRequestHandler<GetFinancialProjectsByUserQuery, FinancialProjectsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _service;

        public GetFinancialProjectsByUserQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<FinancialProjectsVm> Handle(GetFinancialProjectsByUserQuery request, CancellationToken cancellationToken)
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