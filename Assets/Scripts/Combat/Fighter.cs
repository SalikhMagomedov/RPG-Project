using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        private static readonly int StopAttackTrigger = Animator.StringToHash("StopAttack");

        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private Mover _mover;
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage = 10f;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private Transform handTransform;
        [SerializeField] private AnimatorOverrideController weaponOverride;

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            _animator.ResetTrigger(StopAttackTrigger);
            _animator.SetTrigger(StopAttackTrigger);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
        }

        private void Start()
        {
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            _animator.runtimeAnimatorController = weaponOverride;
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;
            if (_target.IsDead) return;
            if (!IsInRange())
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (!(_timeSinceLastAttack > timeBetweenAttacks)) return;
            TriggerAttack();
            _timeSinceLastAttack = 0;
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(StopAttackTrigger);
            _animator.SetTrigger(AttackAnimation);
        }

        private void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(weaponDamage);
        }

        private bool IsInRange() => Vector3.Distance(transform.position, _target.transform.position) < weaponRange;

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Attack(GameObject target)
        {
            _actionScheduler.StartAction(this);
            _target = target.GetComponent<Health>();
        }
    }
}