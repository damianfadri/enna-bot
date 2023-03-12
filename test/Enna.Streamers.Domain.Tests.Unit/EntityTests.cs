using Enna.Core.Domain;
using FluentAssertions;
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

                entity.Id.Should().Be(id);
            }
        }

        public class AddEvents_Should
        {
            [Fact]
            public void AddEvent()
            {
                var entity = new Entity(Guid.NewGuid());

                entity.GetEvents().Should().BeEmpty();

                entity.AddEvent(new DummyEvent());

                entity.GetEvents().Should().HaveCount(1);
            }
        }

        public class GetEvents_Should
        {
            [Fact]
            public void BeEmpty_When_Initialized()
            {
                var entity = new Entity(Guid.NewGuid());

                entity.GetEvents().Should().BeEmpty();
            }

            [Fact]
            public void ReturnEvents()
            {
                var entity = new Entity(Guid.NewGuid());

                var event1 = new DummyEvent();
                var event2 = new DummyEvent();
                entity.AddEvent(event1);
                entity.AddEvent(event2);

                entity.GetEvents().Should().HaveCount(2);
                entity.GetEvents().First().Should().Be(event1);
                entity.GetEvents().Last().Should().Be(event2);
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

                entity.GetEvents().Should().HaveCount(2);

                entity.ClearEvents();

                entity.GetEvents().Should().BeEmpty();
            }
        }
    }

    internal sealed record DummyEvent() : IDomainEvent;
}
