using UnityEngine;

namespace RPG.Ui.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageText;
        
        private void Start()
        {
            Spawm(11);
        }

        public void Spawm(float damageAmount)
        {
            var instance = Instantiate(damageText, transform);
        }
    }
}