using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Dashboard.API.Helper
{
    public class MyTools : IMyTools
    {
        public string GetUserOfRequest(IEnumerable<Claim> claims)
        {
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return username;
        }

        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNM";

            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
