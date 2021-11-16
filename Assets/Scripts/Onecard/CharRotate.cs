using UnityEngine;
using System.Collections;

public class CharRotate : MonoBehaviour {
    float rotateSpeed = 60f;
    Vector3 noramlScale;
    Vector3 selectedScale;

    void Awake()
    {
        noramlScale = transform.localScale;
        selectedScale = transform.localScale * 1.2f;
    }
	void Start () {
	    
	}
	
	void Update () {
        transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
	}
    void Selected()
    {
        this.transform.localScale = selectedScale;

    }
    void DeSelected()
    {

        this.transform.localScale = noramlScale;

    }
    void OnMouseDown()
    {
        Selected();
        
    }
    void OnMouseUp()
    {
        DeSelected();
    }
}
