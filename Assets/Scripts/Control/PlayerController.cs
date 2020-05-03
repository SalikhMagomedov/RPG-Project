using System;
using System.Collections.Generic;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        private Health _health;
        private Mover _mover;

        [SerializeField] private CursorMapping[] cursorMappings;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float maxNavPathLength = 40f;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (InteractWithUi()) return;
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
            var hits = RaycastAllSorted();

            foreach (var hit in hits)
            {
                if (hit.transform == null) continue;
                
                var raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (!raycastable.HandleRaycast(this)) continue;
                    
                    SetCursor(raycastable.CursorType);
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<RaycastHit> RaycastAllSorted()
        {
            var hits = new RaycastHit[5];
            Physics.RaycastNonAlloc(GetMouseRay(), hits);
            var distances = new float[hits.Length];
            for (var i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
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
            var hasHit = RaycastNavMesh(out var target);
            if (!hasHit) return false;
            if (Input.GetMouseButton(0)) _mover.StartMoveAction(target, 1f);

            SetCursor(CursorType.Movement);

            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;

            var hasCastToNavMesh = NavMesh.SamplePosition(hit.point,
                out var navMeshHit,
                maxNavMeshProjectionDistance,
                NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            var path = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            return !(GetPathLength(path) > maxNavPathLength);
        }

        private float GetPathLength(NavMeshPath path)
        {
            var total = 0f;

            if (path.corners.Length < 2) return total;

            for (var i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            
            return total;
        }

        private Ray GetMouseRay() => _camera.ScreenPointToRay(Input.mousePosition);

        [Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
    }
}