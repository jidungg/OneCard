using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
public class NetworkPlayer : Photon.MonoBehaviour
{



    void Awake()
    {

    }
    void Start()
    {

        if (photonView.isMine)
        {
            GetComponent<ThirdPersonUserControl>().enabled = true;
            GetComponent<ThirdPersonCharacter>().enabled = true;
            GetComponent<Interact>().enabled = true;
            
        }

    }

    void Update()
    {

    }

}


