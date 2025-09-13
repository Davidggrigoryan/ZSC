using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ZombieSoccer
{
    public static class Web
    {
        
        #region CORE

        public static async Task<string> Request( string RequestType, string QueryString, string IdToken = null, string Payload = null)
        {
            UnityWebRequest www = new UnityWebRequest(QueryString, RequestType);
            www.SetRequestHeader("Content-Type", "application/json");
            if (IdToken != null)
            {
                www.SetRequestHeader("Authorization", IdToken);
            }

            if (Payload != null)
            {
                var jsonBytes = Encoding.UTF8.GetBytes(Payload);
                www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            }

            www.downloadHandler = new DownloadHandlerBuffer();
            var status = www.SendWebRequest();
            
            while(!status.isDone)
            {
                await Task.Yield();
            }
            
            if( www.error != null)
            {
                Debug.LogError(www.error);
            }
            return www.downloadHandler.text;
        }

        public static async Task<T> Request<T>( string RequestType, string QueryString, string IdToken = null, string Payload = null)
        {
            UnityWebRequest www = new UnityWebRequest(QueryString, RequestType);
            www.SetRequestHeader("Content-Type", "application/json");
            if (IdToken != null)
            {
                www.SetRequestHeader("Authorization", IdToken);
            }

            if (Payload != null)
            {
                var jsonBytes = Encoding.UTF8.GetBytes(Payload);
                www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            }

            www.downloadHandler = new DownloadHandlerBuffer();
            var status = www.SendWebRequest();
            
            while(!status.isDone)
            {
                await Task.Yield();
            }
            
            if( www.error != null)
            {
                Debug.LogError(www.error);
            }
            
            return JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
        }
        #endregion

        #region COMMON METHODS
        static public string ParseToken(string token, string json)
        {
            JToken jt;
            JObject.Parse(json).TryGetValue(token, out jt);
            return jt.ToString();
        }

        #endregion

        #region FIELDS

        public static class JsonTerms
        {
            public const string idToken = "idToken";
            public const string localId = "localId";
            public const string error = "error";
            public const string refreshToken = "refreshToken";
            public const string message = "message";
        }
        
        public static class ErrorMessages
        {
            public const string invalidEmail = "INVALID_EMAIL";
            public const string invalidPassword = "INVALID_PASSWORD";
        }
        
        public static string IdToken { get; set; }
        public static string RefreshToken { get; set; }
        public static string LocalId { get; set; }

        public const string POST_MergeApiFormat = "api/character/{0}/merge";
        public const string POST_SignInApi = "api/user/signin";
        public const string POST_SignUpApi = "api/user/signup";
        public const string POST_PortalCalledApiFormat = "api/portal/{0}/summon";
        public const string POST_RefreshTokenApi = "api/user/refresh";
        public const string POST_UserInit = "api/user/init";
        public const string POST_PlayMatchFormat = "api/match/{0}/play";
        
        public const string PATCH_UpgradeApiFormat = "api/character/{0}/upgrade";

        // Full list by level
        public const string GET_UpgradesPrice = "api/character/upgrade/price/list";
        public const string GET_UpgradePriceApi = "api/character/{0}/upgrade";

        public const string GET_GetUserApi = "api/user";
        public const string GET_WorldState = "api/scenario/world";

        public const string DELETE_CaracterFormat = "api/character/{0}/delete";

        public const string POST = "POST";
        public const string GET = "GET";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string PATCH = "PATCH";

        // debug endpoints
        public const string POST_ApiDebug = "api/debug";
        public const string POST_ResetWallet = "api/debug/wallet/reset";
        public const string POST_ResetCharacters = "api/debug/characters/reset";
        public const string POST_FillWallet = "api/debug/wallet/fill";
        public const string POST_ResetScenario = "api/debug/scenario/reset";
        public const string POST_ResetPieces = "api/debug/pieces/reset";
        public const string POST_UpgradeAllCharacters = "api/debug/characters/upgrade/maxlevel";
        public const string POST_ResetCharactersToFirstLvl = "api/debug/characters/upgrade/initialLevel";
        //
        
        public const string GoogleApTokeniUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyCyF0QrgNd_OkVpoRfkHs-rCM5tAUjP4cY";

        public const string UrlToServer = "http://51.250.93.2:8080";

        #endregion
    }
}
