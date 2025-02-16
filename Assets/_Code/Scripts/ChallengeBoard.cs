using System.Collections.Generic;
using UnityEngine;

public class ChallengeBoard : MonoBehaviour
{
    public ConnectionManager ConnectionManager { get; private set; }
    public List<ElectricalComponent> ElectricalComponentsList { get; private set; }
    public List<EC_Generator> GeneratorsList { get; private set; }

    private void OnEnable()
    {
        if(ConnectionManager)
            ConnectionManager.onConnectionsUpdateEvent += ConnectionManager_OnConnectionsUpdateEvent;
    }

    private void OnDisable()
    {
        if(ConnectionManager)
            ConnectionManager.onConnectionsUpdateEvent -= ConnectionManager_OnConnectionsUpdateEvent;
    }

    private void Start()
    {
        ConnectionManager = GetComponent<ConnectionManager>();
        ConnectionManager.onConnectionsUpdateEvent += ConnectionManager_OnConnectionsUpdateEvent;

        ElectricalComponentsList = new List<ElectricalComponent>(FindAllComponents<ElectricalComponent>());
        GeneratorsList = new List<EC_Generator>(FindAllComponents<EC_Generator>());
    }

    private List<TValue> FindAllComponents<TValue>()
    {
        List<TValue> electricalComponents = new List<TValue>();

        foreach (Transform child in transform)
        {
            if(child.TryGetComponent(out TValue component))
            {
                electricalComponents.Add(component);
            }
        }
        return electricalComponents;
    }

    public void UpdateElectricalComponentsStatus()
    {
        ConnectionManager.DisableAllEnergyCables();
        foreach(ElectricalComponent component in ElectricalComponentsList)
        {
            component.ResetStatus();
        }
        foreach(EC_Generator generator in GeneratorsList)
        {
            generator.StartEnergy();
        }
    }

    private void ConnectionManager_OnConnectionsUpdateEvent()
    {
        UpdateElectricalComponentsStatus();
    }
}
