using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        private Canvas _canvas;
        
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;

        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>();
        }

        private void Update()
        {
            if (Mathf.Approximately(health.Fraction, 0) || Mathf.Approximately(health.Fraction, 1))
            {
                _canvas.enabled = false;
                return;
            }

            _canvas.enabled = true;
            
            foreground.localScale = new Vector3(health.Fraction, 1f, 1f);
        }
    }
}