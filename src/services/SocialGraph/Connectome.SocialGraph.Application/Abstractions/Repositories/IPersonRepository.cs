using Connectome.SocialGraph.Domain.Models;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.Common;

namespace Connectome.SocialGraph.Application.Abstractions.Repositories
{
    public interface IPersonRepository
    {
        Task<Result<NewRecordResponse>> CreateAsync(Person person, CancellationToken cl);
    }
}