
using System;
using System.Collections.Generic;
using UnityEngine;
using ZombieSoccer.UI.Widget;

namespace ZombieSoccer

{
    public class StoreItemScrolling : MonoBehaviour
    {
        [SerializeField]
        private int panCount = 10;
        [Range(0, 500)]
        public int panOffset;
        [Range(0f, 20f)]
        public float snapSpeed;
        [Range(0f, 50f)]
        public float scaleOffset;
        [Range(1f, 20f)]
        public float scaleSpeed;
        [Header("Other Objects")]
        public GameObject panPrefab;

        
        private GameObject[] instPans;
        private Vector2[] pansPos;
        private Vector2[] pansScale;
        private float panSizeX;
        private float panSizeY;

        private RectTransform contentRect;
        private Vector2 contentVector;

        public int selectPanID;
        private bool InScrolling;
        public float distance;
        public float nearestPos;
        public int StartIndex;
        public float j;

        private float currentAnchored = 0f;
        private float targetAnchored => currentAnchored;
        
        private List<WidgetWallet> l = new List<WidgetWallet>();
        
        private void Start()
        {
            Debug.Log("START STORE");
            panSizeX = panPrefab.GetComponent<RectTransform>().sizeDelta.x;
            panSizeY = panPrefab.GetComponent<RectTransform>().sizeDelta.y;
            SpawnPanels();
        }

        private float sizeDeltaX, sizeDeltaY;
        private Vector2 startPositionForSmallComponents = new Vector2(303f, 0f);
        private const int LEFT_SIDE = -1,
                          CENTER_SIDE = 0,
                          RIGHT_SIDE = 1,
                          TO_NEXT_LINE = 1,
                          CURRENT_LINE = 0;
        
        public void SpawnPanels()
        {
            Debug.Log("INIT STORE ");
            Init();
            instPans = new GameObject[panCount];
            InitFirstTwoComponents();
            
            float offsetOnY = 230;
            int stepOnNextLine, positionXSign = -1;
            float y = instPans[0].transform.localPosition.y ;
            float scale = 1;
            float sizeDelta2_16__ = sizeDeltaY * 1.6f / 2f;
            float sizeDelta2_1__ = sizeDeltaY / 2f;
            for (int i = 2; i < panCount; i++)
            {
                scale = i <= 4 ? sizeDelta2_16__ : sizeDelta2_1__;
                stepOnNextLine = ( 1 + i ) % 3 == 0 ?
                    TO_NEXT_LINE : CURRENT_LINE ;
                positionXSign = TO_NEXT_LINE == stepOnNextLine ?
                    LEFT_SIDE
                    :
                    positionXSign == LEFT_SIDE ?
                        CENTER_SIDE : RIGHT_SIDE;
                y -= TO_NEXT_LINE == stepOnNextLine ? 
                    scale + offsetOnY : 0;
                
                InitSmallComponent( i );
                
                instPans[i].transform.localPosition = new Vector3( 
                    positionXSign * startPositionForSmallComponents.x, 
                    y, 0 );
            }
        }
        
        private void InitSmallComponent( int index )
        {
            instPans[index] = Instantiate(panPrefab);
            instPans[index].transform.SetParent( contentRect );
            instPans[index].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            
            
        }
        
        private void InitFirstTwoComponents()
        {
            float startPositionOnX = 223f;
            float startPositionOnY = 261f;
            for (int i = 0; i < 2; i++)
            {
                instPans[i] = Instantiate(panPrefab);
                instPans[i].transform.SetParent( contentRect );
                instPans[i].transform.localScale = new Vector3(1.6f, 1.6f, 1f);
                instPans[i].transform.localPosition = new Vector3(Mathf.Pow(-1, 1 + i ) * startPositionOnX, startPositionOnY, 0);
            }
        }
        
        private void Init()
        {
            contentRect = GetComponent<RectTransform>();
            sizeDeltaX = panPrefab.GetComponent<RectTransform>().sizeDelta.x;
            sizeDeltaY = panPrefab.GetComponent<RectTransform>().sizeDelta.y;
        }

        private const float UpGround = 0f;
       private void Update()
       {
           float MaxGround = (sizeDeltaY * 1.6f) + ((panCount - 2) / 3) * sizeDeltaY ;
           float speed = 2.5f;
           float smooth = 1 - Mathf.Pow(0.5f, Time.deltaTime * speed);

           currentAnchored = contentRect.anchoredPosition.y;
            
           if (currentAnchored < UpGround)
           {
               currentAnchored = Mathf.Lerp(currentAnchored, UpGround, smooth * 100f );
           }else if (currentAnchored > MaxGround)
           {
               currentAnchored = Mathf.Lerp(currentAnchored, MaxGround, smooth * 100f );
           }
           Debug.Log("current anchored " + currentAnchored);
           Debug.Log("MAX GROUND " + MaxGround);
           contentRect.anchoredPosition = new Vector2(0f, currentAnchored);
           /* if (InScrolling == true)
            { 
            nearestPos = float.MaxValue;
                for (int i = 0; i < panCount; i++)
                {
                    distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
                    if (distance < nearestPos)
                    {
                        nearestPos = distance;
                        selectPanID = i;

                    }
                    float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
                    pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
                    pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
                    instPans[i].transform.localScale = new Vector3(pansScale[i].x, pansScale[i].y, 1);
                }
            }


            if (InScrolling == true) return;
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectPanID].x, snapSpeed * Time.fixedDeltaTime);
            contentRect.anchoredPosition = contentVector ;*/
       }

        public void Scrolling(bool scroll)
        {
            InScrolling = scroll;
        }
    }

}

