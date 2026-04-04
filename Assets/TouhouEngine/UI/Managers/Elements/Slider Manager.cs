using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SliderManager : MonoBehaviour
{
    public static SliderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void ManageSliderActions(bool subscribe, params (Slider slider, EventCallback<ChangeEvent<float>> handler)[] sliderActions)
    {
        foreach (var (slider, handler) in sliderActions)
        {
            if (slider == null || handler == null) continue;

            if (subscribe)
            {
                slider.RegisterValueChangedCallback(handler);
            }
            else
            {
                slider.UnregisterValueChangedCallback(handler);
            }
        }
    }

    /// <summary>
    /// Overload para SliderInt u otros BaseSlider de tipo int
    /// </summary>
    public void ManageSliderActions(bool subscribe, params (BaseSlider<int> slider, EventCallback<ChangeEvent<int>> handler)[] sliderActions)
    {
        foreach (var (slider, handler) in sliderActions)
        {
            if (slider == null || handler == null) continue;

            if (subscribe)
            {
                slider.RegisterValueChangedCallback(handler);
            }
            else
            {
                slider.UnregisterValueChangedCallback(handler);
            }
        }
    }
}
