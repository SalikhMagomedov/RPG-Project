using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float range = 2f;
        [SerializeField] private bool isRightHanded = true;

        public float Damage => damage;

        public float Range => range;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (weaponPrefab != null) Instantiate(weaponPrefab, isRightHanded ? rightHand : leftHand);

            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
        }
    }
}