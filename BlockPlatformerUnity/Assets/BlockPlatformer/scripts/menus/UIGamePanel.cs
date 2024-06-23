using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePanel : MenuScreenBase
{
    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    
    public void OnDisconnectClicked()
    {
        _clientController.Client.Disconnect();
        _uiMainMenu.GoToConnectPanel();
    }
}
