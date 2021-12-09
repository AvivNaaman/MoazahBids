using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoazahBids.Web.Models
{
    public class AppConfig
    {
        public List<UserDetails> Admins { get; set; }

        public class UserDetails
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
    }

}
