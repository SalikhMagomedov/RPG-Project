using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        private int _currentLevel = 0;

        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private Progression progression;

        private void Start()
        {
            _currentLevel = Level;
        }

        private void Update()
        {
            var newLevel = CalculateLevel();
            if (newLevel <= _currentLevel) return;
            _currentLevel = newLevel;
            print("Levelled Up!");
        }

        public float GetStat(Stat stat) => progression.GetStat(stat, characterClass, Level);

        public int Level => _currentLevel;

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