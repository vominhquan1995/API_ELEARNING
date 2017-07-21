using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api_ELearning.Repositories
{
    public class ExamDetailsRepository : IExamDetailsRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public ExamDetailsRepository(ELearningDbContext context, ILogger<ExamDetailsRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<ExamDetails> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.ExamDetailses.ToList();
            }
            set { }
        }

        public bool Add(ExamDetails o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.ExamDetailses.Add(o);
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

        public bool Edit(ExamDetails o)
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

        public ExamDetails Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.ExamDetailses.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.ExamDetailses.Remove(_context.ExamDetailses.Find(id));
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
        public ExamDetails Get(int idUser, int idExam)
        {
           return  _context.ExamDetailses.Include(o => o.Exam).Where(a => a.ExamId == idExam && a.AccountId == idUser).OrderBy(a=>a.EditTime).FirstOrDefault();
        }
    }
}
