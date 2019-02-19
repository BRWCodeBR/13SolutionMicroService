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
        //private UserContext db = new UserContext();        

        /// <summary>
        /// UserRetrieved
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(FaceServiceModel request)
        {
            try
            {
                var faceGuid = await FacialService.UpsertBase64(request.face);
                var imageFace = UserStaticContext.UserFace.Where(x => x.faceId == faceGuid.Value.ToString()).FirstOrDefault();                

                if(imageFace != null) //JA TEM CADASTRADO
                {
                    MessageService.SendPersistedIdMessage(imageFace.faceId);
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
