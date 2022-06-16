using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class Result
    {
        public float totalScore = 0;

        [Header("REF UI")]
        public Text textTime;
        public Text textTotalScore;

        [Header("REF RESET SCREEN")]
        public Text textResultScore;
        public Text textInfo;
        public GameObject resultCanvas;

        [Space(20)]
        public float maxDamage = 5; 

        [HideInInspector] public MiniGame currentMiniGame;

        public void ShowResult()
        {
            //show damage dealt
            textResultScore.text = totalScore.ToString();

            StringBuilder textInfoResult = new StringBuilder("Your damage is ");
            if (currentMiniGame.gameType == MiniGameTypeEnum.Scramble)
            {
                ScrambleMiniGame scrableMiniGame = (ScrambleMiniGame)currentMiniGame;
                var damage = scrableMiniGame.correctPercent * maxDamage; //max damage is when player has 100% correct words
                textInfoResult.Append(damage);
                BattleEvents.RaiseOnPlayerAttack(damage);
            }
            textInfoResult.Append(" damages");
            textInfo.text = textInfoResult.ToString();

            //int allTimeLimit = WordScramble.main.GetAllTimeLimit();

            resultCanvas.SetActive(true);
        }
    }

    public class CombatCommandsManager : MonoBehaviour
    {
        [Header("Current Result")]
        public Result currentResult;

        [Header("PlayerState Variable")]
        public StateManagerVariables playerStates;

        [Header("Games")]
        public MiniGameTypeEnum currentGameType;

        bool gameStarted;

        [HideInInspector] public ScrambleMiniGame scrambleMiniGame;

        private void Start()
        {
            scrambleMiniGame = GetComponent<ScrambleMiniGame>();
            scrambleMiniGame.result = currentResult;
        }

        private void Update()
        {
            if(gameStarted)
            {
                switch (currentGameType)
                {
                    case MiniGameTypeEnum.Scramble:
                        scrambleMiniGame.Tick();
                        break;
                    case MiniGameTypeEnum.Others:
                        break;
                    default:
                        break;
                }
            }
        }

        public void GetRandomAttackCommandGame()
        {
            int random = Random.Range(1, 101);
            if (random <= 101)
            {
                currentGameType = MiniGameTypeEnum.Scramble;
            }

            switch (currentGameType)
            {
                case MiniGameTypeEnum.Scramble:
                    UIManager.singleton.FadeInScrambleGameUI();
                    scrambleMiniGame.Init();
                    currentResult.currentMiniGame = scrambleMiniGame;
                    gameStarted = true;
                    BattleEvents.RaiseOnBattleStart();
                    break;
                case MiniGameTypeEnum.Others:
                    break;
                default:
                    break;
            }
        }
    }
}