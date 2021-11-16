using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeckEdit : MonoBehaviour {
    public static DeckEdit instance;
    public EntryUI _entryUI;
    public  int MaxDeckUnitCount=3;
    public RectTransform allUnitsparent;
    public RectTransform deckparent;
    public InputField deckNameInputField;
    public DeckSelect _deckSelect;

    public Button unitButton;
    public Button deckUnitButton;
    public List<Button> deckUnitButtonList = new List<Button>();

    
    public List<GameObject> unitsInDeck = new List<GameObject>();

    void Awake()
    {
        PlayerPrefs.SetInt("MDC", MaxDeckUnitCount);

        ShowUnits();
        
    }
    public void SetDeckUnits()
    {

        if (unitsInDeck.Count > 0)
        {
            print("remove unitsindeck");
            unitsInDeck.RemoveRange(0, unitsInDeck.Count );
        }

        for (int i =0 ; i< DeckSelect.instance.selectedDeck.deckUnits.Length ; i++)
        {
            if(DeckSelect.instance.selectedDeck.deckUnits[i]!=null)
            unitsInDeck.Add( DeckSelect.instance.selectedDeck.deckUnits[i]);
        }
        print(unitsInDeck.Count);

    }
    void Start () {
        
        
    }

    void Update () {
        
    }




    public void ChangeDeckName()
    {
        DeckSelect.instance.deckList[DeckSelect.instance.selectedDeckIndex].deckName = deckNameInputField.text ;
        PlayerPrefs.SetString("D" + _deckSelect.selectedDeckIndex + "N", deckNameInputField.text);
    }

    public void ShowUnits()
    {
        
        for(int i = 0; i*5< _entryUI. allUnits.Count; i++)
        {
            for( int j=0; j <= 4 && i*5+j< _entryUI.allUnits.Count; j++)
            {
                Button but=(Button) Instantiate(unitButton,allUnitsparent);
                
                but.GetComponentInChildren<Text>().text = _entryUI.allUnits[i * 5 + j].name;
                but.GetComponent<DeckUnit>().unitId = _entryUI.allUnits[i * 5 + j];
                but.GetComponent<RectTransform>().anchoredPosition = new Vector2(-220 + (j * 110), 140 - (110 * i));
            }
        }


    }


    public void ShowDeckUnits()
    {
        print("ShowDeckUnits");
        for(int i = 0; i < unitsInDeck.Count; i++)
        {
            int k = 1,n=0;
            for(int j =0; j < i; j ++)
            {
               if (unitsInDeck[j]== unitsInDeck[i])
                {
                    k++;
                    n = j;
                    break;
                }
            }
            if (k==1)
            {
                Button but = (Button)Instantiate(deckUnitButton, deckparent);
                but.GetComponent<DeckUnitIn>().unitId = unitsInDeck[i];
                deckUnitButtonList.Add(but);
                but.GetComponent<RectTransform>().anchoredPosition = new Vector2(1, 175 - (deckUnitButtonList.Count-1) * 50);
                
                Text[] txt = but.GetComponentsInChildren<Text>();
                txt[0].text =unitsInDeck[i].name;
                txt[1].text = k.ToString();
            }
            else if (k == 2)
            {
                for(int l=0; l < deckUnitButtonList.Count; l++)
                {
                    if (deckUnitButtonList[l].GetComponentsInChildren<Text>()[0].text == unitsInDeck[n].name)
                    {
                        deckUnitButtonList[l].GetComponentsInChildren<Text>()[1].text = "2";
                        break;
                    }
                    
                }

            }

        }

    }
    public void ClearButtons()
    {

        print(deckUnitButtonList.Count);
        for (int i=0; i <=deckUnitButtonList.Count-1; i++)
        {

            deckUnitButtonList[i].GetComponent<DeckUnitIn>().DestroyThisButton();

        }


        deckUnitButtonList.RemoveRange(0, deckUnitButtonList.Count);
        print(deckUnitButtonList.Count);

    }


    public void DeckUnitButtonFind(string unitname)
    {
        
        foreach (Button button in deckUnitButtonList)
        {
            if(button.GetComponentInChildren<Text>().text == unitname)
            {
                Button but = button;
                Text[] tex = but.GetComponentsInChildren<Text>();
                tex[1].text = "2";
                break;
            }

        }
        
    }


    public void InstantDUButton(GameObject unit)
    {
        Button but = (Button)Instantiate(deckUnitButton, deckparent);
        but.GetComponent<DeckUnitIn>().unitId = unit;
        deckUnitButtonList.Add(but);
        but.GetComponent<RectTransform>().anchoredPosition = new Vector2(1, 175 - (deckUnitButtonList.Count - 1) * 50);

        Text[] txt = but.GetComponentsInChildren<Text>();
        txt[0].text = unit.name;
        txt[1].text = "1";
    }


    public void RemoveDUButton(int index)
    {

        deckUnitButtonList[index].GetComponent<DeckUnitIn>().DestroyThisButton();
        deckUnitButtonList.RemoveAt(index);
        for (int j=index ; j<= deckUnitButtonList.Count - 1 ; j++)
         {

            deckUnitButtonList[j].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, -50);
         }

    }

    public void RemoveFromDeck(string unitname)
    {
        int i = 0;
        foreach(GameObject obj in unitsInDeck)
        {
            if (obj.name == unitname)
            {
                unitsInDeck.RemoveAt(i);
                break;
            }
            i++;
        }
        
    }

    public void SubmitButton()
    {
        for(int i=0; i <= MaxDeckUnitCount-1; i++)
        {
            if (i<=unitsInDeck.Count-1) {
                PlayerPrefs.SetInt("D" + _deckSelect.selectedDeckIndex + "U" + i, unitsInDeck[i].GetComponent<PlayerUnits>().idNum);
                print("D" + _deckSelect.selectedDeckIndex + "U" + i + unitsInDeck[i].GetComponent<PlayerUnits>().idNum);
            }
            else if (i > unitsInDeck.Count - 1)
            {
                PlayerPrefs.SetInt("D" + _deckSelect.selectedDeckIndex + "U" + i, _entryUI.allUnits.Count);
            }
        }

  
        
    }
}
