using UnityEngine;

public abstract class Rotator : MonoBehaviour
{
    [Header("Rotator References")]
    [SerializeField] private PlayerInputReader _playerInput;

    [Header("Rotator Settings")]
    [SerializeField] protected float _rotateSpeed = 2f;
    protected Vector2 _rotateInputVector;

    private void OnEnable()
    {
        _playerInput.onLookEvent += PlayerInputReader_OnLook;
    }

    private void OnDisable()
    {
        _playerInput.onLookEvent -= PlayerInputReader_OnLook;
    }
    
    private void PlayerInputReader_OnLook(Vector2 vector)
    {
        _rotateInputVector = vector;
    }

    protected virtual void FixedUpdate()
    {
        Rotate();
    }

    protected abstract void Rotate();
}
