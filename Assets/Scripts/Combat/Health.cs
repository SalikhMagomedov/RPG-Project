using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        [SerializeField] private float health = 100f;

        public bool IsDead { get; private set; }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (Math.Abs(health) < float.Epsilon) Die();
        }

        private void Die()
        {
            if (IsDead) return;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            IsDead = true;
        }
    }
}