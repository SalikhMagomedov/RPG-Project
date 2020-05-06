using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        private const string WeaponName = "Weapon";

        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private float damage = 10f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private float percentageBonus;
        [SerializeField] private Projectile projectile;
        [SerializeField] private float range = 2f;
        [SerializeField] private Weapon weaponPrefab;

        public float PercentageBonus => percentageBonus;

        public float Damage => damage;

        public float Range => range;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (weaponPrefab != null)
            {
                var weapon = Instantiate(weaponPrefab, GetTransform(rightHand, leftHand));
                weapon.gameObject.name = WeaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
            else if (overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(WeaponName);
            if (oldWeapon == null) oldWeapon = leftHand.Find(WeaponName);

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public bool HasProjectile() => projectile != null;

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator,
            float calculatedDamage)
        {
            var projectileInstance =
                Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
    }
}