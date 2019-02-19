using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Users.Contract
{
    public class FoodRestriction
    {
        public Guid FoodId { get; set; }
        public string FoodName { get; set; }
    }
}
