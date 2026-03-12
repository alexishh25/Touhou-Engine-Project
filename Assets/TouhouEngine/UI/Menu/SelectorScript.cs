using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class SelectorScript : ScreenLogic
{
    [SerializeField] private SelectorCharacterData characterData;

    [Header("Variables")]
    [SerializeField] private float transitionDuration = 0.2f;

    private int currentIndex = 0;
    private VisualElement root;

    Button left_button;
    Button right_button;

    VisualElement portrait;
    VisualElement subcontent;

    private InputAction navigateAction;

    private Coroutine updateDataCoroutine;

    protected override void DefinirBotones(VisualElement currentRoot)
    {
        root = currentRoot;
        left_button = currentRoot.Q<Button>("BtnAnterior");
        right_button = currentRoot.Q<Button>("BtnSiguiente");
        portrait = currentRoot.Q<VisualElement>("Portrait");
        subcontent = currentRoot.Q<VisualElement>("SubContent");
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
        navigateAction = UIManager.Instance.navigateActions
            .FindActionMap("UI")
            .FindAction("Navigate");

        navigateAction.performed += OnNavigate;
        ButtonManager.Instance.AlternateRegisterHoverSFX(false, buttons);
        UpdateData();
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        if (input.x > 0.5f)
            SwitchingSelection(true);
        else if (input.x < -0.5f)
            SwitchingSelection(false);
    }

    private void OnLeftClicked()
    {
        SwitchingSelection(false);
    }

    private void OnRightClicked()
    {
        SwitchingSelection(true);
    }

    private void SwitchingSelection(bool @switch)
    {
        ButtonManager.Instance.PlayHoverSFX();
        if (@switch)
        {
            currentIndex++;
            if (currentIndex >= characterData.characterDataArray.Length) currentIndex = 0;
            UpdateData();
        }
        else
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = characterData.characterDataArray.Length - 1;
            UpdateData();
        }
    }

    private void UpdateData()
    {
        if (updateDataCoroutine != null)
            StopCoroutine(updateDataCoroutine);

        updateDataCoroutine = StartCoroutine(UpdateDataCoroutine());
    }

    private IEnumerator UpdateDataCoroutine()
    {
        var data = characterData.characterDataArray[currentIndex];

        portrait.style.opacity = 0;
        subcontent.style.opacity = 0;

        yield return new WaitForSeconds(transitionDuration);

        root.Q<Label>("Subdes").text = data.subdesc;
        root.Q<Label>("Subdes").style.color = new StyleColor(data.colorSubdes);
        root.Q<Label>("Title").text = data.Name;
        root.Q<Label>("Title").style.color = new StyleColor(data.colorName);
        root.Q<Label>("Description").text = data.desc;
        root.Q<VisualElement>("Character").style.backgroundImage = new StyleBackground(data.Sprite);
        root.Q<VisualElement>("Shadow").style.backgroundImage = new StyleBackground(data.Sprite_shadow);

        portrait.style.opacity = 1;
        subcontent.style.opacity = 1;
    }
}
