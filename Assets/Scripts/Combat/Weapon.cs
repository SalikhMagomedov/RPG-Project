using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float range = 2f;
        [SerializeField] private GameObject weaponPrefab;

        public float Damage => damage;

        public float Range => range;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (weaponPrefab != null) Instantiate(weaponPrefab, handTransform);

            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
        }
    }
}