using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class ClientConnectionController : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{

    [SerializeField] private UIMainMenu _uiMainMenu;

    private string _currentPlayerName;

    public string CurrentPlayerName => _currentPlayerName;
    
    public QuantumLoadBalancingClient Client { get; set; }
    void Update()
    {
        Client?.Service();
    }
    
    private void OnDestroy() {
        if (Client != null && Client.IsConnected == true) {
            Client.Disconnect();
        }
    }


    // public accesors

    public void SetPlayerName(string newName)
    {
        _currentPlayerName = newName;
    }

    public void GetRoomList()
    {
        TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
        if (!Client.OpGetGameList(sqlLobby, "C0 = 0"))
        {
            Client.Disconnect();
            Debug.LogError($"Failed to send get rooms list operation.");
        }
    }
    
    
    
    
    //IConnectionCallbacks
    
    public void OnConnected()
    {
        Debug.Log("CONNECTED!!!!!!!!");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED TO MASTER!!!!!!!!");
        _uiMainMenu.GoToHostOrJoinPanel();
        
        // TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
        // bool check = Client.OpGetGameList(sqlLobby, "C0 = 0");
        // Debug.Log("Found sql lobby: " + check);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        throw new System.NotImplementedException();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("Region list received:" + JsonUtility.ToJson(regionHandler.EnabledRegions));
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    
    
    // IMatchMakingCallbacks
    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        Debug.Log("Room has been created!!!");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed!!!");
        Debug.Log("Return code: " + returnCode + ", Message: " + message);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("Joined the room!!!");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }
    
    
    // ILobbyCallbacks

    public void OnJoinedLobby()
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftLobby()
    {
        throw new System.NotImplementedException();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Room: " + room.Name + ", Max Players: " + room.MaxPlayers);
        }
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        throw new System.NotImplementedException();
    }
}
