using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController Instance { get; private set; }

    [Header("Referencias a las Transiciones de la Escena")]
    [Tooltip("El PlayableDirector usado para transiciones principales del Menú (ej: Replay, PlayerData)")]
    [SerializeField] private PlayableDirector mainTransitionDirector;

    // Aquí puedes añadir todos los directores específicos de esta escena que necesites:
    // [SerializeField] private PlayableDirector introCinematic;
    // [SerializeField] private PlayableDirector selectCharacterTransition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // === MÉTODOS INTERMEDIARIOS PARA EL RESTO DE TUS SCRIPTS === //

    /// <summary>
    /// Reproduce la transición genérica principal asignada en este controlador.
    /// </summary>
    public void PlayMainTransition(bool reverse = false)
    {
        if (mainTransitionDirector == null)
        {
            Debug.LogWarning("Falta asignar mainTransitionDirector en el TimelineController.");
            return;
        }

        TimelineManager.Instance.PlayForward(mainTransitionDirector, reverse);
    }

    /// <summary>
    /// Método genérico por si necesitas pasar otro PlayableDirector directamente al Manager.
    /// </summary>
    public void PlayTimeline(PlayableDirector director, bool reverse = false)
    {
        TimelineManager.Instance.PlayForward(director, reverse);
    }

    // Aquí abajo puedes crear más métodos si tienes transiciones muy específicas
    // public void PlayCharacterSelectIntro() { TimelineManager.Instance.PlayForward(selectCharacterTransition, false); }
}
