using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ZombieSoccer
{
    public class StoreItem : MonoBehaviour
    {
        [SerializeField]
        private Button Less, More;
        [SerializeField]
        private TMP_InputField CountLabel;
        public int ResourceCount;

        private void Start()
        {
            Less.onClick.AddListener(DecreaseCount);
            More.onClick.AddListener(IncreaseCount);
            ResourceCount = 1;
            Showcount();
        }

        void Showcount()
        {
            CountLabel.text = ResourceCount.ToString();
        }

        public void IncreaseCount()
        {
            ResourceCount++;
            Showcount();
        }

        public void DecreaseCount()
        {
            if (ResourceCount > 1)
            { 
            ResourceCount--;
            Showcount();
            }
        }
    }
}
