using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CharSel : MonoBehaviour {
    public List<GameObject> charList;
    GameObject selectedChar;
    public Transform centerTransform;
    public GameObject cam;
    public Text informText;
    float x;
    float xSpeed = 75f;
    float charTerm=3.45f;
    int nowIndex;

    void Start () {
        SetNowIndex(0);
	}
	
	void Update () {

    }
 
    void SetNowIndex(int data)
    {
        nowIndex = data;
        selectedChar = charList[nowIndex];
        informText.text = (nowIndex+1) + "/" + charList.Count;
    }
    public void ScrollLeft()
    {

        if (nowIndex > 0)
        {

            SetNowIndex(nowIndex-1);
            iTween.MoveAdd(gameObject, new Vector3(charTerm, 0, 0), 3f);
        }
        else
        {
            return;
        }
    }
    public void ScrollRight()
    {

        if (nowIndex+1 < charList.Count)
        {

            SetNowIndex(nowIndex+1);
            iTween.MoveAdd(gameObject, new Vector3(-charTerm, 0, 0), 3f);
        }
        else
        {
            return;
        }
    }
    public void GameStart()
    {
        PlayerPrefs.SetString("Avatar", selectedChar.gameObject.name);
        SceneManager.LoadScene(1);
    }
}
