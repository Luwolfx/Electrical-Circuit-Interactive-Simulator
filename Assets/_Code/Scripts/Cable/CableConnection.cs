using Unity.VisualScripting;
using UnityEngine;

public class CableConnection
{
    public CableSlot FirstSlot { get; private set; }
    public CableSlot SecondSlot { get; private set; }
    public Cable Cable { get; private set; }
    public ConnectionManager Manager { get; private set; }

    public CableConnection(Cable cable, CableSlot slot, ConnectionManager manager)
    {
        Cable = cable;
        FirstSlot = slot;
        Manager = manager;
    }

    public void CloseConnection(CableSlot slot)
    {
        SecondSlot = slot;
        FirstSlot.SetConnection(this);
        slot.SetConnection(this);
    }

    public void UpdateConnection(CableSlot firstSlot, CableSlot secondSlot, Cable cable)
    {
        Cable = cable;
        FirstSlot = firstSlot;
        SecondSlot = secondSlot;
        FirstSlot.SetConnection(this);
        SecondSlot.SetConnection(this);
    }

    public void DestroyConnection()
    {
        FirstSlot.SetConnection(null);
        if(SecondSlot) SecondSlot.SetConnection(null);
        Cable.DestroySelf();
        Manager.RemoveConnection(this);
    }

    public void PassEnergyToOtherEnd(CableSlot slot, EC_Generator energyGenerator)
    {
        if(slot == FirstSlot)
        {
            SecondSlot.ReceiveEnergy(energyGenerator);
        }
        else if(slot == SecondSlot)
        {
            FirstSlot.ReceiveEnergy(energyGenerator);
            Manager.InvertConnection(this);
        }

        if(energyGenerator.CompleteCircuit)
            Cable.ToogleEnergy(true);
    }

    public void DisableCableEnergy()
    {
        Cable.ToogleEnergy(false);
    }

    public CableSlot GetOtherEnd(CableSlot slot)
    {
        if(slot == FirstSlot) return SecondSlot;
        if(slot == SecondSlot) return FirstSlot;
        return null;
    }
}
