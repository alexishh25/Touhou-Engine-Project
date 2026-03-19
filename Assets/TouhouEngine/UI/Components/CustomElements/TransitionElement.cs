using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class TransitionElement : MonoBehaviour
{
    // Asigna tu UIDocument en el Inspector
    [SerializeField] private UIDocument uiDocument;

    private VisualElement screenA;
    private VisualElement screenB;
    private VisualElement slash1, slash2;

    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        // Selectors: exactamente como aprendiste con UITTimeline
        screenA = root.Q("screen-a");
        screenB = root.Q("screen-b");
        slash1 = root.Q("slash-1");
        slash2 = root.Q("slash-2");

        // Botón de Screen A dispara la transición
        root.Q<Button>("btn-go")
            .RegisterCallback<ClickEvent>(evt => StartCoroutine(PlayTransition()));
    }

    IEnumerator PlayTransition()
    {
        // ── Fase 1: slash entra (scale X de 0 → 1) ──────────
        SetTransition(slash1, "width", 0.25f);
        slash1.style.width = Length.Percent(50);
        yield return new WaitForSeconds(0.25f);

        SetTransition(slash2, "width", 0.2f);
        slash2.style.width = Length.Percent(100);
        yield return new WaitForSeconds(0.2f);

        // ── Fase 2: ocultar Screen A ─────────────────────────
        screenA.style.display = DisplayStyle.None;
        screenB.style.display = DisplayStyle.Flex;
        screenB.style.opacity = 0;

        // ── Fase 3: slashes salen + Screen B hace fade in ───
        SetTransition(slash1, "translate", 0.3f);
        slash1.style.translate = new Translate(Length.Percent(-110), 0);

        SetTransition(slash2, "translate", 0.3f);
        slash2.style.translate = new Translate(Length.Percent(-110), 0);

        SetTransition(screenB, "opacity", 0.4f);
        screenB.style.opacity = 1;

        yield return new WaitForSeconds(0.4f);
        // ── Transición completa ───────────────────────────────
    }

    // Helper: aplica transitionDuration a una sola propiedad
    void SetTransition(VisualElement el, string prop, float secs)
    {
        el.style.transitionProperty = new List<StylePropertyName> { prop };
        el.style.transitionDuration = new List<TimeValue> { new TimeValue(secs) };
        el.style.transitionTimingFunction = new List<EasingFunction>
            { new EasingFunction(EasingMode.EaseInOut) };
    }
}