using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _animator;

        private static readonly int Property = Animator.StringToHash("Forward Speed");
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            // if (Input.GetMouseButton(0)) MoveToCursor();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            var velocity = _agent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;

            _animator.SetFloat(Property, speed);
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.destination = destination;
            _agent.isStopped = false;
        }

        public void Stop()
        {
            _agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination)
        {
            _fighter.Cancel();
            MoveTo(destination);
        }
    }
}