using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace ZombieSoccer.Localization
{

    public sealed class LocalizationManager : IInitializable
    {
        AsyncOperationHandle _initializeOperation;

        Dictionary<string, Locale> _localesDictionary = new Dictionary<string, Locale>();

        Dictionary<string, LocalizedStringTable> _stringTablesDictionary = new Dictionary<string, LocalizedStringTable>();

        Dictionary<(String, String, Locale), string> _strings = new Dictionary<(String, String, Locale), string>();        

        public void Initialize()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (_initializeOperation.IsDone)
                InitializeCompleted(_initializeOperation);
            else
                _initializeOperation.Completed += InitializeCompleted;
            LocalizeDecorator.LocalizationManager = this;
        }

        void InitializeCompleted( AsyncOperationHandle a)
        {            
            var locales = LocalizationSettings.AvailableLocales.Locales;
            for(int i = 0; i < locales.Count; i++)
            {
                var locale = locales[i];                
                _localesDictionary.Add(locale.name, locale);
            }
        }

        #region LANGUAGE SELECTION METHODS

        public void SelectEnglish()
        {
            SelectLanguage("English (en)");
        }
        public void SelectRussian()
        {
            SelectLanguage("Russian (ru)");
        }
        public void SelectLanguage( string language )
        {
            Locale locale;
            _localesDictionary.TryGetValue(language, out locale);
            LocalizationSettings.SelectedLocale = locale;            
        }

        #endregion

        #region METHODS FOR RETURNING DATA FROM TABLES

        public async Task<string> GetString(string tableName, string keyName)
        {
            string returnString;
            _strings.TryGetValue( (tableName, keyName, LocalizationSettings.SelectedLocale), out returnString);
            
            if (returnString != null)
                return returnString;
            else
                return await FillReceiveData(tableName, keyName);
            
        }
        
        private async Task<string> FillReceiveData(string tableName, string keyName)
        {
            LocalizedStringTable table;
            _stringTablesDictionary.TryGetValue(tableName, out table);
            if (null == table)
            {
                table = new LocalizedStringTable { TableReference = tableName };
                _stringTablesDictionary.Add(tableName, table);
            }
            var stringTable = table.GetTableAsync();
            
            while (!stringTable.IsDone)
                await Task.Yield();
            var entry = stringTable.Result.GetEntry(keyName);
            string returnString = entry.GetLocalizedString();
            
            _strings.Add((tableName, keyName, LocalizationSettings.SelectedLocale), returnString);

            return returnString;
        }
        
        public LocalizedString GetLocalizedString(string tableName, string keyName)
        {
            LocalizedString localizedString = new LocalizedString();
            localizedString.TableReference = tableName;
            localizedString.TableEntryReference = keyName;
            
            return localizedString;
        }

        #endregion        

    }
}
