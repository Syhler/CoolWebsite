using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class GetFinancialProjectByIdQuery : IRequest<FinancialProjectDto>
    {
        public string ProjectId { get; set; }
    }

    public class GetFinancialProjectByIdQueryHandler 
        : IRequestHandler<GetFinancialProjectByIdQuery, FinancialProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;


        public GetFinancialProjectByIdQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService userService, IIdentityService identity)
        {
            _mapper = mapper;
            _context = context;
            _identity = identity;
            _context.UserId = userService.UserID;
        }

        public async Task<FinancialProjectDto> Handle(GetFinancialProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.FinancialProjects
                .Include(x => x.Receipts)
                .Include(x => x.FinancialProjectApplicationUsers)
                .Where(x => x.Id == request.ProjectId);

            
            if (entity.ToList().Count <= 0)
            {
                throw new NotFoundException(nameof(FinancialProject), request.ProjectId);
            }
            
            if (entity.ToList().Count >= 2)
            {
                throw new NotFoundException(nameof(FinancialProject) + " (multiple objects found)", request.ProjectId);
            }
            
            var mapped = entity.ProjectTo<FinancialProjectDto>(_mapper.ConfigurationProvider).First();

            return mapped;
        }
    }
}