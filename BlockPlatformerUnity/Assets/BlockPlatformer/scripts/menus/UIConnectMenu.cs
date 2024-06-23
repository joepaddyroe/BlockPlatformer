using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConnectMenu : MenuScreenBase
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    

    // Update is called once per frame
    void Update()
    {
        _clientController.Client?.Service();
    }
    
    private void OnDestroy() {
        if (_clientController.Client != null && _clientController.Client.IsConnected == true) {
            _clientController.Client.Disconnect();
        }
    }


    public void OnConnectClicked()
    {
        string tempName = "Joe";

        var appSettings = PhotonServerSettings.CloneAppSettings(PhotonServerSettings.Instance.AppSettings);

        if (_clientController.Client != null)
        {
            _clientController.Client.RemoveCallbackTarget(_clientController);
        }

        _clientController.Client = new QuantumLoadBalancingClient(PhotonServerSettings.Instance.AppSettings.Protocol);
        _clientController.Client.AddCallbackTarget(_clientController);
        
        if (_clientController.Client.ConnectUsingSettings(appSettings, tempName))
        {
            _uiMainMenu.OnConnectionStarted();
            Debug.Log("Starting Connection Process....");
        }
        else
        {
            Debug.LogError($"Failed to connect with app settings: '{appSettings.ToStringFull()}'");
        }
        
    }
}
