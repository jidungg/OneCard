using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public Vector2 gridPositon=Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnMouseEnter()
    {

        GetComponent<Renderer>().material.color += new Color(-1000f, -1000f, 0f, 0f);
    }
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color += new Color(1000f, 1000f, 0f, 0f);
    }
    void OnMouseUp()
    {
    }
}
