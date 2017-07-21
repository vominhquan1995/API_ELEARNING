using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.ActionModels;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class CategorysController : Controller
    {
        private readonly ICategoryRepository _category;
        public CategorysController(ICategoryRepository category)
        {
            _category = category;
        }
        [HttpGet]
        public IActionResult get()
        {
             try
            { 
                return new ObjectResult(_category.All );
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}