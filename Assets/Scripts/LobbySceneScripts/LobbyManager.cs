using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager :Photon. MonoBehaviour {
    const string Version = "v 0.0.1";
    public string roomname = "";
    public InputField joinRoomNameInput;
    public InputField createRoomNameInput;
    public InputField playerNameInput;
    public GameObject cerateRoomNameObject;
    public GameObject joinRoomNameObject;
    public GameObject joinRoomErrorMessage;
    public GameObject roomListPanel;
    public GameObject roomButton;
    public RectTransform roomListPanelRect;
    public GameObject canvas;
    public static string playerNamePrefKey = "PlayerName";
    RoomInfo[] roomInfoList = new RoomInfo[0] { };
    
    



    void Start () {
        PhotonNetwork.ConnectUsingSettings(Version);
        SavingPlayerName();
        PhotonNetwork.automaticallySyncScene = true;
    }
	
	void Update () {
        if(PhotonNetwork.offlineMode)print("offline");

    }

    //==========================================================================================================
    void OnConnectedToMaster()
    {
        
        Debug.Log("connected to Master");
    }
    void OnPhotonCreateRoomFailed()
    {
        print("create room failed");
    }
    void OnPhotonJoinRoomFailed()
    {
        joinRoomErrorMessage.SetActive(true);
        Debug.Log("joinRoomFailed");
    }
    void OnJoinedLobby()
    {
        print(PhotonNetwork.lobby.Type);
        Debug.Log("joinedLobby");
        canvas.SetActive(true);
    }
    void OnCreatedRoom()
    {
        
        print("createdroom");
    }
    void OnJoinedRoom()
    {
        Debug.Log("joinedRoom");
        Debug.Log(PhotonNetwork.room.name);



    }

    void OnPhotonPlayerConnected(PhotonPlayer _new)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (2 == PhotonNetwork.room.playerCount)
            {

                PhotonNetwork.LoadLevel(2);

            }
            else
            {
                print("wait");
            }
        }

    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("random join failed");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 2 };
        roomOptions.CustomRoomProperties = new Hashtable() { { "T", Random.Range(0, 1) } };
        
        //joinRoomErrorMessage.SetActive(true);roomname, new RoomOptions() { IsVisible = false, MaxPlayers = 2 }, TypedLobby.Default
        PhotonNetwork.CreateRoom(null,roomOptions,null);
       
    }
    
    //==========================================================================================================


    public void DeactivePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void JoiningRandomRoom()
    {
        print("joining randomroom");
        PhotonNetwork.JoinRandomRoom();

    }

    public void CreateRoomButton()
    {
        cerateRoomNameObject.SetActive(true);
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(createRoomNameInput.text, roomOptions,null);
    }

    public void JoinRoomButton()
    {
        joinRoomNameObject.SetActive(true);
        
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomNameInput.text);
    }



    public void SetPlayerName()
    {
        
        PhotonNetwork.playerName = playerNameInput.text + " "; 
        PlayerPrefs.SetString(playerNamePrefKey, playerNameInput.text);
        
    }
    void SavingPlayerName()
    {
        string defaultName = "";

        if (playerNameInput != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {

                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                playerNameInput.text = defaultName;
                
            }
        }
        PhotonNetwork.playerName = defaultName;
    }


}
