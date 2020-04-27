using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        private float _health = -1f;
        private BaseStats _baseStats;

        public bool IsDead { get; private set; }

        public float Percentage => 100 * _health / _baseStats.GetStat(Stat.Health);

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            if (_health < 0)
            {
                _health = _baseStats.GetStat(Stat.Health);
            }
        }

        public object CaptureState() => _health;

        public void RestoreState(object state)
        {
            _health = (float) state;
            if (Mathf.Abs(_health) < Mathf.Epsilon) Die();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _health = Mathf.Max(_health - damage, 0);
            if (!(Mathf.Abs(_health) < Mathf.Epsilon)) return;
            
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