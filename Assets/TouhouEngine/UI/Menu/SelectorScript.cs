using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SelectorScript : ScreenLogic
{
    Button left_button;
    Button right_button;

    protected override void DefinirBotones(VisualElement currentRoot)
    {
        left_button = currentRoot.Q<Button>("BtnAnterior");
        right_button = currentRoot.Q<Button>("BtnSiguiente");

        AddButtonIfNotNull(left_button);
        AddButtonIfNotNull(right_button);
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        ButtonManager.Instance.ManageButtonActions(active,
            (left_button, OnLeftClicked),
            (right_button, OnRightClicked)
        );
    }

    private void OnLeftClicked()
    {
        Debug.Log("Left button clicked");
    }

    private void OnRightClicked()
    {
        Debug.Log("Right button clicked");
    }
}
