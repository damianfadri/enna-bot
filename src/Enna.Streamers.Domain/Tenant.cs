using Enna.Core.Domain;

namespace Enna.Streamers.Domain
{
    public class Tenant<TKey> : Entity where TKey : struct
    {
        public TKey KeyId { get; set; }

        public Tenant(Guid id, TKey keyId) : base(id)
        {
            KeyId = keyId;
        }
    }
}
