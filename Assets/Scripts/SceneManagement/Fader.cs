using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private Coroutine _activeFade;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private Coroutine Fade(float target, float time)
        {
            if (_activeFade != null) StopCoroutine(_activeFade);

            _activeFade = StartCoroutine(FadeRoutine(target, time));
            return _activeFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        public Coroutine FadeOut(float time)
        {
           return Fade(1, time);
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }
    }
}