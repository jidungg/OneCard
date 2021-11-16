using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    public Vector3 moveDestination;
    public float moveSpeed = 10f;
    public int idNum;

    public bool moving = false;
    public bool attacking = false;
    
    void Awake()
    {
        
        moveDestination = transform.position;
    }
    void Start () {
	
	}
	

	void Update () {
	
	}
    public virtual void TurnUpdate()
    {

    }
}
