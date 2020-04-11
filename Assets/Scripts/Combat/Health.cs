using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        private bool _isDead;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (Math.Abs(health) < float.Epsilon) Die();
        }

        private void Die()
        {
            if (_isDead) return;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            _isDead = true;
        }
    }
}