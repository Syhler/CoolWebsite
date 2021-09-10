using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateWebsiteInformation
{
    public class CreateWebsiteInformationCommand : IRequest<bool>
    {
        public CreateWebsiteInformationCommand(string url, DateTime dateTime, bool isInFocus, bool isCurrentPage)
        {
            Url = url;
            DateTime = dateTime;
            IsInFocus = isInFocus;
            IsCurrentPage = isCurrentPage;
        }

        public string Url { get; private set; }
        public DateTime DateTime { get; private set; }
        public bool IsInFocus { get; private set; }
        public bool IsCurrentPage { get; private set; }
        
        
    }

    public class CreateWebsiteInformationCommandHandler : IRequestHandler<CreateWebsiteInformationCommand, bool>
    {
        private readonly IWebsiteInformationRepository _websiteInformationRepository;

        public CreateWebsiteInformationCommandHandler(IWebsiteInformationRepository websiteInformationRepository)
        {
            _websiteInformationRepository = websiteInformationRepository;
        }

        public async Task<bool> Handle(CreateWebsiteInformationCommand request, CancellationToken cancellationToken)
        {

            var model = new WebsiteInformation
            {
                Url = request.Url,
                IsCurrentPage = request.IsCurrentPage,
                IsInFocus = request.IsInFocus,
                Created = request.DateTime,
                CreatedBy = Guid.Empty.ToString() //Current user
            };
            

            return await _websiteInformationRepository.Insert(model);
        }
    }
}