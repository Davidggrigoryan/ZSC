using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSoccer.Models.UserM
{
    public class UserAuthResponse
    {
        public string? kind { get; set; }
        public string? localId { get; set; }
        public string? email { get; set; }
        public string? displayName { get; set; }
        public string? idToken { get; set; }
        public bool? registered { get; set; }
        public string? refreshToken { get; set; }
        public string? expiresIn { get; set; }
        public string? error { get; set; }

    }
}
