using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Models
{
    [Table("UserFood")]
    public class UserFood
    {
        [Key]
        public int codUserFood { get; set; }

        public string nameUser { get; set; }


        public virtual ICollection<UserFace> userFace { get; set; }

        public virtual ICollection<UserFoodRestriction> userFoodRestriction { get; set; }

        public string others { get; set; }
    }
}
