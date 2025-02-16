using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Interactor References")]
    [SerializeField] private PlayerInputReader _playerInput;

    [Space(50)]
    private IInteractable _hoveringInteractable;
    private IInteractable _currentInteractable;


    void OnEnable()
    {
        _playerInput.onInteractEvent += PlayerInputReader_OnInteractEvent;
    }

    void OnDisable()
    {
        _playerInput.onInteractEvent -= PlayerInputReader_OnInteractEvent;
    }

    private void Update()
    {
        InteractionHover();
    }

    private void InteractionHover()
    {
        IInteractable interactionFound = RaycastCheck<IInteractable>();
        if(interactionFound == null)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow);
            StopHovering();
            return;
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.green);
        if(_hoveringInteractable != interactionFound)
        {
            StartHovering(interactionFound);
        }
    }

    private void StartHovering(IInteractable interactable)
    {
        if(_hoveringInteractable != null) 
            StopHovering();

        interactable.InteractorEnter();
        _hoveringInteractable = interactable;
    }

    private void StopHovering()
    {
        if(_hoveringInteractable != null)
        {
            _hoveringInteractable.InteractorExit();
            _hoveringInteractable = null;
        }
    }

    private void StartInteraction(IInteractable interactable)
    {
        interactable.StartInteraction(this);
        _currentInteractable = interactable;
    }

    private void StopInteraction()
    {
        if(_currentInteractable != null)
        {
            _currentInteractable.StopInteraction(this);
            _currentInteractable = null;
        }
    }

    private void PlayerInputReader_OnInteractEvent(bool toggle)
    {
        if(_hoveringInteractable != null && toggle)
        {
            StartInteraction(_hoveringInteractable);
        }
        else if(!toggle)
        {
            StopInteraction();
        }
    }

    public TValue RaycastCheck<TValue>()
    {
        Vector3 forwardVector = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forwardVector, out RaycastHit raycastHit, 50))
        {
            GameObject objectHit = raycastHit.collider.gameObject;
            if(objectHit.TryGetComponent(out TValue interaction))
            {
                return interaction;
            }
        }
        return default;
    }

    public Vector3 RaycastGetCollisionPoint()
    {
        Vector3 forwardVector = transform.TransformDirection(Vector3.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, forwardVector, out raycastHit, 50))
        {
            return raycastHit.point;
        }
        return Vector3.zero;
    }
}
