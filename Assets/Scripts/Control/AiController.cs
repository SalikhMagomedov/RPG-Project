using Combat;
using UnityEngine;

namespace Control
{
    public class AiController : MonoBehaviour
    {
        private GameObject _player;
        private Fighter _fighter;

        [SerializeField] private float chaseDistance = 5f;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else
            {
                _fighter.Cancel();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer <= chaseDistance;
        }
    }
}