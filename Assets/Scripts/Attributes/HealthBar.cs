using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        
        private void Update()
        {
            foreground.localScale = new Vector3(health.Fraction, 1f, 1f);
        }
    }
}