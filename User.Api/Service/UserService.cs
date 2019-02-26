using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Models;
using User.Api.ServiceModel;

namespace User.Api.Service
{
    /// <summary>
    /// Service especializado na User e suas dependências
    /// </summary>
    public static class UserService
    {
        private static readonly UserContext _db = new UserContext();

        /// <summary>
        /// Upsert de Food Restrictions
        /// </summary>
        /// <param name="serviceModel"></param>
        public static void UpsertFoodRestrictions(UserServiceModel serviceModel)
        {
            var user = _db.UserFood.First(x => x.codUserFood == serviceModel.UserId);

            //Adicionando as novas foodrestricions que ainda não existem associados ao User
            foreach(var foodRestriction in serviceModel.Restrictions)
            {
                if (!user.userFoodRestriction.Any(x => x.foodRestriction.ToLower().Trim() == foodRestriction.ToLower().Trim()))
                {
                    var newFoodRestriction = new UserFoodRestriction()
                    {
                        codUserFoodFK = serviceModel.UserId,
                        foodRestriction = foodRestriction
                    };

                    _db.UserFoodRestriction.Add(newFoodRestriction);
                }                
            }

            _db.SaveChanges();

            //Removendo possíveis FoodRestrictions removidas pela request
            foreach (var foodRestriction in user.userFoodRestriction.Where(x => !serviceModel.Restrictions.Any(y => y == x.foodRestriction)))
            {
                _db.UserFoodRestriction.Remove(foodRestriction);
            }

            _db.SaveChanges();
        }
    }
}
