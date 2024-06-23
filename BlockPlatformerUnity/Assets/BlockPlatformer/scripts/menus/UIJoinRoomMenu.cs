using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoinRoomMenu : MenuScreenBase
{
    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }

    public void OnEnter()
    {
        _clientController.GetRoomList();
    }
}
