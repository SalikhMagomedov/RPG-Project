using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent _agent;
    private Ray _lastRay;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastRay = _camera.ScreenPointToRay(Input.mousePosition);
        }

        Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100);
        _agent.destination = target.position;
    }
}