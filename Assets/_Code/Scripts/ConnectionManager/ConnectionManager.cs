using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [field: Header("References")]
    [field:SerializeField] public Cable CablePrefab { get; private set; }
    [field:SerializeField] public GameObject ConnectorPrefab { get; private set; }
    [SerializeField] private AudioSource _connectAudio;
    [SerializeField] private AudioSource _wrongConnectAudio;

    public enum ConnectionManagerStatus { NONE, CONNECTING }
    public ConnectionManagerStatus Status { get; private set; }

    public List<CableConnection> Connections { get; private set; } = new List<CableConnection>();
    private CableConnection _currentConnection;
    private Transform _currentConnector;
    private Interactor _currentInteractor;
    public Action onConnectionsUpdateEvent;
    public Action<string> onConnectionErrorEvent;

    private void Update()
    {
        if(Status == ConnectionManagerStatus.CONNECTING)
        {
            Connecting();
        }
    }

    private void Connecting()
    {
        if(_currentConnector == null)
        {
            _currentConnector = Instantiate(ConnectorPrefab).transform;
            _currentConnector.SetParent(transform);
        }

        if(_currentInteractor != null)
        {
            _currentConnector.position = _currentInteractor.RaycastGetCollisionPoint();
            _currentConnection.Cable.UpdateEndPointPosition(_currentConnector.position);
        }
    }

    private void ResetConnectionStatus()
    {
        _currentConnection = null;
        Status = ConnectionManagerStatus.NONE;
        _currentInteractor = null;
        Destroy(_currentConnector.gameObject);
    }

    private void ErrorConnecting(string errorText)
    {
        _currentConnection.DestroyConnection();
        ResetConnectionStatus();
        _wrongConnectAudio.Play();
        onConnectionErrorEvent?.Invoke(errorText);
    }

    private void EndConnection(CableSlot slot)
    {
        _currentConnection.Cable.UpdateEndPointPosition(slot.transform.position);
        _currentConnection.CloseConnection(slot);
        Connections.Add(_currentConnection);
        ResetConnectionStatus();

        onConnectionsUpdateEvent?.Invoke();
        _connectAudio.Play();
    }

    public void StartConnection(CableSlot slot, Interactor interactor)
    {
        Status = ConnectionManagerStatus.CONNECTING;
        _currentInteractor = interactor;

        Cable instantiatedCable = Instantiate(CablePrefab);
        instantiatedCable.transform.parent = transform;
        _currentConnection = new CableConnection(instantiatedCable, slot, this);
        _currentConnection.Cable.SetStartPoint(slot.transform.position);

        _connectAudio.Play();
    }

    public void TryEndConnection()
    {
        if(Status != ConnectionManagerStatus.CONNECTING)
        {
            ErrorConnecting("Você precisa começar uma conexão antes de poder termina-la!");
            return;
        }

        CableSlot endSlot = _currentInteractor.RaycastCheck<CableSlot>();
        if(endSlot == null)
        {
            ErrorConnecting("Você precisa conectar o cabo em um slot válido!");
            return;
        }

        if(endSlot == _currentConnection.FirstSlot)
        {
            ErrorConnecting("Você não pode conectar as duas pontas do cabo no mesmo slot!");
            return;
        }

        if(endSlot.Type != CableSlot.SlotType.NONE && endSlot.Type == _currentConnection.FirstSlot.Type)
        {
            ErrorConnecting("Ambos os Slots tem a mesma polaridade!");
            return;
        }

        if(_currentConnection.FirstSlot.GetBoard() != endSlot.GetBoard())
        {
            ErrorConnecting("Você não pode conectar slots de quadros diferentes!");
            return;
        }

        if(_currentConnection.FirstSlot.GetElectricalComponent() == endSlot.GetElectricalComponent())
        {
            ErrorConnecting("Você não pode conectar slots do mesmo componente!");
            return;
        }

        if(endSlot.CurrentConnection != null)
        {
            endSlot.CurrentConnection.DestroyConnection();
        }

        EndConnection(endSlot);
    }

    public void RemoveConnection(CableConnection connection)
    {
        if(Connections.Contains(connection))
        {
            Connections.Remove(connection);
            onConnectionsUpdateEvent?.Invoke();
            _wrongConnectAudio.Play();
        }
    }

    public void InvertConnection(CableConnection connection)
    {
        connection.Cable.SetStartPoint(connection.SecondSlot.transform.position);
        connection.Cable.UpdateEndPointPosition(connection.FirstSlot.transform.position);

        connection.UpdateConnection(connection.SecondSlot, connection.FirstSlot, connection.Cable);
    }

    public void DisableAllEnergyCables()
    {
        foreach(CableConnection connection in Connections)
        {
            connection.DisableCableEnergy();
        }
    }

    public bool IsConnecting()
    {
        return Status == ConnectionManagerStatus.CONNECTING;
    }
}
