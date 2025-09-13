using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ZombieSoccer.UI.Views
{
    public class ViewStore : MonoBehaviour
    {
        [SerializeField]
        private GameObject OffersPanel, DustPanel, TabPanel;
        [SerializeField]
        private Button OffersButton, DustButton, TabButton;
        [SerializeField]
        private GameObject BuyButton;
        [SerializeField]
        private GameObject ButtonPanel;
        [SerializeField]
        private GameObject[] OffersPoints;
        [SerializeField]
        private TMP_FontAsset White, Yellow;

        private void Start()
        {
            OffersButton.onClick.AddListener(OffersPanelShow);
            DustButton.onClick.AddListener(DustPanelShow);
            TabButton.onClick.AddListener(TabPanelShow);
        }

        private void OffersPanelShow()
        {
            PanelShow(OffersPanel, OffersButton, new GameObject[] {DustPanel, TabPanel}, new Button [] { DustButton, TabButton });
            foreach (GameObject _poin in OffersPoints)
            {
                _poin.SetActive(true);
            }
        }

        private void DustPanelShow()
        {
            PanelShow(DustPanel, DustButton, new GameObject[] { OffersPanel, TabPanel }, new Button[] { TabButton, OffersButton });
            foreach (GameObject _poin in OffersPoints)
            {
                _poin.SetActive(false);
            }
        }

        private void TabPanelShow()
        {
            PanelShow(TabPanel, TabButton, new GameObject[] { OffersPanel, DustPanel }, new Button[] { DustButton, OffersButton });
            foreach (GameObject _poin in OffersPoints)
            {
                _poin.SetActive(false);
            }
        }

        void PanelShow(GameObject ActivePanel, Button ActiveButton, GameObject[] OtherPanels, Button[] OtherButtons)
        {
            foreach (GameObject Panel in OtherPanels)
            {
                Panel.SetActive(false);
            }

            foreach (Button _button in OtherButtons)
            {
                _button.transform.GetChild(0).gameObject.SetActive(false);
                _button.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().font = White;
            }

            ActivePanel.SetActive(true);
            ActiveButton.transform.GetChild(0).gameObject.SetActive(true);
            ActiveButton.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().font = Yellow;
        }
    }
}
