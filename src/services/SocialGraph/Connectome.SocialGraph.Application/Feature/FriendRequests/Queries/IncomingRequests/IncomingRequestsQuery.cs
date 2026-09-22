using MediatR;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Queries.IncomingRequests
{
    public sealed record IncomingRequestsQuery(Guid UserId) : IRequest<List<IncomingFriendResponse>>;
}