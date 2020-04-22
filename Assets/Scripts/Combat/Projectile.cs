using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private Health _target;
        [SerializeField] private float speed;

        public Health Target
        {
            set => _target = value;
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
    }
}