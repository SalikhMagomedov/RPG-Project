using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private BaseStats _baseStats;
        private LazyValue<float> _healthPoints;

        [SerializeField] private float regeneratePercentage = 70f;
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;

        public bool IsDead { get; private set; }

        public float Percentage => 100 * Fraction;
        
        public float Fraction => CurrentHealth / _baseStats.GetStat(Stat.Health);

        public float CurrentHealth => _healthPoints.value;

        public float MaxHealth => _baseStats.GetStat(Stat.Health);

        public object CaptureState() => CurrentHealth;

        public void RestoreState(object state)
        {
            _healthPoints.value = (float) state;
            if (Mathf.Abs(CurrentHealth) < Mathf.Epsilon) Die();
        }

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();

            _healthPoints = new LazyValue<float>(() => _baseStats.GetStat(Stat.Health));
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            _baseStats.OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            var regeneratePoints = _baseStats.GetStat(Stat.Health) * regeneratePercentage / 100;
            _healthPoints.value = Mathf.Max(CurrentHealth, regeneratePoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(CurrentHealth - damage, 0);

            if (Mathf.Abs(CurrentHealth) < Mathf.Epsilon)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
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

        [Serializable]
        private class TakeDamageEvent : UnityEvent<float>
        {
        }

        public void Heal(float healthToRestore)
        {
            _healthPoints.value = Mathf.Min(CurrentHealth + healthToRestore, MaxHealth);
        }
    }
}