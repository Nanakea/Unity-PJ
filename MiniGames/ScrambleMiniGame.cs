using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class Word
    {
        public string word;
        [Header("leave empty if you want to randomized")]
        public string desiredRandom;

        [Space(10)]
        public int timeLimit;

        public string GetString()
        {
            if (!string.IsNullOrEmpty(desiredRandom))
            {
                return desiredRandom;
            }

            // Why would do while check if here you already declaried word is result?
            string result = word;

            while (result == word)
            {
                result = "";
                List<char> characters = new List<char>(word.ToCharArray());
                while (characters.Count > 0)
                {
                    int indexChar = UnityEngine.Random.Range(0, characters.Count - 1);
                    result += characters[indexChar];

                    characters.RemoveAt(indexChar);
                }
            }

            return result;
        }
    }

    public class ScrambleMiniGame : MiniGame
    {
        public Word[] words;

        [Header("UI REFERENCE")]
        public GameObject wordCanvas;
        public CharObject prefab;
        public Transform container;
        public float space;
        public float lerpSpeed = 5;

        public float resultChangeRate = 30;

        List<CharObject> charObjects = new List<CharObject>();
        CharObject firstSelected;

        public int currentWord;
        public int timeLimit;

        public float totalScore;

        float correctWords;
        public float correctPercent { get; set; }

        private void Awake()
        {
            BattleEvents.OnBattleStart += RestartWords;
        }

        private void OnDestroy()
        {
            BattleEvents.OnBattleStart -= RestartWords;
        }

        private void RestartWords()
        {
            currentWord = 0;
            correctWords = 0;
        }

        public void Init()
        {
            ShowScramble(currentWord);
            result.textTotalScore.text = result.totalScore.ToString();
        }

        public void Tick()
        {
            RepositionObject();

            //Update Score and calculate NEED TO FIX
            if (totalScore != result.totalScore)
            {
                // Time.frameCount records how many frames has been passed, here resultChangeRate is 30, which means it will execute the script twice per second. 
                if (Time.frameCount % resultChangeRate == 0)
                {
                    // First I calculated the difference between two scores
                    float resultDifference = result.totalScore - totalScore;

                    // IncrementPoint will always be 1
                    float incrementPoint = resultDifference / resultDifference;

                    // If the new total score higher than the old one, which means the score should be going upward.
                    if (resultDifference > 0)
                    {
                        totalScore += incrementPoint;
                    }
                    // Otherwise, we minus it...
                    else
                    {
                        totalScore -= incrementPoint;
                    }
                }
            }
            
            result.textTotalScore.text = Mathf.RoundToInt(totalScore).ToString();
        }

        void RepositionObject()
        {
            if (charObjects.Count == 0)
            {
                return;
            }

            float center = (charObjects.Count - 1) / 2;
            for (int i = 0; i < charObjects.Count; i++)
            {
                charObjects[i].rectTransform.anchoredPosition
                    = Vector2.Lerp(charObjects[i].rectTransform.anchoredPosition,
                    new Vector2((i - center) * space, 0), lerpSpeed * Time.deltaTime);
                charObjects[i].index = i;
            }
        }

        void Swap(int indexA, int indexB)
        {
            CharObject tmpA = charObjects[indexA];

            charObjects[indexA] = charObjects[indexB];
            charObjects[indexB] = tmpA;

            charObjects[indexA].transform.SetAsLastSibling();
            charObjects[indexB].transform.SetAsLastSibling();

            StartCoroutine(CheckWord());
        }

        //Check if right or wrong
        IEnumerator CheckWord()
        {
            yield return new WaitForSeconds(0.5f);

            string word = "";
            foreach (CharObject charObject in charObjects)
            {
                word += charObject.character;
            }

            //if (timeLimit <= 0)
            // {
            //    currentWord++;
            //     ShowScramble(currentWord);        //buggy code
            //     yield break;
            // }

            
            if (word == words[currentWord].word)
            {
                result.totalScore += Mathf.RoundToInt(timeLimit);

                //StopCorontine(TimeLimit());
                correctWords++;
                //word was correct
            }
            else
            {
                //word was wrong
                //nothing for now
            }
            correctPercent = correctWords / (float)words.Length;
            currentWord++;
            ShowScramble(currentWord);
        }

        IEnumerator TimeLimit()
        {
            float timeLimit = words[currentWord].timeLimit;
            result.textTime.text = Mathf.RoundToInt(timeLimit).ToString();

            int myWord = currentWord;

            yield return new WaitForSeconds(1);

            while (timeLimit > 0)
            {
                if (myWord != currentWord) { yield break; }

                timeLimit -= Time.deltaTime;
                result.textTime.text = Mathf.RoundToInt(timeLimit).ToString();
                yield return null;
            }

            Debug.Log(timeLimit);
            //score text
            StartCoroutine(CheckWord());
        }

        public void ShowScramble(int index)
        {
            charObjects.Clear();

            int childCount = container.childCount;
            if (childCount != 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Destroy(container.GetChild(i).gameObject);
                }
            }

            //FINISHED DEBUG
            if (index > words.Length - 1)
            {
                result.ShowResult();
                wordCanvas.SetActive(false);
                //Debug.LogError("index out of range, please enter number between 0-" + (words.Length - 1).ToString());
                return;
            }

            char[] chars = words[index].GetString().ToCharArray();
            foreach (char c in chars)
            {
                CharObject clone = Instantiate(prefab.gameObject).GetComponent<CharObject>();
                clone.transform.SetParent(container);
                clone.Init(c);

                charObjects.Add(clone);

            }

            currentWord = index;
            StartCoroutine(TimeLimit());
        }

        public void Select(CharObject charObject)
        {
            if (firstSelected)
            {
                Swap(firstSelected.index, charObject.index);

                //Unselect
                firstSelected.Select(this);
                charObject.Select(this);

            }
            else
            {
                firstSelected = charObject;
            }
        }

        public void UnSelect()
        {
            firstSelected = null;
        }

        public int GetAllTimeLimit()
        {
            float result = 0;
            foreach (Word w in words)
            {
                result += w.timeLimit / 2;
            }

            return Mathf.RoundToInt(result);
        }
    }
}