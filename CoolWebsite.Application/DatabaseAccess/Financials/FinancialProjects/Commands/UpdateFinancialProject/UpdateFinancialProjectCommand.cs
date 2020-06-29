﻿using System;
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
                    throw new NotFoundException(nameof(Domain.Entities.Financial.FinancialProject), request.Id);
                }

                var users = new List<FinancialProjectApplicationUser>();

                foreach (var applicationUser in request.Users)
                {
                    users.Add(new FinancialProjectApplicationUser
                    {
                        FinancialProjectId = entity.Id,
                        UserId = applicationUser.Id
                    });
                }
                
                
                
                //add new user
                foreach (var newUser in request.Users)
                {
                    foreach (var projectApplicationUser in entity.FinancialProjectApplicationUsers)
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
                            FinancialProjectId = entity.Id,
                            UserId = newUser.Id,
                            OwedUserId = projectApplicationUser.UserId
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
                
                
                //add old user to new users
                foreach (var projectApplicationUser in entity.FinancialProjectApplicationUsers)
                {
                    foreach (var applicationUser in request.Users)
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
                            FinancialProjectId = entity.Id,
                            UserId = projectApplicationUser.UserId,
                            OwedUserId = applicationUser.Id
                        };

                        await _context.OweRecords.AddAsync(oweRecord, cancellationToken);
                    }
                }
                
                
                entity.Title = request.Name;
                entity.FinancialProjectApplicationUsers = users;


                await _context.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }
        }
    }
}