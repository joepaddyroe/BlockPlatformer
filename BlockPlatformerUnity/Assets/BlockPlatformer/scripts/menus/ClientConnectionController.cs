using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Quantum;
using Quantum.Demo;
using UnityEngine;

public class ClientConnectionController : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks, IOnEventCallback
{

    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] public static int MAX_PLAYER_COUNT = 2;
    [SerializeField] private RuntimeConfigContainer _runtimeConfigContainer;
    [SerializeField] private bool _spectate;
    [SerializeField] private ClientIdProvider.Type _idProvider = ClientIdProvider.Type.NewGuid;
    
    private string _currentPlayerName;
    private bool _isRejoining;

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
        
        if (Client != null && Client.InRoom && Client.LocalPlayer.IsMasterClient && Client.CurrentRoom.IsOpen) {
            if (!Client.OpRaiseEvent((byte)PhotonEventCode.StartGame, null, new RaiseEventOptions {Receivers = ReceiverGroup.All}, SendOptions.SendReliable)) {
                Debug.LogError($"Failed to send start game event");
            }
        }
        else
        {
            Debug.Log("Something went wrong here.....");
        }
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
        _uiMainMenu.RoomListUpdated(roomList);
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        throw new System.NotImplementedException();
    }
    
    
    //IOnEventCallback
    // All of this event processing and Start Game functionality I literally lifted from the demo content and modified
    // for this technical test
    
    enum PhotonEventCode : byte {
        StartGame = 110
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code) {
            case (byte)PhotonEventCode.StartGame:

                Client.CurrentRoom.CustomProperties.TryGetValue("MAP-GUID", out object mapGuidValue);
                if (mapGuidValue == null) {
                    Debug.Log("Error, Failed to read the map guid during start");
                    Client?.Disconnect();
                    return;
                }

                if (Client.LocalPlayer.IsMasterClient) {
                    // Save the started state in room properties for late joiners (TODO: set this from the plugin)
                    var ht = new ExitGames.Client.Photon.Hashtable {{"STARTED", true}};
                    Client.CurrentRoom.SetCustomProperties(ht);

                    // No need to hide th room here so others can join late I assume!?
                    
                    // if (Client.CurrentRoom.CustomProperties.TryGetValue("HIDE-ROOM", out var hideRoom) && (bool)hideRoom) {
                    //     Client.CurrentRoom.IsVisible = false;
                    // }
                }

                Debug.Log("Ok, trying to start quantum game with Asset GUID: " + (AssetGuid)(long)mapGuidValue);
                
                StartQuantumGame((AssetGuid)(long)mapGuidValue);

                _uiMainMenu.GoToGamePanel();

                break;
        }
    }
    
    
    private void StartQuantumGame(AssetGuid mapGuid) {
        
        if (QuantumRunner.Default != null) {
            // There already is a runner, maybe because of duplicated calls, button events or race-conditions sending start and not deregistering from event callbacks in time.
            Debug.LogWarning($"Another QuantumRunner '{QuantumRunner.Default.name}' has prevented starting the game");
            return;
        }

        var config = _runtimeConfigContainer != null ? RuntimeConfig.FromByteArray(RuntimeConfig.ToByteArray(_runtimeConfigContainer.Config)) : new RuntimeConfig();
        
        config.Map.Id = mapGuid;
        
        var param = new QuantumRunner.StartParameters {
            RuntimeConfig             = config,
            DeterministicConfig       = DeterministicSessionConfigAsset.Instance.Config,
            ReplayProvider            = null,
            GameMode                  = Photon.Deterministic.DeterministicGameMode.Multiplayer,
            FrameData                 = null,
            InitialFrame              = 0,
            PlayerCount               = Client.CurrentRoom.MaxPlayers,
            LocalPlayerCount          = 1,
            RecordingFlags            = RecordingFlags.None,
            NetworkClient             = Client,
            StartGameTimeoutInSeconds = 10.0f
        };

        Debug.Log($"Starting QuantumRunner with map guid '{mapGuid}' and requesting {param.LocalPlayerCount} player(s).");

        // Joining with the same client id will result in the same quantum player slot which is important for reconnecting.
        var clientId = ClientIdProvider.CreateClientId(_idProvider, Client);
        QuantumRunner.StartGame(clientId, param);

        //ReconnectInformation.Refresh(Client, TimeSpan.FromMinutes(1));
    }
    
    
}
