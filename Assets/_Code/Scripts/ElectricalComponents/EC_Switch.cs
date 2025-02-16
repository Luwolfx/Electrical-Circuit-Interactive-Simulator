using System.Collections;
using UnityEngine;

public class Switch : ElectricalComponent, IInteractable
{
    [Header("References")]
    [SerializeField] private AudioSource _onAudio;
    [SerializeField] private AudioSource _offAudio;

    public bool IsOn { get; private set; }
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator UpdateStatusDelayedForAnimation(bool newStatus, float seconds)
    {
        _animator.SetBool("IsOn", newStatus);
        yield return new WaitForSeconds(seconds);
        UpdateStatus(newStatus);
    }

    private void UpdateStatus(bool newStatus)
    {
        IsOn = newStatus;
        _animator.SetBool("IsOn", IsOn);
        if(_currentGenerator) GetBoard().UpdateElectricalComponentsStatus();
        _uiInfo.SetText(Info, GetUIStatus());
        PlayAudio();
    }

    private void PlayAudio()
    {
        if(IsOn)
            _onAudio.Play();
        else
            _offAudio.Play();
    }

    public override bool CanPassEnergy()
    {
        return IsOn;
    }

    public override void StartInteraction(Interactor interactor)
    {
        if(!IsOn)
            StartCoroutine(UpdateStatusDelayedForAnimation(!IsOn, .3f));
        else
            UpdateStatus(!IsOn);
    }

    public override string GetUIStatus()
    {
        string status = HasEnergy && _currentGenerator.CompleteCircuit ? "<color=green>ENERGIZADO</color>" : "<color=red>SEM ENERGIA</color>";
        status += " | "+ (IsOn ? "LIGADO" : "DESLIGADO");
        return status;
    }

    public override void InteractorEnter()
    {
        _uiInfo.SetupAndEnable(Info, GetUIStatus());
    }
}
