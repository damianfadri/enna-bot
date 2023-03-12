using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Domain.Tests.Unit
{
    public class TenantTests
    {
        public class Constructor_Should 
        {
            [Fact]
            public void SetProperties()
            {
                var id = Guid.NewGuid();
                var tenant = new Tenant<int>(id, 0);

                tenant.Id.Should().Be(id);
                tenant.KeyId.Should().Be(0);
            }
        }
    }
}
