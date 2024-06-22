using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Really rediculously simple follow cam for player
// uses a Vector3 Offset and interpolation speed value

public class LocalCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private float _followSpeed;

    private void Update()
    {
        if (!_targetTransform)
        {
            return;
        }
        
        transform.position = Vector3.Lerp(transform.position, _targetTransform.position + _followOffset,
            Time.deltaTime * _followSpeed);
    }
}
