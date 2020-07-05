using System;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities.Financial;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts
{
    public class CreateReceiptsCommand : IRequest<string>
    {
        public string FinancialProjectId { get; set; }

        public string Location { get; set; }

        public DateTime DateVisited { get; set; }
        public string Note { get; set; }

    }

    public class CreateReceiptsCommandHandler : IRequestHandler<CreateReceiptsCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateReceiptsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _context.UserId = currentUserService.UserID;
        }


        public async Task<string> Handle(CreateReceiptsCommand request, CancellationToken cancellationToken)
        {
            
            var entity = new Receipt
            {
                FinancialProjectId = request.FinancialProjectId,
                Id = Guid.NewGuid().ToString(),
                DateVisited = request.DateVisited,
                Location = request.Location,
                Note = request.Note
            };

            await _context.Receipts.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}