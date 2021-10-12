using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Syhler.InformationGathering.Domain.ValueObject
{
    public class WebsiteId
    {
        [MemberNotNullWhen(false, nameof(_newId))]
        public int? Id { get; }

        private bool _newId;

        private WebsiteId(int? id, bool newId)
        {
            Id = id;
            _newId = newId;
        }

        public static WebsiteId NewId()
        {
            return new(null,true);
        }

        public static WebsiteId InitializeId(int id)
        {
            return new(id,false);
        }

        public bool IsNewId()
        {
            return Id == null;
        }

        public override bool Equals(object? obj)
        {
            if (obj is WebsiteId websiteId)
            {
                return Equals(websiteId);
            }

            return false;
        }

        private bool Equals(WebsiteId other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}