using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Transitions/isPlayerFaceEnemy_Transition")]
    public class isPlayerFaceEnemy_Transition : Transition
    {
        public override void Check_Transition(StateManager states)
        {
            if(states.isFacedEnemy)
            {
                UIManager ui = UIManager.singleton;

                ui.FadeOutRotationUI();
                ui.PlayUITransitionMoveIn();
                states.currentState = forwardState;
            }
        }
    }
}