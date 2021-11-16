using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckUnit : MonoBehaviour {
    public GameObject unitId;
    public DeckEdit _deckEdit;
   
    void Awake()
    {
        
       _deckEdit= GameObject.Find("DeckEditPanel").GetComponent<DeckEdit>();
    }
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}


    void OnMouseUp()
    {
        print("Mouse UP");
        
    
        DeckEdit.instance.unitsInDeck.Add(unitId);
  
        
    }
    public void MouseEnter()
    {
        StartCoroutine("MouseOver");
    }
    public void MouseExit()
    {
        StopCoroutine("MouseOver");
    }

    IEnumerator MouseOver()
    {

        if (Input.GetMouseButtonUp(1))
        {
            
            if( _deckEdit.MaxDeckUnitCount > _deckEdit.unitsInDeck.Count ) {
                int j = 0;
                for (int i = 0; i < _deckEdit.unitsInDeck.Count; i++)
                {
                    if (_deckEdit.unitsInDeck[i] == unitId)
                    {
                        j++;
                    }
                }
                if (j < 2)
                {
                    AddToDeck();
                    if (j == 1)
                    {
                        _deckEdit.DeckUnitButtonFind(unitId.name);
 
                    }
                    else if (j == 0)
                    {
                        _deckEdit.InstantDUButton(unitId);
                    }
                }

            }

        }
        
        yield return null;
        StartCoroutine("MouseOver");
    }
    void AddToDeck()
    {
        _deckEdit.unitsInDeck.Add(unitId);
        
    }

}
