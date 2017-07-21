using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.ActionModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using System.Text;
using System;
using Api_ELearning.Models;
using Api_ELearning.Services;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class ExamsController : Controller
    {
        private readonly IFileService _file;
        private readonly IResultQuestionRepository _resultQuestion;
        private readonly IExamRepository _exam;
        private readonly IExamDetailsRepository _examDetails;
        private readonly IAccountRepository _account;
        private readonly IQuestionRepository _question;
        private readonly ILearnRepository _learn;
        private readonly IHostingEnvironment _hostingEnv;
        public ExamsController(IExamRepository exam, IExamDetailsRepository examDetails,
            IAccountRepository account, IQuestionRepository question, ILearnRepository learn,
            IHostingEnvironment hostingEnv, IFileService file,
            IResultQuestionRepository resultQuestion)
        {
            _exam = exam;
            _examDetails = examDetails;
            _account = account;
            _question = question;
            _learn = learn;
            _file = file;
            _hostingEnv = hostingEnv;
            _resultQuestion = resultQuestion;
        }
        [HttpGet("{idLesson}/information")]
        public IActionResult getExamInformation(int idLesson)
        {
            try
            {
                return new ObjectResult(_exam.Get(idLesson));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idExam}")]
        public IActionResult getExam(int idExam)
        {
            try
            {
                return new ObjectResult(_exam.RandomQuestion(idExam).ToList());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idExam}/getQuestion")]
        public IActionResult getQuestionExam(int idExam)
        {
            try
            {
                return new ObjectResult(_exam.RandomQuestionNew(idExam).ToList());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idUser}/{idCourses}/{numberQuestion}")]
        public IActionResult getExamCourses(int idUser, int idCourses, int numberQuestion)
        {
            try
            {
                if (_learn.GetProgress(idUser, idCourses) == 100)
                {
                    return new ObjectResult(_exam.ExamCourses(idCourses, numberQuestion));
                }
                else
                {
                    return new ObjectResult("You don't complete lesson before");
                }
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost("update-result")]
        public IActionResult updateResultExam([FromBody]object json)
        {
            try
            {
                dynamic data = JObject.Parse(json.ToString());
                string result = _exam.UpdateResultExam((int)data["idUser"], (int)data["idExam"], (int)data["numberRight"]);
                return new ObjectResult(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("update-result-final")]
        public IActionResult updateFinalExam([FromBody]object json)
        {
            try
            {
                dynamic data = JObject.Parse(json.ToString());
                string result = _exam.UpdateFinalExam((int)data["idUser"], (int)data["idCourses"], (int)data["numberRight"], (int)data["numberQuestion"]);
                if (result != null)
                {
                    return new ObjectResult(result);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("check-answer")]
        public IActionResult checkAnswer([FromBody]object json)
        {
            try
            {
                dynamic data = JObject.Parse(json.ToString());
                int numberRight;
                if (data["listID"].ToString() == "")
                {
                    numberRight = 0;
                }
                else
                {
                    string[] listIDQuestion = data["listID"].ToString().Split(',');
                    numberRight = _exam.CheckAnswer((int)data["idExam"], listIDQuestion);
                }
                string message = _exam.UpdateResultExam((int)data["idUser"], (int)data["idExam"], numberRight);
                return new JsonResult(new ExamActionModel { numberRight = numberRight, Message = message });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idUser}/{idExam}")]
        public IActionResult getExamDetails(int idUser, int idExam)
        {
            try
            {
                var history = _examDetails.Get(idUser, idExam);
                if (history != null)
                {
                    return new ObjectResult(new ExamDetailsActionModel
                    {
                        ExamName=history.Exam.ExamName,
                        LessonId=history.Exam.LessonId,
                        ExamId = history.ExamId,
                        Time = history.EditTime,
                        Point = history.Point,
                        Status = history.Status == true ? "Hoàn Thành" : "Chưa Hoàn Thành"
                    });
                }
                return new ObjectResult(new ExamDetailsActionModel
                {                    
                    Point = 0,
                    Status = "Chưa Hoàn Thành"
                });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idLesson}/list-exam")]
        [AllowAnonymous]
        public IActionResult getListLesson(int idLesson)
        {
            try
            {
                return new ObjectResult(_exam.getListExam(idLesson));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult addExam([FromBody]Exam model)
        {
            try
            {
                return new ObjectResult(_exam.AddExam(model));
            }
            catch
            {
                return BadRequest();
            }
          
        }
        [HttpPost("uploadQuestion")]
        public ActionResult uploadQuestion(int id)
        {
            var httpRequest = HttpContext.Request.Form;
            //upload file
            var url = _file.UploadFile(httpRequest.Files[0]);
            //var idLesson =httpRequest.Keys[0];
            string sWebRootFolder = _hostingEnv.WebRootPath;
            //string sFileName = @"test.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder+url));
            int errorRow = 0;
            try
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    //get sheet one
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    for (int row = 3; row <= rowCount; row++)
                    {
                        errorRow = row;
                        int idQuestion = _question.addQuestion(new Question
                        {
                            Content = worksheet.Cells[row, 1].Value.ToString(),
                            Level = Int32.Parse(worksheet.Cells[row, 2].Value.ToString()),
                            ExamId = id,
                        });
                        if (idQuestion != -1)
                        {
                            //print error
                            string comlumnError = null;
                            try
                            {
                                var resultRight = worksheet.Cells[row, 3].Value.ToString();
                                for (int column = 4; column <= ColCount; column++)
                                {
                                    comlumnError = column.ToString();
                                    _resultQuestion.Add(new ResultQuestion
                                    {
                                        Content = worksheet.Cells[row, column].Value.ToString(),
                                        QuestionId = idQuestion,
                                        Status = resultRight == worksheet.Cells[row, column].Value.ToString() ? true : false
                                    });
                                }
                            }
                            catch
                            {
                                return new JsonResult(new UploadQuestionActionModel { Status = false, Message = "Lỗi tại dòng " + row.ToString() + "cột " + comlumnError });
                            }

                        }
                        else
                        {
                            return new JsonResult(new UploadQuestionActionModel { Status = false, Message = "Lỗi tại dòng " + row.ToString() });
                        }
                    }

                }
                return new JsonResult(new UploadQuestionActionModel { Status = true, Message = "Thêm thành công " + (errorRow-2).ToString() +"câu hỏi."});
            }
            catch (Exception ex)
            {
                return new JsonResult(new UploadQuestionActionModel { Status = false, Message ="Thêm thành công "+(errorRow-2-1).ToString() + ". Lỗi tại dòng " + errorRow.ToString()});
            }
        }
        [HttpGet("{idExam}/list-question")]
        [AllowAnonymous]
        public IActionResult getlistQuestion(int idExam)
        {
            try
            {
                return new ObjectResult(_question.getListQuestion(idExam));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{idExam}")]
        public IActionResult delete(int idExam)
        {
            try
            {
                return new ObjectResult(_exam.Remove(idExam));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult edit([FromBody]Exam model)
        {
            try
            {
                return new ObjectResult(this._exam.Edit(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idUser}/list-history-exam")]
        public IActionResult randomExam(int idUser)
        {
            try
            {
                return new ObjectResult(this._exam.listHistoryExam(idUser));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}