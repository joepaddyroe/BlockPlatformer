using System.Collections;
using System.Collections.Generic;
using Quantum.Demo;
using UnityEngine;

public class UICreateRoomMenu : MenuScreenBase
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private List<MapAsset> _mapAssets = new List<MapAsset>();
    
    
    //private List<MapInfo> _mapInfo;
    private List<UIRoomPlayer> _players = new List<UIRoomPlayer>();
    
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }
}
