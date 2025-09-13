using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace ZombieSoccer.GameLayer.Flow
{
    public class PinsAnimation : MonoBehaviour
    {
        [Button]
        public void TestAnimate()
        {
            AnimatePins(gameObject.GetComponentInChildren<GameLocation>().pins.Select(p => p.transform).ToList());
        }

        [Button]
        public void StopAnimation()
        {
            StopAllCoroutines();
        }

        public void AnimatePins(List<Transform> pins)
        {
            foreach (var p in pins)
            {
                p.gameObject.SetActive(false);
                StartCoroutine(AnimatePins(p, pins.IndexOf(p)));
            }
        }

        public void AnimateCurrentMatchPin(Transform _currentMatchPin)
        {
            //StartCoroutine(AnimateCurrentMatchPinIenum(currentMatchPin));
            //var animator = currentMatchPin.gameObject.AddComponent<Animator>();
            //var timelineAsset = (TimelineAsset)timeline.playableAsset;
            ////var track = (TrackAsset)timelineAsset.outputs.ToArray()[0].sourceObject;
            //timeline.SetGenericBinding(timelineAsset.outputs.ToArray()[0].sourceObject, currentMatchPin.gameObject);

            ////var animationOutput = (AnimationPlayableOutput)timeline.playableGraph.GetOutput(0);
            ////animationOutput.SetTarget(animator);
            currentMatchPin.SetParent(_currentMatchPin, false);
            _currentMatchPin.GetComponent<Image>().enabled = false;

            _currentMatchPin.name = "Current";
            _currentMatchPin.gameObject.SetActive(true);
            currentMatchPin.gameObject.SetActive(true);
            currentMatchPin.GetComponent<Image>().enabled = true;
            currentMatchPin.GetComponent<Animator>().enabled = true;
            timeline.Play();
        }

        public AnimationCurve onStartAnimationCurve;
        public float pinsAnimationsBeetwenTimeOffset, pinsAnimationsStartTimeOffset, pinAnimationTime;
        public float animationStartPosition;

        public PlayableDirector timeline;
        public Transform currentMatchPin;

        IEnumerator AnimatePins(Transform p, int index)
        {
            Time.timeScale = 1f;
            yield return new WaitForSeconds(pinsAnimationsStartTimeOffset + (float)index * pinsAnimationsBeetwenTimeOffset);
            p.gameObject.SetActive(true);
            var t = 0f;
            var originalPos = p.localPosition;
            var startAnimPos = originalPos + Vector3.up * animationStartPosition;

            while (t < pinAnimationTime)
            {
                var cu = onStartAnimationCurve.Evaluate(t / pinAnimationTime);
                p.localPosition = Vector3.Lerp(startAnimPos, originalPos, t / pinAnimationTime * cu);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
