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
    public void OnBackClicked()
    {
        _uiMainMenu.OnBackToHostJoinClicked();
    }

    public void OnEnter()
    {
        _clientController.GetRoomList();
    }

    public void RoomListUpdated(List<RoomInfo> rooms)
    {
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


