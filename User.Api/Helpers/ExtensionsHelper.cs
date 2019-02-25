using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Models;
using User.Api.ServiceModel;

namespace User.Api.Helpers
{
    public static class ExtensionsHelper
    {
        public static UserServiceModel ToServiceModel (this UserFood model, int requesterId = 1111)
        {
            return new UserServiceModel()
            {
                Others = model.others,
                RequesterId = requesterId,
                Restrictions = model.userFoodRestriction.Select(x => x.foodRestriction),
                UserId = model.codUserFood
            };
        }
    }
}
