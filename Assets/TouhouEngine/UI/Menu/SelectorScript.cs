using System.Collections;
using UnityEngine;
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

    private CharacterData personaje;
    private int currentIndex = 0;
    private VisualElement root;

    VisualElement left_button;
    VisualElement right_button;

    VisualElement portrait;
    VisualElement subcontent;

    private InputAction navigateAction, submitAction, cancelAction;

    private Coroutine updateDataCoroutine;

    protected override void DefinirElementos(VisualElement currentRoot)
    {
        root = currentRoot;
        left_button = currentRoot.Q<VisualElement>("BtnAnterior");
        right_button = currentRoot.Q<VisualElement>("BtnSiguiente");

        portrait = currentRoot.Q<VisualElement>("Portrait");
        subcontent = currentRoot.Q<VisualElement>("SubContent");
    }

    protected override void ButtonActionAlterSusYUnsuscribe(bool active)
    {
        if (active)
        {
            left_button.RegisterCallback<ClickEvent>(OnLeftClicked);
            right_button.RegisterCallback<ClickEvent>(OnRightClicked); 
        }
        else
        {
            left_button.UnregisterCallback<ClickEvent>(OnLeftClicked);
            right_button.UnregisterCallback<ClickEvent>(OnRightClicked);
        }
    }

    protected override void LoadData()
    {
        var map = InputManager.Instance.inputActions.FindActionMap("UI");
        navigateAction = map.FindAction("Navigate");
        submitAction = map.FindAction("Submit");
        cancelAction = map.FindAction("Cancel");

        navigateAction.performed += OnNavigate;
        submitAction.performed += OnSubmit;
        cancelAction.performed += OnCancel;
        ButtonManager.Instance.AlternateRegisterHoverSFX(false, buttons);

        VFX.Simulate(5f, true, false);
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
        InputManager.Instance.SwitchActionMap("Player");
        UIManager.Instance.InterpolateScreenLoad("Gameplay");

        ButtonManager.Instance.Enable();
        Debug.Log($"Selected character: {characterData.characterDataArray[currentIndex].Name}");
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        ButtonManager.Instance.PlayCancelSFX();
        UIManager.Instance.ChangeScreen(ScreenType.MainMenu);
    }

    private void OnLeftClicked(ClickEvent evt)
    {
        SwitchingSelection(false);
    }

    private void OnRightClicked(ClickEvent evt)
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
    }

    public override void Dispose()
    {
        navigateAction.performed -= OnNavigate;
        submitAction.performed -= OnSubmit;
        cancelAction.performed -= OnCancel;
        base.Dispose();
    }
}
