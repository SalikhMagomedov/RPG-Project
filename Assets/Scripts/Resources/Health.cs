using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private BaseStats _baseStats;

        [SerializeField] private float regeneratePercentage = 70f;

        public bool IsDead { get; private set; }

        public float Percentage => 100 * CurrentHealth / _baseStats.GetStat(Stat.Health);

        public float CurrentHealth { get; private set; } = -1f;

        public float MaxHealth => _baseStats.GetStat(Stat.Health);

        public object CaptureState() => CurrentHealth;

        public void RestoreState(object state)
        {
            CurrentHealth = (float) state;
            if (Mathf.Abs(CurrentHealth) < Mathf.Epsilon) Die();
        }

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            _baseStats.OnLevelUp += RegenerateHealth;
            if (CurrentHealth < 0) CurrentHealth = _baseStats.GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            var regeneratePoints = _baseStats.GetStat(Stat.Health) * regeneratePercentage / 100;
            CurrentHealth = Mathf.Max(CurrentHealth, regeneratePoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            if (!(Mathf.Abs(CurrentHealth) < Mathf.Epsilon)) return;

            Die();
            AwardExperience(instigator);
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}