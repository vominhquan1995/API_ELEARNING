using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.ActionModels;
using Api_ELearning.Models;
using Newtonsoft.Json.Linq;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class StudentsController : Controller
    {
        private readonly ILearnRepository _learn;
        private readonly ICourseDetailsRepository _CourseDetails;
        private readonly IAccountRepository _account;
        public StudentsController(ILearnRepository learn, ICourseDetailsRepository CourseDetails, IAccountRepository account)
        {
            _learn = learn;
            _CourseDetails = CourseDetails;
            _account = account;
        }
        [HttpGet("{id}")]
        public IActionResult getInfomation(int id)
        {
            try
            {
                return new ObjectResult(_account.Get(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        // get progress bar learn off user return percent 
        [HttpGet("{idStudent}/{idCourses}")]
        public IActionResult getProgress(int idStudent, int idCourses)
        {
            try
            {
                return new ObjectResult( _learn.GetProgress(idStudent, idCourses));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get history learn of user
        [HttpGet("{idStudent}/history")]
        public IActionResult getHistory(int idStudent)
        {
            try
            {
                return new ObjectResult(_CourseDetails.GetCourseDetails(idStudent));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get progress plan, with status to set visible for tree progress
        [HttpGet("{idStudent}/{idCourses}/progress-plan-courses")]
        public IActionResult getProgressPlan(int idStudent, int idCourses)
        {
            try
            {
                return new ObjectResult(_learn.GetProgressPlan(idStudent, idCourses));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get learning plane 
        [HttpGet("{idStudent}/learning-plan")]
        public IActionResult getLearningPlan(int idStudent)
        {
            try
            {
                return new ObjectResult(_learn.GetLearningPlan(idStudent));
            }
            catch
            {
                return BadRequest();
            }
        }
        //edit user info
        [HttpPut]
        public IActionResult EditInfo([FromBody]Account model)
        {
                if (_account.EditWithoutPassword(model))
                {
                    return Ok(true);
                }
                return BadRequest();
        }
        //buy course
        [HttpGet("{idUser}/{idCourse}/buy-course")]
        public IActionResult BuyCourse(int idUser,int idCourse)
        {
            try
            {
                return new ObjectResult(_learn.BuyCourse(idUser, idCourse));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}