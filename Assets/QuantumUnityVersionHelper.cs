using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This only exists as I have run into an issue with
// Unity 2021.3.14f1 not having 'FindFirstObjectByType'
// - this needs to be fixed but solving like this with static helper for now


public class QuantumUnityVersionHelper
{
    static T FindFirstObjectByType<T>()
    {
        return FindObjectsOfType<T>()[0];
    }
}
