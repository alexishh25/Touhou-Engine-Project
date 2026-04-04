using UnityEngine;

public static class SettingsManager 
{
    private const string BGMVOLUME_KEY = "BGMVolume";
    private const string SFXVOLUME_KEY = "SFXVolume";
    private const string WINDOW_KEY = "WindowMode";

    public static float BGMVolume
    {
        get => PlayerPrefs.GetFloat(BGMVOLUME_KEY, 0.75f);
        set
        {
            PlayerPrefs.SetFloat(BGMVOLUME_KEY, value);
            SoundManager.Instance.SetVolumeBGM(value);
        }
    }
    public static float SFXVolume
    {
        get => PlayerPrefs.GetFloat(SFXVOLUME_KEY, 0.75f);
        set
        {
            PlayerPrefs.SetFloat(SFXVOLUME_KEY, value);
            SoundManager.Instance.SetVolumeSFX(value);
        }
    }

    public static bool WindowMode
    {
        get => PlayerPrefs.GetInt(WINDOW_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(WINDOW_KEY, value ? 1 : 0);
            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }
    }

    public static void LoadSettings()
    {
        SoundManager.Instance.SetVolumeBGM(BGMVolume);
        SoundManager.Instance.SetVolumeSFX(SFXVolume);
        Screen.fullScreenMode = WindowMode ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public static void SaveSettings() => PlayerPrefs.Save();

    public static void ResetSettings()
    {
        PlayerPrefs.DeleteKey(BGMVOLUME_KEY);
        PlayerPrefs.DeleteKey(SFXVOLUME_KEY);
        PlayerPrefs.DeleteKey(WINDOW_KEY);
        LoadSettings();
        SaveSettings();
    }
}
