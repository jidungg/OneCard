using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManage:Photon. PunBehaviour {
    public static GameManage instance;
   
    public static int beforePlayerIndex;
    public static int currentPlayerIndex;

    public GameObject endTurnButton;
    public GameObject yourTurnText;


    public GameObject TilePrefap;
    public int mapsize_x=15, mapsize_y = 15;
    List<List<Tile>> map = new List<List<Tile>>();
    public List<GameObject> castleTile1 = new List<GameObject>();
    public List<GameObject> castleTile2 = new List<GameObject>();
    public List<List<GameObject>> castleTiles = new List<List<GameObject>>();
    List<Player> playerlist = new List<Player>();
    public List<GameObject> allUnits = new List<GameObject>();

    public string playerPrefapName = "Car";
    public Transform mapPositon;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform deckUnitTrans1;
    public Transform deckUnitTrans2;

    public bool murliganing;
    public GameObject murButton;
    void Awake()
    {

        instance = this;
        generateMap();
        generatePlayers();
        castleTiles.Add(castleTile1);
        castleTiles.Add(castleTile2);
        if (PhotonNetwork.isMasterClient)
        { 
            PhotonNetwork.room.SetCustomProperties(new Hashtable() { { "T", (int)Random.Range(0, 1) } });
            
        }   
    }
    void Start () {

        //if (PhotonNetwork.isMasterClient) { currentPlayerIndex = (int)Random.Range(0, 1); }
        
       
        beforePlayerIndex = (int)PhotonNetwork.room.customProperties["T"];
        SetPlayersCost();
        SetDeckUnits();
        //if ((int)PhotonNetwork.room.customProperties["T"] + 1 == PhotonNetwork.player.ID) { Player.me.StartMyTurn(); }
        Murlligan();
    }
	
    bool CheckTurnChanged(int currntturn)
    {
        if((int)PhotonNetwork.room.customProperties["T"] == currntturn)
        {
            return false;
        }
        return true;
    }

	void Update () {
        if (CheckTurnChanged(beforePlayerIndex) )
        {    
            if((int)PhotonNetwork.room.customProperties["T"] + 1 == PhotonNetwork.player.ID)
                Player.me.StartMyTurn();
            else Player.me.EndMyTurn();
        }
        beforePlayerIndex = (int)PhotonNetwork.room.customProperties["T"];

    }

    void generateMap()
    {
        map = new List<List<Tile>>();
        for(int i = 0; i < mapsize_x; i++)
        {
            List<Tile> row = new List<Tile>();
            for(int j=0; j <mapsize_y; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefap
                                       , new Vector3(mapPositon.position.x + i, mapPositon.position.y, mapPositon.position.z + j)
                                       , Quaternion.Euler(new Vector3(90,0,0)),mapPositon)).GetComponent<Tile>();
                tile.gridPositon = new Vector2(i, j);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void generatePlayers()
    {
        print("player Id" + PhotonNetwork.player.ID);
        if (PhotonNetwork.player.ID == 1)
        {
            print("id==1");
            playerlist.Add(((GameObject)(PhotonNetwork.Instantiate(playerPrefapName, spawnPoint1.position, spawnPoint1.localRotation, 0))).GetComponent<Player>());

        }

        if(PhotonNetwork.player.ID == 2)
        {
            print("id==2");
            playerlist.Add(((GameObject)(PhotonNetwork.Instantiate(playerPrefapName, spawnPoint2.position, spawnPoint2.localRotation, 0))).GetComponent<Player>());
        }

    }

    public void NextTurn()
    {
       if((int)PhotonNetwork.room.customProperties["T"]+1!= PhotonNetwork.player.ID)
        {
            return;
            
        }
        if ((int)PhotonNetwork.room.customProperties["T"] + 1 < PhotonNetwork.playerList.Length)
        {

            PhotonNetwork.room.SetCustomProperties(new Hashtable() { { "T", (int)PhotonNetwork.room.customProperties["T"]+1 } });

            if((int)PhotonNetwork.room.customProperties["P"+ (int)PhotonNetwork.room.customProperties["T"] + "C"] <= 10)
            {
                PhotonNetwork.room.SetCustomProperties(new Hashtable() { { "P" + (int)PhotonNetwork.room.customProperties["T"] + "C",
                        (int)PhotonNetwork.room.customProperties["P" + (int)PhotonNetwork.room.customProperties["T"] + "C"]+1 } });
            }
           
        }
        else
        {

            PhotonNetwork.room.SetCustomProperties(new Hashtable() { { "T", 0 } }); ;

            if ((int)PhotonNetwork.room.customProperties["P" + (int)PhotonNetwork.room.customProperties["T"] + "C"] <= 10)
            {
                PhotonNetwork.room.SetCustomProperties(new Hashtable() { { "P" + (int)PhotonNetwork.room.customProperties["T"] + "C",
                        (int)PhotonNetwork.room.customProperties["P" + (int)PhotonNetwork.room.customProperties["T"] + "C"]+1 } });
            }

        }

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //print(stream.isWriting);
            stream.SendNext(currentPlayerIndex);

        }
        else
        {
            //print(stream.isWriting);
            currentPlayerIndex = (int)stream.ReceiveNext();

        }
    }

    void OnGUI()
    {
        GUILayout.Label("Current playerIndex is:"+ (int)PhotonNetwork.room.customProperties["T"]);
        GUILayout.Label("My Player ID is:" + PhotonNetwork.player.ID);
        GUILayout.Label("Player"+ PhotonNetwork.player.ID +"Cost is: "+ (int)PhotonNetwork.room.customProperties["P" + (PhotonNetwork.player.ID-1) + "C"]);
        GUILayout.Label(Player.me.myHand.Count.ToString());
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.name); // seen when other disconnects
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.isMasterClient)
        {

            PhotonNetwork.LoadLevel(1);
        }
    }
    void SetPlayersCost()
    {
        for(int i=0; i< PhotonNetwork.playerList.Length; i++)
        {
            PhotonNetwork.room.SetCustomProperties(new Hashtable() { { "P"+i+"C", 0 } });
        }
       
    }
    void SetDeckUnits()
    {
        for (int j = 0; j < PlayerPrefs.GetInt("MDC"); j++)
            {

                if (PhotonNetwork.player.ID == 1)
                {
                    Player.me.deckList.Add((GameObject)Instantiate(allUnits[PlayerPrefs.GetInt("D" + PlayerPrefs.GetInt("SDI") + "U" + j)], deckUnitTrans1.position, deckUnitTrans1.rotation));
                    Player.me.deckList[j].SetActive(false);
                }
                else if (PhotonNetwork.player.ID == 2)
                {
                    Player.me.deckList.Add((GameObject)Instantiate(allUnits[PlayerPrefs.GetInt("D" + PlayerPrefs.GetInt("SDI") + "U" + j)], deckUnitTrans2.position, deckUnitTrans2.rotation));
                    Player.me.deckList[j].SetActive(false);
                }

            }       
    }
    void Murlligan()
    {
        instance.murliganing = true;
        Player.me.DrowUnit(1);
        murButton.gameObject.SetActive(true);
    }
    public void MurliganButton()
    {
        print(Player.me.myHand.Count);
        for(int i=0;i< Player.me.myHand.Count; i++)
        {
            print(i);
            if(Player.me.myHand[i].GetComponent<PlayerUnits>().murlignSel ==false )
            {
                print(Player.me.myHand[i].GetComponent<PlayerUnits>().murlignSel);
                Player.me.myHand[i].GetComponent<PlayerUnits>().UnDrowMoveThisUnit();
            }
        }
        instance.murliganing = false;
        murButton.gameObject.SetActive(false);
    }
}
