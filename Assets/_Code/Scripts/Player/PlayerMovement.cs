using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _playerInput;
    [SerializeField] private float _moveSpeed = 2f;
    private Rigidbody _rigidbody;
    private Vector2 _moveInputVector;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _playerInput.onMoveEvent += PlayerInputReader_OnMove;
    }

    private void OnDisable()
    {
        _playerInput.onMoveEvent -= PlayerInputReader_OnMove;
    }

    private void PlayerInputReader_OnMove(Vector2 vector)
    {
        _moveInputVector = vector;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movementVector = (transform.forward * _moveInputVector.y + transform.right * _moveInputVector.x).normalized;
        _rigidbody.linearVelocity = movementVector * _moveSpeed * Time.fixedDeltaTime;
    }


}
