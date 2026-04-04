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

    private FilledSlider musicVolumeSlider, sfxVolumeSlider;

    private Button windowButton, fullButton, defButton, keyButton, quitButton;

    private Action OnLoadStarted;

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        musicVolumeSlider = currentRoot.Q<FilledSlider>("MusicSlider");
        sfxVolumeSlider = currentRoot.Q<FilledSlider>("SFXSlider");

        windowButton = currentRoot.Q<Button>("WindowButton");
        fullButton = currentRoot.Q<Button>("FullButton");
        defButton = currentRoot.Q<Button>("DefButton");
        keyButton = currentRoot.Q<Button>("KeyButton");
        quitButton = currentRoot.Q<Button>("QuitButton");

        AddButtonIfNotNull(windowButton);
        AddButtonIfNotNull(fullButton);
        AddButtonIfNotNull(defButton);
        AddButtonIfNotNull(keyButton);
        AddButtonIfNotNull(quitButton);
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
            (keyButton, OnKeyButtonClicked),
            (quitButton, OnQuitButtonClicked)
        );
    }

    private void OnWindowButtonClicked()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Debug.Log($"Resolucion actual: {Screen.width}x{Screen.height}");
    }

    private void OnFullButtonClicked()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Debug.Log($"Resolucion actual: {Screen.width}x{Screen.height}");
    }

    private void OnDefButtonClicked()
    {

    }
    private void OnKeyButtonClicked()
    {

    }
    private void OnQuitButtonClicked()
    {

    }
    private void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        SoundManager.Instance.SetVolumeBGM(evt.newValue);
    }

    private void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        SoundManager.Instance.SetVolumeSFX(evt.newValue);
    }

    protected override void LoadData()
    {
        leafs.Simulate(3f, true, false);
        leafs.Play();
        OnLoadStarted?.Invoke();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
