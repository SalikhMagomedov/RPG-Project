using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        private static readonly int StopAttackTrigger = Animator.StringToHash("StopAttack");

        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private Weapon _currentWeapon;
        private Mover _mover;
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private Weapon defaultWeapon;

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        public object CaptureState() => _currentWeapon.name;

        public void RestoreState(object state)
        {
            var weapon = UnityEngine.Resources.Load<Weapon>((string) state);
            EquipWeapon(weapon);
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
            if (_currentWeapon != null) return;
            
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        public Health Target => _target;

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

            if (_currentWeapon.HasProjectile())
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject);
            else
                _target.TakeDamage(gameObject, _currentWeapon.Damage);
        }

        private void Shoot()
        {
            Hit();
        }

        private bool IsInRange() =>
            Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.Range;

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