using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LevelSwitcherManager : MonoBehaviour
    {
        [HideInInspector] public Animator anim;

        public Camera mainSessionCamera;
        public Camera battleSessionCamera;

        public bool transitionFinished;

        private void Start()
        {
            UIManager.singleton.levelSwitcherManager = this;
            anim = GetComponent<Animator>();

            battleSessionCamera.enabled = false;
            mainSessionCamera.enabled = true;
        }
    }
}