using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Domain.Enums;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateWebsiteInformation
{
    public class CreateWebsiteInformationCommand : IRequest<bool>
    {
        public CreateWebsiteInformationCommand(string url, DateTime timeVisited, bool isInFocus, bool isCurrentPage)
        {
            Url = url;
            TimeVisited = timeVisited;
            IsInFocus = isInFocus;
            IsCurrentPage = isCurrentPage;
        }

        public string Url { get; private set; }
        public DateTime TimeVisited { get; private set; }
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

            //TODO check if website is productive or not
            var model = new WebsiteInformation(
                WebsiteId.NewId(),
                request.Url,
                request.IsCurrentPage,
                request.IsInFocus,
                WebsiteInformationType.UNKOWN,
                request.TimeVisited);
       
            

            return await _websiteInformationRepository.InsertAsync(model);
        }
    }
}