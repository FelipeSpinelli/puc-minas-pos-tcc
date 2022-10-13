using System;
using System.Linq;
using System.Security.Claims;

namespace ArquiveSe.Functions.Authorization.Models
{
    public class AuthenticatedUser
    {
        private const string ACCOUNTID_CLAIM_TYPE = "AccountId";

        private readonly ClaimsPrincipal _user;

        public AuthenticatedUser(ClaimsPrincipal user)
        {
            if (user == null)
            {
                return;
            }

            _user = user;
            IsAuthenticated = true;
        }

        public bool IsAuthenticated { get; init; }

        public string GetAccountId() => _user.Claims.FirstOrDefault(x => x.Type.Equals(ACCOUNTID_CLAIM_TYPE)).Value;

        internal bool IsAuthorized(string v)
        {
            throw new NotImplementedException();
        }
    }
}
