using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Feature.Friendships.Queries.Friends
{
    public sealed class GetFriendsQueryHandler(IFriendshipRepository repository) : IRequestHandler<GetFriendsQuery, Result<List<FriendResponse>>>
    {
        public async Task<Result<List<FriendResponse>>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
            => await repository.Friends(request.UserId);
    }
}