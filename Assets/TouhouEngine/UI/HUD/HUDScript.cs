using UnityEngine;
using UnityEngine.UIElements;

public class HUDScript : ScreenLogic
{
    VisualElement _root;

    [SerializeField] private PauseUIScript _useUIScript;

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        _root = currentRoot;
    }
    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        // No buttons to manage in the HUD
    }

    protected override void LoadData()
    {
        // No data to load for the HUD
    }
}
