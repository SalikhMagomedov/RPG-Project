using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
    public class AiController : MonoBehaviour
    {
        private ActionScheduler _actionScheduler;
        private int _currentWaypointIndex;
        private Fighter _fighter;
        private LazyValue<Vector3> _guardPosition;
        private Health _health;
        private Mover _mover;
        private GameObject _player;
        private float _timeSinceArrivedWaypoint = Mathf.Infinity;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float dwellTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private float waypointTolerance = 1f;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = .2f;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
            
            _guardPosition = new LazyValue<Vector3>(() => transform.position);
        }

        private void Start()
        {
            _guardPosition.ForceInit();
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
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPosition = _guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedWaypoint > dwellTime) _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint() => patrolPath.GetWaypoint(_currentWaypointIndex);

        private bool AtWaypoint()
        {
            var distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
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