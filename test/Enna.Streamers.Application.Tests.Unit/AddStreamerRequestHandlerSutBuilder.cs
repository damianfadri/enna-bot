using Enna.Bot.SeedWork;
using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class AddStreamerRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;
        private IChannelRepository _channelRepository;
        private IUnitOfWork _unitOfWork;

        public AddStreamerRequestHandlerSutBuilder() 
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
            _channelRepository= new Mock<IChannelRepository>().Object;
            _unitOfWork = new Mock<IUnitOfWork>().Object;
        }

        public AddStreamerRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithNullChannelRepository()
        {
            _channelRepository = null!;
            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithNullUnitOfWork()
        {
            _unitOfWork = null!;
            return this;
        }

        public AddStreamerRequestHandler Build()
        {
            return new AddStreamerRequestHandler(
                _streamerRepository,
                _channelRepository,
                _unitOfWork);
        }
    }
}
