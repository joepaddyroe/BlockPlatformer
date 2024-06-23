using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoinRoomMenu : MenuScreenBase
{
    [SerializeField] private UIMainMenu _uiMainMenu;
    
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }
}
