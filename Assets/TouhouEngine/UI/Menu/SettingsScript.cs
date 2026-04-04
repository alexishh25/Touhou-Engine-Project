using System;
using TouhouEngine.UI.Components;
using UIT_VFX;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsScript : ScreenLogic
{
    [Header("VFX")]
    [SerializeField] private GameObject[] gear = new GameObject[2];
    [SerializeField] private ParticleSystem leafs;

    [Header("Transition Data")]
    [SerializeField] private TransitionScreenData menu;

    private FilledSlider musicVolumeSlider, sfxVolumeSlider;

    private Button bgmButton, sfxButton, windowButton, fullButton, defButton, quitButton;

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        musicVolumeSlider = currentRoot.Q<FilledSlider>("MusicSlider");
        sfxVolumeSlider = currentRoot.Q<FilledSlider>("SFXSlider");

        bgmButton = currentRoot.Q<Button>("BGMButton");
        sfxButton = currentRoot.Q<Button>("SFXButton");
        windowButton = currentRoot.Q<Button>("WindowButton");
        fullButton = currentRoot.Q<Button>("FullButton");
        defButton = currentRoot.Q<Button>("DefButton");
        quitButton = currentRoot.Q<Button>("QuitButton");

        AddButtonIfNotNull(bgmButton);
        AddButtonIfNotNull(sfxButton);
        AddButtonIfNotNull(windowButton);
        AddButtonIfNotNull(fullButton);
        AddButtonIfNotNull(defButton);
        AddButtonIfNotNull(quitButton);

        defaultCancelButton = quitButton;
    }

    protected override void ElementsActionAlterSusYUnsuscribe(bool active)
    {
        SliderManager.Instance.ManageSliderActions(active,
            (musicVolumeSlider, OnMusicVolumeChanged),
            (sfxVolumeSlider, OnSFXVolumeChanged)
        );
        ButtonManager.Instance.ManageButtonActions(active,
            (windowButton, OnWindowButtonClicked),
            (fullButton, OnFullButtonClicked),
            (defButton, OnDefButtonClicked),
            (quitButton, OnQuitButtonClicked)
        );
        bgmButton.RegisterCallback<FocusEvent>(OnButtonFocus);
        sfxButton.RegisterCallback<FocusEvent>(OnButtonFocus);
    }

    protected override void LoadData()
    {
        LoadValuesElements();

        leafs.Simulate(3f, true, false);
        leafs.Play();
    }

    private void LoadValuesElements()
    {
        musicVolumeSlider.value = SettingsManager.BGMVolume;
        sfxVolumeSlider.value = SettingsManager.SFXVolume;
    }

    private void OnButtonFocus(FocusEvent evt)
    {
        if (evt.target is Button button)
        {
            FilledSlider targetSlider = null;

            if (button.name == "BGMButton")
                targetSlider = musicVolumeSlider;
            else if (button.name == "SFXButton")
                targetSlider = sfxVolumeSlider;

            if (targetSlider == null) return;

            button.RegisterCallback<NavigationMoveEvent>(evt =>
            {
                if (evt.direction == NavigationMoveEvent.Direction.Left)
                {
                    targetSlider.value = Mathf.Clamp(targetSlider.value - 0.02f, 0f, 1f);
                    evt.StopPropagation();
                }
                else if (evt.direction == NavigationMoveEvent.Direction.Right)
                {
                    targetSlider.value = Mathf.Clamp(targetSlider.value + 0.02f, 0f, 1f);
                    evt.StopPropagation();
                }
            });
        }
    }

    private void OnWindowButtonClicked()
    {
        ButtonManager.Instance.PlayClickSFX();
        SettingsManager.WindowMode = false;
        Debug.Log($"Resolucion actual: {Screen.width}x{Screen.height}");
    }

    private void OnFullButtonClicked()
    {
        ButtonManager.Instance.PlayClickSFX();
        SettingsManager.WindowMode = true;
        Debug.Log($"Resolucion actual: {Screen.width}x{Screen.height}");
    }

    private void OnDefButtonClicked()
    {
        SettingsManager.ResetSettings();
        LoadValuesElements();
        ButtonManager.Instance.PlayClickSFX();
    }
    private void OnQuitButtonClicked()
    {
        UIManager.Instance.ChangeScreen(ScreenType.MainMenu, menu);
        SettingsManager.SaveSettings();
        ButtonManager.Instance.PlayCancelSFX();
    }
    private void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        SettingsManager.BGMVolume = evt.newValue;
    }

    private void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        SettingsManager.SFXVolume = evt.newValue;
    }

    public override void Dispose()
    {
        bgmButton.UnregisterCallback<FocusEvent>(OnButtonFocus);
        sfxButton.UnregisterCallback<FocusEvent>(OnButtonFocus);
        base.Dispose();
    }
}
