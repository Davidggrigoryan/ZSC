#if ZSC_USE_FIREBASE
﻿using Firebase.DynamicLinks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ZombieSoccer.DeepLink
{
    public class DeepLinkBuilder
    {

        private string _domain              = "https://zombiesoccerdev.page.link";
        private string _uri;
        private string _iosParameters       = "ru.tor.zombiesoccerdev";
        private string _androidParameters   = "ru.tor.zombiesoccerdev";

        public DeepLinkBuilder SetDomain( string domain )
        {
            this._domain = domain;
            return this;
        }
        public DeepLinkBuilder SetUri( string uri)
        {
            this._uri = uri;
            return this;
        }
        public DeepLinkBuilder SetIosParameters( string iosParameters)
        {
            _iosParameters = iosParameters;
            return this;
        }
        public DeepLinkBuilder SetAndroidParameters( string androidParameters)
        {
            this._androidParameters = androidParameters;
            return this;
        }
        
        //  terminal function 
        public DynamicLinkComponents BuildLongLink()
        {
            var link = new Firebase.DynamicLinks.DynamicLinkComponents(
                new System.Uri(_uri),
                _domain)
            {
                IOSParameters = new Firebase.DynamicLinks.IOSParameters(_iosParameters),
                AndroidParameters = new Firebase.DynamicLinks.AndroidParameters(_androidParameters)
            };
            return link;
        }


        //  param `components` is long link component
        public async Task<Uri> BuildShortLink( DynamicLinkComponents? components = null)
        {
            /*
             * if after
             * DeepLinkBuilder()
             *          .Set(...)
             *          ...
             *          .BuildShortLink()
             */
            if( null == components)
            {
                components = BuildLongLink();
            }

            var options = new Firebase.DynamicLinks.DynamicLinkOptions
            {
                PathLength = DynamicLinkPathLength.Unguessable
            };

            await Firebase.DynamicLinks.DynamicLinks.GetShortLinkAsync(components, options).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("[DeepLinkBuilder] GetShortLinkAsync was canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("[DeepLinkBuilder] GetShortLinkAsync encountered an error: " + task.Exception);
                }

                Firebase.DynamicLinks.ShortDynamicLink link = task.Result;
                Debug.Log($"[DeepLinkBuilder] Short link {link.Url}");

                var warnings = new System.Collections.Generic.List<string>(link.Warnings);
                if( warnings.Count > 0)
                {
                    foreach (var warning in warnings)
                    {
                        Debug.Log($"[DeepLinkBuilder] warning: {warning}");
                    }
                }
                //_lazyBox.Value = link.Url;
            });
            return null;
            //return await _lazyBox.ReturnAfterWait();
        }
    }
}

#endif
