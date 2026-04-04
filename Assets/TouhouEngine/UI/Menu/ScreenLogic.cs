using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public abstract class ScreenLogic : MonoBehaviour
{
    protected List<Button> buttons = new List<Button>();
    protected Button defaultCancelButton; // Button Exit
    [NonSerialized] private InputAction cancelAction;

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

        // Mapping the "Cancel" action from the "UI" action map to our OnCancelActionPerformed method
        var map = InputManager.Instance.inputActions.FindActionMap("UI");
        if (map != null)
        {
            cancelAction = map.FindAction("Cancel");
            if (cancelAction != null)
                cancelAction.performed += OnCancelActionPerformed;
        }
    }

    public virtual void Dispose()
    {
        if (cancelAction != null)
            cancelAction.performed -= OnCancelActionPerformed;

        ElementsActionAlterSusYUnsuscribe(false);
        ButtonManager.Instance.AlternateRegisterHoverSFX(false, buttons);
        buttons.Clear();
    }

    protected void AddButtonIfNotNull(Button btn)
    {
        if (btn != null)
            buttons.Add(btn);
    }

    private void OnCancelActionPerformed(InputAction.CallbackContext ctx)
    {
        if (defaultCancelButton == null) return; 
        // Verificamos si el botón ya tiene el foco
        if (defaultCancelButton.panel?.focusController?.focusedElement == defaultCancelButton)
        {
            using (var e = new NavigationSubmitEvent() { target = defaultCancelButton})
                defaultCancelButton.SendEvent(e);
        }
        else
        {
            // Si no tiene el foco, se lo damos
            defaultCancelButton.Focus();
        }
    }
}
