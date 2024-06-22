using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetupManager : MonoBehaviour
{
    // this script exists to provide a simple one stop
    // location for handling anything to do with local player loading into
    // scene for the first time

    public static Action<GameObject> PlayerLoaded;
}
