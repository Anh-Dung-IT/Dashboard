using Dashboard.API.Entities;
using System;
using System.Linq;

namespace Dashboard.API.Repository
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly DashboardContext _dashboardContext;

        public RefreshTokensRepository(DashboardContext dashboardContext)
        {
            this._dashboardContext = dashboardContext;
        }

        public RefreshTokens AddRefreshToken(RefreshTokens refreshTokens)
        {
            _dashboardContext.RefreshTokens.Add(refreshTokens);
            _dashboardContext.SaveChanges();

            return refreshTokens;
        }

        public RefreshTokens GetRefreshToken(string token, string refreshToken)
        {
            return _dashboardContext.RefreshTokens.FirstOrDefault(r => r.RefreshToken == refreshToken
                                                                    && r.Token == token
                                                                    && r.IsRevorked == false
                                                                    && r.IsUsed == false
                                                                    && r.ExpiryDate >= DateTime.UtcNow);
        }

        public void UpdateRefreshToken(RefreshTokens refreshTokens)
        {
            _dashboardContext.RefreshTokens.Update(refreshTokens);
            _dashboardContext.SaveChanges();
        }
    }
}
