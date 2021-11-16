using UnityEngine;
using System.Collections;

public class CastleDoor : MonoBehaviour {

    public Animator doorAni;

	void Start () {

    }
	

	void Update () {
        
        

    }

    void OnTriggerEnter(Collider col)
    {

        doorAni.SetBool("Door Open", true);
    }

    void OnTriggerExit(Collider col)
    {

        doorAni.SetBool("Door Open", false);
    }
}
