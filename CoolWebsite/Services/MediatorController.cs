using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CoolWebsite.Services
{
    public class MediatorController : Controller
    {
        private IMediator? _iMediator;

        protected IMediator Mediator => _iMediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}