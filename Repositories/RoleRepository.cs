using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.Models;
using Api_ELearning.Data;
using Microsoft.Extensions.Logging;

namespace Api_ELearning.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;
        public RoleRepository(ELearningDbContext context, ILogger<RoleRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Role> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Roles.ToList();
            }
            set { }
        }

        public bool Add(Role o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Roles.Add(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Add");
                return true;
            }
            catch(Exception e)
            {
                _log.LogError("ERROR <= Add : [%s]", e);
                return false;
            }
        }

        public bool Edit(Role o)
        {

            try
            {
                _log.LogInformation("BEGIN => Edit");
                _context.Update(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Edit");
                return true;
            }
            catch(Exception e)
            {
                _log.LogError("ERROR => Edit : [%s]", e);
                return false;
            }
        }

        public Role Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Roles.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Roles.Remove(_context.Roles.Find(id));
                _context.SaveChanges();
                _log.LogInformation("END <= Remove");
                return true;
            }
            catch(Exception e)
            {
                _log.LogError("ERROR => Remove : [%s]", e);
                return false;
            }
        }
    }
}
