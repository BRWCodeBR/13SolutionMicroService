using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Models
{
    [Table("UserFace")]
    public class UserFace
    {
        [Key]
        public string codUserFace { get; set; }

        public string faceId { get; set; }

        public string codUserFoodFK { get; set; }

        [ForeignKey("codUserFoodFK")]
        public virtual UserFood userFood { get; set; }
    }
}
