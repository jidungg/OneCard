using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Interact : MonoBehaviour {
    public GameObject interactThing;

	void Start () {
	
	}
	
	void Update () {
        if(interactThing != null)
        {
            if (CrossPlatformInputManager.GetButtonUp("e"))
            {
                if(interactThing.tag == "GameTable")
                {

                    MiniGameManager gamman = interactThing.GetComponent<MiniGameManager>();
                    gamman.AddPlayer(gameObject);
                }
                else
                {
                    
                }
            }
        }

	}
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            interactThing = col.gameObject;
            
        }
       

    }

    void OnTriggerExit(Collider other)
    {
        if (interactThing != null && other.gameObject == interactThing)
        {
            
            interactThing = null;
        }

               
    }
}
