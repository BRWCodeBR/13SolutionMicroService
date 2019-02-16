using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.ViewModels
{

    public class ResponseUser
    {
        public string[] restrictions { get; set; }

        public string others { get; set; }

        public string userId { get; set; }

        public string requesterId { get; set; }
    }
}
