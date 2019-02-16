using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Models
{
    [Table("UserFoodRestriction")]
    public class UserFoodRestriction
    {

        [Key]
        public int codUserFoodRestriction { get; set; }

        public string foodRestriction { get; set; }

        public string codUserFoodFK { get; set; }

        [ForeignKey("codUserFoodFK")]
        public virtual UserFood userFood { get; set; }
    }
}
