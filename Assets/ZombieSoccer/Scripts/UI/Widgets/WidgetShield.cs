using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;

namespace ZombieSoccer.UI.Widget
{
    public class WidgetShield : WidgetBase
    {
        [Inject]
        private UserModel user { get; }

        [Inject]
        protected ShieldPalleteScriptableObject shieldPallete { get; }

        [SerializeField]
        protected TMPro.TextMeshProUGUI m_userName;

        [SerializeField]
        protected Image shieldImageTarget, maskImageTarget, detailsImageTarget, tapeImageTarget, ballImageTarget;

        [SerializeField]
        protected float[] tapePositions = new float[9] { -28f, -24f, -45f, -52f, -34f, -14f, -26f, -38f, -38f };

        [SerializeField]
        protected float[] namePositionsY = new float[9] { 7f, -11f, 35f, 38f, 9f, -6f, 7f, 15f, 9f };

        public override void Enable()
        {
            Debug.Log("[Shield]");
            user.Shield.OnDataChangedAction += UpdateData;
            user.Info.OnDataChangedAction += UpdateData;

            UpdateData();
        }

        public override void Disable()
        {
            user.Shield.OnDataChangedAction -= UpdateData;
            user.Info.OnDataChangedAction -= UpdateData;
        }

        public void UpdateData()
        {
            //Debug.LogError("[Widget.Shield] UpdateData");

            var info = (user.Info.newData == null) ? user.Info.data : user.Info.newData;
            var shild = (user.Shield.newData == null) ? user.Shield.data : user.Shield.newData;

            Preview(shild.BlazonIndex, shild.DetailIndex, shild.TapeIndex, shild.BallIndex, info.NickName);
        }

        public void Preview(int blazonIndex, int detaileIndex, int tapeIndex, int ballIndex, string nickName)
        {
            if (!string.IsNullOrEmpty(nickName))
                m_userName.text = nickName;

            maskImageTarget.sprite = shieldPallete.shieldsSprites[blazonIndex];
            shieldImageTarget.sprite = shieldPallete.shieldsSprites[blazonIndex];
            detailsImageTarget.sprite = shieldPallete.detailsSprites[detaileIndex];
            tapeImageTarget.sprite = shieldPallete.tapesSprites[tapeIndex];
            ballImageTarget.sprite = shieldPallete.ballsSprites[ballIndex];

            // TODO FUCKING FUCK
            tapeImageTarget.GetComponent<RectTransform>().pivot = tapeImageTarget.sprite.pivot / tapeImageTarget.sprite.rect.size;
            tapeImageTarget.GetComponent<RectTransform>().sizeDelta = new Vector2(shieldPallete.tapesSprites[tapeIndex].rect.size.y / 4, shieldPallete.tapesSprites[tapeIndex].rect.size.y);
            tapeImageTarget.GetComponent<RectTransform>().localPosition = new Vector2(0, tapePositions[tapeIndex]);
            m_userName.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(0, namePositionsY[tapeIndex]);
            tapeImageTarget.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.7f, 1f);
        }
    }
}
