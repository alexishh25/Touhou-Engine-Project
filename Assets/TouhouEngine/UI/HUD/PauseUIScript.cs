using UnityEngine;
using UnityEngine.UIElements;

public class PauseUIScript : ScreenLogic
{
    VisualElement _root;
    
    Button Continue_button;
    Button QaRtS_button;
    Button RTG_button;

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        _root = currentRoot;

        Continue_button = currentRoot.Q<Button>("Continue");
        QaRtS_button = currentRoot.Q<Button>("QaRtS");
        RTG_button = currentRoot.Q<Button>("RTG");

        AddButtonIfNotNull(Continue_button);
        AddButtonIfNotNull(QaRtS_button);
        AddButtonIfNotNull(RTG_button);
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        ButtonManager.Instance.ManageButtonActions(active,
            (Continue_button, OnContinueClicked),
            (QaRtS_button, OnQaRtSClicked),
            (RTG_button, OnRTGClicked)
        );
    }

    protected override void LoadData()
    {
        // No data to load for the pause menu
    }

    private void OnContinueClicked()
    {

    }
    private void OnQaRtSClicked() 
    { 
    
    }
    private void OnRTGClicked() 
    { 
    
    }
}
