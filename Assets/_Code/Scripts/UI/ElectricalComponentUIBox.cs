using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ElectricalComponentUIBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _Canvas;   
    [SerializeField] private TMP_Text _nameText;   
    [SerializeField] private TMP_Text _descriptionText;   
    [SerializeField] private TMP_Text _statusText;   

    public void SetupAndEnable(ElectricalComponentInfo info, string status)
    {
        SetText(info, status);
        StartCoroutine(SetActive(1f));
    }

    public IEnumerator SetActive(float time)
    {
        yield return new WaitForSeconds(time);
        _Canvas.SetActive(true);
    }

    public void SetText(ElectricalComponentInfo info, string status)
    {
        _nameText.text = info.Name;
        _descriptionText.text = info.Description;
        _statusText.text = status;
    }

    public void Disable()
    {
        StopAllCoroutines();
        _Canvas.SetActive(false);
    }
}
