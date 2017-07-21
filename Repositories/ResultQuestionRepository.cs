using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Repositories
{
    public class ResultQuestionRepository:IResultQuestionRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public ResultQuestionRepository(ELearningDbContext context, ILogger<ResultQuestionRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<ResultQuestion> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.ResultQuestions.ToList();
            }
            set { }
        }

        public bool Add(ResultQuestion o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.ResultQuestions.Add(o);
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

        public bool Edit(ResultQuestion o)
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

        public ResultQuestion Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.ResultQuestions.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.ResultQuestions.Remove(_context.ResultQuestions.Find(id));
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
        public IEnumerable<ResultQuestion> ListResult(int id)
        {
            return _context.ResultQuestions.Where(a => a.Question.Id == id).ToList();
        }
        bool IResultQuestionRepository.CheckAnswer(int id)
        {
            return _context.ResultQuestions.Where(a => a.Id == id).Select(a => a.Status).FirstOrDefault();
        }
    }
}
