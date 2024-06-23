using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIJoinRoomMenu : MenuScreenBase
{
    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    [SerializeField] private Transform _roomsScrollContent;
    [SerializeField] private GameObject _roomDetailContainerPrefab;
    [SerializeField] private float _refreshRoomListInterval = 2f;

    private List<RoomInfo> _roomInfoList = new List<RoomInfo>();
    private float _refreshRoomListTimer = 0;
    private bool _refreshingRoomData = false;
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }

    public void OnJoinRoomClicked()
    {

        if (_roomInfoList.Count < 1)
        {
            Debug.Log("Room info list is empty...");
            return;
        }
        
        var joinRoomParams = new EnterRoomParams();
        joinRoomParams.RoomName = _roomInfoList[0].Name; //  this needs o be controlled by selection!!! - handle this as an index onclick of menu or something
        //joinRoomParams.Lobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
        
        Debug.Log("Attempting to join room...");

        _refreshingRoomData = false;
        
        if (!_clientController.Client.OpJoinRoom(joinRoomParams))
        {
            _clientController.Client.Disconnect();
            Debug.Log("Failed to join room....");
        }
    }

    public void OnEnter()
    {
        _clientController.GetRoomList();
        _refreshingRoomData = true;
    }

    void Update()
    {
        if (!_refreshingRoomData)
            return;
        
        if (_refreshRoomListTimer >= _refreshRoomListInterval)
        {
            _refreshRoomListTimer = 0;
            _clientController.GetRoomList();
        }

        _refreshRoomListTimer += Time.deltaTime;
    }

    public void RoomListUpdated(List<RoomInfo> rooms)
    {
        _roomInfoList = rooms;
        
        // first clear the existing room details if any
        foreach (Transform detail in _roomsScrollContent)
        {
            Destroy(detail.gameObject);
        }

        foreach (RoomInfo roomInfo in rooms)
        {
            GameObject roomDetailContainer = Instantiate(_roomDetailContainerPrefab, _roomsScrollContent, false);
            UIRoomDetailContainer detailContainer = roomDetailContainer.GetComponent<UIRoomDetailContainer>();
            detailContainer.SetRoomName(roomInfo.Name);
            detailContainer.SetMapName("Fix This");
            detailContainer.SetPlayerCount(roomInfo.PlayerCount, roomInfo.MaxPlayers);
        }
    }
}


