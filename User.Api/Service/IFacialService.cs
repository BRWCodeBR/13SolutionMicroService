using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Service
{
    interface IFacialService : FacialService
    {
        private static async Task<bool> UpsertFaceListAndCheckIfContainsFaceAsync();
        private static async Task<Guid?> FindSimilarAsync(Guid faceId, Guid faceListId);
        private static async Task<Face> DetectFaceAsync(string imageFilePath);
        private static async Task<Guid?> AddFaceAsync(Guid faceListId, string imageFilePath);
    }
}
