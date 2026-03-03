using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PruebaUI : MonoBehaviour
{
    public InputActionAsset navigateActions;

    Button GameStart_button;
    Button ExtraStart_button;
    Button PracticeStart_button;
    Button Replay_button;
    Button PlayerData_button;
    Button MusicRoom_button;
    Button Option_button;
    Button Quit_button;

    [SerializeField] public AudioClip sfx_button;

    Button[] button_set = new Button[8];

    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;        
        GameStart_button = root.Q<Button>("GameStart");
        ExtraStart_button = root.Q<Button>("ExtraStart");
        PracticeStart_button = root.Q<Button>("PracticeStart");
        Replay_button = root.Q<Button>("Replay");
        PlayerData_button = root.Q<Button>("PlayerData");
        MusicRoom_button = root.Q<Button>("MusicRoom");
        Option_button = root.Q<Button>("Option");
        Quit_button = root.Q<Button>("Quit");

        button_set[0] = GameStart_button;
        button_set[1] = ExtraStart_button;
        button_set[2] = PracticeStart_button;
        button_set[3] = Replay_button;
        button_set[4] = PlayerData_button;
        button_set[5] = MusicRoom_button;
        button_set[6] = Option_button;
        button_set[7] = Quit_button;
    }

    private void OnEnable()
    {
        ButtonActionSuscribe();
        navigateActions.FindActionMap("UI").Enable();
        button_set[0].Focus();
    }
    private void OnDisable()
    {
        ButtonActionUnsuscribe();
        navigateActions.FindActionMap("UI").Disable();
    }

    void ButtonActionSuscribe()
    {
        GameStart_button.clicked += OnGameStartClicked;
        ExtraStart_button.clicked += OnExtraDataClicked;
        PracticeStart_button.clicked += OnPracticeStartClicked;
        Replay_button.clicked += OnReplayClicked;
        PlayerData_button.clicked += OnPlayerDataClicked;
        MusicRoom_button.clicked += OnMusicRoomClicked;
        Option_button.clicked += OnOptionClicked;
        Quit_button.clicked += OnQuitClicked;
    }

    void ButtonActionUnsuscribe()
    {
        GameStart_button.clicked -= OnGameStartClicked;
        ExtraStart_button.clicked -= OnExtraDataClicked;
        PracticeStart_button.clicked -= OnPracticeStartClicked;
        Replay_button.clicked -= OnReplayClicked;
        PlayerData_button.clicked -= OnPlayerDataClicked;
        MusicRoom_button.clicked -= OnMusicRoomClicked;
        Option_button.clicked -= OnOptionClicked;
        Quit_button.clicked -= OnQuitClicked;
    }
    private void OnGameStartClicked() => MenuButtonClicked("Game Start");
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
        SoundManager.Instance.PlaySFX(sfx_button);
        if (message == "Quit") Application.Quit();
    }

    private void MenuButtonEnter(Button btn)
    {
        btn.RegisterCallback<MouseEnterEvent>(evt =>
        {
            SoundManager.Instance.PlaySFX(sfx_button);
        });
    }
}
