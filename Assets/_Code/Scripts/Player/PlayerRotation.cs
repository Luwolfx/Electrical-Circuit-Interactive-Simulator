using System;
using UnityEngine;

public class PlayerRotation : Rotator
{
    protected override void Rotate()
    {
        if(_rotateInputVector != Vector2.zero)
        {
            transform.rotation *= Quaternion.Euler(0, _rotateInputVector.x * _rotateSpeed * Time.fixedDeltaTime, 0);
        }
    }
}
