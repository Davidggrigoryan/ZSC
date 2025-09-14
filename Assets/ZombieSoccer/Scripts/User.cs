using Firebase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.ApplicationLayer.Data
{

    [System.Serializable]
    public class UserInfo
    {
        public string NickName { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
    }


    [System.Serializable]
    public class Characters
    {
        [SerializeField]
        public Dictionary<string, Character> list = new Dictionary<string, Character>();
    }


    [System.Serializable]
    public class Wallet
    {
        public int Elixir;
        public int Dust;
        public int Crystal;

        public bool ResourceSufficient(Wallet otherWallet)
        {
            bool elixirSufficient = Elixir >= otherWallet.Elixir;
            bool dustSufficient = Dust >= otherWallet.Dust;
            bool crystalSufficient = Crystal >= otherWallet.Crystal;

            return elixirSufficient && dustSufficient && crystalSufficient;
        }
    }

    [System.Serializable]
    public class PortalData
    {
        [SerializeField]
        public List<UserPortal> Portals { get; set; } = new List<UserPortal>();
    }

    [System.Serializable]
    public class UserPortal
    {
        public string? Name { get; set; }
        public DateTime? LastUsedTimestamp { get; set; }
    }

    [System.Serializable]
    public class ShieldData
    {
        [SerializeField]
        public int BlazonIndex, TapeIndex, DetailIndex, BallIndex;
    }

    [System.Serializable]
#if ZSC_USE_FIREBASE
    public class UserModel
    {        

        [NonSerialized]
        public string id;        

        public UserModel(string id, FirebaseApp firebaseApp )
        {
            this.id = id;
            var commonPath = $"{CommonStrings.DBTableUserPath}/{id}/";
            
            Info =          new DBStruct<UserInfo>      ($"{commonPath}{CommonStrings.DBFieldUserInfoPath}", firebaseApp);
            //Characters =    new DBStruct<Dictionary<string, Character>>($"{commonPath}{CommonStrings.DBFieldUserCharactersPath}", firebaseApp);
            Wallet =        new DBStruct<Wallet>    ($"{commonPath}{CommonStrings.DBFieldUserWalletPath}", firebaseApp);
            Portal =        new DBStruct<PortalData>    ($"{commonPath}{CommonStrings.DBFieldUserPortalDataPath}", firebaseApp);
            Shield =        new DBStruct<ShieldData>    ($"{commonPath}{CommonStrings.DBFieldUserShieldDataPath}", firebaseApp);
            Scenarios =     new DBStruct<Dictionary<string, string>>($"{commonPath}{CommonStrings.DBFieldUserScenariosDataPath}", firebaseApp);

        }

        //public void PushData()
        //{
        //    Info.PushData();
        //    Characters.PushData();
        //    Wallet.PushData();
        //    Portal.PushData();
        //    Shield.PushData();
        //}

        public void ApplyRemoteChanges()
        {
            Info.ApplyRemoteChanges();
            //Characters.ApplyRemoteChanges();
            Wallet.ApplyRemoteChanges();
            Portal.ApplyRemoteChanges();
            Shield.ApplyRemoteChanges();
            Scenarios.ApplyRemoteChanges();
        }

        public static async Task MakeNewEmpty(string id, FirebaseApp firebaseApp)
        {
            //var characters = new UserCharacters(){ list = CharactersManager.MakeDefaultCharactersSet()};

            var result = new
            {
                Info = new UserInfo(),
                Characters = new Characters(),
                Wallet = new Wallet(),
                Portal = new PortalData(),
                Shield = new ShieldData()
            };

            var json = JsonConvert.SerializeObject(result);
            var database = Firebase.Database.FirebaseDatabase.GetInstance(firebaseApp);
            await database.RootReference.Child($"{CommonStrings.DBTableUserPath}/{id}").SetRawJsonValueAsync(json);
        }

        public DBStruct<UserInfo> Info { get; private set; }        
        //public DBStruct<Dictionary<string, Character>> Characters { get; private set; }
        public DBStruct<Wallet> Wallet { get; private set; }
        public DBStruct<PortalData> Portal { get; private set; }
        public DBStruct<ShieldData> Shield { get; private set; }
        public DBStruct<Dictionary<string, string>> Scenarios { get; private set; }
    }
#endif
}
