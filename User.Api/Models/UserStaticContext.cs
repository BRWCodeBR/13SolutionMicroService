using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Models
{
    public static class UserStaticContext
    {
        public static List<UserFood> _UserFood { get;set; }

        public static ICollection<UserFace> _UserFace { get; set; }

        public static ICollection<UserFoodRestriction> _UserFoodRestriction { get; set; }

        public static List<UserFood> UserFood
        {
            get {
                if (_UserFood == null)
                    _UserFood = new List<UserFood>();
                return _UserFood;
            }

            set { _UserFood = value; }
        }

        public static ICollection<UserFace> UserFace
        {
            get
            {
                if (_UserFace == null)
                    _UserFace = new List<UserFace>();
                return _UserFace;
            }

            set { _UserFace = value; }
        }

        public static ICollection<UserFoodRestriction> UserFoodRestriction
        {
            get
            {
                if (_UserFoodRestriction == null)
                    _UserFoodRestriction = new List<UserFoodRestriction>();
                return _UserFoodRestriction;
            }

            set { _UserFoodRestriction = value; }
        }
    }
}
