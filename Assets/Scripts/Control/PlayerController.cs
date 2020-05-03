using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;

        [SerializeField] private CursorMapping[] cursorMappings;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_health.IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            var hits = new RaycastHit[5];
            var size = Physics.RaycastNonAlloc(GetMouseRay(), hits);

            if (size <= 0) return false;

            foreach (var hit in hits)
            {
                if (hit.transform == null) continue;

                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!_fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0)) _fighter.Attack(target.gameObject);

                SetCursor(CursorType.Combat);

                return true;
            }

            return false;
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
            Combat
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