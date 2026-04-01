using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

public class MenuScript : ScreenLogic
{
    Button GameStart_button;
    Button PracticeStart_button;
    Button Option_button;
    Button Quit_button;

    [SerializeField] private TransitionScreenData transitionScreenData; 

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        GameStart_button = currentRoot.Q<Button>("GameStart");
        PracticeStart_button = currentRoot.Q<Button>("PracticeStart");
        Option_button = currentRoot.Q<Button>("Option");
        Quit_button = currentRoot.Q<Button>("Quit");

        AddButtonIfNotNull(GameStart_button);
        AddButtonIfNotNull(PracticeStart_button);
        AddButtonIfNotNull(Option_button);
        AddButtonIfNotNull(Quit_button);
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        ButtonManager.Instance.ManageButtonActions(active,
            (GameStart_button, OnGameStartClicked),
            (PracticeStart_button, OnPracticeStartClicked),
            (Option_button, OnOptionClicked),
            (Quit_button, OnQuitClicked)
        );
    }

    protected override void LoadData()
    {
        // No data to load for the main menu
    }
    private void OnGameStartClicked()
    {
        UIManager.Instance.ChangeScreen(ScreenType.SelectCharacter, transitionScreenData);
        ButtonManager.Instance.PlayClickSFX();
    }
    private void OnPracticeStartClicked() => MenuButtonClicked("Practice Start");

    private void OnOptionClicked()
    {
        UIManager.Instance.ChangeScreen(ScreenType.Settings, transitionScreenData);
        ButtonManager.Instance.PlayClickSFX();
    }
    private void OnQuitClicked()
    {
        async UniTaskVoid QuitRoutine()
        {
            ButtonManager.Instance.PlayCancelSFX();
            await UniTask.Delay(500);
            Application.Quit();
            Debug.Log("Quit button clicked, exiting application...");
        }
        QuitRoutine().Forget();
    }

    private void MenuButtonClicked(string message)
    {
        ButtonManager.Instance.PlayCancelSFX();
        Debug.Log(message + " clicked");
    }

    
}
