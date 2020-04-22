using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private Health _target;
        private float _damage;
        
        [SerializeField] private float speed;

        public void SetTarget(Health value, float damage)
        {
            _target = value;
            _damage = damage;
        }

        private void Update()
        {
            transform.LookAt(GetAimLocation());
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
            
            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}