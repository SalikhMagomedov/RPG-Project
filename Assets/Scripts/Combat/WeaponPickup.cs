using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private float respawnTime = 5f;
        [SerializeField] private float healthToRestore;
        [SerializeField] private WeaponConfig weaponConfig;

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0)) Pickup(controller.gameObject);

            return true;
        }

        public CursorType CursorType => CursorType.Pickup;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Pickup(other.gameObject);
        }

        private void Pickup(GameObject subject)
        {
            if (weaponConfig != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weaponConfig);
            }

            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
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