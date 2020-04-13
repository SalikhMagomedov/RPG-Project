using Combat;
using Core;
using UnityEngine;

namespace Control
{
    public class AiController : MonoBehaviour
    {
        private Fighter _fighter;
        private Health _health;
        private GameObject _player;

        [SerializeField] private float chaseDistance = 5f;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (_health.IsDead) return;
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
                _fighter.Attack(_player);
            else
                _fighter.Cancel();
        }

        private bool InAttackRangeOfPlayer()
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer <= chaseDistance;
        }
    }
}