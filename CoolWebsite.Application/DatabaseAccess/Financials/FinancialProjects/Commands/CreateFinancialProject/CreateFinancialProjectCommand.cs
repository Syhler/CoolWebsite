using System;
using System.Collections.Generic;
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
        public string Title { get; set; }
        public IList<ApplicationUser> Users { get; set; }
    }
    
    public class CreateFinancialProjectCommandHandler : IRequestHandler<CreateFinancialProjectCommand, FinancialProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService userService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _context.UserId = userService.UserID;
        }

        public async Task<FinancialProjectDto> Handle(CreateFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = new FinancialProject
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title
            };
            
            var users = new List<FinancialProjectApplicationUser>();

            foreach (var applicationUser in request.Users)
            {
                users.Add(new FinancialProjectApplicationUser
                {
                    FinancialProjectId = entity.Id,
                    UserId = applicationUser.Id
                });

                foreach (var user in request.Users)
                {
                    if (user.Id == applicationUser.Id)
                    {
                        continue;
                    }
                    
                    //create OweRecord
                    var oweRecord = new OweRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        Amount = 0,
                        FinancialProjectId = entity.Id,
                        UserId = applicationUser.Id,
                        OwedUserId = user.Id
                    };

                    await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                }
               
            }

            
            entity.FinancialProjectApplicationUsers = users;
            

            await _context.FinancialProjects.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FinancialProjectDto>(entity);
        }
    }
}