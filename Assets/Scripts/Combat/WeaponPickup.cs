using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private float respawnTime = 5f;
        [SerializeField] private Weapon weapon;

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0)) Pickup(controller.GetComponent<Fighter>());

            return true;
        }

        public CursorType CursorType => CursorType.Pickup;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Pickup(other.GetComponent<Fighter>());
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform) child.gameObject.SetActive(shouldShow);
        }
    }
}