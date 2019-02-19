using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Users.Contract
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid FaceId { get; set; }

        public List<FoodRestriction> FoodRestrictions { get; set; }
    }
}
