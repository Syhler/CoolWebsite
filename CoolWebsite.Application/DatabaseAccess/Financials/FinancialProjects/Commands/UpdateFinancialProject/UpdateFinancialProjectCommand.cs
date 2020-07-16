using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.UpdateFinancialProject
{
    public class UpdateFinancialProjectCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        
        
        public class UpdateFinancialProjectCommandHandler : IRequestHandler<UpdateFinancialProjectCommand>
        {
            private readonly IApplicationDbContext _context;

            private string _financialProjectId = null;

            public UpdateFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _context.UserId = currentUserService.UserID;
            }


            public async Task<Unit> Handle(UpdateFinancialProjectCommand request, CancellationToken cancellationToken)
            {
                var entity = _context.FinancialProjects
                    .Include(x=>x.FinancialProjectApplicationUsers)
                    .Include(x => x.OweRecords)
                    .FirstOrDefault(e => e.Id ==request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(FinancialProject), request.Id);
                }

                _financialProjectId = entity.Id;
                

                var users = request.Users.Select(applicationUser
                    => new FinancialProjectApplicationUser {FinancialProjectId = _financialProjectId, UserId = applicationUser.Id}).ToList();
                

                await CreateOweRecordForNewUsers(request.Users, entity.FinancialProjectApplicationUsers, cancellationToken);


                await CreateOweRecordForOldToNewUsers(entity.FinancialProjectApplicationUsers, request.Users,
                    cancellationToken);
                
                
                entity.Title = request.Name;
                entity.FinancialProjectApplicationUsers = users;


                await _context.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }
            
            private async Task CreateOweRecordForOldToNewUsers(ICollection<FinancialProjectApplicationUser> currentUsers, List<ApplicationUser> newUsers, CancellationToken cancellationToken)
            {
                //add old user to new users
                foreach (var projectApplicationUser in currentUsers)
                {
                    foreach (var applicationUser in newUsers)
                    {
                        if (projectApplicationUser.UserId == applicationUser.Id)
                        {
                            continue;
                        }
                        
                        //create OweRecord
                        var oweRecord = new OweRecord
                        {
                            Id = Guid.NewGuid().ToString(),
                            Amount = 0,
                            FinancialProjectId = _financialProjectId,
                            UserId = projectApplicationUser.UserId,
                            OwedUserId = applicationUser.Id
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
            }
            
            private async Task CreateOweRecordForNewUsers(List<ApplicationUser> newUsers, ICollection<FinancialProjectApplicationUser> currentUsers, CancellationToken cancellationToken)
            {
                foreach (var newUser in newUsers)
                {
                    foreach (var projectApplicationUser in currentUsers)
                    {
                        if (newUser.Id == projectApplicationUser.UserId)
                        {
                            continue;
                        }
                    
                        //create OweRecord
                        var oweRecord = new OweRecord
                        {
                            Id = Guid.NewGuid().ToString(),
                            Amount = 0,
                            FinancialProjectId = _financialProjectId,
                            UserId = newUser.Id,
                            OwedUserId = projectApplicationUser.UserId
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
            }
        }
    }
}