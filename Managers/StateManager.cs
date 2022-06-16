using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        [Header("StateManagerVariable")]
        public StateManagerVariables stateManagerVariable;

        [Header("Current Enemy")]
        public AIStateManager currentEnemyStates;

        [Header("Current State")]
        public State currentState;

        [Header("Boolean")]
        public bool isDiscovered;
        public bool isFacedEnemy;
        public bool isBattleStarted;

        [Header("References")]
        public NavMeshAgent agent;

        [Header("Player Stats")]
        public PlayerStatsManager playerStatsManager;

        [Header("Non Editable")]
        public float moveAmount;
        public float delta;
        public Vector3 moveDir;
        public Vector3 destinationPosition;

        private const float navMeshSampleDistance = 4f;

        [HideInInspector] public InputHandler inp;
        [HideInInspector] public Transform mTransform;
        [HideInInspector] public CharacterController controllerComponent;

        [HideInInspector] public readonly Vector3 vector3Zero = new Vector3(0, 0, 0);
        [HideInInspector] public Vector2 vector2Zero = new Vector2(0, 0);
        [HideInInspector] public Vector2 vector2Up = new Vector2(0, 1);

        public static StateManager singleton;
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
            }
            else if(singleton != this)
            {
                Destroy(this);
            }
        }

        public void Init()
        {
            mTransform = gameObject.transform;

            gameObject.layer = 8;

            SetupCharacterController();

            SetupStateManagerVariable();

            SetupAgent();

            playerStatsManager.Init();
        }

        public void SetupStateManagerVariable()
        {
            stateManagerVariable.value = this;
        }

        public void SetupCharacterController()
        {
            controllerComponent = GetComponent<CharacterController>();
        }

        public void SetupAgent()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            destinationPosition = transform.position;
            agent.SetDestination(destinationPosition);
        }

        public void Tick(float delta)
        {
            this.delta = delta;

            if (currentState != null)
            {
                currentState.Tick(this);

                if(currentState.forwardTransition != null)
                {
                    currentState.forwardTransition.Check_Transition(this);
                }
            }
        }

        public void FixedTick(float fixedDelta)
        {
            if (currentState != null)
                currentState.FixedTick(this);
        }

        public void OnGroundClick(BaseEventData data)
        {
            inp.SetStartCharacterRotate(false);

            PointerEventData pData = (PointerEventData)data;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
                destinationPosition = hit.position;
            else
                destinationPosition = pData.pointerCurrentRaycast.worldPosition;

            agent.SetDestination(destinationPosition);
            agent.isStopped = false;
        }

        public void ResetToIdleState()
        {
            Debug.Log("ResetToIdleState()");

            //StateManager states = playerStates.value;

            //AIStateManager currentEnemyStates = states.currentEnemyStates;

            currentEnemyStates.currentState = StateDataManager.singleton.enemyPatrolState;
            currentEnemyStates.isAggro = false;
            currentEnemyStates.isFacedPlayer = false;
            currentEnemyStates.IsPatrolInited = false;
            currentEnemyStates.aggroTransitionWaitTimer = 0;

            List<AI_PatrolPoint> currentEnemyPatrolPointList = currentEnemyStates.currentPartrolPointList.patrolPoints;
            currentEnemyStates.mTransform.position = currentEnemyPatrolPointList[currentEnemyPatrolPointList.Count - 1].pos;
            currentEnemyStates = null;

            currentState = StateDataManager.singleton.playerIdleState;
            isDiscovered = false;
            isFacedEnemy = false;
            isBattleStarted = false;

            UIManager.singleton.FadeOutCombatCommandsUI();
            //UIManager.singleton.FadeOutScrambleGameUI();
            UIManager.singleton.PlayUITransitionMoveOut();
        }
    }
}



//public float walkSpeed;
//public float gravity = -13f;
