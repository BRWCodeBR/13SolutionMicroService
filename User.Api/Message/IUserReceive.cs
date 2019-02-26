using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Message
{
    public interface IUserReceive           
    {
        void UserReceive(IConfiguration configuration);
    }
}
