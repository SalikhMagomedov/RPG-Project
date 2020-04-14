using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class AiController : MonoBehaviour
    {
        private ActionScheduler _actionScheduler;
        private Fighter _fighter;
        private Vector3 _guardPosition;
        private Health _health;
        private Mover _mover;
        private GameObject _player;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
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
            {
                _timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            _mover.StartMoveAction(_guardPosition);
        }

        private void SuspicionBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(_player);
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