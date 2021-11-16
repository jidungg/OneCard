using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    private string version = "v 0.1";
    public string playerPrefapName = "Vik";
    public Transform spawnPoint;
    public CameraMove _CameraMove;
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(version);
    }
	void Start () {

    }
	

	void Update () {
	
	}

    void OnJoinedLobby()
    {
        
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }
    void OnCreatedRoom()
    {
        print("RoomCreated");
    }
    void OnJoinedRoom()
    {
        print("JoinedRoom");
        GameObject character= PhotonNetwork.Instantiate(PlayerPrefs.GetString("Avatar"), spawnPoint.position, spawnPoint.rotation,0);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnEventRaised registered.");
            PhotonNetwork.OnEventCall += this.OnEventRaised;
        }
    }
    public void OnEventRaised(byte eventCode, object content, int senderID)
    {

        Debug.Log(string.Format("OnEventRaised: {0}, {1}, {2}", eventCode, content, senderID));
    }
}
