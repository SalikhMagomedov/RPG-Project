using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private Health _target;
        private GameObject _instigator;
        
        [SerializeField] private bool isHoming;
        [SerializeField] private float speed;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float maxLifetime = 10f;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifeAfterImpact = 2;
        [SerializeField] private UnityEvent onHit;

        public void SetTarget(Health value, GameObject instigator, float damage)
        {
            _target = value;
            _damage = damage;
            _instigator = instigator;
            
            Destroy(gameObject, maxLifetime);
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

            onHit.Invoke();
            
            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            _target.TakeDamage(_instigator, _damage);

            speed = 0f;

            foreach (var toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}