using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject
{
    public class CreateFinancialProjectCommand : IRequest<FinancialProjectDto>
    {
        public string Title { get; set; } = null!;
        public IList<ApplicationUser> Users { get; set; } = null!;

        public string? Description { get; set; }
    }
    
    public class CreateFinancialProjectCommandHandler : IRequestHandler<CreateFinancialProjectCommand, FinancialProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService userService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _context.UserId = userService.UserId;
        }

        public async Task<FinancialProjectDto> Handle(CreateFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = new FinancialProject
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Description = request.Description
            };
            
            var users = new List<FinancialProjectApplicationUser>();

            foreach (var applicationUser in request.Users)
            {
                users.Add(new FinancialProjectApplicationUser
                {
                    FinancialProjectId = entity.Id,
                    UserId = applicationUser.Id
                });

                await CreateOweRecord(request.Users, entity.Id, applicationUser.Id, cancellationToken);
            }

            entity.FinancialProjectApplicationUsers = users;

            await _context.FinancialProjects.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FinancialProjectDto>(entity);
        }

        private async Task CreateOweRecord(IEnumerable<ApplicationUser> users, string financialProjectId, string applicationUserId, CancellationToken cancellationToken)
        {
            foreach (var user in users)
            {
                if (user.Id == applicationUserId)
                {
                    continue;
                }
                    
                //create OweRecord
                var oweRecord = new OweRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = 0,
                    FinancialProjectId = financialProjectId,
                    UserId = applicationUserId,
                    OwedUserId = user.Id
                };

                await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
            }
        }
    }
    
}