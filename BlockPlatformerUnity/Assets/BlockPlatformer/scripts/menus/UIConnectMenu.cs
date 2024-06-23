using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIConnectMenu : MenuScreenBase
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private GameObject _connectButton;
    
    private bool _nameEntered = false;
    
    void Start()
    {
        _nameInputField.onValueChanged.AddListener(OnNameInputChanged);
    }
    
    void Update()
    {
        _clientController.Client?.Service();

        _nameEntered = _nameInputField.text != "";
        
        _connectButton.SetActive(_nameEntered);
    }
    
    private void OnDestroy() {
        if (_clientController.Client != null && _clientController.Client.IsConnected == true) {
            _clientController.Client.Disconnect();
        }
        
        _nameInputField.onValueChanged.RemoveListener(OnNameInputChanged);
    }

    private void OnNameInputChanged(string newValue)
    {
        _clientController.SetPlayerName(newValue);
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
