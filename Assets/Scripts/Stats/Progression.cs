using System;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        public float GetStat(Stat stat, CharacterClass characterClass, int level) =>
            (from progressionClass in characterClasses
                where progressionClass.characterClass == characterClass
                from progressionStat in progressionClass.stats
                where progressionStat.stat == stat
                where progressionStat.levels.Length >= level
                select progressionStat.levels[level - 1]).FirstOrDefault();

        [Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [Serializable]
        private class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}