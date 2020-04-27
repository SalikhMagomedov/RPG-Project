using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private Text _text;
        private BaseStats _baseStats;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _text.text = $"{_baseStats.GetLevel()}";
        }
    }
}