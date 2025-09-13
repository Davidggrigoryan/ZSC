//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Zenject;
//using ZombieSoccer.GameLayer.Matching.Time;

//namespace ZombieSoccer.GameLayer.UI
//{
//    public class FieldBallController : MonoBehaviour
//    {
//        //private TrailRenderer trailRender;

//        public static FieldBallController Instance;

//        private Vector2 startPos, destPos;
//        public Transform centerOfField;

//        public AnimationCurve speedCurve;
//        private Transform parent;

//        public float teleportTime = .5f;

//        public float speed = .5f;

//        [Inject]
//        Timer timer;

//        private void Awake()
//        {
//            parent = transform.parent;
//            Instance = this;
//            //trailRender = GetComponentInChildren<TrailRenderer>();
//            ResetPosition();
//        }

//        public void ResetPosition()
//        {
//            prevCharacterView = null;
//            //trailRender.emitting = false;
//            transform.SetParent(parent);
//            gameObject.SetActive(true);
//            transform.position = centerOfField.position;
//            startPos = centerOfField.position;

//            //trailRender.Clear();
//            //trailRender.emitting = true;

//            start = centerOfField.gameObject;
//        }


//        private GameObject start, end;

//        public void SetStartPosition(Transform tra)
//        {
//            startPos = tra.position;
//            start = tra.gameObject;
//        }

//        Coroutine movementCoroutine;

//        private ICharacterView prevCharacterView;

//        public void SetDestinationPosition(ICharacterView characterView)
//        {
//            return;

//            destPos = characterView.GetTransform().position;
//            end = characterView.GetGameObject();

//            var dir = Mathf.Clamp((characterView.GetTransform().position - transform.position).x, -1f, 1f);

//            prevCharacterView?.GetGameObject().GetComponent<CharacterViewVisualFX>().Close();
//            characterView?.GetGameObject().GetComponent<CharacterViewVisualFX>().Open();

//            if (movementCoroutine != null)
//                StopCoroutine(movementCoroutine);

//            movementCoroutine = StartCoroutine(Animate());

//            prevCharacterView = characterView;
//        }


//        IEnumerator Animate(float mul = 1f)
//        {
//            //Debug.LogError($"Move: {start.name} -> {end.name}");
//            var s = srcCharacterView.position;
//            var d = destCharacterView.position;
//            float t = 0f;

//            srcCharacterView.GetComponent<ICharacterView>()?.characterViewVisualFX.Close();
//            destCharacterView.GetComponent<ICharacterView>()?.characterViewVisualFX.Open();

//            while (t <= timer.TimeMultiply() / speed * mul)
//            {
//                var curveValue = speedCurve.Evaluate(t / timer.TimeMultiply() * speed * mul);

//                transform.position = Vector2.Lerp(s, d, t / timer.TimeMultiply() * speed * mul * curveValue);
//                yield return new WaitForEndOfFrame();
//                t += timer.DeltaTime / timer.TimeMultiply() * speed * mul;
//            }
//            //Debug.LogError("STOP");
//            srcCharacterView = destCharacterView;
//            destCharacterView = null;
//        }

//        public static Transform srcCharacterView, destCharacterView;

//        public static void SetSrcView(Transform value)
//        {
//            srcCharacterView = value;
//        }

//        public static void SetDestView(Transform value)
//        {
//            destCharacterView = value;
//        }

//        public static Coroutine Move(float value = 1f)
//        {
//            return Instance.StartCoroutine(Instance.Animate(value));
//        }

//        public static Coroutine Teleport()
//        {
//            return Instance.StartCoroutine(Instance.AnimateTeleport());
//        }

//        IEnumerator AnimateTeleport()
//        {
//            //Debug.LogError($"Move: {start.name} -> {end.name}");
//            var s = srcCharacterView.position;
//            var d = destCharacterView.position;
//            float t = 0f;

//            srcCharacterView.GetComponent<ICharacterView>()?.characterViewVisualFX.Close();
//            destCharacterView.GetComponent<ICharacterView>()?.characterViewVisualFX.Open();
//            transform.position = s;
//            yield return new WaitForSeconds(teleportTime * timer.TimeMultiply());
//            transform.position = d;

//           srcCharacterView = destCharacterView;
//            destCharacterView = null;
//        }
//    }
//}
