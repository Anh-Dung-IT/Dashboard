using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dashboard.API.Helper
{
    public interface IMyTools
    {
        string GetUserOfRequest(IEnumerable<Claim> claims);
        string RandomString(int length);
    }
}
