using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MenuScript : ScreenLogic
{

    Button GameStart_button;
    Button ExtraStart_button;
    Button PracticeStart_button;
    Button Replay_button;
    Button PlayerData_button;
    Button MusicRoom_button;
    Button Option_button;
    Button Quit_button;

    protected override void DefinirBotones(VisualElement currentRoot)
    {
        GameStart_button = currentRoot.Q<Button>("GameStart");
        ExtraStart_button = currentRoot.Q<Button>("ExtraStart");
        PracticeStart_button = currentRoot.Q<Button>("PracticeStart");
        Replay_button = currentRoot.Q<Button>("Replay");
        PlayerData_button = currentRoot.Q<Button>("PlayerData");
        MusicRoom_button = currentRoot.Q<Button>("MusicRoom");
        Option_button = currentRoot.Q<Button>("Option");
        Quit_button = currentRoot.Q<Button>("Quit");

        AddButtonIfNotNull(GameStart_button);
        AddButtonIfNotNull(ExtraStart_button);
        AddButtonIfNotNull(PracticeStart_button);
        AddButtonIfNotNull(Replay_button);
        AddButtonIfNotNull(PlayerData_button);
        AddButtonIfNotNull(MusicRoom_button);
        AddButtonIfNotNull(Option_button);
        AddButtonIfNotNull(Quit_button);
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        ButtonManager.Instance.ManageButtonActions(active,
            (GameStart_button, OnGameStartClicked),
            (ExtraStart_button, OnExtraDataClicked),
            (PracticeStart_button, OnPracticeStartClicked),
            (Replay_button, OnReplayClicked),
            (PlayerData_button, OnPlayerDataClicked),
            (MusicRoom_button, OnMusicRoomClicked),
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
        UIManager.Instance.ChangeScreen(ScreenType.SelectCharacter);
        MenuButtonClicked("Game Start");
    }
    private void OnExtraDataClicked() => MenuButtonClicked("Extra Start");
    private void OnPracticeStartClicked() => MenuButtonClicked("Practice Start");
    private void OnReplayClicked() => MenuButtonClicked("Replay");
    private void OnPlayerDataClicked() => MenuButtonClicked("Player Data");
    private void OnMusicRoomClicked() => MenuButtonClicked("Music Room");
    private void OnOptionClicked() => MenuButtonClicked("Option");
    private void OnQuitClicked() => MenuButtonClicked("Quit");

    private void MenuButtonClicked(string message)
    {
        Debug.Log(message + " clicked");
        ButtonManager.Instance.PlayClickSFX();
        if (message == "Quit") Application.Quit();
    }
}
