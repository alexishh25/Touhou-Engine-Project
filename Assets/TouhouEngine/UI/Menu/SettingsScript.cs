using System;
using UIT_VFX;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsScript : ScreenLogic
{
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
        // Simulate loading data with a delay
        OnLoadStarted?.Invoke();
    }

    public override void Dispose()
    {
        gear_bck?.Unsubscribe(ref OnLoadStarted);
        base.Dispose();
    }
}
