using UnityEngine;
using System.Collections;

public class collider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        print("fuck");
        Destroy(other.gameObject);
    }

    void OnTriggerExit(Collider col)
    {
        print("cube trigger Exit");

    }
}
