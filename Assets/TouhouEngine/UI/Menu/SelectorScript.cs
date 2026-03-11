using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SelectorScript : ScreenLogic
{
    [SerializeField] private SelectorCharacterData characterData;

    private int currentIndex = 0;
    private VisualElement root;

    Button left_button;
    Button right_button;

    protected override void DefinirBotones(VisualElement currentRoot)
    {
        root = currentRoot;
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

    protected override void LoadData()
    {
        UpdateData();
    }

    private void OnLeftClicked()
    {
        currentIndex--;
        ButtonManager.Instance.PlayClickSFX();
        if (currentIndex < 0) currentIndex = characterData.characterDataArray.Length - 1;
        UpdateData();
        Debug.Log("Left button clicked");
    }

    private void OnRightClicked()
    {
        currentIndex++;
        ButtonManager.Instance.PlayClickSFX();
        if (currentIndex >= characterData.characterDataArray.Length) currentIndex = 0;
        UpdateData();
        Debug.Log("Right button clicked");
    }

    private void UpdateData()
    {
        var data = characterData.characterDataArray[currentIndex];

        root.Q<Label>("Subdes").text = data.subdesc;
        root.Q<Label>("Subdes").style.color = new StyleColor(data.colorSubdes);

        root.Q<Label>("Title").text = data.Name;
        root.Q<Label>("Title").style.color = new StyleColor(data.colorName);

        root.Q<Label>("Description").text = data.desc;

        var sprite = root.Q<VisualElement>("Character");
        sprite.style.backgroundImage = new StyleBackground(data.Sprite);
        root.Q<VisualElement>("Shadow").style.backgroundImage = new StyleBackground(data.Sprite_shadow);

        sprite.style.opacity = 0;
        sprite.schedule.Execute(() =>
        {
            sprite.style.backgroundImage = new StyleBackground(data.Sprite);
            sprite.style.opacity = 1;
        }).StartingIn(400);
    }
}
