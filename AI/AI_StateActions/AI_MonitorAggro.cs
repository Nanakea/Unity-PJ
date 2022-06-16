using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI_StateActions/AI_MonitorAggro")]
    public class AI_MonitorAggro : AI_StateAction
    {
        public float aggroAngle = 45;

        public override void Tick(AIStateManager aiStates)
        {
            AIManager aiManager = aiStates.ai;

            if (aiManager.disToPlayer <= aiManager.aggroThershold)
            {
                if (aiManager.angleToPlayer <= aggroAngle)
                {
                    RaycastHit hit;

                    int raycastLayerMask = 1 << 8;

                    Debug.DrawRay(aiStates.mTransform.position, aiManager.dirToPlayer, Color.red);
                    if (Physics.Raycast(aiStates.mTransform.position, aiManager.dirToPlayer, out hit, aiManager.disToPlayer + 0.5f, raycastLayerMask))
                    {
                        aiStates.isAggro = true;
                    }
                }
            }
        }
    }
}