using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable;
        
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();
            
            var levels = _lookupTable[characterClass][stat];

            if (levels.Length < level) return 0;

            return levels[level - 1];
        }

        private void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            
            foreach (var progressionCharacterClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (var progressionStat in progressionCharacterClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                
                _lookupTable[progressionCharacterClass.characterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            var levels = _lookupTable[characterClass][stat];
            return levels.Length;
        }

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