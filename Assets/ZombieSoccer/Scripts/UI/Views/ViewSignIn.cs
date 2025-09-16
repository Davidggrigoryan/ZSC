#if ZSC_USE_FIREBASE
using Firebase.Auth;
using Firebase.Database;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Database;
using ZombieSoccer.Models.UserM;
using ZombieSoccer.UI.Presenters;
using Cysharp.Threading.Tasks;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.Models.TeamM;

namespace ZombieSoccer.UI.Views
{
    public class ViewSignIn : BasePagePresenter
    {
        #region Common
        [Inject]
        FirebaseAppFacade firebaseAppFacade;

        [Inject]
        DiContainer container;

        FirebaseAuth auth;
        FirebaseUser newFirebaseUser;


        public bool autoSignIn;
        public string emailText;
        public string passwordText;

        protected override void Enable()
        {
            base.Enable();
            auth = FirebaseAuth.DefaultInstance;

            task = Logic().AsUniTask();
            //if (Application.internetReachability == NetworkReachability.NotReachable)
            //{
            //    pageManager.FireMessage(MsgTypeEnum.Error,
            //        localizationManager.GetString(CommonStrings.StringMessageCollection, MessageNotify.NoNetworkError.ToString())
            //        );
            //    while (Application.internetReachability == NetworkReachability.NotReachable)
            //        Task.Yield();
            //}
        }
        #endregion

        private async Task Logic()
        {
            while (!autoSignIn)
                await Task.Yield();

#if UNITY_EDITOR
            await SignIn();
#elif UNITY_ANDROID
             await SignInPlayGoogleServices();
#endif

        }


        [Button]
        private async Task SignIn()
        {
            var api = $"{Web.UrlToServer}/{Web.POST_SignInApi}";
            UserAuth userAuth = new UserAuth(emailText, passwordText, true);
            string userAuthPayload = JsonConvert.SerializeObject(userAuth);
            var response = await Web.Request(Web.POST, api, null, userAuthPayload);
            UserAuthResponse userAuthResponse = JsonConvert.DeserializeObject<UserAuthResponse>(response);

            //if ( !String.IsNullOrEmpty(userAuthResponse.error))
            //{
            //    switch (userAuthResponse.error)
            //    {
            //        case Web.ErrorMessages.invalidEmail:
            //            pageManager.FireMessage(MsgTypeEnum.Error,
            //                await localizationManager.GetString(CommonStrings.StringMessageCollection, MessageNotify.InvalidEmail.ToString())
            //                );
            //            break;
            //        case Web.ErrorMessages.invalidPassword:
            //            pageManager.FireMessage(MsgTypeEnum.Error,
            //                await localizationManager.GetString(CommonStrings.StringMessageCollection, MessageNotify.InvalidPassword.ToString())
            //                );
            //            break;
            //        default:
            //            pageManager.FireMessage(MsgTypeEnum.Error,
            //                await localizationManager.GetString(CommonStrings.StringMessageCollection, MessageNotify.UnknownError.ToString())
            //                );
            //            break;
            //    }
            //    throw new Exception($"Error during login process, error message: {userAuthResponse.error}");
            //}

            Web.IdToken = userAuthResponse.idToken;
            Web.LocalId = userAuthResponse.localId;
            BindModelsToContrainer(userAuthResponse.localId);
            await Task.Yield();
            await InitOrFetchUserAsync(userAuthResponse.localId);
            Debug.LogError("[ViewSignIn.SignIn] Done");
        }

        [Button]
        private async Task SignUp()
        {
            // TODO FIX
            autoSignIn = true;

            var api = $"{Web.UrlToServer}/{Web.POST_SignUpApi}";
            UserAuth userAuth = new UserAuth(emailText, passwordText, true);
            string userAuthPayload = JsonConvert.SerializeObject(userAuth);
            var text = await Web.Request(Web.POST, api, null, userAuthPayload);
        }

        #region ANDROID

        private async Task SignInPlayGoogleServices()
        {
            Debug.LogError("[SignIn] Method: PlayGoogleServices");

            PlayGamesClientConfiguration config =
                new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false)
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            bool isDone = false;
            // Sign In and Get a server auth code.
            UnityEngine.Social.localUser.Authenticate(async (bool success) =>
            {
                if (!success)
                {
                    Debug.LogError("[SignIn] Failed to Sign into Play Games Services.");
                    return;
                }

                string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                if (string.IsNullOrEmpty(authCode))
                {
                    Debug.LogError("[SignIn] Signed into Play Games Services but failed to get the server auth code.");
                    return;
                }
                Debug.LogFormat("[SignIn] Auth code is: {0}", authCode);

                // Use Server Auth Code to make a credential
                Credential credential = PlayGamesAuthProvider.GetCredential(authCode);

                // Sign In to Firebase with the credential
                newFirebaseUser = await auth.SignInWithCredentialAsync(credential);
                var IdToken = await newFirebaseUser.TokenAsync(false);
                Web.IdToken = IdToken;

                Debug.LogFormat("[SignIn] User signed in successfully: {0} ({1})", newFirebaseUser.DisplayName, newFirebaseUser.UserId);

                isDone = true;
            });

            while (!isDone)
                await Task.Yield();

            BindModelsToContrainer(newFirebaseUser.UserId);
            Debug.LogError("[ViewSignIn.SignInPlayGoogleServices] Done");

            await InitOrFetchUserAsync(newFirebaseUser.UserId);
        }

        [Button]
        private async Task InitOrFetchUserAsync(string userID)
        {
            Debug.LogWarning($"[SignIn.OnComplete] Fetch user data: {userID}");

            var path = $"{CommonStrings.DBTableUserPath}/{userID}";

            var DatabaseReference = FirebaseDatabase.DefaultInstance.GetReference(path);
            var snapshot = await DatabaseReference.GetValueAsync(); // TODO: wrap into try-catch?
            var json = snapshot.GetRawJsonValue();

            if (String.IsNullOrEmpty(json))
            {
                Debug.LogError("[ViewSignIn.InitOrFetchUserAsync] Init USER");
                string queryString = $"{Web.UrlToServer}/{Web.POST_UserInit}";
                await Web.Request(Web.POST, queryString, Web.IdToken);
            }

            Debug.LogError("[ViewSignIn.InitOrFetchUserAsync] Done");
        }

        private void BindModelsToContrainer(string userID)
        {
            UserModel userModel = new UserModel(userID, firebaseAppFacade.FirebaseApp);

            container
                .Bind<UserModel>()
                .FromInstance(userModel)
                .AsSingle().NonLazy();

            container
                .Bind<ScenarioModel>()
                .FromInstance(new ScenarioModel(firebaseAppFacade.FirebaseApp))
                .AsSingle().NonLazy();

            container.Bind<DeckModel>().AsSingle().WithArguments(userID, firebaseAppFacade).Lazy();

            container.Bind<TeamsGroupModel>().AsSingle().WithArguments(string.Empty).Lazy();
        }

        #endregion
    }
}


#endif
