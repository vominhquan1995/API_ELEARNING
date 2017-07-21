using System;
using System.Collections.Generic;
using System.Linq;
using Api_ELearning.Models;
using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Services;

namespace Api_ELearning.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;
        public AccountRepository(ELearningDbContext context, ILogger<AccountRepository> log)
        {
            _context = context;
            _log = log;
        }
        public bool Add(Account o)
        {
            _log.LogInformation("BEGIN => Add");
            try
            {
                if (_context.Accounts.FirstOrDefault(k => k.Email.Equals(o.Email)) != null)
                {
                    return false;
                }
                _log.LogInformation("BEGIN => Hash Password");
                o.AddTime = DateTime.Now;
                o.EditTime = DateTime.Now;
                o.Password = Protector.HashPassword(o.Password);
                _log.LogInformation("END <= Hash Password");
                _context.Accounts.Add(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Add");
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation("ERROR => Add : [%s]", e);
                return false;
            }
        }

        public bool Edit(Account o)
        {
            _log.LogInformation("BEGIN => Edit");
            try
            {
                _log.LogInformation("BEGIN => Hash Password");
                o.Password = Protector.HashPassword(o.Password);
                _log.LogInformation("END <= Hash Password");
                _context.Accounts.Update(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Edit");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR => Edit : [%s]", e);
                return false;
            }
        }
        public bool EditWithoutPassword(Account o)
        {
            _log.LogInformation("BEGIN => Edit");
            try
            {
                _context.Accounts.Update(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Edit");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR => Edit : [%s]", e);
                return false;
            }
        }

        public Account Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Accounts.ToList();
            }
            set { }
        }

        public bool Remove(int id)
        {
            _log.LogInformation("BEGIN => Remove");
            try
            {
                _context.Accounts.Remove(_context.Accounts.Find(id));
                _context.SaveChanges();
                _log.LogInformation("END <=  Remove");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR => Remove : [%s]", e);
                return false;
            }
        }

        public Account Get(string email, string password)
        {
            _log.LogInformation("GET => [%s]", email);
            return _context.Accounts.FirstOrDefault(o => o.Email.Equals(email) && o.Password.Equals(Protector.HashPassword(password)));
        }

        public Account Get(string email)
        {
            _log.LogInformation("GET => [%s]", email);
            return _context.Accounts.FirstOrDefault(o => o.Email.Equals(email));
        }

        public IEnumerable<Account> accountFilterRole(string roleName)
        {
            return _context.Accounts.Where(o => o.Role.RoleName== roleName).ToList();
        }

        public Tuple<int, string> getEmail(string hashPasswordUrl)
        {
            var a=  _context.Accounts.FirstOrDefault(o=>Protector.HashPassword(o.Password).Equals(hashPasswordUrl));
            if(a!=null){
                return new Tuple<int, string>(a.Id, a.Email);
            }
            return null;
        }
    }
}
