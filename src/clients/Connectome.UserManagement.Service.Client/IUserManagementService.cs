using Crossdyne.Toolkit.Results;
using Shared.Contracts.UserManagement.Responses;

namespace Connectome.UserManagement.Service.Client
{
    public interface IUserManagementService
    {
        Task<Result<ByInviteCodeResponse>> GetByInviteCode(string inviteCode);
    }
}