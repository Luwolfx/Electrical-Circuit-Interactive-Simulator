using UnityEngine;

public class CameraRotation : Rotator
{

    [SerializeField] [Range(10f, 100f)] float _rotationLimit = 45f;
    private float rotationY = 0f;

    protected override void Rotate()
    {
        rotationY += -_rotateInputVector.y * _rotateSpeed * Time.fixedDeltaTime;
        rotationY = Mathf.Clamp(rotationY, -_rotationLimit, _rotationLimit);
        transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }
}
