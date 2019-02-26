using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration config;
        private IUserReceive receive;

        public UsersController(IConfiguration _config)
        {
            config = _config;
            receive = new UserReceive(config);
        }

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
                        MessageService.SendPersistedIdMessage(user);
                    }
                }
                else //NÂO TEM CADASTRADO
                {
                    var newUser = new UserFood()
                    {
                        nameUser = "Nome"
                    };

                    db.UserFood.Add(newUser);
                    db.SaveChanges();

                    var userFace = new UserFace()
                    {
                        codUserFoodFK = newUser.codUserFood,
                        faceId = faceGuid.ToString()
                    };

                    db.UserFace.Add(userFace);
                    db.SaveChanges();

                    MessageService.SendNewIdMessage(newUser);
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }        
    }

        [HttpPost]
        [Route("/foodrestriction")]
        public async Task<IActionResult> FoodRestriction(UserServiceModel request)
        {
            try
            {
                UserService.UpsertFoodRestrictions(request);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
