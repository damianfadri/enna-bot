using Enna.Bot.SeedWork;
using Xunit;

namespace Enna.Streamers.Domain.Tests.Unit
{
    public class EntityTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void SetId()
            {
                var id = Guid.NewGuid();
                var entity = new Entity(id);

                Assert.Equal(id, entity.Id);
            }
        }

        public class AddEvents_Should
        {
            [Fact]
            public void AddEvent()
            {
                var entity = new Entity(Guid.NewGuid());

                Assert.Empty(entity.GetEvents());

                entity.AddEvent(new DummyEvent());

                Assert.NotEmpty(entity.GetEvents());
            }
        }

        public class GetEvents_Should
        {
            [Fact]
            public void BeEmpty_When_Initialized()
            {
                var entity = new Entity(Guid.NewGuid());

                Assert.Empty(entity.GetEvents());
            }

            [Fact]
            public void ReturnEvents()
            {
                var entity = new Entity(Guid.NewGuid());

                var event1 = new DummyEvent();
                var event2 = new DummyEvent();
                entity.AddEvent(event1);
                entity.AddEvent(event2);

                Assert.Equal(event1, entity.GetEvents().First());
                Assert.Equal(event2, entity.GetEvents().Skip(1).First());
            }
        }

        public class ClearEvents_Should
        {
            [Fact]
            public void ClearEvents()
            {
                var entity = new Entity(Guid.NewGuid());

                entity.AddEvent(new DummyEvent());
                entity.AddEvent(new DummyEvent());

                Assert.NotEmpty(entity.GetEvents());

                entity.ClearEvents();

                Assert.Empty(entity.GetEvents());
            }
        }
    }

    internal sealed record DummyEvent() : IDomainEvent;
}
