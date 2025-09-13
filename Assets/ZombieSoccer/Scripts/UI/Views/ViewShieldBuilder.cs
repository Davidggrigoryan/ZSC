using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.UI.Presenters;
using ZombieSoccer.UI.Widget;

namespace ZombieSoccer.UI.Views
{
    public class ViewShieldBuilder : BasePagePresenter
    {
        [Inject]
        UserModel user;

        [Inject]
        ShieldPalleteScriptableObject shieldPallete;

        [Inject]
        PageManager pageManager;

        [FoldoutGroup("Shield's elements")] [SerializeField]
        SnapScrolling blazonScroll, tapeScroll, detailScroll, ballScroll;

        [SerializeField]
        Button applyButton;

        [SerializeField]
        WidgetShield widgetShield;

        protected override void Inititalize()
        {
            base.Inititalize();

            ballScroll.ShieldSprites = shieldPallete.ballsSprites;
            tapeScroll.ShieldSprites = shieldPallete.tapesSprites;
            detailScroll.ShieldSprites = shieldPallete.detailsSprites;
            blazonScroll.ShieldSprites = shieldPallete.shieldsSprites;

            ballScroll.SpawnPanels(user.Shield.data.BallIndex, Preview);
            tapeScroll.SpawnPanels(user.Shield.data.TapeIndex, Preview);
            detailScroll.SpawnPanels(user.Shield.data.DetailIndex, Preview);
            blazonScroll.SpawnPanels(user.Shield.data.BlazonIndex, Preview);
        }

        protected override void Enable()
        {
            ballScroll.InitIndx(user.Shield.newData.BallIndex);
            tapeScroll.InitIndx(user.Shield.newData.TapeIndex);
            detailScroll.InitIndx(user.Shield.newData.DetailIndex);
            blazonScroll.InitIndx(user.Shield.newData.BlazonIndex);

            applyButton.OnClickAsObservable().Subscribe(_ => {
                OnApplyShield();
                pageManager.Fire("UIView_MainMenu");
            }
            ).AddTo(disposables);
        }

        private void OnApplyShield()
        {
            user.Shield.data.BlazonIndex = blazonScroll.CurrentSpriteIndex;
            user.Shield.data.BallIndex = ballScroll.CurrentSpriteIndex;
            user.Shield.data.TapeIndex = tapeScroll.CurrentSpriteIndex;
            user.Shield.data.DetailIndex = detailScroll.CurrentSpriteIndex;
            user.Shield.PushData();
            Preview();
        }
                
        private void Preview()
        {
            widgetShield.Preview(
                blazonScroll.CurrentSpriteIndex,
                detailScroll.CurrentSpriteIndex,
                tapeScroll.CurrentSpriteIndex,
                ballScroll.CurrentSpriteIndex,
                string.Empty);
        }
    }
}
