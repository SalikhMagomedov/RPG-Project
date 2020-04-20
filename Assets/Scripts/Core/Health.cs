using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        [SerializeField] private float health = 100f;

        public bool IsDead { get; private set; }

        public object CaptureState() => health;

        public void RestoreState(object state)
        {
            health = (float) state;
            if (Mathf.Abs(health) < Mathf.Epsilon) Die();
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (Mathf.Abs(health) < Mathf.Epsilon) Die();
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