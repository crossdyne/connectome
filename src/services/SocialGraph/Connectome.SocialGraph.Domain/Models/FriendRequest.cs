using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Results;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;

namespace Connectome.SocialGraph.Domain.Models
{
    public sealed class FriendRequest
    {
        public UserId RequesterUserId { get; private set; }
        public UserId RecipientUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private FriendRequest()
        {
            
        }

        private FriendRequest(UserId requester, UserId recipient)
        {
            RequesterUserId = requester;
            RecipientUserId = recipient;

            CreatedAt = DateTime.UtcNow;
        }

        public static FriendRequest Create(UserId requester, UserId recipient)
        {
            if (requester == recipient)
                throw new DomainException(new Error(AppErrors.SelfFRiendRequestNotAllowed, "Нельзя отправить запрос дружбы самому себе"));
                
            return new FriendRequest(requester, recipient);
        }
    }
}