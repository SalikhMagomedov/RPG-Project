using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Ui.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text damageText;
        
        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            damageText.text = amount.ToString(CultureInfo.InvariantCulture);
        }
    }
}