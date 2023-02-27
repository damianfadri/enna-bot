using Enna.Bot.SeedWork;
using Enna.Streamers.Application.Handlers;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class StreamerOfflineEventHandlerSutBuilder
    {
        private IUnitOfWork _unitOfWork;

        public StreamerOfflineEventHandlerSutBuilder()
        {
            _unitOfWork = new Mock<IUnitOfWork>().Object;
        }

        public StreamerOfflineEventHandlerSutBuilder WithNullUnitOfWork()
        {
            _unitOfWork = null!;
            return this;
        }

        public StreamerOfflineEventHandler Build()
        {
            return new StreamerOfflineEventHandler(
                _unitOfWork);
        }
    }
}
