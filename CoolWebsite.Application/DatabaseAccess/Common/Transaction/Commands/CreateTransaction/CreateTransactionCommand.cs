using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Enums;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<string>
    {
        public string ToUserId { get; set; }
        public TransactionType TransactionType { get; set; }
        public string FinancialProjectId { get; set; }
        public double Amount { get; set; }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, string>
    {

        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateTransactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _context.UserId = currentUserService.UserID;
        }


        public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {

            var oweRecords = _context.OweRecords
                .FirstOrDefault(x => x.FinancialProjectId == request.FinancialProjectId &&
                            x.UserId == _currentUserService.UserID &&
                            x.OwedUserId == request.ToUserId);

            if (oweRecords != null)
            {
                oweRecords.Amount -= request.Amount;
            }
            
            //TODO maybe check if userid/financialProjectID exist idk? 
            
            var transaction = new Domain.Entities.Financial.Transaction
            {
                Amount = request.Amount,
                FinancialProjectId = request.FinancialProjectId,
                TransactionType = request.TransactionType,
                ToUserId = request.ToUserId,
                FromUserId = _currentUserService.UserID,
                Id = Guid.NewGuid().ToString()

            };

            await _context.Transactions.AddAsync(transaction, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}