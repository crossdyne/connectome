using MediatR;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Queries.OutgoingRequests
{
    public sealed record OutgoingRequestsQuery(Guid UserId) : IRequest<List<OutgoingFriendResponse>>;
}