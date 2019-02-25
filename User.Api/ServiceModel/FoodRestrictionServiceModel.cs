using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.ServiceModel
{
    public class FoodRestrictionServiceModel
    {
        public IEnumerable<string> Restrictions { get; set; }
        public string Others { get; set; }
        public int UserId { get; set; }
        public int RequesterId { get; set; }
    }
}
