using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Api_ELearning.ActionModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.Models;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _question;
        private readonly IResultQuestionRepository _resultQuestion;
        public QuestionsController(IQuestionRepository question, IResultQuestionRepository resultQuestion)
        {
            _question = question;
            _resultQuestion = resultQuestion;
        }
        [HttpGet]
        public IActionResult getAll()
        {
            try
            {
                return new ObjectResult(_question.All);

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idQuestion}")]
        public IActionResult get(int idQuestion)
        {
             try
            { 
                return new ObjectResult(_resultQuestion.ListResult(idQuestion).ToList());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idQuestion}/information")]
        public IActionResult getInformation(int idQuestion)
        {
            try
            {
                return new ObjectResult(_question.Get(idQuestion));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idAnswer}/info-answer")]
        public IActionResult getInfoAnswer(int idAnswer)
        {
            try
            {
                return new ObjectResult(_resultQuestion.Get(idAnswer));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idQuestion}/question-result")]
        public IActionResult getListQuestionResult(int idQuestion)
        {
            try
            {
                return new ObjectResult(_question.Get(idQuestion));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult editQuestion([FromBody]Question model)
        {
            try
            {
                return new ObjectResult(this._question.Edit(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("answer")]
        public IActionResult editAnswer([FromBody]ResultQuestion model)
        {
            try
            {
                return new ObjectResult(this._resultQuestion.Edit(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{idAnswer}/answer")]
        public IActionResult deleteAnswer(int idAnswer)
        {
            try
            {
                return new ObjectResult(_resultQuestion.Remove(idAnswer));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{idQuestion}")]
        public IActionResult deleteQuestion(int idQuestion)
        {
            try
            {
                return new ObjectResult(_question.Remove(idQuestion));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult addQuestion([FromBody]Question model)
        {
            try
            {
                return new ObjectResult(_question.Add(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("answer")]
        public IActionResult addAnswer([FromBody]ResultQuestion model)
        {
            try
            {
                return new ObjectResult(_resultQuestion.Add(model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}