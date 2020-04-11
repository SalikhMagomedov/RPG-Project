using Movement;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;

        private Transform _target;
        private Mover _mover;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (_target == null) return;

            if (_target != null && !IsInRange())
                _mover.MoveTo(_target.position);
            else
                _mover.Stop();
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < weaponRange;
        }

        public void Attack(CombatTarget target)
        {
            _target = target.transform;
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}