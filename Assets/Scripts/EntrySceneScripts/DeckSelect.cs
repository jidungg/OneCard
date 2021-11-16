using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeckSelect : MonoBehaviour {

    public static DeckSelect instance;
    public Deck[] deckList = new Deck[11];
    public Button[] deckButtons = new Button[11];
    public  Button selectedDeckButton;
    public Deck selectedDeck;
    public int selectedDeckIndex;

    void Awake()
    {
        
    }
	void Start () {
        instance = this;
        
        SetDeckNames();   
        ShowDecks();
        SetSelectedDeckName();

    }
	

	void Update () {

    }

    void OnEnable()
    {
        ShowDecks();
    }

    void ShowDecks()
    {

        for (int i = 0; i <= 11; i++)
        {
            deckButtons[i].GetComponentInChildren<Text>().text =i+deckList[i].deckName;
            deckList[i].deckIndex = i;
        }
        
    }
    void SetDeckNames()
    {
        for(int i = 0; i < deckList.Length; i++)
        {
            if (PlayerPrefs.HasKey("D" + i + "N"))
            {
                deckList[i].deckName = PlayerPrefs.GetString("D" + i + "N");
            }
        }
        
    }
    void SetSelectedDeckName()
    {
        selectedDeckButton.GetComponentInChildren<Text>().text = deckList[PlayerPrefs.GetInt("SDI")].deckName;
    }

}
