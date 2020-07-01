using System;
using System.Collections.Generic;
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
    public class GetUsersFromFinancialProjectQuery : IRequest<IList<UserDto>>
    {
        public string FinancialProjectId { get; set; }
    }

    public class GetUsersFromFinancialProjectQueryHandler : IRequestHandler<GetUsersFromFinancialProjectQuery, IList<UserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersFromFinancialProjectQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<IList<UserDto>> Handle(GetUsersFromFinancialProjectQuery request, CancellationToken cancellationToken)
        {
            var entity =
                _context.FinancialProjectApplicationUsers
                    .Where(x => x.FinancialProjectId == request.FinancialProjectId)
                    .Select(x => x.User);

            if (!entity.Any())
            {
                throw new NotFoundException(nameof(FinancialProject), request.FinancialProjectId);
            }
            
            var mapped = entity.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToList();

            if (mapped == null)
            {
                throw new NullReferenceException("Mapped object was returned as null");
            }

            return mapped;
        }
    }
    
}