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
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<string> Users { get; set; } = null!;

        public string? Description { get; set; }


        public class UpdateFinancialProjectCommandHandler : IRequestHandler<UpdateFinancialProjectCommand>
        {
            private readonly IApplicationDbContext _context;

            private string _financialProjectId = string.Empty;

            public UpdateFinancialProjectCommandHandler(IApplicationDbContext context,
                ICurrentUserService currentUserService)
            {
                _context = context;
                _context.UserId = currentUserService.UserId;
            }


            public async Task<Unit> Handle(UpdateFinancialProjectCommand request, CancellationToken cancellationToken)
            {
                var entity = _context.FinancialProjects
                    .Include(x => x.FinancialProjectApplicationUsers)
                    .Include(x => x.OweRecords)
                    .FirstOrDefault(e => e.Id == request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(FinancialProject), request.Id);
                }

                var oweRecords = _context.OweRecords.Where(x => x.FinancialProjectId == entity.Id);
                
                _financialProjectId = entity.Id;

                var newUsers = request.Users
                    .Where(newUser => entity.FinancialProjectApplicationUsers
                        .All(x => x.UserId != newUser)).ToList();
                
                await CreateOweRecordForNewUsers(newUsers, entity.FinancialProjectApplicationUsers, cancellationToken);

                await CreateOweRecordForEachOtherNewUser(newUsers, cancellationToken);

                await CreateOweRecordForOldToNewUsers(entity.FinancialProjectApplicationUsers, newUsers, cancellationToken);
                
                await AddNewUsers(request, entity, cancellationToken);
                
                await RemoveUsers(request, entity, oweRecords, cancellationToken);
                
                entity.Title = request.Name;
                entity.Description = request.Description;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            private async Task CreateOweRecordForEachOtherNewUser(List<string> newUsers, CancellationToken cancellationToken)
            {
                foreach (var newUser in newUsers)
                {
                    foreach (var user in newUsers)
                    {
                        if (newUser == user)
                        {
                            continue;
                        }
                        
                        var oweRecord = new OweRecord
                        {
                            Id = Guid.NewGuid().ToString(),
                            Amount = 0,
                            FinancialProjectId = _financialProjectId,
                            UserId = newUser,
                            OwedUserId = user
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
            }

            private async Task RemoveUsers(UpdateFinancialProjectCommand request, FinancialProject entity, IQueryable<OweRecord> oweRecords, CancellationToken cancellationToken)
            {
                foreach (var removedUser in entity.FinancialProjectApplicationUsers.Where( x=> request.Users.All(y => y != x.UserId)))
                {
                    _context.FinancialProjectApplicationUsers.Remove(removedUser);

                    var recordFromRemovedUser = oweRecords.Where(x => x.UserId == removedUser.UserId);
                    
                    _context.OweRecords.RemoveRange(recordFromRemovedUser);
                    
                    var recordToRemovedUser = oweRecords.Where(x => x.OwedUserId == removedUser.UserId);
                   
                    _context.OweRecords.RemoveRange(recordToRemovedUser);
                }
                
            }

            private async Task AddNewUsers(UpdateFinancialProjectCommand request, FinancialProject entity, CancellationToken cancellationToken)
            {
                foreach (var newUser in request.Users.Where(newUser => entity.FinancialProjectApplicationUsers
                        .All(x => x.UserId != newUser)))
                {
                    var user = new FinancialProjectApplicationUser
                    {
                        FinancialProjectId = _financialProjectId,
                        UserId = newUser
                    };
                    
                    await _context.FinancialProjectApplicationUsers.AddAsync(user, cancellationToken);
                }

            }
            
            private async Task CreateOweRecordForOldToNewUsers(ICollection<FinancialProjectApplicationUser> currentUsers, List<string> newUsers, CancellationToken cancellationToken)
            {
                //add old user to new users
                foreach (var projectApplicationUser in currentUsers)
                {
                    foreach (var applicationUser in newUsers)
                    {

                        //create OweRecord
                        var oweRecord = new OweRecord
                        {
                            Id = Guid.NewGuid().ToString(),
                            Amount = 0,
                            FinancialProjectId = _financialProjectId,
                            UserId = projectApplicationUser.UserId,
                            OwedUserId = applicationUser
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
            }

            private async Task CreateOweRecordForNewUsers(List<string> newUsers,
                ICollection<FinancialProjectApplicationUser> currentUsers, CancellationToken cancellationToken)
            {
                foreach (var newUser in newUsers)
                {
                    foreach (var projectApplicationUser in currentUsers)
                    {

                        //create OweRecord
                        var oweRecord = new OweRecord
                        {
                            Id = Guid.NewGuid().ToString(),
                            Amount = 0,
                            FinancialProjectId = _financialProjectId,
                            UserId = newUser,
                            OwedUserId = projectApplicationUser.UserId
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
            }
        }
    }
}