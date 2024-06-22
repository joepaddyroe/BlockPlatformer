using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField] private EntityView _entityView;

    // called by an entity view event
    public void OnEntityInstantiated()
    {
        // grabbing the latest verified frame
        // checking that the frame actually has the EntityRef - it should as this has just been
        // instantiated
        // check that Player value of the playerLink is the local player and send its
        // GameObject ref to the PlayerSetupManager
        
        Frame frame = QuantumRunner.Default.Game.Frames.Verified;

        if (frame != null)
        {
            if (frame.TryGet(_entityView.EntityRef, out PlayerLink playerLink))
            {
                if (QuantumRunner.Default.Game.PlayerIsLocal(playerLink.Player))
                {
                    PlayerSetupManager.PlayerLoaded.Invoke(gameObject);
                }
            }
        }
    }
}
