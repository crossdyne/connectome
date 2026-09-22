using Connectome.SocialGraph.Application.Abstractions.Repositories;
using MediatR;
using Shared.Contracts.SocialGraph.Responses;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Queries.IncomingRequests
{
    public sealed class IncomingRequestsQueryHandler(IFriendRequestRepository repository) : IRequestHandler<IncomingRequestsQuery, List<IncomingFriendResponse>>
    {
        public async Task<List<IncomingFriendResponse>> Handle(IncomingRequestsQuery request, CancellationToken cancellationToken)
            => await repository.IncomingFriendRequests(request.UserId);
    }
}