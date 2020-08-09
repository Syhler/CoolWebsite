using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts
{
    public class UpdateReceiptCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public string? FinancialProjectId { get; set; }
        public string Location { get; set; } = null!;
        public string? Note { get; set; }
        public DateTime DateVisited { get; set; }
    }

    public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserId;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Receipts
                .Include(x => x.Items)
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == request.Id);


            if (entity == null)
            {
                throw new NotFoundException(nameof(Receipt), request.Id);
            }
            

            entity.Location = request.Location;
            entity.DateVisited = request.DateVisited;
            entity.Note = request.Note;

            if (!string.IsNullOrWhiteSpace(request.FinancialProjectId))
            {
                entity.FinancialProjectId = request.FinancialProjectId;
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

       
    }
}