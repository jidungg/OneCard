using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour {
    public OneCardManager _onecardManager;

    void OnTriggerEnter()
    {
        print("OnTriggerEnter");
    }
    void OnTriggerExit()
    {
        print("OnTriggerExit");
    }
    void OnMouseUp()
    {
        print("OnMouseUp");
    }
}
