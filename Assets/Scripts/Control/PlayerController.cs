using System;
using System.Linq;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        private Health _health;
        private Mover _mover;

        [SerializeField] private CursorMapping[] cursorMappings;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if(InteractWithUi()) return;
            if (_health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            var hits = new RaycastHit[5];
            var size = Physics.RaycastNonAlloc(GetMouseRay(), hits);

            if (size <= 0) return false;

            if (!hits.Where(hit => hit.transform != null).SelectMany(hit => hit.transform.GetComponents<IRaycastable>())
                .Any(component => component.HandleRaycast(this))) return false;
            SetCursor(CursorType.Combat);
            return true;

        }

        private bool InteractWithUi()
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return false;
            
            SetCursor(CursorType.Ui);
            return true;
        }

        private void SetCursor(CursorType cursor)
        {
            var mapping = GetCursorMapping(cursor);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in cursorMappings)
                if (mapping.type == type)
                    return mapping;

            return cursorMappings[0];
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;
            if (Input.GetMouseButton(0)) _mover.StartMoveAction(hit.point, 1f);

            SetCursor(CursorType.Movement);

            return true;
        }

        private Ray GetMouseRay() => _camera.ScreenPointToRay(Input.mousePosition);

        private enum CursorType
        {
            None,
            Movement,
            Combat,
            Ui
        }

        [Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
    }
}