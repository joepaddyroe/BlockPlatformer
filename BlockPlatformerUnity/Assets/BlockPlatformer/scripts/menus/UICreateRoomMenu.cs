using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Quantum.Demo;
using TMPro;
using UnityEngine;

public class UICreateRoomMenu : MenuScreenBase
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    [SerializeField] private List<MapAsset> _mapAssets = new List<MapAsset>();
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private GameObject _createRoomButton;
    [SerializeField] private TMP_Dropdown _selectMapDropDown;
    
    private bool _roomNameEntered = false;
    private string _roomName = "";
    private int _currentMapSelectionIndex = -1;
    private MapAsset _currentlySelectedMapAsset;
    
    //private List<MapInfo> _mapInfo;
    private List<UIRoomPlayer> _players = new List<UIRoomPlayer>();
    
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }
    
    public void OnEnter()
    {
        if (_roomName == "")
        {
            _roomName = _clientController.CurrentPlayerName + "'s Room";
        }
        _roomNameInputField.text = _roomName;
        
        _roomNameInputField.onValueChanged.AddListener(OnRoomNameInputChanged);
        SetupMapOptions();
        _selectMapDropDown.onValueChanged.AddListener(OnMapSelected);
    }

    private void OnDestroy()
    {
        _roomNameInputField.onValueChanged.RemoveListener(OnRoomNameInputChanged);
        _selectMapDropDown.onValueChanged.RemoveListener(OnMapSelected);
    }

    private void OnRoomNameInputChanged(string newValue)
    {
        _roomName = newValue;
    }
    
    private void OnMapSelected(int selectionIndex)
    {
        _currentMapSelectionIndex = selectionIndex;
        _currentlySelectedMapAsset = _mapAssets[selectionIndex];
    }

    private void SetupMapOptions()
    {
        _currentlySelectedMapAsset = _mapAssets[0];
        
        _selectMapDropDown.options = new List<TMP_Dropdown.OptionData>();
        foreach (MapAsset mapAsset in _mapAssets)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = mapAsset.name;
            _selectMapDropDown.options.Add(option);
        }
        _selectMapDropDown.value = 0;
        _selectMapDropDown.RefreshShownValue();
    }

    void Update()
    {
        _roomNameEntered = _roomName != "";
        _createRoomButton.SetActive(_roomNameEntered);
    }

    public void OnHostClicked()
    {
        var defaultMapGuid = 0L;

        if (_mapAssets.Count < 1)
        {
            Debug.Log("No map assets to load.....");
            return;
        }
        
        defaultMapGuid = _currentlySelectedMapAsset.AssetObject.Guid.Value;
        
        var enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = new RoomOptions();
        enterRoomParams.RoomName = _roomName;
        enterRoomParams.RoomOptions.IsVisible = true;
        enterRoomParams.RoomOptions.MaxPlayers = ClientConnectionController.MAX_PLAYER_COUNT;
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

        _clientController.StartHostingClicked();
    }
    
}
