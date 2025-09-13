//using UnityEngine;
//using Zenject;

//namespace ZombieSoccer
//{
//    [CreateAssetMenu(fileName = "ShieldScriptableObjectsInstaller", menuName = "Installers/ShieldScriptableObjectsInstaller")]
//    public class ShieldPalleteScriptableObjectInstaller : ScriptableObjectInstaller<ShieldPalleteScriptableObjectInstaller>
//    {
//        public override void InstallBindings()
//        {
//            Container.Bind<ShieldPalleteScriptableObject>().FromScriptableObjectResource(CommonStrings.PalleteShieldPath).AsSingle().NonLazy();
//        }
//    }
//}
