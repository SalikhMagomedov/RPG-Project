using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
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
        private WeaponConfig _currentWeaponConfig;
        private LazyValue<Weapon> _currentWeapon;
        private Mover _mover;
        private float _timeSinceLastAttack = Mathf.Infinity;

        [SerializeField] private WeaponConfig defaultWeaponConfig;
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
            if (stat == Stat.Damage) yield return _currentWeaponConfig.Damage;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return _currentWeaponConfig.PercentageBonus;
        }

        public object CaptureState() => _currentWeaponConfig.name;

        public void RestoreState(object state)
        {
            var weapon = Resources.Load<WeaponConfig>((string) state);
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

            _currentWeaponConfig = defaultWeaponConfig;
            _currentWeapon = new LazyValue<Weapon>(() => AttachWeapon(_currentWeaponConfig));
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeaponConfig = weaponConfig;
            _currentWeapon.value = AttachWeapon(_currentWeaponConfig);
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

            if (_currentWeapon.value != null)
            {
                _currentWeapon.value.OnHit();
            }
            
            if (_currentWeaponConfig.HasProjectile())
                _currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, Target, gameObject, damage);
            else
                Target.TakeDamage(gameObject, damage);
        }

        private void Shoot()
        {
            Hit();
        }

        private bool IsInRange() =>
            Vector3.Distance(transform.position, Target.transform.position) < _currentWeaponConfig.Range;

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!_mover.CanMoveTo(combatTarget.transform.position)) return false;
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