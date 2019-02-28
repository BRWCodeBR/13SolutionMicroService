using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using User.Api.Message;
using User.Api.Models;
using User.Api.Polly;
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
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly ILogger _logger;

        public UsersController(IConfiguration _config, ILogger<UsersController> logger,
                             IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            config = _config;
            //RECEBE AS MENSAGENS ENVIAS POR NÒS COMO TESTE
            //new UserReceive().ReceiveMessages();
            _logger = logger;
            _policyRegistry = policyRegistry;
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
                var client = new HttpClient();
                var retryPolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.BasicRetry)
                             ?? Policy.NoOpAsync<HttpResponseMessage>();

                var context = new Context($"GetSomeData-{Guid.NewGuid()}", new Dictionary<string, object>
                {
                    { PolicyContextItems.Logger, _logger }, { "UserServiceModel", request }
                });
                

                var response = await retryPolicy.ExecuteAsync(async () =>
                {
                    return await UpsertFoodRestrictions(request);
                });
                

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private async Task<HttpResponseMessage> UpsertFoodRestrictions(UserServiceModel request)
        {
            var responseMessage = new HttpResponseMessage();

            UserService.UpsertFoodRestrictions(request);

            return responseMessage;
        }

    }
}
