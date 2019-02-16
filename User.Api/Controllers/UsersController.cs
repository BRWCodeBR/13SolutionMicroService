using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Api.Models;
using User.Api.ViewModels;

namespace User.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserContext db = new UserContext();


        [HttpPost]
        [Route("userretrieved")]
        public IActionResult UserRetrieved(RequestUser request)
        {
            try
            {


                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); 
            }
        } 
    }
}
