using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private Health _target;
        [SerializeField] private bool isHoming;

        [SerializeField] private float speed;

        public void SetTarget(Health value, float damage)
        {
            _target = value;
            _damage = damage;
        }

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (isHoming && !_target.IsDead) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            var targetCapsule = _target.GetComponent<CapsuleCollider>();
            var position = _target.transform.position;

            return targetCapsule == null ? position : position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            if (_target.IsDead) return;

            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}