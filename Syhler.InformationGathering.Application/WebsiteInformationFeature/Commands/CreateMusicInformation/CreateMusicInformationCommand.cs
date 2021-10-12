using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Domain.Enums;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Commands.CreateMusicInformation
{
    public class CreateMusicInformationCommand : IRequest<bool>
    {
        public CreateMusicInformationCommand(string url, DateTime timeVisited,
            bool isInFocus, bool isCurrentPage, string title, string artist)
        {
            Url = url;
            TimeVisited = timeVisited;
            IsInFocus = isInFocus;
            IsCurrentPage = isCurrentPage;
            Title = title;
            Artist = artist;
        }

        public string Url { get; }
        public DateTime TimeVisited { get; }
        public bool IsInFocus { get; }
        public bool IsCurrentPage { get; }
        public string Title { get; }
        public string Artist { get; }
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
            var model = new MusicInformation(
                WebsiteId.NewId(), 
                request.Url, 
                request.IsCurrentPage, 
                request.IsInFocus,
                WebsiteInformationType.Music, 
                request.TimeVisited,
                request.Title, 
                request.Artist);

            return await _musicInformationRepository.Insert(model);

        }
    }
}