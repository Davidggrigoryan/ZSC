using System;
using UnityEngine;
using Zenject;
using ZombieSoccer.Models.TeamM;
using ZombieSoccer.UI.DragAndDrop;

namespace ZombieSoccer.GameLayer.UI
{
    public class CharacterPositionPlaceholder : MonoBehaviour, IDragAndDrop<DetailCharacterView>, IPoolable<IMemoryPool>, IDisposable
    {
        [Inject]
        public LazyInject<TeamsGroupModel> teamsGroupModel;

        [Inject]
        public SignalBus signalBus;

        [SerializeField]
        private GameObject placeholderGraphicsGameObject;

        private DetailCharacterView _character;

        public UIMeshRenderer top;
        public int teamIndex { get; private set; }

        public bool IsBusy { get; private set; }

        public int Index { get; set; }

        IMemoryPool _memoryPool;
        
        private void Start()
        {
            top.GenerateMesh();
        }

        public DetailCharacterView GetObjectFromTarget()
        {
            return _character;
        }

        public void ResetTarget( )
        {
            placeholderGraphicsGameObject.SetActive(true);
            IsBusy = false;            
            //_character = null;
        }

        public void SetObjectToTarget(DetailCharacterView obj)
        {            
            _character = obj;
            _character.transform.SetParent(transform);
            _character.transform.localPosition = Vector3.zero;
            _character.transform.localScale = Vector3.one;
            placeholderGraphicsGameObject.SetActive(false);
            IsBusy = true;
        }

        public void OnDespawned()
        {
            _memoryPool = null;
        }

        public void OnSpawned(IMemoryPool memoryPool)
        {
            _memoryPool = memoryPool;
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            placeholderGraphicsGameObject.SetActive(true);
        }

        public void Dispose()
        {
            if(_memoryPool is null) return;
            _memoryPool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<CharacterPositionPlaceholder>
        {
        }
    }
}
