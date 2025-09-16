#if ZSC_USE_FIREBASE
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Database
{
    public class FirebaseAppFacade : IInitializable
    {
        public FirebaseApp FirebaseApp { get; private set; }
        public FirebaseDatabase Database { get; private set; }

        public bool IsComplete { get; private set; }

        private AppOptions appOptions = new AppOptions
        {
            ApiKey = "AIzaSyCyF0QrgNd_OkVpoRfkHs-rCM5tAUjP4cY",
            AppId = "150208302895",
            DatabaseUrl = new System.Uri("https://zombiesoccer-587c1-default-rtdb.firebaseio.com/"),
            MessageSenderId = "message-sender-id",
            ProjectId = "zombiesoccer-587c1",
            StorageBucket = "zombiesoccer-587c1.appspot.com"
        };

        public void Initialize()
        {
            Debug.LogWarning("[FirebaseAppFacade] Initialize");

            FirebaseApp.CheckDependenciesAsync().ContinueWith(checkTask =>
            {
                // Peek at the status and see if we need to try to fix dependencies.
                Firebase.DependencyStatus status = checkTask.Result;
                if (status != DependencyStatus.Available)
                {
                    return FirebaseApp.FixDependenciesAsync().ContinueWith(t =>
                    {
                        return FirebaseApp.CheckDependenciesAsync();
                    })
                    .Unwrap();
                }
                else
                {
                    return checkTask;
                }
            })
            .Unwrap().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.LogWarning("Firebase complete");
                    FirebaseApp = FirebaseApp.DefaultInstance;
                    Database = FirebaseDatabase.GetInstance(FirebaseApp);
                    IsComplete = true;
                    Debug.LogWarning("[FirebaseAppFacade] Complete");
                }
                else
                {
                    // TODO SEND MESSEGE
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
    }
}

#endif
