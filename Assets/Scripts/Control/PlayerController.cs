﻿using Combat;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        private Fighter _fighter;
        private Mover _mover;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
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
                if (!_fighter.CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0)) _fighter.Attack(target);
                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;
            if (Input.GetMouseButton(0)) _mover.StartMoveAction(hit.point);

            return true;
        }

        private Ray GetMouseRay() => _camera.ScreenPointToRay(Input.mousePosition);
    }
}