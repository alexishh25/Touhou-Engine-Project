using System;
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
        if (Instance == null) Instance = this;
    }

    public void PlayHoverSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(sfx_buttonhover);
        else
            Debug.LogWarning("SoundManager no encontrado. No se puede reproducir el sonido de hover.");
    }

    public void PlayClickSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(sfx_clickbutton);
        else
            Debug.LogWarning("SoundManager no encontrado. No se puede reproducir el sonido de click.");
    }

    public void PlayCancelSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(sfx_cancelbutton);
        else
            Debug.LogWarning("SoundManager no encontrado. No se puede reproducir el sonido de cancelación.");
    }



    // Este método permite suscribir o desuscribir múltiples botones a sus respectivos manejadores de eventos de clic de manera eficiente.
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

    private void OnHoverPointer(PointerEnterEvent evt) => PlayHoverSFX();
    private void OnHoverFocus(FocusInEvent evt) => PlayHoverSFX();
}
