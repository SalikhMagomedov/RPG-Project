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
            var isInRange = Vector3.Distance(transform.position, _target.position) < weaponRange;
            if (_target != null && !isInRange)
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Stop();
            }
        }

        public void Attack(CombatTarget target)
        {
            _target = target.transform;
        }
    }
}