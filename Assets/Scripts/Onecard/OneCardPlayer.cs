using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneCardPlayer : MonoBehaviour {
    public OneCardManager _oneCardManager;
    public Camera cam;
    public List<GameObject> myHand;
    public bool itsMe;
    public bool isOneCardShouted;
    public int playerIndex;
    void Awake()
    {
        isOneCardShouted = false;
    }
    void Start () {

	}

	void Update () {
	    
	}
    
    public void DrowCard(int a)
    {
        StartCoroutine("DrowDelay", a);
    }
    IEnumerator DrowDelay(int num)
    {
        print("DrowDelay Coroutine" + num);
        int i = 0;
        while (i < num)
        {
            if (_oneCardManager.cardset1.Count > 0)
            {           
                myHand.Add(_oneCardManager.cardset1[_oneCardManager.cardset1.Count-1]);
                myHand[myHand.Count - 1].GetComponent<CardScript>().MovingCard(transform.position);              
                _oneCardManager.cardset1.RemoveAt(_oneCardManager.cardset1.Count-1);               
                myHand[myHand.Count - 1].GetComponent<CardScript>()._owner = this;
                isOneCardShouted = false;
            }
            else if (_oneCardManager.cardset1.Count < 0)
            {
                _oneCardManager.pv.RPC("ResetDeck", PhotonTargets.All);
            }
            i++;
            yield return new WaitForSeconds(0.3f);          
        }

        StopCoroutine("DrowDelay");
    }
    public void ArrayHands()
    {
        float term=3.5f/ myHand.Count;
        if (term > 0.5f)
        {
            term = 0.5f;
        }
        float center;
        if (myHand.Count%2==0)
        {
            center = myHand.Count/2;
        }
        else
        {
            center = (myHand.Count / 2) + 1;
        }
        for(int i = 0; i < myHand.Count; i++)
        {
            if (i+1 < center)
            {
                Vector3 pos;
                if (playerIndex == 0)
                {
                    pos = transform.position + new Vector3(-term * (center - (i + 1)), (0.001f * (myHand.Count-i-1)), 0);
                    myHand[i].transform.position = pos;
                }
                else if (playerIndex ==1)
                {
                    pos = transform.position + new Vector3(+term * (center - (i + 1)), (0.001f * (myHand.Count - i - 1)), 0);
                    myHand[i].transform.position = pos;
                }
                else if (playerIndex == 2)
                {
                    pos = transform.position + new Vector3(0, (0.001f * (myHand.Count - i - 1)), +term * (center - (i + 1)));
                    myHand[i].transform.position = pos;
                }
                else if( playerIndex == 3)
                {
                    pos = transform.position + new Vector3(0, (0.001f * (myHand.Count - i - 1)), -term * (center - (i + 1)));
                    myHand[i].transform.position = pos;
                }

            }
            else if(i+1==center)
            {
                Vector3 pos ;
                pos = transform.position + new Vector3(0, (0.001f * (myHand.Count - i - 1)), 0);
                myHand[i].transform.position = pos;
            }
            else if (i+1>center)
            {
                Vector3 pos ;
                if (playerIndex == 0)
                {
                    pos = transform.position + new Vector3(term * ((i + 1) - center), (0.001f * (myHand.Count - i - 1)), 0);
                    myHand[i].transform.position = pos;
                }
                else if (playerIndex == 1)
                {
                    pos = transform.position + new Vector3(-term * ((i + 1) - center), (0.001f * (myHand.Count - i - 1)), 0);
                    myHand[i].transform.position = pos;
                }
                else if (playerIndex == 2)
                {
                    pos = transform.position + new Vector3(0, (0.001f * (myHand.Count - i - 1)), -term * ((i + 1) - center));
                    myHand[i].transform.position = pos;
                }
                else if (playerIndex == 3)
                {
                    pos = transform.position + new Vector3(0, (0.001f * (myHand.Count - i - 1)), +term * ((i + 1) - center));
                    myHand[i].transform.position = pos;
                }

            }
            if (itsMe)
            {
                myHand[i].transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
            else if (!itsMe)
            {
                myHand[i].transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            myHand[i].GetComponent<CardScript>().SaveTransform();
            myHand[i].GetComponent<CardScript>().handIndex = i;
        }

    }

    public void ThrowCard(int cindex)
    {  
        CardScript card = myHand[cindex].GetComponent<CardScript>();
        if (card.cardNum != "7")
        {
            _oneCardManager.topSymButton.SetActive(false);
        }
        _oneCardManager.cardset2.Add(myHand[cindex]);
        myHand[cindex].transform.parent = _oneCardManager.cardStack2.transform;
        myHand.RemoveAt(cindex);
        card.MovingCard(_oneCardManager.cardStack2.transform.position+ new Vector3(0, 0.0025f * (_oneCardManager.cardset2.Count-1), 0));
        _oneCardManager.ArrayCardSet();
        ArrayHands();
        CheckWin();
    }
    void CheckWin()
    {
        print("CheckWIn");
        if(myHand.Count == 0)
        {
            _oneCardManager.EndGame(playerIndex);
        }
       
    }
}
