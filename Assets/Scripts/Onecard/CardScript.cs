using UnityEngine;
using System.Collections;
using System.Text;

public class CardScript : UpperCardScript {
    public OneCardPlayer _owner;
    public GameObject _quad2;  
    public bool isMoving = false;
    public int handIndex;
    public Vector3 movingPos;
    Vector3 highlightedScale;  
    Vector3 deHighlightedScale;
    Vector3 deHighlightedPos;
    private bool dragging = false;
    public bool canThrow;
    private float distance;
    public string cardNum;
    public string cardSym;

    void Start () {
        
        deHighlightedScale = transform.localScale;
    }
	

	void Update () {
       if (isMoving)
        {
            
            if (Vector3.Distance(transform.position, movingPos) >= 0.1f)
            {
                transform.position += (movingPos - transform.position).normalized * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, movingPos) < 0.1f)
                {
                    print("MoveCard Coroutine: " + gameObject.name + " position:" + gameObject.transform.position);
                    transform.position = movingPos;
                    _owner.ArrayHands();
                    SaveTransform();
                    isMoving = false;                    
                }
            }
        }
        if (dragging&&_owner.itsMe)
        {
            
            Ray ray = _owner.cam.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector3( rayPoint.x,transform.position.y,rayPoint.z);
        }

    }

    public void MovingCard(Vector3 pos)
    {
        movingPos = pos;
        isMoving = true;

    }
    public void SaveTransform()
    {
        deHighlightedPos = transform.position;
        highlightedScale = transform.localScale * 1.2f;
        
    }

    public void Highlight()
    {
        if(_owner!= null)
        {
            if (_owner.itsMe)
            {
                transform.localScale = highlightedScale;
            }
        }

    } 

    public void DeHighlight()
    {
        if (_owner != null)
        {
            if (_owner.itsMe)
            {
                transform.localScale = deHighlightedScale;
                transform.position = deHighlightedPos;
            }
        }


    }
    void OnMouseEnter()
    {
        Highlight();
    }
    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, _owner.cam.transform.position);
        dragging = true;
        _owner._oneCardManager.selectedCard = this.gameObject;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void OnMouseExit()
    {
        DeHighlight();
    }

    void OnMouseUp()
    {
        
        DeHighlight();
        dragging = false;
        Ray ray = _owner.cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit)){
            print("Ray casted"+hit.collider.gameObject.name);
            if (hit.collider.gameObject == _quad2)
            {
                print("quad2 hited");
                _owner._oneCardManager.TryThrowCard(this);
            }
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        _owner._oneCardManager.selectedCard = null;
    }


}
