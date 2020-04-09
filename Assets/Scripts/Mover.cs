using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent _agent;
    private Camera _camera;
    private Animator _animator;

    private static readonly int Property = Animator.StringToHash("Forward Speed");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) MoveToCursor();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        var velocity = _agent.velocity;
        var localVelocity = transform.InverseTransformDirection(velocity);
        var speed = localVelocity.z;

        _animator.SetFloat(Property, speed);
    }

    private void MoveToCursor()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hasHit = Physics.Raycast(ray, out var hit);
        if (hasHit) _agent.destination = hit.point;
    }
}