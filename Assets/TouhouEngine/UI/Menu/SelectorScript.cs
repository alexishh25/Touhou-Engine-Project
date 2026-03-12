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

    [Header("Animation Shadow")]
    [SerializeField] private float TargetRight = 10f;
    [SerializeField] private float duration = 0.3f;

    [Header("Referencias")]
    [SerializeField] private ParticleSystem VFX;

    private int currentIndex = 0;
    private VisualElement root;

    Button left_button;
    Button right_button;

    VisualElement portrait;
    VisualElement subcontent;

    private InputAction navigateAction, submitAction;

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
        submitAction = UIManager.Instance.navigateActions
            .FindActionMap("UI")
            .FindAction("Submit");

        navigateAction.performed += OnNavigate;
        submitAction.performed += OnSubmit;
        ButtonManager.Instance.AlternateRegisterHoverSFX(false, buttons);

        VFX.Play();
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

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        ButtonManager.Instance.PlayClickSFX();
        
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

        StartCoroutine(SlideShadow());
    }

    private IEnumerator SlideShadow()
    {
        var shadow = root.Q<VisualElement>("Shadow");

        float current = 0f;
        float elapsed = 0f;

        shadow.style.right = new StyleLength(new Length(0f, LengthUnit.Percent));


        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float value = Mathf.Lerp(current, TargetRight, elapsed / duration);
            shadow.style.right = new StyleLength(new Length(value, LengthUnit.Percent));
            yield return null;
        }

        Debug.Log("Terminé la animación del shadow");
    }
}
