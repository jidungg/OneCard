using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class CameraMove : Photon.MonoBehaviour {

    public GameObject character;
    Camera cam;
    public GameObject quad ;
    public GameObject text;

    Quaternion normalRot;
    float x;
    float y;
    float xSpeed =75f;
    float ySpeed=40f;
    void Awake()
    {
        normalRot = transform.rotation;
    }
	void Start () {
        cam = Camera.main;
	}
	void OnJoinedRoom()
    {
        
        GameObject[] list = GameObject.FindGameObjectsWithTag("Player");
        for (int i=0; i < list.Length; i++)
        {
            if (list[i].GetComponent<PhotonView>().isMine)
            {
                character = list[i];
            }
        }
    }
	void Update () {
        if (PhotonNetwork.inRoom) {
            transform.position = character.transform.position;
        }

        if(Input.touchCount>0 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(Input.touchCount - 1).fingerId))
            {
                StartCoroutine("ScreenDrag", Input.GetTouch(Input.touchCount - 1).fingerId);
            }           
        }
      
    }


    IEnumerator ScreenDrag(int finger)
    {
        while (true)
        {
            print("ScreenDrag");
            Touch touch = Input.GetTouch(0);
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).fingerId == finger)
                {

                    touch = Input.GetTouch(i);

                }
            }

            text.GetComponent<TextMesh>().text = touch.phase.ToString();
            if (touch.phase == TouchPhase.Moved)
            {

                print("Touch Moved");

                x += touch.deltaPosition.x * xSpeed * Time.deltaTime;
                y -= touch.deltaPosition.y * ySpeed * Time.deltaTime;
                y = Mathf. Clamp(y, -45f, 45f);
                Quaternion rotation = Quaternion.Euler(y, x, 0);
                transform.rotation = rotation;
                //transform.Rotate(new Vector3(-touch.deltaPosition.y, touch.deltaPosition.x, 0));



            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StopCoroutine("ScreenDrag");
                yield break;
            }
            yield return null;
        }

       
    }
    void OnGUI()
    {
        if (Input.touchCount > 0)
        {
            Touch touch;
            touch = Input.GetTouch(Input.touchCount - 1);
            GUILayout.Label(touch.phase.ToString());
            GUILayout.Label(touch.fingerId.ToString());
            GUILayout.Label(transform.rotation.eulerAngles.ToString());
        }

        
    }
}
