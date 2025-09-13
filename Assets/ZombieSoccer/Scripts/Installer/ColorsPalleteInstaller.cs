//using UnityEngine;
//using Zenject;

//namespace ZombieSoccer.Utilities
//{
//    [CreateAssetMenu(fileName = "ColorsPalleteScriptableObjectsInstaller", menuName = "Installers/ColorsPalleteScriptableObjectsInstaller")]
//    public class ColorsPalleteInstaller : ScriptableObjectInstaller<ColorsPalleteInstaller>
//    {
//        public override void InstallBindings()
//        {
//            Container.Bind<ColorsPallete>().FromScriptableObjectResource(CommonStrings.PalleteColorsPath).AsSingle().NonLazy();
//        }
//    }
//}