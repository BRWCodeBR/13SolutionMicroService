using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace User.Api.Service
{
    public static class FacialService
    {
        private static async Task<bool> UpsertFaceListAndCheckIfContainsFaceAsync()
        {
            var faceListId = FaceListId.ToString();
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
            var bytes = Convert.FromBase64String(resizeImage.Content);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        }

        public static async string UpsertBase64(var base64)
        {
            SaveBase64String(base64);

            var facesFolder = HttpContext.Current.Server.MapPath("~/Faces");
            
            var sourceImage = "";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            FaceListId = Guid.Parse(Configuration["FaceListId"]);

            faceServiceClient = new FaceServiceClient(Configuration["FaceAPIKey"], "https://eastus.api.cognitive.microsoft.com/face/v1.0");

            Console.WriteLine("Face Detection console. Please inform file name to check and hit enter. To exit, type exit");

            var command = "";
                try
                {
                    var containsAnyFaceOnList = UpsertFaceListAndCheckIfContainsFaceAsync().Result;

                    var face = DetectFaceAsync(sourceImage).Result;
                    if (face != null)
                    {
                        Guid? persistedId = null;
                        if (containsAnyFaceOnList)
                            persistedId = FindSimilarAsync(face.FaceId, FaceListId).Result;

                        if (persistedId == null)
                        {
                            persistedId = AddFaceAsync(FaceListId, sourceImage).Result;
                            Console.WriteLine($"New User with FaceId {persistedId}");
                        }
                        else
                            Console.WriteLine($"Face Exists with Face {persistedId}");
                    }
                    else
                    {
                        Console.WriteLine("Not a face!");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Probably Rate Limit for API was reached, please try again later");
                }         
        }
    }
}
