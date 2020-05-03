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
            if (!fighter.CanAttack(this.gameObject)) return false;

            if (Input.GetMouseButton(0)) fighter.Attack(this.gameObject);

            return true;
        }
    }
}