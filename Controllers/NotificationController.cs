

using Api_ELearning.Models;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_ELearning.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notification;
        public NotificationController(INotificationRepository notification)
        {
            _notification = notification;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return new ObjectResult(_notification.All);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult addNotification([FromBody]Notification model)
        {
            try
            {
                return new ObjectResult(_notification.Add(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{idNotification}")]
        public IActionResult Get(int idNotification)
        {
            try
            {
                return new ObjectResult(_notification.Get(idNotification));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult deleteCourses(int id)
        {
            try
            {
                return new ObjectResult(_notification.Remove(id));
            }
            catch
            {
                return new ObjectResult(false);
            }
        }

    }
}