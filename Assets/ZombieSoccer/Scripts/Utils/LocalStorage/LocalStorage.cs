using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Utils.LocalStorage
{
    public class LocalStorage// : IInitializable
    {
        #region Constants

        const string c_NewCharactersIdx = "NewCharactersIdx";

        public bool areChanging = false;
        #endregion

        #region Methods for identification new characters 

        /// <summary>
        /// Identifiers of new characters are stored in a string variable separated by a space
        ///     'aaa bbb' -> []{'aaa', 'bbb'}
        /// </summary>        

        public String[] GetNewCharactersIdx()
        {            
            string str = PlayerPrefs.GetString(c_NewCharactersIdx);
            if (String.IsNullOrEmpty(str))
                return new string[0];
            String[] array = str.Split(' ');

            areChanging = false;
            return array;
        }        

        public void AddNewCharacterIdx( string idx )
        {            
            string str = PlayerPrefs.GetString(c_NewCharactersIdx);
            if (String.IsNullOrEmpty(str))
                str = idx;
            else
                str = $"{str} {idx}";
            PlayerPrefs.SetString(c_NewCharactersIdx, str);
            PlayerPrefs.Save();
            
            areChanging = true;
        }

        public void SetNewCharactersIdx( string[] newCharactersIdx)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string str in newCharactersIdx)
            {
                sb.Append(str);
                sb.Append(" ");
            }
            PlayerPrefs.SetString(c_NewCharactersIdx, sb.ToString() );
            PlayerPrefs.Save();

            areChanging = true;
        }

        public void ClearNewCharactersIdx()
        {
            PlayerPrefs.DeleteKey(c_NewCharactersIdx);

            areChanging = false;
        }

        #endregion

    }
}
