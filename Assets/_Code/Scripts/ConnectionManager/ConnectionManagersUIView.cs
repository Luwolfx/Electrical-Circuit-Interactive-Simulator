using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UITextManager))]
public class ConnectionManagersUIView : MonoBehaviour
{
    private UITextManager _textManager;

    private void Start()
    {
        _textManager = GetComponent<UITextManager>();
        SubscribeToConnectionManagersErrorEvent();
    }

    private void SubscribeToConnectionManagersErrorEvent()
    {
        foreach(ConnectionManager connectionManager in FindObjectsByType<ConnectionManager>(FindObjectsSortMode.InstanceID))
        {
            connectionManager.onConnectionErrorEvent += ConnectionManager_OnConnectionErrorEvent;
        }
    }

    public void SendErrorToUI(string error)
    {
        _textManager.ShowErrorText("Connection", error);
    }

    private void ConnectionManager_OnConnectionErrorEvent(string error)
    {
        SendErrorToUI(error);
    }
}
