using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.UserManagement.Responses;
using Shared.Kernel.Errors;

namespace Connectome.UserManagement.Service.Client
{
    public sealed class UserManagementService(HttpClient http) : IUserManagementService
    {
        private readonly static JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<Result<ByInviteCodeResponse>> GetByInviteCode(string inviteCode)
        {
            try
            {
                var response = await http.GetAsync($"internal/api/users/by-invite-code/{inviteCode}");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ByInviteCodeResponse>(jsonOptions);

                return result;
            }
            catch (Exception ex)
            {
                return Result<ByInviteCodeResponse>.Failure(new Error(AppErrors.Api, $"Произошла ошибка при отправке запроса на дружбу: {ex}"));
            }
        }
    }
}