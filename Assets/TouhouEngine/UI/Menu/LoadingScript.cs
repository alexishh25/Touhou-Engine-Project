using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingScript : ScreenLogic
{
    public static LoadingScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private VisualElement root;

    [Header("Referencias")]
    [SerializeField] private ParticleSystem VFX;

    private Coroutine Coroutine;

    protected override void DefinirElementos(VisualElement currentroot)
    {
        root = currentroot;
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        
    }

    protected override void LoadData()
    {
        VFX.Simulate(2f, true, false);
        VFX.Play();
    }

    private void Update() 
    {
    
    }


}
