using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Abstractions.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<Result<Unit>> Send(FriendRequest request);
        Task<Result<Unit>> Decline(UserId fromUserId, UserId toUserId);
        Task<Result<Unit>> Cancel(UserId fromUserId, UserId toUserId);
        Task<List<IncomingFriendResponse>> IncomingFriendRequests(Guid userId);
        Task<List<OutgoingFriendResponse>> OutgoingFriendRequests(Guid userId);
    }
}