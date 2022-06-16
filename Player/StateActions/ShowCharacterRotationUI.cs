using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/ShowCharacterRotationUI")]
    public class ShowCharacterRotationUI : StateAction
    {
        public override void Tick(StateManager states)
        {
            UIManager ui = UIManager.singleton;

            if (ui.levelSwitcherManager.transitionFinished)
            {
                ui.levelSwitcherManager.transitionFinished = false;
                ui.FadeInRotationUI();
            }
        }
    }
}