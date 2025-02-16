using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Light : ElectricalComponent
{
    public UnityEvent onLightOn;
    public UnityEvent onLightOff;

    private bool _isOn;

    private void OnEnable()
    {
        if(_currentGenerator)
            _currentGenerator.onCompleteCircuitUpdateEvent += EC_Generator_OnCompleteCircuitUpdateEvent;
    }

    private void OnDisable()
    {
        if(_currentGenerator)
            _currentGenerator.onCompleteCircuitUpdateEvent -= EC_Generator_OnCompleteCircuitUpdateEvent;
    }

    private void CheckStatus()
    {
        if(_currentGenerator && _currentGenerator.CompleteCircuit && HasEnergy)
        {
            _isOn = true;
            onLightOn?.Invoke();
        }
        else
        {
            _isOn = false;
            onLightOff?.Invoke();
        }

        _uiInfo.SetText(Info, GetUIStatus());
    }

    public override void ReceiveEnergy(CableSlot slot, EC_Generator energyGenerator)
    {
        energyGenerator.onCompleteCircuitUpdateEvent += EC_Generator_OnCompleteCircuitUpdateEvent;
        base.ReceiveEnergy(slot, energyGenerator);

    }

    public override void PassEnergy(List<CableSlot> slots)
    {
        base.PassEnergy(slots);
    }

    public override void ResetStatus()
    {
        if(_currentGenerator)
            _currentGenerator.onCompleteCircuitUpdateEvent -= EC_Generator_OnCompleteCircuitUpdateEvent;

        base.ResetStatus();

        CheckStatus();
    }

    private void EC_Generator_OnCompleteCircuitUpdateEvent()
    {
        CheckStatus();
    }

    public override string GetUIStatus()
    {
        string status = HasEnergy && _currentGenerator.CompleteCircuit ? "<color=green>ENERGIZADO</color>" : "<color=red>SEM ENERGIA</color>";
        status += " | "+ (_isOn ? "LIGADO" : "DESLIGADO");
        return status;
    }

    public override void InteractorEnter()
    {
        _uiInfo.SetupAndEnable(Info, GetUIStatus());
    }
    
}
