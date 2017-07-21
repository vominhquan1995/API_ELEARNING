using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.Models;
using Api_ELearning.Data;
using Microsoft.Extensions.Logging;

namespace Api_ELearning.Repositories
{
    public class CetificateRepository : ICetificateRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public CetificateRepository(ELearningDbContext context, ILogger<CetificateRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Cetificate> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Cetificates.ToList();
            }
            set { }
        }

        public bool Add(Cetificate o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Cetificates.Add(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Add");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR <= Add : [%s]", e);
                return false;
            }
        }

        public bool Edit(Cetificate o)
        {
            try
            {
                _log.LogInformation("BEGIN => Edit");
                _context.Update(o);
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

        public Cetificate Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Cetificates.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Cetificates.Remove(_context.Cetificates.Find(id));
                _context.SaveChanges();
                _log.LogInformation("END <= Remove");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR => Remove : [%s]", e);
                return false;
            }
        }
        public Cetificate GetCetificate(int idCourses)
        {
            return _context.Cetificates.Where(a => a.Id == _context.Courses.Where(b => b.Id == idCourses).Select(b => b.Cetificate.Id).FirstOrDefault()).FirstOrDefault();
        }
    }
}
