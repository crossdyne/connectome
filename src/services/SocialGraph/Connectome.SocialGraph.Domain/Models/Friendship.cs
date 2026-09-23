using Connectome.SocialGraph.Domain.ValueObjects.Common;

namespace Connectome.SocialGraph.Domain.Models
{
    public sealed class Friendship
    {
        public UserId RequesterUserId { get; private set; }
        public UserId AcceptorUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? BlockedAt { get; private set; }
        public UserId? BlockedBy { get; private set; }

        private Friendship()
        {
            
        }

        private Friendship(UserId requesterUserId, UserId acceptorUserId)
        {
            RequesterUserId = requesterUserId;
            AcceptorUserId = acceptorUserId;
            CreatedAt = DateTime.UtcNow;    
        }

        public static Friendship Create(UserId requesterUserId, UserId acceptorUserId)
        {
            return new(requesterUserId, acceptorUserId);
        }
    }
}