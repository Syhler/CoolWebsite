﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject
{
    public class DeleteFinancialProjectCommand : IRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteFinancialProjectCommandHandler : IRequestHandler<DeleteFinancialProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteFinancialProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _context.UserId = currentUserService.UserId;
        }

        public async Task<Unit> Handle(DeleteFinancialProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.FinancialProjects
                .FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Financial.FinancialProject), request.Id);
            }
            entity.Deleted = DateTime.Now;
            entity.DeletedByUserId = _currentUserService.UserId;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}