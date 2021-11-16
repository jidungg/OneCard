using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;


public class OneCardManager : MonoBehaviour {
    public MiniGameManager _miniGameManager;
    public PhotonView pv;
    public GameObject cardStack1;
    public GameObject cardStack2;
    public List<GameObject> cardset1;
    public List<GameObject> cardset2;
    public GameObject mainCam;
    public GameObject mainCanvas;
    public GameObject gameCanvas;
    public GameObject EndTurnButton;
    public GameObject HeartButton;
    public GameObject DiaButton;
    public GameObject ClobButton;
    public GameObject SpadeButton;
    public GameObject topSymButton;
    public GameObject shoutOneButton;
    public AudioSource oneCardShout;
    public GameObject gameOverWindow;
    public GameObject scoreBoard;
    bool isMyTurn;
    public List<GameObject> players;
    public int murliganCardAmount;
    bool isGameSet;
    public int nowTurn=0;
    public GameObject selectedCard;
    public Text apText;

    int attackPoint=0;
    int cardsThrownInMyTrun;
    bool defendFale;
    string topSym;
    bool reversTurn=false;
    bool isKingThrown;
    bool canThrow;
    void Awake()
    {
        isGameSet = false;
        pv = GetComponent<PhotonView>();
        SetCards();
    }
    void Start () {
        print("OneCardStart " + _miniGameManager.myPlayerIndex);
        if (_miniGameManager.myPlayerIndex == 0)
        {
            print("_miniGameManager.myPlayerIndex == 0");
            ShuffleDeck();
            Murligan();
        }

        pv.RPC("SyncTurn", PhotonTargets.All, nowTurn);
    }
	
