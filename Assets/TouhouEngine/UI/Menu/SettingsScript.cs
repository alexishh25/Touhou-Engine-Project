using System;
using UIT_VFX;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsScript : ScreenLogic
{
    [Header("VFX")]
    [SerializeField] private GameObject[] gear = new GameObject[2];
    [SerializeField] private ParticleSystem leafs;

    private Action OnLoadStarted;
    private RotatingElement gear_bck;

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        gear_bck = currentRoot.Q<RotatingElement>("Gear");
        gear_bck?.Subscribe(ref OnLoadStarted);
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {

    }

    protected override void LoadData()
    {
        leafs.Simulate(3f, true, false);
        leafs.Play();
        OnLoadStarted?.Invoke();
    }

    public override void Dispose()
    {
        gear_bck?.Unsubscribe(ref OnLoadStarted);
        base.Dispose();
    }
}
