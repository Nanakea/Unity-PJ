using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class UIManager : MonoBehaviour
    {
        public Image cursor;

        [Header("Cursor")]
        public Sprite normalCursor;
        public Sprite clickedCursor;

        [Header("Rect Transform")]
        public RectTransform arrow_roundedLeftRect;
        public RectTransform arrow_roundedRightRect;
        public RectTransform playerHealthRect;
        public RectTransform combatCommandsUIRect;
        public RectTransform scrambleGameUIRect;

        [Header("Canvas")]
        [HideInInspector] public Canvas combatCommandsUICanvas;
        [HideInInspector] public Canvas scrambleGameUICanvas;

        [Header("Slider")]
        Slider playerHealthSlider;

        [Header("Level Switcher Manager")]
        public LevelSwitcherManager levelSwitcherManager;

        [Header("Controller Stats")]
        public bool CursorVisible;
        public float healthbarBuffer;
        
        [Header("References")]
        public InputHandler inp;

        public static UIManager singleton;
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void Init()
        {
            InitPlayerHealthSlider();
            InitCombatCommandsUI();
            InitScrambleGameUI();
        }

        #region Init

        void InitPlayerHealthSlider()
        {
            //playerHealthRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inp.states.playerStatsManager.hp * healthbarBuffer);
            //playerHealthSlider = playerHealthRect.GetComponent<Slider>();
            //playerHealthSlider.maxValue = inp.states.playerStatsManager.hp;
        }

        void InitCombatCommandsUI()
        {
            combatCommandsUICanvas = combatCommandsUIRect.GetComponent<Canvas>();
            combatCommandsUICanvas.enabled = false;
            LeanTween.alpha(combatCommandsUIRect, 0, 0.1f);
        }

        void InitScrambleGameUI()
        {
            scrambleGameUICanvas = scrambleGameUIRect.GetComponent<Canvas>();
            scrambleGameUICanvas.enabled = false;
            LeanTween.alpha(scrambleGameUIRect, 0, 0.1f);
        }

        #endregion

        public void Tick(InputHandler inp)
        {
            Cursor.visible = CursorVisible;

            cursor.rectTransform.position = inp.mousePosition;

            if (inp.mouse0)
                cursor.sprite = clickedCursor;
            else
                cursor.sprite = normalCursor;
        }

        public void FadeOutRotationUI()
        {
            LeanTween.alpha(arrow_roundedLeftRect, 0, 0.3f);
            LeanTween.alpha(arrow_roundedRightRect, 0, 0.3f);
        }

        public void FadeInRotationUI()
        {
            LeanTween.alpha(arrow_roundedLeftRect, 1, 0.3f);
            LeanTween.alpha(arrow_roundedRightRect, 1, 0.3f);
        }

        public void FadeInCombatCommandsUI()
        {
            if(!combatCommandsUICanvas.enabled)
                combatCommandsUICanvas.enabled = true;

            LeanTween.alpha(combatCommandsUIRect, 1, 0.3f);
        }

        public void FadeOutCombatCommandsUI()
        {
            if (combatCommandsUICanvas.enabled)
                combatCommandsUICanvas.enabled = false;

            LeanTween.alpha(combatCommandsUIRect, 0, 0.1f);
        }

        public void FadeInScrambleGameUI()
        {
            if (!scrambleGameUICanvas.enabled)
                scrambleGameUICanvas.enabled = true;

            LeanTween.alpha(scrambleGameUIRect, 1, 0.3f);
        }

        public void FadeOutScrambleGameUI()
        {
            if (scrambleGameUICanvas.enabled)
                scrambleGameUICanvas.enabled = false;

            LeanTween.alpha(scrambleGameUIRect, 0, 0.1f);
        }

        public void PlayUITransitionMoveIn()
        {
            levelSwitcherManager.anim.Play("transition_move_in");
        }

        public void PlayUITransitionMoveOut()
        {
            levelSwitcherManager.anim.Play("transition_move_out");
        }
    }
}