using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
         private float _health = 100f;

         public void TakeDamage(float damage)
         {
             _health = Mathf.Max(_health - damage, 0);
             print(_health);
         }
    }
}