using RPG.Core;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private static readonly int Property = Animator.StringToHash("Forward Speed");

        private ActionScheduler _actionScheduler;
        private NavMeshAgent _agent;
        private Animator _animator;
        private Health _health;

        [SerializeField] private float maxSpeed = 6f;

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        public object CaptureState() => new SerializableVector3(transform.position);

        public void RestoreState(object state)
        {
            _agent.enabled = false;
            transform.position = ((SerializableVector3) state).ToVector3();
            _agent.enabled = true;
        }

        private void Awake()
        {
            _health = GetComponent<Health>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _agent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            var velocity = _agent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;

            _animator.SetFloat(Property, speed);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _agent.destination = destination;
            _agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            _agent.isStopped = false;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }
    }
}