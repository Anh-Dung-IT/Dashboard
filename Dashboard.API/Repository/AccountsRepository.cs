using Dashboard.API.Entities;
using System.Linq;

namespace Dashboard.API.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly DashboardContext _dashboardContext;

        public AccountsRepository(DashboardContext dashboardContext)
        {
            this._dashboardContext = dashboardContext;
        }

        public Accounts AddAccount(Accounts account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            _dashboardContext.Accounts.Add(account);
            _dashboardContext.SaveChanges();

            return account;
        }

        public Accounts GetAccount(string username)
        {
            return _dashboardContext.Accounts.FirstOrDefault(a => a.Username == username);
        }
    }
}
