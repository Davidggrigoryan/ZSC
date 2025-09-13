using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.UI;
using ZombieSoccer.UI.Presenters;

namespace ZombieSoccer
{
    public class PageOfMergeResult : BasePagePresenter
    {        

        [Inject]
        ResourcesManager resourcesManager;

        [Inject]
        CharacterAttributes rarityBackground;

        [Inject]
        CharacterAttributes rarityElements;

        [Inject]
        PageManager pageManager;

        [SerializeField] Image Background;

        [SerializeField] GameObject FullBody;

        [SerializeField] Image RarityImage;

        [SerializeField] TextMeshProUGUI CharacterName;

        ReactiveProperty<Character> _reactiveCharacter = new ReactiveProperty<Character>();

        protected override void Inititalize()
        {
            base.Inititalize();

            _reactiveCharacter.Where( e => e != null ).Subscribe( async character =>
            {
                FullBody.GetComponent<Image>().sprite = resourcesManager.GetCharacterSprites(character).FullBody;
                rarityBackground.UpdateBackgroundOnRarity(Background, (int)character.rarity);
                rarityElements.UpdateIconOnRarity(RarityImage, (int)character.rarity);
                CharacterName.text = character.characterName;

                await Task.Delay(1000);
                pageManager.Fire("UIView_CharacterUpdateRarity", (System.Object)_reactiveCharacter.Value);
            });
        }

        protected override void Enable()
        {
            base.Enable();

            _reactiveCharacter.Value = (Character)GetComponentInChildren<PageSignalHandler>()?.args?[0];           
        }
    }
}
