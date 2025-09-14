#if ZSC_USE_FIREBASE
﻿using Firebase;
using Firebase.DynamicLinks;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using System.Text.RegularExpressions;
using ZombieSoccer.UI;

namespace ZombieSoccer.DeepLink
{
    public class DeepLinkManager : MonoBehaviour
    {
        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

        [Inject]
        PageManager pageManager;

        private void Awake()
        {
            Initialize();
        }
        public async void Initialize()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    await Task.Run(() => InitializeFirebase());
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }

        private void InitializeFirebase()
        {
            DynamicLinks.DynamicLinkReceived += ProcessingPageManager;
        }

        private void ProcessingPageManager(object sender, ReceivedDynamicLinkEventArgs e)
        {
            var receivedUrl = e.ReceivedDynamicLink.Url.ToString();
            string path = GetPathParam(receivedUrl);
            pageManager.Fire(path);
        }

        // from `https://appp/?path=UIView_Match`
        // return `UIView_Match`        
        public string GetPathParam(string url)
        {
            string retrievingArgs = @"\w+=\w+[[\:\/\/\?\.\w+=]*]?";
            var rgx = new Regex(retrievingArgs);
            var receivedUrl = url;

            var argsCollection = rgx.Matches(receivedUrl);
            var argsMap = new Dictionary<string, string>();

            for (int idx = 0; idx < argsCollection.Count; idx++)
            {
                var arg = argsCollection[idx].Value.Split(new Char[] { '=' }, 2);
                argsMap.Add(arg[0], arg[1]);
            }
            string value;
            argsMap.TryGetValue("path", out value);
            return value;
        }
        public class Factory : PlaceholderFactory<string, DeepLinkManager> { }
    }
}

#endif
