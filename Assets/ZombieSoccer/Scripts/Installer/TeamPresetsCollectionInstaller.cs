//using UnityEngine;
//using Zenject;
//using ZombieSoccer;

//[CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjectsInstaller")]
//public class TeamPresetsCollectionInstaller : ScriptableObjectInstaller<TeamPresetsCollectionInstaller>
//{
//    public override void InstallBindings()
//    {
//        Container.Bind<TeamPositionsPresetsCollection>().FromScriptableObjectResource(CommonStrings.PalleteTeamPositionsPath).AsSingle().NonLazy();
//    }
//}