using Connectome.SocialGraph.Domain.Models;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Abstractions.Repositories
{
    public interface IFriendshipRepository
    {
        Task<Result<Unit>> Accept(Friendship friendship);
        Task<List<FriendResponse>> Friends(Guid userId);
    }
}