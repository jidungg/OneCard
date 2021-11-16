using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EntryUI : MonoBehaviour {
    public Animator uiani;
    public static EntryUI instance;
    public GameObject deckEditPanel;
    public GameObject myDeckPanel;
    public DeckSelect _deckSelect;
    public DeckEdit _deckEdit;
    public List<GameObject> allUnits = new List<GameObject>();

    bool vailed;
    
    void Awake()
    {
        SetUnitNum();
    }
    void Start () {
        

    }

	void Update () {


    }

    void SetUnitNum()
    {
        for(int i=0; i <= allUnits.Count-1;i++)
        {

            allUnits[i].GetComponent<PlayerUnits>().idNum = i;
        }
    }
    void OnMouseEnter()
    {

        uiani.SetBool("highlight", true);
    }

    void OnMouseOver()
    {


        
       
    }

    void OnMouseExit()
    {
       
        uiani.SetBool("highlight", false);

    }

    void OnMouseDown()
    {
        uiani.SetBool("pressed", true);

    }
    void OnMouseUp()
    {
        uiani.SetBool("pressed", false);
        if (!vailed) { SceneManager.LoadScene(1); }
        
    }

    public void DeckEditButton()
    {
        if (DeckSelect.instance.selectedDeck != null) {

            deckEditPanel.SetActive(true);
            myDeckPanel.SetActive(false);
        }

    }

    public void DeckEditButtonX()
    {
        deckEditPanel.SetActive(false);
        vailed = false;
    }
    public void MyDeckButton()
    {
        myDeckPanel.SetActive(true);
        deckEditPanel.SetActive(false);
        vailed = true;
        
    }

    public void MyDeckButtonX()
    {
        myDeckPanel.SetActive(false);
        vailed = false;
    }

    public void SetSelectedDeckUnit()
    {

        for(int i =0; i< _deckEdit.MaxDeckUnitCount; i++)
        {
            if (PlayerPrefs.GetInt("D" + _deckSelect.selectedDeckIndex + "U" + i)<allUnits.Count && PlayerPrefs.HasKey("D" + _deckSelect.selectedDeckIndex + "U" + i))
            {
                for (int j = 0; j <= allUnits.Count - 1; j++)
                {
                    if (j == PlayerPrefs.GetInt("D" + _deckSelect.selectedDeckIndex + "U" + i))
                    {
                        _deckSelect.selectedDeck.deckUnits[i] = allUnits[j];
                        break;
                    }
                }

            }
            else {
                _deckSelect.selectedDeck.deckUnits[i] = null;
                
            }
            

        }

    }

}
