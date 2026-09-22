using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Results;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;

namespace Connectome.SocialGraph.Domain.Models
{
    public sealed class FriendRequest
    {
        public UserId FromUserId { get; private set; }
        public UserId ToUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private FriendRequest()
        {
            
        }

        private FriendRequest(UserId from, UserId to)
        {
            FromUserId = from;
            ToUserId = to;

            CreatedAt = DateTime.UtcNow;
        }

        public static FriendRequest Create(UserId from, UserId to)
        {
            if (from == to)
                throw new DomainException(new Error(AppErrors.SelfFRiendRequestNotAllowed, "Нельзя отправить запрос дружбы самому себе"));
                
            return new FriendRequest(from, to);
        }
    }
}