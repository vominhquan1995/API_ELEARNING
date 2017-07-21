using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Repositories
{
    public class QuestionRepository:IQuestionRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public QuestionRepository(ELearningDbContext context, ILogger<CetificateRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Question> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Questions.ToList();
            }
            set { }
        }

        public bool Add(Question o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Questions.Add(o);
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
        public int addQuestion(Question question)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Questions.Add(question);
                _context.SaveChanges();
                _log.LogInformation("END <= Add");
                return question.Id;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR <= Add : [%s]", e);
                return -1;
            }
        }

        public bool Edit(Question o)
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

        public Question Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Questions.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Questions.Remove(_context.Questions.Find(id));
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
       public IEnumerable<Question> getListQuestion(int idExam)
        {
            return _context.Questions.Where(a => a.ExamId == idExam).ToList();
        }
    }
}
