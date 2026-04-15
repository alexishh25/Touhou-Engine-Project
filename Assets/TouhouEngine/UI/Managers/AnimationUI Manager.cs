using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AnimationUIManager : MonoBehaviour
{
    public static AnimationUIManager Instance { get; private set; }

    // Animation of shake values for buttons when hovered
    public float duration = 0.4f;
    public float magnitude = 10f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
    }

    public void Shake(VisualElement element)
    {
        ShakeRoutine(element, duration, magnitude).Forget();
    }

    private async UniTaskVoid ShakeRoutine(VisualElement element,
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
            await UniTask.Yield(PlayerLoopTiming.Update);
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
