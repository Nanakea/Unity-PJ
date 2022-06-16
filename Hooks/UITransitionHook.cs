using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UITransitionHook : MonoBehaviour
    {
        LevelSwitcherManager levelSwitcherManager;

        private void Awake()
        {
            levelSwitcherManager = GetComponent<LevelSwitcherManager>();
        }

        public void CameraSwitchOut()
        {
            levelSwitcherManager.mainSessionCamera.enabled = false;
            levelSwitcherManager.battleSessionCamera.enabled = true;
        }

        public void CameraSwitchIn()
        {
            levelSwitcherManager.mainSessionCamera.enabled = true;
            levelSwitcherManager.battleSessionCamera.enabled = false;
        }

        public void SetTransitionFinishedStatus(int v)
        {
            if(v == 1)
            {
                levelSwitcherManager.transitionFinished = true;
            }
            else
            {
                levelSwitcherManager.transitionFinished = false;
            }
        }
    }
}