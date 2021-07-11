using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace BlazorServerWithAuth.Pages.Admin.Receipt.Services.Interface
{
    public interface IProjectViewService
    {
        Task<FinancialProjectDto> GetProject(string id);
    }
}