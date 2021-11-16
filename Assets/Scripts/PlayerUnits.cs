using UnityEngine;
using System.Collections;

public class PlayerUnits : Unit {
    public bool murlignSel;
    bool drowpoint=false;
    private IEnumerator drowMoving;
    private IEnumerator unDrowMoving;
    private IEnumerator moVing;
    void Awake()
    {
        murlignSel = true;
        
    }
	void Start () {
 

    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("stop drowmoving");
            StopCoroutine(drowMoving);
        }
    }
    
    void OnMouseUp()
    {
        if(GameManage.instance.murliganing == true)
        {
            if(murlignSel == true) { murlignSel = false; print(murlignSel); }
            else if (murlignSel == false) { murlignSel = true; print(murlignSel); }
        }
    }

    public override void TurnUpdate()
    {
        if (Vector3.Distance(moveDestination, transform.position) >= 0.1f)
        {

            if (Vector3.Distance(moveDestination, transform.position) < 0.1f)
            {

            }
        }
        base.TurnUpdate();
    }
    public void MoveThisUnit(Transform pos)
    {
        print("move");
        moving = true;
        moVing = MovingUnit(pos);
        StartCoroutine(moVing);
    }

    IEnumerator MovingUnit(Transform destile)
    {
       
        if (Vector3.Distance(destile.position, transform.position) >= 0.1f)
        {
            transform.position += (destile.position - transform.position).normalized * moveSpeed * Time.deltaTime;

        }
        if (Vector3.Distance(destile.position, transform.position) < 0.1f)
        {

            transform.position = destile.position;
            moving = false;
            StopCoroutine(moVing);
        }
        yield return null;
        StartCoroutine(MovingUnit(destile));
    }
    public void DrowMoveThisUnit(int handCount)
    {
        drowMoving = DrowMoving(handCount);
        StartCoroutine(drowMoving);

    }
    IEnumerator DrowMoving(int handCount)
    {
        print("DrowMoving co, drowpoint ="+drowpoint);
        if (drowpoint == false) {
            if (Vector3.Distance(GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][9].transform.position, transform.position) >= 0.1f)
            {
                transform.position += (GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][9].transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][9].transform.position, transform.position) < 0.1f)
                {
                    transform.position = GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][9].transform.position;

                    drowpoint = true;
                }
            }
        }
        else if (drowpoint == true)
        {

            if (Vector3.Distance(GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][handCount-1].transform.position, transform.position) >= 0.1f)
            {
                transform.position += (GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][handCount - 1].transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][handCount - 1].transform.position, transform.position) <10f)
                {
                    transform.position = GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][handCount - 1].transform.position;

                    drowpoint = true;
                    print("Stop DrowMoving");
                    StopCoroutine(drowMoving);
                }
            }
            
        }

        yield return null;
        StartCoroutine(drowMoving);
    }

    public void UnDrowMoveThisUnit()
    {
        print("Undrow");
        
        MoveThisUnit(GameManage.instance.castleTiles[PhotonNetwork.player.ID - 1][9].transform);
        unDrowMoving = UnDrowMoving();
        StartCoroutine(UnDrowMoving());
    }
    IEnumerator UnDrowMoving()
    {
        print("UndrowMoving");
        if (moving == false)
        {
            print("moving=" + moving);
            if(PhotonNetwork.player.ID == 1) {
                MoveThisUnit(GameManage.instance.deckUnitTrans1);
            }
            if (PhotonNetwork.player.ID == 2) {
                MoveThisUnit(GameManage.instance.deckUnitTrans2);
            }
            StopCoroutine(UnDrowMoving());
        }
        yield return null;
        StartCoroutine(UnDrowMoving());
    }
}
