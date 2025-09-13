using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSoccer.Models.UserM
{
    public class UserAuth
    {
        public string? email { get; set; }
        public string? password { get; set; }
        public bool? returnSecureToken { get; set; }

        public UserAuth(string email, string password, bool returnSecureToken)
        {
            this.email = email;
            this.password = password;
            this.returnSecureToken = returnSecureToken;
        }
    }
}
