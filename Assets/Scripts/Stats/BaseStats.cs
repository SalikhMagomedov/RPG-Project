using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        private int _currentLevel;

        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticleEffect;

        public event Action OnLevelUp;
        
        private void Start()
        {
            _currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnExperienceGained += () =>
                {
                    var newLevel = CalculateLevel();
                    if (newLevel <= _currentLevel) return;
                    _currentLevel = newLevel;
                    LevelUpEffect();
                    OnLevelUp?.Invoke();
                };
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());

        public int GetLevel()
        {
            if (_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }
            return _currentLevel;
        }

        private int CalculateLevel()
        {
            var experience = GetComponent<Experience>();

            if (experience == null)
            {
                return startingLevel;
            }
            
            var currentXp = experience.Points;

            var penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (var level = 1; level <= penultimateLevel; level++)
            {
                var xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > currentXp)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}