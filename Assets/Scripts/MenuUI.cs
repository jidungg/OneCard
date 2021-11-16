using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {
    public GameObject menuPanel;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ExitRoom()
    {
        print("exit room");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(1);
    }

    public void MenuButton()
    {
        menuPanel.SetActive(true);
    }

    public void MenuXButton()
    {
        menuPanel.SetActive(false);
    }
    public void ExitGame()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }
}
