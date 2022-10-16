using Auth0.AuthenticationApi.Models;

namespace ArquiveSe.Application.Models
{
    internal class CurrentAccessToken : AccessTokenResponse
    {
        private DateTime _expiration = DateTime.MinValue;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public CurrentAccessToken()
        {
        }

        public async Task UpdateIfNecessary(Func<bool, AccessTokenResponse, Task<AccessTokenResponse>> getToken)
        {
            await _semaphore.WaitAsync();
            var accessToken = await getToken(DateTime.Now >= _expiration, (AccessTokenResponse)this);
            AccessToken = accessToken.AccessToken;
            TokenType = accessToken.TokenType;
            ExpiresIn = accessToken.ExpiresIn;

            _expiration = DateTime.Now.AddSeconds(ExpiresIn).AddMinutes(-10);
            _semaphore.Release();
        }
    }
}
