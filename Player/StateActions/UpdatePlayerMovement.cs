using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/UpdatePlayerMovement")]
    public class UpdatePlayerMovement : StateAction
    {
        public float agentSpeed;
        public float turnSmoothing = 15f;
        public float slowingSpeed = 0.175f;
        public float turnSpeedThreshold = 0.5f;
        private const float stopDistanceProportion = 0.1f;

        public override void Tick(StateManager states)
        {
            NavMeshAgent agent = states.agent;

            agent.speed = agentSpeed;

            if (agent.pathPending)
                return;

            float speed = agent.desiredVelocity.magnitude;

            if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
            {
                Stopping(agent, states);
                speed = 0;
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //Debug.Log("Slowing");
                Slowing(out speed, agent.remainingDistance, agent, states);
            }

            else if (speed > turnSpeedThreshold)
            {
                //Debug.Log("Turning");
                Moving(agent, states);
            }
        }

        private void Stopping(NavMeshAgent agent, StateManager states)
        {
            agent.isStopped = true;
            states.mTransform.position = states.destinationPosition;
        }

        private void Slowing(out float speed, float distanceToDestination, NavMeshAgent agent, StateManager states)
        {
            Transform transform = states.mTransform;

            agent.isStopped = true;
            float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
            Quaternion targetRotation = transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);
            transform.position = Vector3.MoveTowards(transform.position, states.destinationPosition, slowingSpeed * Time.deltaTime);
            speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);
        }

        private void Moving(NavMeshAgent agent, StateManager states)
        {
            Transform transform = states.mTransform;

            Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        }

    }
}