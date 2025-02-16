using UnityEngine;

public interface IInteractable
{
    public void StartInteraction(Interactor interactor);
    public void StopInteraction(Interactor interactor);
    public void InteractorEnter();
    public void InteractorExit();
}
