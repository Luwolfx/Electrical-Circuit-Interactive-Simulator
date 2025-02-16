using UnityEngine;

public class CableSlot : MonoBehaviour, IInteractable
{
    [Header("Slot References")] 
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private ElectricalComponentInfo _info;
    [SerializeField] private ElectricalComponentUIBox _uiInfo;

    [field: Header("Slot Settings")] 
    [field: SerializeField] public SlotType Type {get; private set;}
    [SerializeField] private Material _noneMaterial;
    [SerializeField] private Material _phaseMaterial;
    [SerializeField] private Material _neutralMaterial;
    public enum SlotType { NONE, PHASE, NEUTRAL }
    public CableConnection CurrentConnection { get; private set; } 

    private void Start()
    {
        SetupTypeMaterial();
    }

    private void SetupTypeMaterial()
    {
        switch (Type)
        {
            case SlotType.PHASE:
                _meshRenderer.material = _phaseMaterial;
                break;
            case SlotType.NEUTRAL:
                _meshRenderer.material = _neutralMaterial;
                break;
        }
    }

    public void SetConnection(CableConnection connection)
    {
        CurrentConnection = connection;
    }

    public void StartInteraction(Interactor interactor)
    {
        if(CurrentConnection == null)
        {
            GetBoard().ConnectionManager.StartConnection(this, interactor);
        }
        else
        {
            CurrentConnection.DestroyConnection();
        }
    }

    public void StopInteraction(Interactor interactor)
    {
        if(CurrentConnection == null && GetBoard().ConnectionManager.IsConnecting())
        {
            GetBoard().ConnectionManager.TryEndConnection();
        }
    }

    public void InteractorEnter()
    {
        _uiInfo.SetupAndEnable(_info, "");
    }

    public void InteractorExit()
    {
        _uiInfo.Disable();
    }

    public ElectricalComponent GetElectricalComponent()
    {
        if(transform.parent.gameObject.TryGetComponent(out ElectricalComponent component))
        {
            return component;
        }
        return null;
    }

    public ChallengeBoard GetBoard()
    {
        return GetElectricalComponent()?.GetBoard();
    }

    public void PassEnergy(EC_Generator energyGenerator)
    {
        if(CurrentConnection != null)
            CurrentConnection.PassEnergyToOtherEnd(this, energyGenerator);
    }

    public void ReceiveEnergy(EC_Generator energyGenerator)
    {
        GetElectricalComponent().ReceiveEnergy(this, energyGenerator);
    }

}
