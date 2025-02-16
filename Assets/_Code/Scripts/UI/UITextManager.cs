using UnityEngine;
using TMPro;

public class UITextManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _text;

    public void ShowText(string text)
    {
        _text.text = text;
        _animator.SetTrigger("Trigger");
    }

    public void ShowErrorText(string error, string text)
    {
        ShowText($"<color=red><size=75> {error} ERROR! </size>\n {text}");
    }
}
