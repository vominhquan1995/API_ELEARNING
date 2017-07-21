using Microsoft.AspNetCore.Mvc;
using Api_ELearning.ActionModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Api_ELearning.Repositories;
using Api_ELearning.Services;
using Api_ELearning.Models;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _account;
        private readonly IEmailService _email;
        private readonly IFileService _file;
        public AccountsController(IAccountRepository account,
            IEmailService email, IFileService file)
        {
            _account = account;
            _email = email;
            _file = file;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp(SignUpActionModel model)
        {
            if (ModelState.IsValid)
            {
                if (_account.Get(model.Email) != null)
                {
                    return BadRequest(new JsonResult("Account existed!"));
                }
                bool result = _account.Add(new Account
                {
                    Email = model.Email,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    Password = model.Password,
                    RoleId = 1
                });
                if (result) return new ObjectResult(model.Email);
            }
            return BadRequest(new ObjectResult("Sign up failed!"));
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody]string email)
        {
            //user emailservices to send email; 
            if (email != null)
            {
                bool emailSucceed = await _email.SendMailRecoveryPassword(email);
                if (emailSucceed)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
        [HttpPost("{id}/upload-file")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult UploadAvatar(int id)
        {
            var httpRequest = HttpContext.Request.Form;
            if (httpRequest.Files.Count > 0)
            {
                var url = _file.UploadFile(httpRequest.Files[0]);
                //upload url User
                var user = _account.Get(id);
                user.UrlAvatar = url;
                // update with change password
                if (_account.EditWithoutPassword(user))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        return new ObjectResult(url);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            return BadRequest();
        }

        [HttpGet("{url}")]
        [AllowAnonymous]
        public IActionResult GetEmail(string url)
        {
            var account = _account.getEmail(url);
            if (account != null)
            {
                return new JsonResult(account);
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RequestChangePassword([FromBody]ResetPasswordActionModel model)
        {
            var account = _account.Get(model.Id);
            if (account != null)
            {
                account.Password = model.Password;
                if (_account.Edit(account))
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }
    }
}