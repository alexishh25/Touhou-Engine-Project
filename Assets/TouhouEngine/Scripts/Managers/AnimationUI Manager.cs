using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class AnimationUIManager : MonoBehaviour
{
    public static AnimationUIManager Instance { get; private set; }

    // Animation of shake values for buttons when hovered
    public float duration = 0.4f;
    public float magnitude = 10f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Shake(VisualElement element)
    {
        StartCoroutine(ShakeRoutine(element, duration, magnitude));
    }

    private IEnumerator ShakeRoutine(VisualElement element,
                                     float duration,
                                     float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float fade = 1f - (elapsed / duration);
            float x = Random.Range(-magnitude, magnitude) * fade;
            float y = Random.Range(-magnitude * 0.3f, magnitude * 0.3f) * fade;

            element.style.translate = new StyleTranslate(
                new Translate(new Length(x), new Length(y))
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        ResetTranslate(element);
    }

    private void ResetTranslate(VisualElement element)
    {
        element.style.translate = new StyleTranslate(
            new Translate(Length.Percent(0), Length.Percent(0))
        );
    }
}
