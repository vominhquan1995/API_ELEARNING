using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class TeachersController : Controller
    {
        private readonly IAccountRepository _account;
        private readonly ICoursesRepository _courses;
        private readonly IRateRepository _rate;
        //private readonly  
        public TeachersController(IAccountRepository account,ICoursesRepository courses, IRateRepository rate)
        {
            _account = account;
            _courses = courses;
            _rate = rate;
        }
        [HttpGet("{roleName}")]
        [HttpGet]
        public IActionResult getFilterRole(string roleName)
        {
            try
            {
                return new ObjectResult(_account.accountFilterRole(roleName));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idTeacher}/information")]
        public IActionResult getInformation(int idTeacher)
        {
            try
            {
                return new ObjectResult(_account.Get(idTeacher));
            }
            catch
            {
                return BadRequest();
            }
          }
        [HttpGet("{idTeacher}/list-courses")]
        public IActionResult listCourses(int idTeacher)
        {
            try
            {
                return new ObjectResult(_courses.ListCoursesTeacher(idTeacher));
            }
            catch
            {
                return BadRequest();
            }
         }
        //get rate
        [HttpGet("{idTeacher}/rate")]
        public IActionResult getRate(int idTeacher)
        {
            try
            {
                return new ObjectResult(_rate.GetRateTeacher(idTeacher));
            }
            catch
            {
                return BadRequest();
            }            
        }
        [HttpGet("{idTeacher}/review")]
        //get Review 
        public IActionResult getReview(int idTeacher)
        {
            try
            {
                return new ObjectResult( _rate.GetReviewTeacher(idTeacher));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}