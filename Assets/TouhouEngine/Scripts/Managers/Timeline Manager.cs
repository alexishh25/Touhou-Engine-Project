using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    // Almacena los directores que actualmente están reproduciéndose en reversa
    private List<PlayableDirector> reversingDirectors = new List<PlayableDirector>();

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayForward(PlayableDirector timeline, bool reverse = false)
    {
        if (timeline == null)
        {
            Debug.LogError("PlayableDirector es null");
            return;
        }

        if (reverse)
        {
            // Registramos que el director va en reversa
            if (!reversingDirectors.Contains(timeline))
                reversingDirectors.Add(timeline);

            // Detenemos su lógica de avance automática normal
            timeline.Pause();

            // Seteamos el tiempo al final y forzamos la evaluación de ese último frame
            timeline.time = timeline.duration;
            timeline.Evaluate();
        }
        else
        {
            // Si antes estaba en reversa, lo sacamos de la lista
            if (reversingDirectors.Contains(timeline))
                reversingDirectors.Remove(timeline);

            // Reproducción hacia adelante normal
            timeline.time = 0;
            timeline.Play();
            if (timeline.playableGraph.IsValid())
            {
                timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
            }
        }
    }

    private void RegistroEnReversa()
    {
        // Procesamos todos los timelines que deban ir en reversa manualmente
        for (int i = reversingDirectors.Count - 1; i >= 0; i--)
        {
            PlayableDirector director = reversingDirectors[i];

            // Si el director dejó de existir o fue destruido, se remueve de la lista
            if (director == null)
            {
                reversingDirectors.RemoveAt(i);
                continue;
            }

            // Restamos el tiempo
            director.time -= Time.deltaTime;

            // Llegamos al principio (tiempo 0)
            if (director.time <= 0)
            {
                director.time = 0;
                director.Evaluate(); // Última evaluación en tiempo 0 para asentar estados finales
                reversingDirectors.RemoveAt(i); // Terminó la animación
            }
            else
            {
                // Forzamos evaluación para actualizar los visuales
                director.Evaluate();
            }
        }
    }

    private void Update()
    {
        RegistroEnReversa();
    }
}
