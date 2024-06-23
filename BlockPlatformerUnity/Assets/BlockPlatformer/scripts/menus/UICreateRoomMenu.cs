using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Quantum.Demo;
using UnityEngine;

public class UICreateRoomMenu : MenuScreenBase
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    [SerializeField] private List<MapAsset> _mapAssets = new List<MapAsset>();
    
    
    //private List<MapInfo> _mapInfo;
    private List<UIRoomPlayer> _players = new List<UIRoomPlayer>();
    
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }

    public void OnHostClicked()
    {

        var defaultMapGuid = 0L;

        if (_mapAssets.Count > 0)
        {
            defaultMapGuid = _mapAssets[0].AssetObject.Guid.Value;
        }
        
        var enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = new RoomOptions();
        enterRoomParams.RoomName = "Joe" + "'s Room"; // need to grab name from stord value somewhere - also need to input name somewhere, probably on opening screen
        enterRoomParams.RoomOptions.IsVisible = true;
        enterRoomParams.RoomOptions.MaxPlayers = 2; // this should be a variable set somewhere
        enterRoomParams.RoomOptions.Plugins = new string[] { "QuantumPlugin" };
        enterRoomParams.RoomOptions.CustomRoomPropertiesForLobby = new string[] { "C0" };
        enterRoomParams.Lobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
        enterRoomParams.RoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { 
            { "HIDE-ROOM", false }, 
            { "MAP-GUID", defaultMapGuid }, 
            { "C0", 0 }};

        Debug.Log("Creating a new room...");

        if (!_clientController.Client.OpCreateRoom(enterRoomParams))
        {
            _clientController.Client.Disconnect();
            Debug.LogError($"Failed to send create room operation");
        }
    }
}
