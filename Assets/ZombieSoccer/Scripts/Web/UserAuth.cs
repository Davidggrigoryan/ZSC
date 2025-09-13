using Newtonsoft.Json;

namespace ZombieSoccer.ZombieSoccer.Scripts.Web
{
    public class UserAuth
    {
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("returnSecureToken")]
        public bool returnSecureToken { get; set; } // has to be true

        public UserAuth(string email, string password)
        {
            this.email = email;
            this.password = password;
            this.returnSecureToken = true;
        }
    }
}