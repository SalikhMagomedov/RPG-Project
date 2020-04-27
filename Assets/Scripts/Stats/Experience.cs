using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints;

        public float Points => experiencePoints;
        
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        public object CaptureState() => experiencePoints;

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }
    }
}