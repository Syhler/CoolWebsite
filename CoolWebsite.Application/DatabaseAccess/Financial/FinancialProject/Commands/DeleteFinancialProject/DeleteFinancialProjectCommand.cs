﻿using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.DataProtection.Infrastructure;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.DeleteFinancialProject
{
    public class DeleteFinancialProjectCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteFinancialProjectCommandHandler : IRequestHandler<DeleteFinancialProjectCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }

        public async Task<Unit> Handle(DeleteFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.FinancialProjects.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Financial.FinancialProject), request.Id);
            }

            _context.FinancialProjects.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}