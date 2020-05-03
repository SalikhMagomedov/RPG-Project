using RPG.Control;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController controller)
        {
            var fighter = controller.GetComponent<Fighter>();
            if (!fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0)) fighter.Attack(gameObject);

            return true;
        }

        public CursorType CursorType => CursorType.Combat;
    }
}