using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class ClientConnectionController : MonoBehaviour, IConnectionCallbacks
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    
    public QuantumLoadBalancingClient Client { get; set; }
    void Update()
    {
        Client?.Service();
    }
    
    private void OnDestroy() {
        if (Client != null && Client.IsConnected == true) {
            Client.Disconnect();
        }
    }
    
    
    //IConnectionCallbacks
    
    public void OnConnected()
    {
        Debug.Log("CONNECTED!!!!!!!!");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED TO MASTER!!!!!!!!");
        _uiMainMenu.GoToHostOrJoinPanel();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        throw new System.NotImplementedException();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("Region list received:" + JsonUtility.ToJson(regionHandler.EnabledRegions));
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }
}
