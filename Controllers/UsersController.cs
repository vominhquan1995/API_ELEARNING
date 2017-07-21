using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api_ELearning.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Api_ELearning.Models;

namespace Api_ELearning.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer", Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly IAccountRepository _accounts;
        private readonly IRoleRepository _roles;
        private readonly ILogger _log;
        public UsersController(IAccountRepository accounts,
            ILogger<UsersController> log,
            IRoleRepository roles)
        {
            _accounts = accounts;
            _log = log;
            _roles = roles;
        }
        [HttpGet]
        public IActionResult Gets()
        {
            _log.LogInformation("GET => All");
            return new OkObjectResult(_accounts.All);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _log.LogInformation(@"GET => [{id}]");
            return new OkObjectResult(_accounts.Get(id));
        }
        [HttpPut]
        public IActionResult Edit([FromBody]Account o)
        {
            _log.LogInformation(@"BEGIN => Edit account has id : {o.Id}");
            if (_accounts.EditWithoutPassword(o))
            {
                _log.LogInformation(@"END => Edit account has id : {o.Id}");
                return Ok();
            }
            _log.LogInformation(@"FAILED => Edit account has id : {o.Id}");
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            _log.LogInformation(@"BEGIN => Remove account has id : {id}");
            if (_accounts.Remove(id))
            {
                _log.LogInformation(@"END => Remove account has id : {id}");
                return Ok();
            }
            _log.LogInformation(@"FAILED => Remove account has id : {id}");
            return BadRequest();
        }
        [HttpGet("roles")]
        public IActionResult GetRole()
        {
            _log.LogInformation(@"GET => Role");
            return new OkObjectResult(_roles.All);
        }
    }
}