using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _text.text = $"{_experience.Points:0}";
        }
    }
}