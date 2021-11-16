using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

    IEnumerator rouTine;
	// Use this for initialization
	void Start () {

        rouTine = Routine();
        StartCoroutine(rouTine);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, 0.1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -0.1f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            print("stop");
            StopCoroutine("Routine");
        }
    }

    IEnumerator Routine()
    {
        print("routine");
        yield return null;
        StartCoroutine("Routine");
    }
}
