using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Models.Requests;
using ArquiveSe.Application.UseCases.Models.Responses;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArquiveSe.Application.UseCases
{
    internal class CreateAccountUseCase : ICreateAccountUseCase
    {
        private readonly IManagementApiClient _managementApiClient;
        private readonly IMediator _bus;
        private readonly ILogger<CreateAccountUseCase> _logger;

        public CreateAccountUseCase(
            IManagementApiClient managementApiClient,
            IMediator bus,
            ILogger<CreateAccountUseCase> logger)
        {
            _bus = bus;
            _logger = logger;
            _managementApiClient = managementApiClient;
        }

        public async Task<CreateAccountResponse> Execute(CreateAccountRequest request)
        {
            var user = await _managementApiClient.Users.GetAsync(request.UserId);
            if (user == null)
            {
                throw new Exception($"User not found {request.UserId}");
            }

            if (user.UserMetadata?.AccountId != null)
            {
                return new CreateAccountResponse { AccountId = user.UserMetadata.AccountId };
            }

            var accountId = Guid.NewGuid().ToString();
            await _managementApiClient.Users.UpdateAsync(user.UserId, new UserUpdateRequest
            {
                UserMetadata = new
                {
                    AccountId = accountId
                }
            });

            //await _bus.Send(command);
            return new CreateAccountResponse { AccountId = accountId };
        }
    }
}
