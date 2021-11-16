using UnityEngine;
using System.Collections;

public class Sphere :Photon.MonoBehaviour {
    public int m_data;

	void Start () {
        m_data = 0;

	}

	void Update () {
	
	}

    void OnGUI()
    {
        GUILayout.Label(m_data.ToString());
    }

    
    public void IncreaseData()
    {

        m_data++;
        PhotonNetwork.RaiseEvent(0, m_data, true, null);
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SendData", PhotonTargets.All,  m_data);

    }
    [PunRPC]
    public void SendData(int data)
    {
        print("SendData RpC, data:"+data);
        m_data = data;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_data);

        }
        else
        {
            m_data = (int)stream.ReceiveNext();
            print("read onphotonserializeview" + info.sender);
        }
    }

    public void SerializeState(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(m_data);

        }
        else
        {
            // Network player, receive data
            m_data = (int)stream.ReceiveNext();
            print("read serializeState" + info.sender);
        }
    }

}
