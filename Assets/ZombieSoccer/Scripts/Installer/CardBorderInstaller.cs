using UnityEngine;
using Zenject;
using ZombieSoccer;

namespace ZombieSoccer.Utilities
{
    [CreateAssetMenu(fileName = "CardBordersInstaller", menuName = "Installers/CardBordersInstaller")]
    public class CardBorderInstaller : ScriptableObjectInstaller<CardBorderInstaller>
    {
        public override void InstallBindings()
        {            
            Container.Bind<CardBorder>().FromScriptableObjectResource(CommonStrings.CardBordersPath).AsSingle().NonLazy();
        }
    }
}