//using Sirenix.OdinInspector;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using ZombieSoccer.GameLayer.AnimationSystem;
//using ZombieSoccer.GameLayer.Skills;

//namespace ZombieSoccer
//{
//    public class CharacterViewOrderHelper : MonoBehaviour
//    {
//        [Button]
//        public void SetZeroOrder()
//        {
//            transform.parent.SetSiblingIndex(0);
//        }

//        [Button]
//        public void SetLastOrder()
//        {
//            transform.parent.SetSiblingIndex(transform.parent.parent.childCount-1);
//        }

//        [Button]
//        public void OnSkillActivationComplete()
//        {
//            MatchAnimationSystem.Instance.skillIsComplete = true;
//            Debug.LogError($"{transform.parent.name} skill complete");
//            //SetZeroOrder();
//        }

//        public void ContinueAnimation()
//        {
//            GetComponentInParent<CharacterEnergy>().ContinueAnimation();
//        }
//    }
//}