	void Update () {
        

	}
    void SetCamera()
    {
        print("SetCamera");
        Camera.main.gameObject.SetActive(false);
        for (int i=0; i < _miniGameManager.numOfPlayers; i++)
        {
            print("i" + i);
            if(i == _miniGameManager.myPlayerIndex)
            {
                players[i].GetComponent<OneCardPlayer>().cam.gameObject.SetActive(true);
            }
            else
            {
                players[i].GetComponent<OneCardPlayer>().cam.gameObject.SetActive(false);
            }
        }
        gameCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        EndTurnButton.SetActive(false);
        
    }
    void SetCards()
    {
        for (int i=0; i< cardStack1.transform.childCount; i++)
        {
            cardset1.Add(cardStack1.transform.GetChild(i).gameObject);
        }
    }
    void SetCardPosition()
    {
        
        for (int i = 0; i < cardset1.Count; i++)
        {
            cardset1[i].transform.position = cardStack1.transform.position+ new Vector3(0, 0.0025f * i, 0);
            cardset1[i].transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
    void SetPlayers()
    {
        print("SetPlayers");
        for (int i=0; i < _miniGameManager.numOfPlayers; i++)
        {
            print("_miniGameManager.numOfPlayers: "+ _miniGameManager.numOfPlayers+" i:" + i);
            players.Add(transform.Find("P" + (i + 1)).gameObject);
            players[i].SetActive(true);
            _miniGameManager.GetComponent<BoxCollider>().enabled = false;
            
            players[i].GetComponent<OneCardPlayer>().playerIndex = i;
            if (i == _miniGameManager.myPlayerIndex)
            {
                players[i].GetComponent<OneCardPlayer>().itsMe = true;
            }
        }
        

    }
    public void ShuffleDeck()
    {
        print("ShuffleDeck");
        
        for (int i = 0; i < cardset1.Count; i++)
        {
            int j = Random.Range(0, cardset1.Count - 1);
            GameObject temp = cardset1[j];
            cardset1[j] = cardset1[i];
            cardset1[i] = temp;

        }
        for (int i = 0; i < cardset1.Count; i++)
        {
            pv.RPC("SyncDeck", PhotonTargets.All, cardset1[i].name, i);
        }
        pv.RPC("SetGame", PhotonTargets.All);

    }

    void Murligan()
    {
        print("Murligan");
        for (int i=0; i< _miniGameManager.numOfPlayers; i++)
        {
            StartCoroutine("DrowCoroutine",i);
        }

    }
    IEnumerator DrowCoroutine(int i)
    {
        yield return new WaitForSeconds(2f * i);
        pv.RPC("Drow", PhotonTargets.All, i, 5);
    }
    public void EndTurn()
    {
        isMyTurn = false;
        if (defendFale && attackPoint!=0)
        {
            pv.RPC("Drow", PhotonTargets.All, _miniGameManager.myPlayerIndex, attackPoint);
            attackPoint = 0;
            pv.RPC("SyncAP", PhotonTargets.All, attackPoint);
        }
        else if (cardsThrownInMyTrun == 0 && attackPoint==0)
        {
            pv.RPC("Drow", PhotonTargets.All, _miniGameManager.myPlayerIndex, 1);
        }
        if (!reversTurn)
        {
            if (nowTurn + 1 < _miniGameManager.numOfPlayers)
            {
                nowTurn++;
            }
            else
            {
                nowTurn = 0;
            }

        }
        else if (reversTurn)
        {
            if (nowTurn > 0)
            {
                nowTurn--;
            }
            else
            {
                nowTurn = _miniGameManager.numOfPlayers-1;
            }
        }

        if (nowTurn == _miniGameManager.myPlayerIndex)
        {
        }
        else
        {
        }
        isKingThrown = false;
        EndTurnButton.SetActive(false);
        pv.RPC("SyncTurn", PhotonTargets.All, nowTurn);
    }
    public void TryThrowCard(CardScript card)
    {
        if (nowTurn != _miniGameManager.myPlayerIndex)
        {
            return;
        }
        if (isKingThrown)
        {
            isKingThrown = false;
            if (card.cardNum == "2")
            {
                AttackTHrow(2, card.handIndex);
                return;
            }
            else if (card.cardNum == "A" && card.cardSym == "S")
            {
                AttackTHrow(5, card.handIndex);
                return;
            }
            else if (card.cardNum == "A")
            {
                AttackTHrow(3, card.handIndex);
                return;
            }
            else if (card.cardNum == "Jk")
            {
                if (card.cardSym == "B")
                {
                    AttackTHrow(7, card.handIndex);
                    return;
                }
                if (card.cardSym == "R")
                {
                    AttackTHrow(10, card.handIndex);
                    return;
                }
            }
            else if (card.cardNum == "7")
            {
                SevenThrow(card.handIndex);
                return;
            }
            else if (card.cardNum == "J")
            {
                JackThrow(card.handIndex);
                return;
            }
            else if (card.cardNum == "Q")
            {
                QueenThrow(card.handIndex);
                return;
            }
            else if (card.cardNum == "K")
            {
                KingThrow(card.handIndex);
                return;
            }
            else
            {
                NormalThrow(card.handIndex);
                return;
            }
            
        }

        string Top;
        CardScript c = cardset2[cardset2.Count - 1].GetComponent<CardScript>();
        if (c.cardNum == "7")
        {
            Top = topSym;
        }
        else {
            Top = c.cardSym;
        }

        if (card.cardNum == "Jk")
        {
            if (card.cardSym == "B")
            {
                AttackTHrow(7, card.handIndex);
                return;
            }
            if (card.cardSym == "R")
            {
                AttackTHrow(10, card.handIndex);
                return;
            }
        }
        
        if (attackPoint != 0 && cardsThrownInMyTrun ==0)
        {
            if (c.cardNum == "Jk")
            {
                if (Top == "R")
                {
                    if (card.cardNum == "Jk")
                    {
                        AttackTHrow(7,card.handIndex);
                        return;
                    }
                }
                if (Top == "B")
                {
                    if (card.cardNum == "Jk")
                    {
                        AttackTHrow(10, card.handIndex);
                        return;
                    }
                    else if(Top == "S" && c.cardNum == "A")
                    {
                        AttackTHrow(5, card.handIndex);
                        return;
                    }
                }
            }
            if(c.cardSym=="S" && c.cardNum == "A")
            {
                if(card.cardNum == "Jk")
                {
                    if (card.cardSym == "B")
                    {
                        AttackTHrow(7, card.handIndex);
                        return;
                    }
                    if (card.cardSym == "R")
                    {
                        AttackTHrow(10, card.handIndex);
                        return;
                    }
                }

            }
            else if(c.cardNum == "A")
            {
                if(card.cardNum == "A"&&card.cardSym == "S")
                {
                    AttackTHrow(5, card.handIndex);
                    return;
                }
                if (card.cardNum == "A")
                {
                    AttackTHrow(3, card.handIndex);
                    return;
                }
                if(card.cardNum == "Jk")
                {
                    if (card.cardSym == "B")
                    {
                        AttackTHrow(7, card.handIndex);
                        return;
                    }
                    if (card.cardSym == "R")
                    {
                        AttackTHrow(10, card.handIndex);
                        return;
                    }
                }
            }
            else if (c.cardNum == "2")
            {
                if(card.cardNum == "2")
                {
                    AttackTHrow(2, card.handIndex);
                    return;
                }
                else if (card.cardNum == "A" && card.cardSym == c.cardSym)
                {
                    if (card.cardSym == "S")
                    {
                        AttackTHrow(5, card.handIndex);
                        return;
                    }
                    else
                    {
                        AttackTHrow(3, card.handIndex);
                        return;
                    }
                }

            }
            return;
        }
        if (attackPoint==0 && c.cardNum =="Jk")
        {
                if (card.cardNum == "2")
                {
                    AttackTHrow(2, card.handIndex);
                    return;
                }
                else if (card.cardNum == "A" && card.cardSym == "S")
                {
                    AttackTHrow(5, card.handIndex);
                    return;
                }
                else if (card.cardNum == "A")
                {
                    AttackTHrow(3, card.handIndex);
                    return;
                }
                else if (card.cardNum == "Jk")
                {
                    if (card.cardSym == "B")
                    {
                        AttackTHrow(7, card.handIndex);
                        return;
                    }
                    if (card.cardSym == "R")
                    {
                        AttackTHrow(10, card.handIndex);
                        return;
                    }
                }
                else if (card.cardNum == "7")
                {
                    SevenThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "J")
                {
                    JackThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "Q")
                {
                    QueenThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "K")
                {
                    KingThrow(card.handIndex);
                    return;
                }
                else
                {
                    NormalThrow(card.handIndex);
                    return;
                }
        }
        else if (cardsThrownInMyTrun == 0)
        {
            if (card.cardSym == Top|| card.cardNum == c.cardNum)
            {
                if (card.cardNum == "2")
                {
                    AttackTHrow(2, card.handIndex);
                    return;
                }
                else if (card.cardNum == "A" && card.cardSym == "S")
                {
                    AttackTHrow(5, card.handIndex);
                    return;
                }
                else if (card.cardNum == "A")
                {
                    AttackTHrow(3, card.handIndex);
                    return;
                }
                else if (card.cardNum == "Jk")
                {
                    if (card.cardSym == "B")
                    {
                        AttackTHrow(7, card.handIndex);
                        return;
                    }
                    if (card.cardSym == "R")
                    {
                        AttackTHrow(10, card.handIndex);
                        return;
                    }
                }
                else if (card.cardNum == "7")
                {
                    SevenThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "J")
                {
                    JackThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "Q")
                {
                    QueenThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "K")
                {
                    KingThrow(card.handIndex);
                    return;
                }
                else
                {
                    NormalThrow(card.handIndex);
                    return;
                }

            }
        }
        else if(cardsThrownInMyTrun != 0)
        {
            if (card.cardNum == c.cardNum)
            {
                if(card.cardNum == "2")
                {
                    AttackTHrow(2, card.handIndex);
                    return;
                }
                else if(card.cardNum=="A" && card.cardSym == "S")
                {
                    AttackTHrow(5, card.handIndex);
                    return;
                }
                else if (card.cardNum == "A")
                {
                    AttackTHrow(3, card.handIndex);
                    return;
                }
                
                else if (card.cardNum == "7")
                {
                    SevenThrow(card.handIndex);
                    return;
                }
                else if(card.cardNum == "J")
                {
                    JackThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "Q")
                {
                    QueenThrow(card.handIndex);
                    return;
                }
                else if (card.cardNum == "K")
                {
                    KingThrow(card.handIndex);
                    return;
                }
                else
                {
                    NormalThrow(card.handIndex);
                    return;
                }
            }
        }

    }

    void AttackTHrow(int atk,int handindex)
    {
        cardsThrownInMyTrun++;
        attackPoint = attackPoint + atk;
        defendFale = false;
        pv.RPC("SyncAP", PhotonTargets.All, attackPoint);
        pv.RPC("ThrowCard", PhotonTargets.All, _miniGameManager.myPlayerIndex, handindex);
    }
    void NormalThrow(int handindex)
    {
        cardsThrownInMyTrun++;
        pv.RPC("ThrowCard", PhotonTargets.All, _miniGameManager.myPlayerIndex, handindex);
    }
    void SevenThrow(int handindex)
    {
        cardsThrownInMyTrun++;
        ClobButton.SetActive(true);
        HeartButton.SetActive(true);
        DiaButton.SetActive(true);
        SpadeButton.SetActive(true);
        canThrow = false;
        pv.RPC("ThrowCard", PhotonTargets.All, _miniGameManager.myPlayerIndex, handindex);
    }
    public void clobButton()
    {
        topSym = "C";
        pv.RPC("SyncTopSym", PhotonTargets.All, topSym);
        ClobButton.SetActive(false);
        HeartButton.SetActive(false);
        DiaButton.SetActive(false);
        SpadeButton.SetActive(false);
        canThrow = true;


    }
    public void diaButton()
    {
        topSym = "D";
        pv.RPC("SyncTopSym", PhotonTargets.All, topSym);
        ClobButton.SetActive(false);
        HeartButton.SetActive(false);
        DiaButton.SetActive(false);
        SpadeButton.SetActive(false);
        canThrow = true;

    }
    public void heartButton()
    {
        topSym = "H";
        pv.RPC("SyncTopSym", PhotonTargets.All, topSym);
        ClobButton.SetActive(false);
        HeartButton.SetActive(false);
        DiaButton.SetActive(false);
        SpadeButton.SetActive(false);
        canThrow = true;

    }
    public void spadeButton()
    {
        topSym = "S";
        pv.RPC("SyncTopSym", PhotonTargets.All, topSym);
        ClobButton.SetActive(false);
        HeartButton.SetActive(false);
        DiaButton.SetActive(false);
        SpadeButton.SetActive(false);
        canThrow = true;

    }
    void JackThrow(int handindex)
    {
        cardsThrownInMyTrun++;
        for( int i=0; i < 2; i++)
        {
            if (!reversTurn)
            {
                if (nowTurn + 1 < _miniGameManager.numOfPlayers)
                {
                    nowTurn++;
                }
                else
                {
                    nowTurn = 0;
                }

            }
            else if (reversTurn)
            {
                if (nowTurn > 0)
                {
                    nowTurn--;
                }
                else
                {
                    nowTurn = _miniGameManager.numOfPlayers - 1;
                }
            }
        }
        
        pv.RPC("SyncTurn", PhotonTargets.All, nowTurn);
        pv.RPC("ThrowCard", PhotonTargets.All, _miniGameManager.myPlayerIndex, handindex);
    }
    void QueenThrow(int handindex)
    {
        print("Queen throw");
        cardsThrownInMyTrun++;
        if (reversTurn)
        {
            reversTurn = false;
        }
        else
        {
            reversTurn = true;
        }
        pv.RPC("SyncRevers", PhotonTargets.All, reversTurn);
        pv.RPC("ThrowCard", PhotonTargets.All, _miniGameManager.myPlayerIndex, handindex);
    }
    void KingThrow(int handindex)
    {
        print("King throw");
        print("king");
        cardsThrownInMyTrun ++;
        isKingThrown = true;
        pv.RPC("ThrowCard", PhotonTargets.All, _miniGameManager.myPlayerIndex, handindex);
    }
    void StartMyTurn()
    {
        print("StartMyTurn");
        EndTurnButton.SetActive(true);
        cardsThrownInMyTrun = 0;
        defendFale = true;
        isMyTurn = true;
    }

    public void ArrayCardSet()
    {
        for(int i=0; i < cardset2.Count; i++)
        {
            cardset2[i].transform.position = cardStack2.transform.position + new Vector3(0, 0.0025f * i, 0);
            cardset2[i].transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }
    public void ShoutOneCard()
    {
        bool shoutSuc=false;
        for(int i=0; i < players.Count; i++)
        {
            if (players[i].GetComponent<OneCardPlayer>().myHand.Count == 1 )
            {
                if(!players[i].GetComponent<OneCardPlayer>().isOneCardShouted)
                {
                    if (i == _miniGameManager.myPlayerIndex)
                    {

                        pv.RPC("ShoutOne", PhotonTargets.All,true,i);
                        shoutSuc = true;
                    }
                    else if (i != _miniGameManager.myPlayerIndex)
                    {
                        pv.RPC("ShoutOne", PhotonTargets.All, true, i);
                        pv.RPC("Drow", PhotonTargets.All,i,1);
                        shoutSuc = true;
                    }
                }
            }
        }
        if (!shoutSuc)
        {
            pv.RPC("ShoutOne", PhotonTargets.All, false, _miniGameManager.myPlayerIndex);
            pv.RPC("Drow", PhotonTargets.All, _miniGameManager.myPlayerIndex, 1);
        }
        shoutOneButton.SetActive(false);
        Invoke("ActivateShout", 3f);

    }
    void ActivateShout()
    {
        shoutOneButton.SetActive(true);
    }
    public void EndGame(int winnerindex)
    {
        gameOverWindow.SetActive(true);
        scoreBoard.GetComponent<Text>().text = "Winner: " + players[winnerindex].name;
    }
    public void EndGameButton()
    {
        print("EndGame");
        print("players.Count " + players.Count);
        for(int i=0; i < players.Count; i++)
        {
            OneCardPlayer playerscript =players[i].GetComponent<OneCardPlayer>();
            for(int j=0; j< playerscript.myHand.Count; j++)
            {
                playerscript.myHand[j].transform.parent = cardStack1.transform;
                playerscript.myHand.RemoveAt(j);
            }
        }
        for (int i = 0; i < cardset2.Count; i++)
        {

            cardset2[i].transform.parent = cardStack1.transform;
            cardset2.RemoveAt(i);
        }
        for (int i=0; i < players.Count; i++)
        {
            players[i].SetActive(false);
            players.RemoveAt(i);
            
        }
        HeartButton.SetActive(false);
        DiaButton.SetActive(false);
        SpadeButton.SetActive(false);
        ClobButton.SetActive(false);
        gameCanvas.SetActive(false);
        if (CrossPlatformInputManager.AxisExists("Horizontal"))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis("Horizontal");
        }
        if (CrossPlatformInputManager.AxisExists("Vertical"))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis("Vertical");
        }
        mainCanvas.SetActive(true);
        
        mainCam.SetActive(true);
        pv.RPC("GameStateChange", PhotonTargets.All, MiniGameManager.miniGameState.End);
    }

    [PunRPC]
    void Drow(int pindex,int num)
    {
        players[pindex].GetComponent<OneCardPlayer>().DrowCard(num);
    }
    [PunRPC]
    void SyncDeck(string obj,int i)
    {
        cardset1[i] = cardStack1.transform.Find(obj).gameObject;

    }
    [PunRPC]
    void SetGame()
    {
        print("SetGame");
        cardset2.Add(cardset1[cardset1.Count - 1]);
        cardset1[cardset1.Count - 1].transform.parent = cardStack2.transform;
        cardset1.RemoveAt(cardset1.Count - 1);
        cardset2[0].transform.position = cardStack2.transform.position;
        ArrayCardSet();
        SetCardPosition();     
        SetPlayers();
        SetCamera();
        isGameSet = true;
    }
    [PunRPC]
    void SyncTurn(int turn)
    {   
        nowTurn = turn;
        if (nowTurn == _miniGameManager.myPlayerIndex)
        {
            StartMyTurn();
        }
        else if(nowTurn != _miniGameManager.myPlayerIndex)
        {
            EndTurnButton.SetActive(false);
        }
    }
    [PunRPC]
    void SyncAP(int ap)
    {
        attackPoint = ap;
        apText.text = ": " + attackPoint;
    }
    [PunRPC]
    void SyncTopSym(string sym)
    {
        topSym = sym;
        print("topSym: " + topSym);
        if (topSym == "D")
        {
            topSymButton.SetActive(true);
            topSymButton.GetComponentInChildren<Text>().text = "◆";
        }
        else if (topSym == "H")
        {

            topSymButton.SetActive(true);
            topSymButton.GetComponentInChildren<Text>().text = "♥";
        }
        else if (topSym == "S")
        {

            topSymButton.SetActive(true);
            topSymButton.GetComponentInChildren<Text>().text = "♠";
        }
        else if (topSym == "C")
        {

            topSymButton.SetActive(true);
            topSymButton.GetComponentInChildren<Text>().text = "♣";
        }
    }
    [PunRPC]
    void SyncRevers(bool revers)
    {
        reversTurn = revers;
    }
    [PunRPC]
    public void ThrowCard(int pindex, int cindex)
    {
        players[pindex].GetComponent<OneCardPlayer>().ThrowCard(cindex);
    }
    [PunRPC]
    void ShoutOne(bool succeed,int pindex)
    {
        if (succeed)
        {
            players[pindex].GetComponent<OneCardPlayer>().isOneCardShouted = succeed;
        }

        oneCardShout.Play();
    }
    [PunRPC]
    void ResetDeck()
    {
        for (int i= cardset2.Count-1; i >=0; i --)
        {
            cardset1.Add(cardset2[i]);
            cardset2[i].transform.parent = cardStack1.transform;
            cardset2.RemoveAt(i);
            
        }
        SetCardPosition();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
        else
        {

        }
    }
}
