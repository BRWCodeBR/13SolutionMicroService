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

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/")]
        public async Task<IActionResult> Post(FaceServiceModel request)
        {
            try
            {
                //PROCESSANDO
                MessageService.SendProcessingMessage(Guid.NewGuid().ToString());
                //ANALISA IMAGEM
                var faceGuid = await FacialService.UpsertBase64(request.face);

                //PEGA USUARIO
                var imageFace = UserStaticContext.UserFace.Where(x => x.faceId == faceGuid.Value.ToString()).FirstOrDefault();                

                //PUBLICAR RESTRICOES
                if(imageFace != null) //JA TEM CADASTRADO
                {
                    UserFood user = db.UserFood.Where(x => x.codUserFood == imageFace.codUserFoodFK).FirstOrDefault();
                    if(user != null)
                    {
                        var restricoes = user.userFoodRestriction;
                        MessageService.SendPersistedIdMessage(imageFace.faceId);
                    }
                }
                else //NÂO TEM CADASTRADO
                {
                    
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
