using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AIStateManager : MonoBehaviour
    {
        [Space(10)]
        public GameObject activeModel;

        [Space(10)]
        public AIManager ai;

        [Header("Current Player")]
        public StateManagerVariables currentPlayerStates;

        [Header("Current AI_State")]
        public AI_State currentState;

        [Header("Delta and mTransform")]
        public float delta;
        public Transform mTransform;

        [Header("Patrol Points")]
        public int targetAIPartrolPointListId;
        public bool isPatrolPointListInit;
        public AI_PatrolPointList currentPartrolPointList;

        [Header("Boolean")]
        public bool IsPatrolInited;
        public bool isDead;
        public bool isAggro;
        public bool isFacedPlayer;

        [Header("Float")]
        public float aggroTransitionWaitTimer;

        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public Collider enemyCollider;
        [HideInInspector] public Animator anim;
        [HideInInspector] public AI_AnimatorHook a_hook;

        [HideInInspector] public readonly Vector3 vector3Zero = new Vector3(0, 0, 0);
        [HideInInspector] public readonly Vector3 vector3Up = new Vector3(0, 1, 0);
        [HideInInspector] public readonly Vector3 vector3Right = new Vector3(1, 0, 0);

        [Header("Material")]
        public Renderer eRender;

        public void Awake()
        {
            mTransform = this.transform;

            SetupAnimator();

            SetupHealth();

            SetupAIManager();
        }

        public void Start()
        {
            Init();
        }

        public void Update()
        {
            delta = Time.deltaTime;
            
            if(!isPatrolPointListInit)
                currentPartrolPointList = GetAIPatrolPointList();

            if (currentState != null)
            {
                currentState.Tick(this);

                if(currentState.forwardTransition != null)
                {
                    currentState.forwardTransition.CheckAI_Transition(this);
                }
            }
        }

        public void FixedUpdate()
        {
            if (currentState != null)
                currentState.FixedTick(this);
        }

        public void Init()
        {
            SetupRigidbody();

            SetupCollider();

            SetupAniamtorHook();

            eRender = GetComponent<Renderer>();
        }

        public void SetupAnimator()
        {
            if(activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            anim.applyRootMotion = false;
        }

        public void SetupHealth()
        {
            isDead = false;


        }

        public void SetupAIManager()
        {
            ai = GetComponent<AIManager>();
            ai.aiStates = this;
        }

        public void SetupRigidbody()
        {
            rb = GetComponent<Rigidbody>();
            rb.angularDrag = 999;
            rb.drag = 4;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        public void SetupCollider()
        {
            enemyCollider = GetComponent<Collider>();
        }

        public void SetupAniamtorHook()
        {
            a_hook = activeModel.GetComponent<AI_AnimatorHook>();
            if(a_hook == null)
            {
                a_hook = activeModel.AddComponent<AI_AnimatorHook>();
            }
        }

        public AI_PatrolPointList GetAIPatrolPointList()
        {
            List<AI_PatrolPointList> onSceneAIPatrolPoints = AI_PatrolPointManager.singleton.onScenePatrolPointsList;
            int onSceneAIPatrolPointsCount = onSceneAIPatrolPoints.Count;
            for (int i = 0; i < onSceneAIPatrolPointsCount; i++)
            {
                if(targetAIPartrolPointListId == onSceneAIPatrolPoints[i].patrolPointListId)
                {
                    isPatrolPointListInit = true;
                    return onSceneAIPatrolPoints[i];
                }
            }

            isPatrolPointListInit = true;
            Debug.Log("No target Patrol Point List can be found!");
            return null;
        }
    }
}