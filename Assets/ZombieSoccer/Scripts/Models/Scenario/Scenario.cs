#if ZSC_USE_FIREBASE
﻿using Firebase;
using System.Collections.Generic;
using ZombieSoccer.Models;

namespace ZombieSoccer.ApplicationLayer.Data
{

    public class ScenarioModel
    {
        public ScenarioModel(FirebaseApp firebaseApp)
        {
            Cities = new DBStruct<Dictionary<string, City>>( CommonStrings.DB_CitiesPath, firebaseApp);
        }

        public void ApplyRemoteChanges()
        {
            Cities.ApplyRemoteChanges();
        }

        public DBStruct<Dictionary<string, City>> Cities { get; private set; }

    }
}

#endif
