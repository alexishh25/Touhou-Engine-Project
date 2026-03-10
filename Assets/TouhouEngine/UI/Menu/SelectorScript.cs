using UnityEngine;
using UnityEngine.UIElements;


public class SelectorScript : MonoBehaviour
{
    Button left_button;
    Button right_button;

    private void Awake()
    {
        if (UIManager.Instance != null) return;
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        left_button = root.Q<Button>("BtnAnterior");
        right_button = root.Q<Button>("BtnSiguiente");
    }

    public void Setup(VisualElement currentRoot)
    {
        //if ()
        //{
            
        //}
    }
}
