using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class onetoOneChatTest : MonoBehaviourPunCallbacks
{
    public static string currentPlayer;
    bool isActive = false;
    private void Start()
    {
        Invoke("test",5);
    }
    void test()
    {
        isActive = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && transform.name != other.transform.name)
        {
            if(PhotonChatManager.PhotonChatManagerInst.currentPlayerName == other.transform.name)
            {
                Debug.LogError("Ext");
                PhotonChatManager.PhotonChatManagerInst.oneToOneChatDialog.SetActive(false);
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (this.photonView.IsMine)
        {
            if (other.CompareTag("Player") && transform.name != other.transform.name )
            {
                if (isActive)
                {
                    Debug.Log("________" + other.name);
                    PhotonChatManager.PhotonChatManagerInst.playerNameTxtOnPrvtChat.text = "Do you want to chat with " + other.transform.name;
                    PhotonChatManager.PhotonChatManagerInst.currentPlayerName = other.transform.name;
                    PhotonChatManager.PhotonChatManagerInst.oneToOneChatDialog.SetActive(true);
                }
            }
        }      
    }
}
