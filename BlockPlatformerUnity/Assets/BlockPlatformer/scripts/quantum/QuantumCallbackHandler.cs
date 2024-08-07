using Quantum;
using UnityEngine;

public class QuantumCallbackHandler : QuantumCallbacks
{
    [SerializeField] private RuntimePlayer _runtimePlayer;
    public override void OnGameStart(Quantum.QuantumGame game) {
        // paused on Start means waiting for Snapshot
        if (game.Session.IsPaused) return;

        foreach (var lp in game.GetLocalPlayers()) {
            Debug.Log("CustomCallbacks - sending player: " + lp);
            game.SendPlayerData(lp, _runtimePlayer);
        }
    }

    public override void OnGameResync(Quantum.QuantumGame game)
    {
        Debug.Log("Detected Resync. Verified tick: " + game.Frames.Verified.Number);
        
        foreach (var lp in game.GetLocalPlayers()) {
            Debug.Log("CustomCallbacks - sending player: " + lp);
            game.SendPlayerData(lp, _runtimePlayer);
        }
    }
}
