using Connectome.SocialGraph.Application.Abstractions.Repositories;
using MediatR;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Queries.OutgoingRequests
{
    public sealed class OutgoingRequestsQueryHandler(IFriendRequestRepository repository) : IRequestHandler<OutgoingRequestsQuery, List<OutgoingFriendResponse>>
    {
        public async Task<List<OutgoingFriendResponse>> Handle(OutgoingRequestsQuery request, CancellationToken cancellationToken)
            => await repository.OutgoingFriendRequests(request.UserId);
    }
}