using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;

namespace ZombieSoccer
{
    public class VFXPreviewHalperEditor : MonoBehaviour
    {
        public PlayableDirector director;
        [Button]
        void SetupSpeed(float speed)
        {
            for (int i = 0; i < director.playableGraph.GetOutputCount(); i++)
            {
                director.playableGraph.GetOutput(i).GetSourcePlayable().SetSpeed(speed);
            }
            //director.Play();
        }

        [Button]
        void Play(float speed)
        {
            director.Play();
            //play = true;
        }

        //[Button]
        //void SetRootSpeed(float speed)
        //{
        //    director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        //}

        public bool play;
        //private void LateUpdate()
        //{
        //    if (play)
        //    {
        //        director.playableGraph.Evaluate(Time.deltaTime);
        //    }
        //}

        void OnEnable()
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            if (director == aDirector)
                Debug.LogError("PlayableDirector named " + aDirector.name + " is now stopped.");
        }

        void OnDisable()
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
        //public int currentIndex = 0;
        //public bool defaultLoop = false;

        //private void Awake()
        //{
        //    gameObject.GetComponentsInChildren<ParticleSystem>().ToList().ForEach(x => x.loop = defaultLoop);
        //}

        //[Button]
        //public void PlayNext(bool loop = false)
        //{
        //    if (currentIndex == transform.childCount)
        //        currentIndex = 0;

        //    var particle = transform.GetComponentsInChildren<UnityEngine.UI.Extensions.UIParticleSystem>()[currentIndex];
        //    particle.gameObject.GetComponent<ParticleSystem>().loop = loop;

        //    Debug.LogError(particle.gameObject.name);
        //    particle.StartParticleEmission();
        //    currentIndex++;
        //}
    }
}
