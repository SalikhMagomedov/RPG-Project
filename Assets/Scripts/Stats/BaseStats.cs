using System;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        private int _currentLevel;

        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private GameObject levelUpParticleEffect;
        [SerializeField] private Progression progression;
        [SerializeField] private bool shouldUseModifiers;
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;

        public event Action OnLevelUp;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if (experience != null)
                experience.OnExperienceGained += () =>
                {
                    var newLevel = CalculateLevel();
                    if (newLevel <= _currentLevel) return;
                    _currentLevel = newLevel;
                    LevelUpEffect();
                    OnLevelUp?.Invoke();
                };
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) =>
            (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifier(stat) / 100);

        private float GetBaseStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());

        private float GetAdditiveModifiers(Stat stat) =>
            !shouldUseModifiers
                ? 0
                : GetComponents<IModifierProvider>()
                    .SelectMany(provider => provider.GetAdditiveModifiers(stat))
                    .Sum();

        private float GetPercentageModifier(Stat stat) =>
            !shouldUseModifiers
                ? 0
                : GetComponents<IModifierProvider>()
                    .SelectMany(provider => provider.GetPercentageModifiers(stat))
                    .Sum();

        public int GetLevel()
        {
            if (_currentLevel < 1) _currentLevel = CalculateLevel();
            return _currentLevel;
        }

        private int CalculateLevel()
        {
            var experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            var currentXp = experience.Points;

            var penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (var level = 1; level <= penultimateLevel; level++)
            {
                var xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > currentXp) return level;
            }

            return penultimateLevel + 1;
        }
    }
}