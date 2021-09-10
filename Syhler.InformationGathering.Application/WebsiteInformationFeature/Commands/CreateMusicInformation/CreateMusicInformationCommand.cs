using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateMusicInformation
{
    public class CreateMusicInformationCommand : IRequest<bool>
    {
        public string Url { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public bool IsInFocus { get; set; }
        public bool IsCurrentPage { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
    
    public class CreateMusicInformationCommandHandler : IRequestHandler<CreateMusicInformationCommand, bool>
    {
        private readonly IMusicInformationRepository _musicInformationRepository;

        public CreateMusicInformationCommandHandler(IMusicInformationRepository musicInformationRepository)
        {
            _musicInformationRepository = musicInformationRepository;
        }

        public async Task<bool> Handle(CreateMusicInformationCommand request, CancellationToken cancellationToken)
        {
            var model = new MusicInformation
            {
                Artist = request.Artist,
                Genre = "UNKOWN",
                IsCurrentPage = request.IsCurrentPage,
                IsInFocus = request.IsInFocus,
                Title = request.Title,
                Created = request.DateTime,
                Url = request.Url
            };

            return await _musicInformationRepository.Insert(model);

        }
    }
}