using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Connectome.SocialGraph.Domain.ValueObjects.FriendRequest;
using Crossdyne.Toolkit.Results;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Primitives;

namespace Connectome.SocialGraph.Domain.Models
{
    public sealed class FriendRequest : AggregateRoot<FriendRequestId>
    {
        public UserId FromUserId { get; private set; }
        public UserId ToUserId { get; private set; }
        public RequestStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RespondedAt { get; private set; }

        private FriendRequest()
        {
            
        }

        private FriendRequest(UserId from, UserId to) : base(FriendRequestId.New())
        {
            FromUserId = from;
            ToUserId = to;

            CreatedAt = DateTime.UtcNow;
            Status = RequestStatus.Request;
        }

        public static FriendRequest Create(UserId from, UserId to)
        {
            if (from == to)
                throw new DomainException(new Error(AppErrors.SelfFRiendRequestNotAllowed, "Нельзя отправить запрос дружбы самому себе"));
                
            return new FriendRequest(from, to);
        }
    }
}