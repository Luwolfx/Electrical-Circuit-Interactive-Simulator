using System.Collections.Generic;
using UnityEngine;

public abstract class ElectricalComponent : MonoBehaviour, IInteractable
{
    [field: Header("Component References")]
    [field: SerializeField] public ElectricalComponentInfo Info { get; private set; }
    [SerializeField] protected ElectricalComponentUIBox _uiInfo;
    [SerializeField] protected List<CableSlot> _cableSlots;
    [field:SerializeField] public bool HasEnergy { get; protected set; }
    protected EC_Generator _currentGenerator;

    private List<CableSlot> RemoveSlot(CableSlot slot)
    {
        List<CableSlot> remainingSlots = new List<CableSlot>();
        foreach (CableSlot slotToCheck in _cableSlots)
        {
            if(slotToCheck != slot) remainingSlots.Add(slotToCheck);
        }
        return remainingSlots;
    }

    public ChallengeBoard GetBoard()
    {
        if(transform.parent.gameObject.TryGetComponent(out ChallengeBoard board))
        {
            return board;
        }
        return null;
    }

    public virtual bool CanPassEnergy()
    {
        return true;
    }

    public virtual void ReceiveEnergy(CableSlot slot, EC_Generator energyGenerator)
    {
        HasEnergy = true;
        _currentGenerator = energyGenerator;
        _uiInfo.SetText(Info, GetUIStatus());
        if(CanPassEnergy()) PassEnergy(RemoveSlot(slot));
    }

    public virtual void PassEnergy(List<CableSlot> slots)
    {
        if(!CanPassEnergy()) return;

        foreach(CableSlot slot in slots)
            slot.PassEnergy(_currentGenerator);
    }

    public virtual void ResetStatus()
    {
        _currentGenerator = null;
        HasEnergy = false;
    }

    public virtual string GetUIStatus()
    {
        string status = HasEnergy && _currentGenerator.CompleteCircuit ? "<color=green>ENERGIZADO</color>" : "<color=red>SEM ENERGIA</color>";

        return status;
    }

    public virtual void StartInteraction(Interactor interactor)
    {
        
    }

    public virtual void StopInteraction(Interactor interactor)
    {
        
    }

    public virtual void InteractorEnter()
    {
        _uiInfo.SetupAndEnable(Info, GetUIStatus());
    }

    public virtual void InteractorExit()
    {
        _uiInfo.Disable();
    }
}
