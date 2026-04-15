using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance { get; private set; }

    [Header("Efectos de sonido")]

    [SerializeField] public AudioClip sfx_buttonhover;
    [SerializeField] public AudioClip sfx_clickbutton;
    [SerializeField] public AudioClip sfx_cancelbutton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void PlayHoverSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(sfx_buttonhover);
        else
            Debug.LogWarning("SoundManager not found. Cannot play hover sound.");
    }

    public void PlayClickSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(sfx_clickbutton);
        else
            Debug.LogWarning("SoundManager not found. Cannot play click sound.");
    }

    public void PlayCancelSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(sfx_cancelbutton);
        else
            Debug.LogWarning("SoundManager not found. Cannot play cancel sound.");
    }



    // Allows subscribing or unsubscribing multiple buttons to their respective click event handlers efficiently.
    public void ManageButtonActions(bool suscribe, params (Button button, Action handler)[] buttonActions)
    {
        foreach (var (button, handler) in buttonActions)
        {
            if (button == null || handler == null) continue;

            if (suscribe)
                button.clicked += handler;
            else
                button.clicked -= handler;
        }
    }

    public void AlternateRegisterHoverSFX(bool register, List<Button> buttonhoverActions)
    {
        foreach (var button in buttonhoverActions)
        {
            if (button == null) continue;

            if (register)
            {
                button.RegisterCallback<PointerEnterEvent>(OnHoverPointer);
                button.RegisterCallback<FocusInEvent>(OnHoverFocus);
            }
            else
            {
                button.UnregisterCallback<PointerEnterEvent>(OnHoverPointer);
                button.UnregisterCallback<FocusInEvent>(OnHoverFocus);
            }
        }
    }

    private void OnHoverPointer(PointerEnterEvent evt)
    {
        if (evt.target is Button button)
            AnimationUIManager.Instance.Shake(button);
        PlayHoverSFX();
    }
    private void OnHoverFocus(FocusInEvent evt)
    {
        if (evt.target is Button button)
            AnimationUIManager.Instance.Shake(button);
        PlayHoverSFX();
    }
}
