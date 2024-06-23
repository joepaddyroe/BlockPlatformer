using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHostJoinMenu : MenuScreenBase
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;

    public void OnHostClicked()
    {
        _uiMainMenu.OnHostClicked();
    }
    
    public void OnJoinClicked()
    {
        _uiMainMenu.OnJoinClicked();
    }
}
