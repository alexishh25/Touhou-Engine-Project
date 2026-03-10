using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance { get; private set; }

    [Header("Efectos de sonido")]

    [SerializeField] public AudioClip sfx_buttonhover;
    [SerializeField] public AudioClip sfx_clickbutton;

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


}
