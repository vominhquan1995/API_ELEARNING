using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Models;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net;
using Api_ELearning.ActionModels;
using Newtonsoft.Json.Linq;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class CetificatesController : Controller
    {
        private readonly ICetificateRepository _cetificate;
        public CetificatesController(ICetificateRepository cetificate)
        {
            _cetificate = cetificate;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return new ObjectResult(_cetificate.All);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idCourses}")]
        public IActionResult Get(int idCourses)
        {
            try
            {
                return new ObjectResult(_cetificate.GetCetificate(idCourses));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}