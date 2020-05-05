using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _text.text = $"{_health.CurrentHealth:0}/{_health.MaxHealth:0}";
        }
    }
}