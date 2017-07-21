using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Repositories
{
    public class CategoryRepository :ICategoryRepository
    {

        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public CategoryRepository(ELearningDbContext context, ILogger<CategoryRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Category> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Categorys.ToList();
            }
            set { }
        }

        public bool Add(Category o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Categorys.Add(o);
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

        public bool Edit(Category o)
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

        public Category Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Categorys.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Categorys.Remove(_context.Categorys.Find(id));
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
    }
}
