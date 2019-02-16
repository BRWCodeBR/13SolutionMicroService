using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [Route("userretrieved")]
        public IActionResult UserRetrieved()
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
