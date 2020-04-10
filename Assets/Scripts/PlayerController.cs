using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera _camera;
    private Mover _mover;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) MoveToCursor();
    }

    private void MoveToCursor()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hasHit = Physics.Raycast(ray, out var hit);
        if (hasHit) _mover.MoveTo(hit.point);
    }
}