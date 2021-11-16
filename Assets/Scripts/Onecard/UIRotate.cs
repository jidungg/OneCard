using UnityEngine;
using System.Collections;

public class UIRotate : MonoBehaviour {
    public GameObject _camera;

	void Start () {
        
    }
	
	void Update () {
        transform.LookAt(_camera.transform);
        transform.Rotate(0, 180, 0);
    }
    
}
