using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player :Photon. MonoBehaviour
{
    public static Player me;
   
    public GameObject mycamera;
    public float turnTime = 10f;
    public bool isMyTurn = false;
    int currentUnitIndex=0;
    int hp;
    int exhaustStack=1;
    
    public List<GameObject> myUnitlist =new List<GameObject>();
    public List<GameObject> deckList = new List<GameObject>();
    public List<GameObject> myHand = new List<GameObject>();


    void Awake()
    {


        print("MEMEME");
        GameManage.instance.yourTurnText.SetActive(false);
        if (photonView.isMine)
        {
            me = this;
            mycamera.SetActive(true);
            GetComponent<SimpleMove>().enabled = true;
            me.ShuffleDeck();
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void StartMyTurn()
    {
        StartCoroutine(OnMyTurn());
        print("startmyturn ");
        isMyTurn = true;
    }
    public void EndMyTurn()
    {
        StopCoroutine(OnMyTurn());
        print("stopmyturn ");
        isMyTurn = false;
    }
    IEnumerator OnMyTurn()
    {
        //isMyTurn = true;

        
        yield return new WaitForSeconds(turnTime);

        if(isMyTurn == true)
        {
            GameManage.instance.NextTurn();
        }
        
    }



    public void SpawnUnit( PlayerUnits unit,Tile tile)
    {
        me.myUnitlist.Add(((GameObject) PhotonNetwork.Instantiate(unit.name, tile.transform.position, tile.transform.rotation, 1)));
    }



    public void ShuffleDeck()
    {
        print("SHuffle");
        for(int i=0; i < me.deckList.Count; i++)
        {
            int j = Random.Range(0, me.deckList.Count - 1);
            GameObject temp = me.deckList[j];
            me.deckList[j] = me.deckList[i];
            me.deckList[i] = temp;
        }
    }

    public void DrowUnit(int num)
    {
        print("Drow" + num);
        StartCoroutine("DrowDelayTime", num);
    }

    IEnumerator DrowDelayTime(int num)
    {
        
        int i = 0;
        while (i < num)
        {
            if (me.deckList.Count > 0)
            {

                me.deckList[0].SetActive(true);
                me.myHand.Add(me.deckList[0]);
                print(me.myHand.Count);
                me.deckList[0].GetComponent<PlayerUnits>().DrowMoveThisUnit(me.myHand.Count);            
                me.deckList.RemoveAt(0);

            }
            else if (me.deckList.Count == 0)
            {
                hp = hp - exhaustStack;
                exhaustStack++;
            }
            yield return new WaitForSeconds(0.3f);
            i++;
        }

        StopCoroutine("DelayTime");
    }



}
