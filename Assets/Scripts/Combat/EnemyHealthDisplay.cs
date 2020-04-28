using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Text _text;
        private Fighter _fighter;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.Target == null)
            {
                _text.text = "N/A";
                return;
            }

            var health = _fighter.Target;
            _text.text = $"{health.CurrentHealth:0}/{health.MaxHealth:0}";
        }
    }
}