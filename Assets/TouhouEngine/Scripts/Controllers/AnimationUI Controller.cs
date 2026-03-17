using UnityEngine;

public class AnimationUIController : MonoBehaviour
{
    [Header("Animation of Button")]
    [SerializeField] public float duration = 0.4f;
    [SerializeField] public float magnitude = 10f;
    void Start()
    {
        if (AnimationUIManager.Instance == null)
        {
            Debug.LogError("AnimationUIManager instance not found. Please ensure there is an AnimationUIManager in the scene.");
            return;
        }
        
        AnimationUIManager.Instance.duration = duration;
        AnimationUIManager.Instance.magnitude = magnitude;
    }

    private void OnValidate()
    {
        if (AnimationUIManager.Instance != null)
        {
            AnimationUIManager.Instance.duration = duration;
            AnimationUIManager.Instance.magnitude = magnitude;
        }
    }
}
