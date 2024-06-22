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


    private void Awake()
    {
        PlayerSetupManager.PlayerLoaded += HandlePlayerLoaded;
    }

    private void OnDestroy()
    {
        PlayerSetupManager.PlayerLoaded -= HandlePlayerLoaded;
    }

    private void HandlePlayerLoaded(GameObject player)
    {
        _targetTransform = player.transform;
    }
    
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
