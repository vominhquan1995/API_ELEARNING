using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account Get(string email, string password);
        Account Get(string email);
        bool EditWithoutPassword(Account o);
        IEnumerable<Account> accountFilterRole(string roleName);

        Tuple<int , string> getEmail(string hashPasswordUrl);
    }
}
