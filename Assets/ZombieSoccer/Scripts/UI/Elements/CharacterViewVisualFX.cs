//using Sirenix.OdinInspector;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Zenject;
//using ZombieSoccer.GameLayer.Characters;
//using ZombieSoccer.GameLayer.Matching.Time;
//using ZombieSoccer.GameLayer.Skills;
////using ZombieSoccer.GameLayer.States;
////using ZombieSoccer.GameLayer.VisualFX;

//namespace ZombieSoccer.GameLayer.UI
//{
//    public class CharacterViewVisualFX : MonoBehaviour
//    {
//        //private CharacterView characterView;

//        //private Coroutine animationCoroutine;

//        //[Inject]
//        //private Matching.Settings.MatchSettings matchSettings;

//        //[Inject]
//        //private HightlightCharactersManager hightlighter;

//        //public float scale = 1.5f;

//        //private Vector3 localScale;

//        //private void Start()
//        //{
//        //    localScale = transform.localScale;
//        //    characterView = GetComponent<CharacterView>();
//        //    //hightlighter.OnHightlightCharacterViewEvent += OnHightlightCharacterViewEventHandler;


//        //    // Init movement
//        //    startPosition = defaultPosition = transform.localPosition;
//        //    movementSign = (Random.Range(0, 2) == 0);
//        //    targetPosition = defaultPosition + Vector2.up * Random.Range(0f, movementDelta) * ((movementSign)? 1f : -1f);
//        //    currentSpeed = Random.Range(movementSpeed.x, movementSpeed.y);
//        //}

//        //private void OnEnable()
//        //{
//        //    Match.OnStartMatchEvent += OnStartMatchEventHandler;
//        //}

//        //private bool canMove = false;
//        //private void OnStartMatchEventHandler()
//        //{
//        //    canMove = true;
//        //}

//        //private void OnDisable()
//        //{
//        //    Match.OnStartMatchEvent -= OnStartMatchEventHandler;
//        //}

//        //private Vector2 targetPosition;
//        //private Vector2 defaultPosition;
//        //private Vector2 startPosition;

//        //private bool movementSign;

//        //public float movementDelta = 50f;
//        //public Vector2 movementSpeed = new Vector2(1f, 1.5f);
//        //private float currentSpeed;
//        //private float lerpValue = 0f;

//        //private void Update()
//        //{
//        //    if (!canMove)
//        //        return;

//        //    if (Vector2.Distance(targetPosition, transform.localPosition) > 1f)
//        //    {
//        //        transform.localPosition = Vector2.Lerp(startPosition, targetPosition, lerpValue * currentSpeed);
//        //        lerpValue += Time.deltaTime;
//        //    }
//        //    else
//        //    {
//        //        movementSign = !movementSign;
//        //        startPosition = transform.localPosition;
//        //        lerpValue = 0f;
//        //        targetPosition = defaultPosition + Vector2.up * Random.Range(0f, movementDelta) * ((movementSign) ? 1f : -1f);
//        //        currentSpeed = Random.Range(movementSpeed.x, movementSpeed.y);
//        //    }
//        //}


//        #region Skills
//        public GameObject skillGreenFXPrefab, skillRedFXPrefab;

//        public Transform skillsTargetTransform;

//        Dictionary<SkillAttributeModifyView, GameObject> activeSkills = new Dictionary<SkillAttributeModifyView, GameObject>();
//        public GameObject ActivateSkillFX(Skill _skill, SkillAttributeModifyView skillAttributeModifyView, bool teamColor)
//        {
//            //var targetPrefab = (_skill.value > 0) ? skillGreenFXPrefab : skillRedFXPrefab;
//            var targetPrefab = (teamColor) ? skillGreenFXPrefab : skillRedFXPrefab;

//            var obj = Instantiate(targetPrefab, this.transform);

//            obj.transform.SetParent(skillsTargetTransform);
//            //obj.transform.localPosition = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

//            activeSkills.Add(skillAttributeModifyView, obj);
//            return obj;
//        }

//        public void RemoveSkillFX(Skill _skill, SkillAttributeModifyView skillAttributeModifyView)
//        {
//            var obj = activeSkills[skillAttributeModifyView];
//            activeSkills.Remove(skillAttributeModifyView);
//            DestroyImmediate(obj);
//        }
//        #endregion

//        #region Cards animations


//        [SerializeField]
//        private Animator currentAnimator;

//        [SerializeField]
//        private Animation currentAnimation;

//        [SerializeField]
//        private float defaultRotationSpeed = 1.5f;
//        private Quaternion defaultRotation;

//        [SerializeField]
//        private GameObject animationRoot;

//        public void Setup()
//        {
//            currentAnimator = GetComponentInChildren<Animator>();
//            currentAnimation = GetComponentInChildren<Animation>();

//            //transform.Rotate(Vector3.up, 180f);
//            defaultRotation = transform.rotation;
//            //animationRoot.SetActive(false);
//        }


//        public bool isOpen = false;

//        private static event System.Action ForceCloseAllEvent;
//        public static void ForceCloseAll()
//        {
//            ForceCloseAllEvent?.Invoke();
//        }

//        private void OnEnable()
//        {
//            ForceCloseAllEvent += Close;
//        }

//        private void OnDisable()
//        {
//            ForceCloseAllEvent -= Close;
//        }


//        [Button]
//        public void Open()
//        {
//            if (isOpen)
//                return;

//            currentAnimator.SetTrigger("Open");
//            isOpen = true;
//        }

//        [Button]
//        public void Close()
//        {
//            if (!currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleClose"))
//            {
//                isOpen = false;
//                currentAnimator.SetTrigger("Close");
//                return;
//            }

//            if (!isOpen)
//                return;

//            isOpen = false;
//            currentAnimator.SetTrigger("Close");
//        }

//        [Button]
//        public void Dribbling()
//        {
//            currentAnimator.SetTrigger("Dribbling");
//        }

//        [Button]
//        public void ActivateSkill()
//        {
//            currentAnimator.SetTrigger("ActivateSkill");
//        }


//        [Button]
//        public void Kick()
//        {
//            currentAnimator.SetTrigger("Kick");
//        }

//        [Inject]
//        public Timer timer;
//        #endregion
//    }
//}
