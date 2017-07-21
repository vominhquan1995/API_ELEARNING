using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.ActionModels;
using Newtonsoft.Json.Linq;
using Api_ELearning.Services;
using Microsoft.AspNetCore.Http;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    public class CoursesController : Controller
    {
        private readonly ICoursesRepository _courses;
        private readonly ICategoryRepository _category;
        private readonly ICetificateRepository _cetificate;
        private readonly IRateRepository _rate;
        private readonly ILessonRepository _lesson;
        private readonly IFileService _file;
        public CoursesController(ICoursesRepository courses,
         ICategoryRepository category,
         IRateRepository rate, ICetificateRepository cetificate,
         ILessonRepository lesson, IFileService file)
        {
            _courses = courses;
            _category = category;
            _rate = rate;
            _cetificate = cetificate;
            _lesson = lesson;
            _file = file;
        }
        [HttpPut]
        [AllowAnonymous]
        public IActionResult EditCourses([FromBody]CoursesActionModel model)
        {
            try
            {
                var coure = new Models.Course
                {
                    Id = model.Id,
                    Name = model.NameCourses,
                    Price = model.Price,
                    Description = model.Description,
                    NumberCredits = model.NumberCredits,
                    Donors = model.Donors,
                    UrlImage = model.UrlImage,
                    DateStart = model.DateStart,
                    DateEnd = model.DateEnd,
                    AccountId =model.idTeacher,
                    CategoryId =model.idCategory,
                    CetificateId =model.idCetificate
                };
                bool result = _courses.Edit(coure);
                return new ObjectResult(result);
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
                return new ObjectResult(_courses.Remove(id));
            }
            catch
            {
                return new ObjectResult(false);
            }
        }
        //get list all courses
        [HttpGet]
        [AllowAnonymous]
        public IActionResult gets()
        {
            try
            {
                return new ObjectResult(_courses.ListCourse());
            }
            catch
            {
                return BadRequest();
            }
        }
        //get list course with type category
        [HttpGet("{idCategory}")]
        [AllowAnonymous]
        public IActionResult get(int idCategory)
        {
            try
            {
                return new ObjectResult(_courses.ListCoursesCategory(idCategory));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get top courses most popular
        [HttpGet("{numberItem}/mostcourses")]
        [AllowAnonymous]
        public IActionResult getMostCourses(int numberItem)
        {
            try
            {
                return new ObjectResult(_courses.ListMostPopular(numberItem));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get top new courses
        [HttpGet("{numberItem}/newcourses")]
        [AllowAnonymous]
        public IActionResult getNewCourses(int numberItem)
        {
            try
            {
                return new ObjectResult(_courses.ListNewCourses(numberItem));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get top review of Courses
        [HttpGet("{numberItem}/top-review")]
        [AllowAnonymous]
        public IActionResult getTopReviewCourses(int numberItem)
        {
            try
            {
                return new ObjectResult(_courses.ListTopReview(numberItem));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get rate for courses
        [HttpGet("{id}/rate")]
        [AllowAnonymous]
        public IActionResult getRate(int id)
        {
            try
            {
                return new ObjectResult(_rate.GetRateCourses(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get review from user may be get top 10 review to show 
        [HttpGet("{id}/review")]
        [AllowAnonymous]
        public IActionResult getReview(int id)
        {
            try
            {
                return new ObjectResult(_rate.GetReviewCourses(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        //get detail courses 
        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public IActionResult getDetail(int id)
        {
            try
            {
                return new ObjectResult(_courses.GetDetailCourses(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        //filter Price
        [HttpGet("{price_min}/{price_max}/filter-price")]
        [AllowAnonymous]
        public IActionResult getFilterPrice(decimal price_min, decimal price_max)
        {
            try
            {
                return new ObjectResult(_courses.FilterPrice(price_min, price_max));
            }
            catch
            {
                return BadRequest();
            }
        }
        //filter rate
        [HttpGet("{rate_min}/{rate_max}/filter-rate")]
        [AllowAnonymous]
        public IActionResult getFilterRate(decimal rate_min, decimal rate_max)
        {
            try
            {
                return new ObjectResult(_courses.FilterRate(rate_min, rate_max));
            }
            catch
            {
                return BadRequest();
            }
        }
        //filter category
        [HttpGet("{idCategory}/filter-category")]
        [AllowAnonymous]
        public IActionResult getFilterCategory(int idCategory)
        {
            try
            {
                return new ObjectResult(_courses.FilterCategory(idCategory));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult addCourses([FromBody]CoursesActionModel model)
        {
            try
            {
                bool result = _courses.Add(new Models.Course
                {
                    Name = model.NameCourses,
                    Price = model.Price,
                    Description = model.Description,
                    NumberCredits = model.NumberCredits,
                    Donors = model.Donors,
                    UrlImage=model.UrlImage,
                    DateStart=model.DateStart,
                    DateEnd =model.DateEnd,
                    AccountId=Int32.Parse(model.Teacher),
                    CategoryId = Int32.Parse( model.Category),
                    CetificateId = Int32.Parse(model.Cetificate)
                });
                return new ObjectResult(result);
            }
            catch
            {
                return new ObjectResult(false);
            }
        }
        [HttpGet("{idCourses}/lessons")]
        [AllowAnonymous]
        public IActionResult GetLesson(int idCourses)
        {
            try
            {
                return new ObjectResult(_lesson.getListLesson(idCourses).OrderBy(o=>o.Index));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("upload-file")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult UploadImage()
        {
            var httpRequest = HttpContext.Request.Form;
            if (httpRequest.Files.Count > 0)
            {
                var url = _file.UploadFile(httpRequest.Files[0]);
                    if (!string.IsNullOrEmpty(url))
                    {
                        return new JsonResult(url);
                    }
            }
            return BadRequest();
        }
        [HttpPost("search")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult Search(string search)
        {
            try
            {
                return new ObjectResult(_courses.Search(search).ToList().Take(20));
            }
            catch
            {
                return BadRequest();
            }
        }

        //get item with number item and index page
        [HttpGet("{numberItem}/{indexPage}/get-page")]
        [AllowAnonymous]
        public IActionResult getListCoursePage(int numberItem,int indexPage)
        {
            try
            {
                return new ObjectResult(_courses.getListCoursePage(numberItem, indexPage));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}