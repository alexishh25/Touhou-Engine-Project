using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public abstract class ScreenLogic : MonoBehaviour
{
    protected List<Button> buttons = new List<Button>();

    protected abstract void DefinirElementos(VisualElement currentRoot);
    protected abstract void ElementsActionAlterSusYUnsuscribe(bool active);

    protected abstract void LoadData();

    public virtual void Initialize(VisualElement screenRoot)
    {
        if (buttons.Count > 0) Dispose();
        Debug.Log($"Initializing {gameObject.name} screen logic.");
        DefinirElementos(screenRoot);
        ElementsActionAlterSusYUnsuscribe(true);
        ButtonManager.Instance.AlternateRegisterHoverSFX(true, buttons);
        LoadData();
    }

    public virtual void Dispose()
    {
        ElementsActionAlterSusYUnsuscribe(false);
        ButtonManager.Instance.AlternateRegisterHoverSFX(false, buttons);
        buttons.Clear();
    }

    protected void AddButtonIfNotNull(Button btn)
    {
        if (btn != null)
            buttons.Add(btn);
    }

}
