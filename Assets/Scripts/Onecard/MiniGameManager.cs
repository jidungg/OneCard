using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MiniGameManager : MonoBehaviour {
    public GameObject gamePlayers;
    public int numOfPlayers=0; 
    public Text _informText;
    public List<Transform> playerTransfroms;
    public GameObject oneCardButton;
    public GameObject startButton;
    public GameObject exitButton;
    PhotonView pv;
    public enum miniGameType
    {
        OneCard
    }
    public enum miniGameState
    {
        Waiting,ChoosingGame,WaitOthers,Playing,End
    }
     miniGameType _miniGameType;
    miniGameState _miniGameState;
    public int minPlayer;
    public delegate void StateDelegate(miniGameState stat);
    public static event StateDelegate OnGameStateChange;
    public int myPlayerIndex;
    bool isJoined;


    void Awake()
    {
       
        OnGameStateChange += InformText;
    }
	void Start () {
        
        print(gameObject.layer);
        
	}
    void OnJoinedRoom()
    {
        pv = GetComponent<PhotonView>();

    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        pv.RPC("GameStateChange", PhotonTargets.All, _miniGameState);
        pv.RPC("SyncGameT", PhotonTargets.All, _miniGameType);


        pv.RPC("SyncPlayer", PhotonTargets.All, numOfPlayers);

    }
    void Update () {

	}
    void InformText(miniGameState stat)
    {
        
        print("InformText: "+ "stat= " + (byte)stat);
        if ((byte)stat == 0)
        {
            _informText.text = "Interact to join game";
            oneCardButton.SetActive(false);
            startButton.SetActive(false);
            exitButton.SetActive(false);
        }
        if ((byte)stat == 1)
        {

            _informText.text = "Select GameType";
            if (isJoined)
            {
                oneCardButton.SetActive(true);
                startButton.SetActive(false);
                exitButton.SetActive(true);
            }

        }
        if ((byte)stat == 2)
        {
            _informText.text = _miniGameType.ToString();
            if (isJoined)
            {
                oneCardButton.SetActive(false);
                startButton.SetActive(true);
                exitButton.SetActive(true);
            }

        }
        if ((byte)stat == 3)
        {
            _informText.text = "Playing"+_miniGameType.ToString();
            if (isJoined)
            {
                oneCardButton.SetActive(false);
                startButton.SetActive(false);
                exitButton.SetActive(false);
                if (_miniGameType == miniGameType.OneCard)
                {
                    OneCardManager onecard = gameObject.GetComponent<OneCardManager>();
                    onecard.enabled = true;

                }
            } 
        }
        if ((byte)stat == 4)
        {
            
            _informText.text = "Game Over";
            OneCardManager onecard = gameObject.GetComponent<OneCardManager>();
            onecard.enabled = false;
            oneCardButton.SetActive(false);
            startButton.SetActive(false);
            exitButton.SetActive(true);
        }
    }

    public void AddPlayer(GameObject obj)
    {
        if(gamePlayers == obj)
        {
            return;
        }
        int id = obj.GetComponent<PhotonView>().viewID;
        myPlayerIndex = numOfPlayers;
        print("myplayerIndex" + myPlayerIndex);
        numOfPlayers++;
        gamePlayers=obj;
        isJoined = true;
  
        pv.RPC("SyncPlayer", PhotonTargets.All, numOfPlayers);
        pv.RPC("SyncCollider", PhotonTargets.All, obj.GetComponent<PhotonView>().viewID,false);
        if (gamePlayers != null&& _miniGameState ==miniGameState.Waiting)
        {
            pv.RPC("GameStateChange", PhotonTargets.All, miniGameState.ChoosingGame);
        }
        InformText(_miniGameState);
        
    }
    public void RemovePlayer()
    {
        if(isJoined == true)
        {
            pv.RPC("SyncCollider", PhotonTargets.All, gamePlayers.GetComponent<PhotonView>().viewID, true);
            gamePlayers = null;
            numOfPlayers--;
            pv.RPC("SyncPlayer", PhotonTargets.All, numOfPlayers);

            isJoined = false;
        }

    }
    public void OneCardButton()
    {
 

        _miniGameType = miniGameType.OneCard;
        pv.RPC("SyncGameT", PhotonTargets.All, _miniGameType);
        pv.RPC("GameStateChange", PhotonTargets.All, miniGameState.WaitOthers);
        
    }
    public void ExitButton()
    {
        RemovePlayer();
        if(numOfPlayers == 0)
        {
            pv.RPC("GameStateChange", PhotonTargets.All, miniGameState.Waiting);
            pv.RPC("SyncGameT", PhotonTargets.All, null);
        }
        exitButton.SetActive(false);
    }
    public void GameStart()
    {
        if(numOfPlayers < minPlayer)
        {

            return;
        }
        else
        {
            pv.RPC("GameStateChange", PhotonTargets.All, miniGameState.Playing);
        }
        

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(numOfPlayers);
            stream.SendNext(_miniGameState);
        }
        else
        {
            numOfPlayers = (int)stream.ReceiveNext();
            _miniGameState = (miniGameState)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void SyncGameT(miniGameType type)
    {
        _miniGameType = type;


    }
    
    [PunRPC]
    void SyncPlayer(int data)
    {
        
        numOfPlayers = data;

    }
    [PunRPC]
    public void GameStateChange(miniGameState stat)
    {

        _miniGameState = stat;
        OnGameStateChange(stat);

    }
    [PunRPC]
    void SyncCollider(int id,bool about)
    {
        for(int i=0;i< FindObjectsOfType<PhotonView>().Length; i++)
        {
            if (FindObjectsOfType<PhotonView>()[i].viewID == id)
            {
                FindObjectsOfType<PhotonView>()[i].gameObject.GetComponent<BoxCollider>().enabled = about;
            }
        }
    }
}
