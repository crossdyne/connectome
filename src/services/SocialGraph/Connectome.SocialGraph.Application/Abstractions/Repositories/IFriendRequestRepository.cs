using Connectome.SocialGraph.Domain.Models;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Abstractions.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<Result<Unit>> Send(FriendRequest request);
        Task<List<IncomingFriendResponse>> IncomingFriendRequests(Guid userId);
        Task<List<OutgoingFriendResponse>> OutgoingFriendRequests(Guid userId);
    }
}