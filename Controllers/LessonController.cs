using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.ActionModels;
using Api_ELearning.Services;
using Api_ELearning.Repositories;
using Api_ELearning.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_ELearning.Controllers
{
     [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IFileService _file;
        private readonly ILessonRepository _lesson;

        public LessonController(IFileService file, ILessonRepository lesson)
        {
            _file = file;
            _lesson = lesson;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult post([FromBody]Lesson model)
        {
            try
            {
                bool result = _lesson.Add(model);
                return new ObjectResult(result);
            }
            catch
            {
                return new ObjectResult(false);
            }
        }
        [HttpPut]
        [AllowAnonymous]
        public IActionResult EditCourses([FromBody]Lesson model)
        {
            try
            {
                return new ObjectResult(_lesson.Edit(model));
            }
            catch
            {
                return new ObjectResult(false);
            }
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult deleteCourses(int id)
        {
            try
            {
                return new ObjectResult(_lesson.Remove(id));
            }
            catch
            {
                return new ObjectResult(false);
            }
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult get(int id)
        {
            try
            {
                return new ObjectResult(_lesson.Get(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getAllLesson()
        {
            try
            {
                return new ObjectResult(_lesson.All);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("upload-file")]
        [AllowAnonymous]
        //[Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult UploadLesson()
        {
            var httpRequest = HttpContext.Request.Form;
                var url = _file.UploadFile(httpRequest.Files[0]);
                if (!string.IsNullOrEmpty(url))
                {
                    return new JsonResult(url);
                }
            return BadRequest();
        }
    }
}
