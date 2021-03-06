﻿using System;
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
        public int codUserFace { get; set; }

        public string faceId { get; set; }

        public int codUserFoodFK { get; set; }

        [ForeignKey("codUserFoodFK")]
        public virtual UserFood userFood { get; set; }
    }
}
