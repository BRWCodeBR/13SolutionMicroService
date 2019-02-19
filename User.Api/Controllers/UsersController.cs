using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Api.Message;
using User.Api.Models;
using User.Api.Service;
using User.Api.ServiceModel;
using User.Api.ViewModels;

namespace User.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserContext db = new UserContext();


        [HttpGet]
        [Route("/")]
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

        [HttpPost]
        public async Task<IActionResult> Post(FaceServiceModel request)
        {
            try
            {
                var faceGuid = await FacialService.UpsertBase64(request.face);
                //var imageFace = db.UserFace.Where(x => x.faceId == faceGuid.Value.ToString()).FirstOrDefault();
                UserFace imageFace = null;

                if(imageFace != null) //JA TEM CADASTRADO
                {
                    var _iUMsg = new UserMessage(FacialService.Configuration);
                    var msg = new Microsoft.Azure.ServiceBus.Message()
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        Body = Encoding.ASCII.GetBytes("Cadastro Persistido: " + imageFace.faceId)
                    };

                    _iUMsg.SendMessagesAsync(msg);
                }
                else //NÂO TEM CADASTRADO
                {
                    var createdFaceId = FacialService.UpsertBase64(request.face);                    
                }


                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }        
    }
    }
}
