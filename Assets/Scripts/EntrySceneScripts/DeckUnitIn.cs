using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckUnitIn : MonoBehaviour {
    public DeckEdit _deckEdit;
    public GameObject unitId;

    void Awake()
    {
        _deckEdit = GameObject.Find("DeckEditPanel").GetComponent<DeckEdit>();
    }
    void Start () {
	
	}
	
	
	void Update () {
	
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

            if (0 < _deckEdit.unitsInDeck.Count)
            {

                for (int i = 0; i <= _deckEdit.deckUnitButtonList.Count - 1; i++)
                {
        
                    Text[] tex= _deckEdit.deckUnitButtonList[i].GetComponentsInChildren<Text>();
                    if(tex[0].text == unitId.name)
                    {

                        if (tex[1].text == "1")
                        {

                            _deckEdit.RemoveDUButton(i);
                            
                        }
                        else if(tex[1].text == "2")
                        {

                            tex[1].text = "1";
                        }
                        _deckEdit.RemoveFromDeck(unitId.name);
                        break;
                    }
                }

            }

   
        }
        yield return null;
        StartCoroutine("MouseOver");
    }

    public void DestroyThisButton()
    {

        Destroy(gameObject);
    }
}
