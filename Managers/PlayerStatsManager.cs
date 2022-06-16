using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "PlayerStatsManager")]
    public class PlayerStatsManager : ScriptableObject
    {
        public float hp;
        public float mp;

        float _hp;
        float _mp;

        public void Init()
        {
            _hp = hp;
            _mp = mp;
        }

        public float GetPlayerHealth()
        {
            return _hp;
        }

        public float GetPlayerMana()
        {
            return _mp;
        }

        public void SetPlayerHealth(float desiredHp)
        {
            _hp = desiredHp;
        }

        public void SetPlayerMana(float desiredMp)
        {
            _mp = desiredMp;
        }
    }
}