using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        [SerializeField] private float health = 100f;
        
        private BaseStats _baseStats;

        public bool IsDead { get; private set; }

        public float Percentage => 100 * health / _baseStats.Health;

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            health = _baseStats.Health;
        }

        public object CaptureState() => health;

        public void RestoreState(object state)
        {
            health = (float) state;
            if (Mathf.Abs(health) < Mathf.Epsilon) Die();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (!(Mathf.Abs(health) < Mathf.Epsilon)) return;
            
            Die();
            AwardExperience(instigator);
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(_baseStats.ExperienceReward);
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