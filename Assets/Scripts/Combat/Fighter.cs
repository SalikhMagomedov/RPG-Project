﻿using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;

        private Transform _target;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (_target == null) return;

            if (!IsInRange())
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            _animator.SetTrigger(AttackAnimation);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < weaponRange;
        }

        public void Attack(CombatTarget target)
        {
            _actionScheduler.StartAction(this);
            _target = target.transform;
        }

        public void Cancel()
        {
            _target = null;
        }

        private void Hit()
        {
        }
    }
}