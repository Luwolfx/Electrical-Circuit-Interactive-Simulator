using System;
using UnityEngine;

public class EC_Generator : ElectricalComponent
{
    [Header("References")]
    [SerializeField] private AudioSource _sparkAudio;
    public bool CompleteCircuit { get; protected set; }

    public Action onCompleteCircuitUpdateEvent;

    private void Start()
    {
        HasEnergy = true;
    }

    public override bool CanPassEnergy()
    {
        return false;
    }

    public override void ResetStatus()
    {
        HasEnergy = true;
        CompleteCircuit = false;
        onCompleteCircuitUpdateEvent?.Invoke();
    }

    public override void ReceiveEnergy(CableSlot slot, EC_Generator energyGenerator)
    {
        if(energyGenerator == this)
        {
            CompleteCircuit = true;
            onCompleteCircuitUpdateEvent?.Invoke();
            _sparkAudio.Play();
        }
    }

    public void StartEnergy()
    {
        CableSlot phaseSlot = _cableSlots.Find(x => x.Type == CableSlot.SlotType.PHASE);
        phaseSlot.PassEnergy(this);
    }

    public override void InteractorEnter()
    {
        _uiInfo.SetupAndEnable(Info, "");
    }
}
