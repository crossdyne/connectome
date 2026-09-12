using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.Common;

namespace Connectome.SocialGraph.Application.Abstractions.Repositories
{
    public interface IPersonRepository
    {
        Task<Result<NewRecordResponse>> CreateAsync(Person person, CancellationToken cl);
        Task<Result<Unit>> RemoveAsync(UserId userId);
    }
}