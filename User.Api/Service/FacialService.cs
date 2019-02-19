using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using User.Api.Message;
using User.Api.Models;

namespace User.Api.Service
{
    public static class FacialService
    {
        public static IConfiguration Configuration;
        public static FaceServiceClient faceServiceClient;
        public static Guid FaceListId;
        private const string Exit = "exit";

        private static async Task<bool> UpsertFaceListAndCheckIfContainsFaceAsync()
        {
            faceServiceClient = new FaceServiceClient(Configuration["FaceAPIKey"], "https://eastus.api.cognitive.microsoft.com/face/v1.0");
            var faceListId = Guid.Parse(Configuration["FaceListId"]).ToString();
            var faceLists = await faceServiceClient.ListFaceListsAsync();
            var faceList = faceLists.FirstOrDefault(_ => _.FaceListId == FaceListId.ToString());

            if (faceList == null)
            {
                await faceServiceClient.CreateFaceListAsync(faceListId, "GeekBurgerFaces", null);
                return false;
            }

            var faceListJustCreated = await faceServiceClient.GetFaceListAsync(faceListId);

            return faceListJustCreated.PersistedFaces.Any();
        }

        private static async Task<Guid?> FindSimilarAsync(Guid faceId, Guid faceListId)
        {
            faceServiceClient = new FaceServiceClient(Configuration["FaceAPIKey"], "https://eastus.api.cognitive.microsoft.com/face/v1.0");
            var similarFaces = await faceServiceClient.FindSimilarAsync(faceId, faceListId.ToString());

            var similarFace = similarFaces.FirstOrDefault(_ => _.Confidence > 0.5);

            return similarFace?.PersistedFaceId;
        }

        private static async Task<Face> DetectFaceAsync(string imageFilePath)
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    faceServiceClient = new FaceServiceClient(Configuration["FaceAPIKey"], "https://eastus.api.cognitive.microsoft.com/face/v1.0");
                    var faces = await faceServiceClient.DetectAsync(imageFileStream);
                    return faces.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<Guid?> AddFaceAsync(Guid faceListId, string imageFilePath)
        {
            try
            {
                AddPersistedFaceResult faceResult;
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    faceServiceClient = new FaceServiceClient(Configuration["FaceAPIKey"], "https://eastus.api.cognitive.microsoft.com/face/v1.0");
                    faceResult = await faceServiceClient.AddFaceToFaceListAsync(faceListId.ToString(), imageFileStream);
                    return faceResult.PersistedFaceId;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Face not included in Face List!");
                return null;
            }
        }

        private static string SaveBase64String(string base64)
        {
            try
            {
                base64 = base64.Replace("data:image/jpeg;base64,", "");
                var filePath = $"{Directory.GetCurrentDirectory()}\\Faces\\{new Guid().ToString()}.jpg";
                var bytes = Convert.FromBase64String(base64);
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                return filePath;
            }catch(Exception ex)
            {
                return "";
            }
        }

        public static async Task<Guid?> UpsertBase64(string base64, string processingTempGuid)
        {
            var sourceImage = SaveBase64String(base64);

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var FaceListId = Guid.Parse(Configuration["FaceListId"]);                        

            try
            {
                var containsAnyFaceOnList = await UpsertFaceListAndCheckIfContainsFaceAsync();
                var _iUMsg = new UserMessage(FacialService.Configuration);

                var face = await DetectFaceAsync(sourceImage);
                if (face != null)
                {
                    Guid? persistedId = null;
                    if (containsAnyFaceOnList)
                        persistedId = await FindSimilarAsync(face.FaceId, FaceListId);

                    if (persistedId == null)
                    {
                        //Adicionando manualmente um novo registro no banco static
                        UserStaticContext.UserFace.Add(new UserFace()
                        {
                            faceId = persistedId.ToString(),
                            codUserFace = processingTempGuid
                        });
                        MessageService.SendNewIdMessage(persistedId.ToString());
                    }
                    else
                    {
                        MessageService.SendPersistedIdMessage(persistedId.ToString());                        
                    }
                    
                    return persistedId;
                }
                else
                {
                    return null;
                    Console.WriteLine("Not a face!");
                }
            }
            catch (Exception ex)
            {
                return null;
                Console.WriteLine("Probably Rate Limit for API was reached, please try again later");
            }
        }
    }
}
