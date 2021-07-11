using System.Threading.Tasks;
using BlazorServerWithAuth.Pages.Admin.Receipt.Services.Interface;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using MediatR;

namespace BlazorServerWithAuth.Pages.Admin.Receipt.Services
{
    public class ProjectViewService : IProjectViewService
    {
        private readonly IMediator _mediator;

        public ProjectViewService(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<FinancialProjectDto> GetProject(string id)
        {
            var query = new GetFinancialProjectByIdQuery {ProjectId = id};
            
            return await _mediator.Send(query);
        }
    }
}