using Dashboard.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Repository
{
    public interface IAccountsRepository
    {
        Accounts GetAccount(string username);

        Accounts AddAccount(Accounts account);
    }
}
