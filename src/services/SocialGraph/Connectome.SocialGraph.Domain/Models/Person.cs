using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Connectome.SocialGraph.Domain.ValueObjects.Person;
using Shared.Kernel.Primitives;

namespace Connectome.SocialGraph.Domain.Models
{
    public sealed class Person : AggregateRoot<PersonId>
    {
        public UserId UserId { get; private set; }
        public UserName UserName { get; private set; }

        private Person()
        {
            
        }

        private Person(UserId userId, UserName userName) : base(PersonId.New())
        {
            UserId = userId;
            UserName = userName;
        }

        public static Person Create(UserId userId, UserName userName)
        {
            return new(userId, userName);
        }
    }
}