using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class ButtonCont : MonoBehaviour {
    public string name= null;

	void Start () {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        //entry.callback.AddListener((eventData) => { manager.Fn(index); });

    }
	

	void Update () {
	    
	}
}
