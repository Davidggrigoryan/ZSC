#if ZSC_USE_FIREBASE
﻿using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ZombieSoccer.Database;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.Utils.LocalStorage;

namespace ZombieSoccer.Models.DeckM
{

    public class SortSignal
    {        
        public List<Character> characters { get; set; }
    }    
    

    public class DeckModel 
    {

        #region Injects        

        [Inject]
        SignalBus signalBus;

        [Inject]
        LocalStorage localStorage;

        #endregion
        
        #region Fields

        public DBStruct<Dictionary<string, Character>> Characters { get; set; }

        private Dictionary< Type, IDeckSort<Character>> _deckSortClassMap = new Dictionary<Type, IDeckSort<Character>>();

        private Type _prevSortType = null;

        private List<Character> _characters = new List<Character>();

        #endregion                

        public DeckModel( string userID , FirebaseAppFacade firebaseAppFacade)
        {            
            Setup( userID, firebaseAppFacade );
        }

        public void Setup(string userID, FirebaseAppFacade firebaseAppFacade)
        {
            var path = $"{CommonStrings.DBTableUserPath}/{userID}/{CommonStrings.DBFieldUserCharactersPath}";
            Action BindCharacters = async () =>
            {
                Characters = new DBStruct<Dictionary<string, Character>>(path, firebaseAppFacade.FirebaseApp);
                while (null == Characters)
                    await UniTask.Yield();
                _characters = GetCharactersBySort<DefaultDeckSort>();
            };
            BindCharacters();
            Characters.OnDataChangedAction = BindCharacters;
        }

        public void SortWithSeparateNew<TSortType>( params Predicate<Character>[] filterPredicate) where TSortType : IDeckSort<Character>, new()
        {
            SortFunc<TSortType>(separateNew: true, filterPredicate);
        }

        public void Sort<TSortType>( params Predicate<Character>[] filterPredicate ) where TSortType : IDeckSort<Character>, new()
        {
            SortFunc<TSortType>(separateNew: false, filterPredicate);
        }

        private void SortFunc<TSortType>(bool separateNew, params Predicate<Character>[] filtersPredicates) where TSortType : IDeckSort<Character>, new()
        {

            var characters = GetCharactersBySort<TSortType>();

            if (filtersPredicates.Length > 0)
                foreach (var predicate in filtersPredicates)
                    characters = characters.Where(e => predicate(e)).ToList();

            if (separateNew)
            {
                string[] newCharactersIdx = localStorage.GetNewCharactersIdx();
                if (newCharactersIdx.Length > 0)
                {
                    characters = characters.OrderByDescending((x) => newCharactersIdx.Contains(x.instanceId)).ToList();
                    localStorage.ClearNewCharactersIdx();
                }
            }
            _prevSortType = typeof(TSortType);
            signalBus.Fire(new SortSignal()
            {
                characters = characters,
            });
        }
        
        public List<Character> GetCharactersBySort<TSortType>( ) where TSortType : IDeckSort<Character>, new()
        {

            if( _prevSortType == typeof(TSortType) && _characters.Count == Characters.data.Values.Count )
                return _characters;

            var sortClass = GetSortClass<TSortType>();

            _characters = Characters.data.Values.ToList();
            
            _characters.Sort(sortClass);
            return _characters;
        }

        private IDeckSort<Character> GetSortClass<TSortType>( ) where TSortType : IDeckSort<Character>, new()
        {            
            var sortType = typeof(TSortType);            

            _deckSortClassMap.TryGetValue(sortType, out var sortClass);

            if (null == sortClass)
            {
                sortClass = new TSortType();
                _deckSortClassMap.Add(sortType, sortClass);
            }
            return sortClass;
        }

        public async Task<Character> UpgradeCharacterLevel(string instanceId)
        {            
            string str = String.Format(Web.PATCH_UpgradeApiFormat, instanceId);
            string api = $"{Web.UrlToServer}/{str}";

            try
            {
                var json = await Web.Request(Web.PATCH, api, Web.IdToken);
                return Characters.data[instanceId];
            } 
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        //public async Task<string> GetUpgradesPrice()
        //{
        //    string api = $"{Web.UrlToServer}/{Web.GET_UpgradesPrice}";

        //    try
        //    {
        //        var json = await Web.Request(Web.GET, api, Web.IdToken);
        //        Debug.LogError(json);
        //        return json;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e);
        //        throw;
        //    }
        //}

        public void Scatter( Action callbackForScatter )
        {            
            callbackForScatter();
        }        

    }    
}

#endif
