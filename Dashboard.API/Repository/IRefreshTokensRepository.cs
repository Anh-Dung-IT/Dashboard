using Dashboard.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Repository
{
    public interface IRefreshTokensRepository
    {
        RefreshTokens AddRefreshToken(RefreshTokens refreshTokens);
        RefreshTokens GetRefreshToken(string token, string refreshToken);
        void UpdateRefreshToken(RefreshTokens refreshTokens);
    }
}
