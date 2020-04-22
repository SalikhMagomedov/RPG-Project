using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private float damage = 10f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile;
        [SerializeField] private float range = 2f;
        [SerializeField] private GameObject weaponPrefab;

        public float Damage => damage;

        public float Range => range;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (weaponPrefab != null) Instantiate(weaponPrefab, GetTransform(rightHand, leftHand));

            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public bool HasProjectile() => projectile != null;

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            var projectileInstance =
                Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage);
        }
    }
}