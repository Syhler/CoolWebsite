using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.UpdateFinancialProject
{
    public class UpdateFinancialProjectCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
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
                var entity = await _context.FinancialProjects.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Financial.FinancialProject), request.Id);
                }

                entity.Name = request.Name;

                await _context.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }
        }
    }
}