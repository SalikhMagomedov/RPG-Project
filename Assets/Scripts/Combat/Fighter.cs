using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        private static readonly int StopAttackTrigger = Animator.StringToHash("StopAttack");

        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private Weapon _currentWeapon;
        private Mover _mover;
        private float _timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] private Weapon defaultWeapon;

        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private float timeBetweenAttacks = 2f;

        public Health Target { get; private set; }

        public void Cancel()
        {
            StopAttack();
            Target = null;
            _mover.Cancel();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return _currentWeapon.Damage;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return _currentWeapon.PercentageBonus;
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

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (Target == null) return;
            if (Target.IsDead) return;
            if (!IsInRange())
            {
                _mover.MoveTo(Target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(Target.transform);
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
            if (Target == null) return;

            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (_currentWeapon.HasProjectile())
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, Target, gameObject, damage);
            else
                Target.TakeDamage(gameObject, damage);
        }

        private void Shoot()
        {
            Hit();
        }

        private bool IsInRange() =>
            Vector3.Distance(transform.position, Target.transform.position) < _currentWeapon.Range;

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Attack(GameObject target)
        {
            _actionScheduler.StartAction(this);
            Target = target.GetComponent<Health>();
        }
    }
}