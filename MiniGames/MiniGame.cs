using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MiniGame : MonoBehaviour
    {
        [Header("Mini Game Type")]
        public MiniGameTypeEnum gameType;
        
        [HideInInspector] public Result result;
    }

    public enum MiniGameTypeEnum
    {
        Scramble,
        Others
    }
}