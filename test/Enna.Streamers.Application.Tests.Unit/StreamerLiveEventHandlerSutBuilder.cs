using Enna.Bot.SeedWork;
using Enna.Streamers.Application.Handlers;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class StreamerLiveEventHandlerSutBuilder
    {
        private IUnitOfWork _unitOfWork;

        public StreamerLiveEventHandlerSutBuilder()
        {
            _unitOfWork = new Mock<IUnitOfWork>().Object;
        }

        public StreamerLiveEventHandlerSutBuilder WithNullUnitOfWork()
        {
            _unitOfWork = null!;
            return this;
        }

        public StreamerLiveEventHandler Build()
        {
            return new StreamerLiveEventHandler(
                _unitOfWork);
        }
    }
}
