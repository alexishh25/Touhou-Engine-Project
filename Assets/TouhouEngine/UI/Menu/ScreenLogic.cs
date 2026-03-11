using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ScreenLogic : MonoBehaviour
{
    protected List<Button> buttons = new List<Button>();

    protected abstract void DefinirBotones(VisualElement currentRoot);
    protected abstract void ButtonActionAlterSusYUnsuscribe(bool active);

    protected abstract void LoadData();

    public virtual void Initialize(VisualElement screenRoot)
    {
        if (buttons.Count > 0) Dispose();
        Debug.Log($"Initializing {gameObject.name} screen logic.");
        DefinirBotones(screenRoot);
        ButtonActionAlterSusYUnsuscribe(true);
        ButtonManager.Instance.AlternateRegisterHoverSFX(true, buttons);
        if (buttons.Count != 0) buttons[0].Focus();
        LoadData();
    }

    public void Dispose()
    {
        ButtonActionAlterSusYUnsuscribe(false);
        ButtonManager.Instance.AlternateRegisterHoverSFX(false, buttons);
        buttons.Clear();
    }

    protected void AddButtonIfNotNull(Button btn)
    {
        if (btn != null)
            buttons.Add(btn);
    }
}
