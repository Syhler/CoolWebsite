using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Entities;
using MediatR;

namespace CoolWebsite.Application.DatabaseAccess.TestEntities.Commands.UpdateTestEntity
{
    public class UpdateTestEntityCommand : IRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public class UpdateTestEntityCommandHandler : IRequestHandler<UpdateTestEntityCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateTestEntityCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _context.UserId = currentUserService.UserID;
            }

            public async Task<Unit> Handle(UpdateTestEntityCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.TestEntities.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(TestEntity), request.Id);
                }

                entity.Name = request.Name;

                await _context.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }
        }
    }
}