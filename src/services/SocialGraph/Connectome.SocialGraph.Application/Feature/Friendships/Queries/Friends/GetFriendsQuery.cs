using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Feature.Friendships.Queries.Friends
{
    public sealed record GetFriendsQuery(Guid UserId) : IRequest<Result<List<FriendResponse>>>;
}