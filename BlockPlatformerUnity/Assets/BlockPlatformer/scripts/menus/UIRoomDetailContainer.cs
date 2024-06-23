using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoomDetailContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_Text _mapName;
    [SerializeField] private TMP_Text _playerCount;

    public void SetRoomName(string roomName)
    {
        _roomName.text = roomName;
    }
    
    public void SetMapName(string mapName)
    {
        _mapName.text = mapName;
    }
    
    public void SetPlayerCount(int playerCount, int maxCount)
    {
        _playerCount.text = playerCount + "/" + maxCount;
    }
}
