using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Deck : MonoBehaviour {
    public string deckName;
    public GameObject[] deckUnits = new GameObject[2];
    public  Button selectedDeckButton;
    public int deckIndex;


    // Use this for initialization
    void Start () {
        deckName = "Empty";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Selecting()
    {
        DeckSelect.instance.selectedDeck = this;
        
        selectedDeckButton.GetComponentInChildren<Text>().text = DeckSelect.instance.selectedDeck.deckName;
        DeckSelect.instance.selectedDeckIndex = deckIndex;
        PlayerPrefs.SetInt("SDI", deckIndex);

    }

}
