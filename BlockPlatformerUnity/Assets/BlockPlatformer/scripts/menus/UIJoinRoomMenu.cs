using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinRoomMenu : MenuScreenBase
{
    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private ClientConnectionController _clientController;
    [SerializeField] private Transform _roomsScrollContent;
    [SerializeField] private GameObject _roomDetailContainerPrefab;
    [SerializeField] private float _refreshRoomListInterval = 2f;

    private List<RoomInfo> _roomInfoList = new List<RoomInfo>();
    private List<UIRoomDetailContainer> _roomDetailContainers = new List<UIRoomDetailContainer>();
    private float _refreshRoomListTimer = 0;
    private bool _refreshingRoomData = false;
    private int _currentlySelectedRoomIndex = -1;
    private RoomInfo _currentSlectedRoomInfo;
    
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

        if (_currentSlectedRoomInfo == null)
        {
            Debug.Log("Curently selected room info list is null...");
            return;
        }
        
        var joinRoomParams = new EnterRoomParams();
        joinRoomParams.RoomName = _currentSlectedRoomInfo.Name; //  this needs o be controlled by selection!!! - handle this as an index onclick of menu or something
        //joinRoomParams.Lobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
        
        Debug.Log("Attempting to join room... " + _currentSlectedRoomInfo.Name);

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
        _roomDetailContainers = new List<UIRoomDetailContainer>();
        
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
            roomDetailContainer.GetComponent<Button>().onClick.AddListener(() =>
            {
                RoomClicked(rooms.IndexOf(roomInfo));
            });
            _roomDetailContainers.Add(detailContainer);
        }
    }

    public void RoomClicked(int index)
    {
        Debug.Log(index);
        if (_roomInfoList.Count < 1)
        {
            Debug.Log("Room info list currently empty...");
            return;
        }

        foreach (var roomDetailContainer in _roomDetailContainers)
        {
            roomDetailContainer.SetSelected(false);
        }
        _roomDetailContainers[index].SetSelected(true);
        _currentSlectedRoomInfo = _roomInfoList[index];
        _currentlySelectedRoomIndex = index;
    }
}


