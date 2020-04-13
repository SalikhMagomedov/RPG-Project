using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class AiController : MonoBehaviour
    {
        private Fighter _fighter;
        private Health _health;
        private GameObject _player;
        private Mover _mover;
        private Vector3 _guardPosition;

        [SerializeField] private float chaseDistance = 5f;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
                _fighter.Attack(_player);
            else
                _mover.StartMoveAction(_guardPosition);
        }

        private bool InAttackRangeOfPlayer()
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer <= chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}