using UnityEngine;
using System.Globalization;

public class PlayerAnimationController : MonoBehaviour
{
    public float currentVal = 0.0f;

    public static PlayerAnimationController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void AlterarProgresivamenteBlend(Animator animator, string nombre, Vector2 moveinput)
    {
        if (moveinput.x == 0.0f)
        {
            animator.SetFloat(nombre, moveinput.x);
            return;
        }
        currentVal = Mathf.Lerp(currentVal, moveinput.x, 8 * Time.deltaTime);
        animator.SetFloat(nombre, currentVal);
        Debug.Log("Blend Alterado a: " + currentVal);
    }
}
