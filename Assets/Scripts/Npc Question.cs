using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class NpcQuestion : MonoBehaviour
{
    public GameObject Uihandler_obj;   
    public GameObject Npc_Question_PopUp;
    public GameObject Npc_Game_PopUp;

    private UIhandler uihandler;

    Camera mainCamera;
    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        mainCamera = Camera.main;
        uihandler = Uihandler_obj.transform.GetComponent<UIhandler>();
    }

    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("npc"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    switch (hit.transform.name)
                    {
                        case "Npc1":
                            uihandler.characterDialogPanelObj.SetActive(true);
                            if(uihandler.playerArmatureObj.GetComponent<ThirdPersonController>())
                            {
                                uihandler.playerArmatureObj.GetComponent<ThirdPersonController>().resetPlayerStates(true);
                                uihandler.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
                            }
                            uihandler.playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
                            break;
                        case "Npc2":
                            if(UiHandlerForDome.instance.isDetailOpen)
                            {
                                return;
                            }
                            if (uihandler.playerArmatureObj.GetComponent<ThirdPersonController>())
                            {
                                uihandler.playerArmatureObj.GetComponent<ThirdPersonController>().resetPlayerStates(true);
                                uihandler.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
                            }
                            uihandler.playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
                            uihandler.descriptionTextNpcIntroductionObj.text = "Would you like to do a quiz to win 50 tennis ball?";
                            Npc_Question_PopUp.transform.parent.parent.gameObject.SetActive(true);
                            Npc_Question_PopUp.transform.parent.GetChild(0).gameObject.SetActive(true);
                            uihandler.playerNameValueTextNpcIntroducationObj.gameObject.SetActive(true);
                            uihandler.descriptionTextNpcIntroductionObj.gameObject.SetActive(true);
                            uihandler.playButtonNpcIntroducationObj.gameObject.SetActive(true);
                            UiHandlerForDome.instance.isDetailOpen = true;
                            break;
                        case "Npc3":
                            uihandler.npcGamePopupPanel.SetActive(true);
                            if (uihandler.playerArmatureObj.GetComponent<ThirdPersonController>())
                            {
                                uihandler.playerArmatureObj.GetComponent<ThirdPersonController>().resetPlayerStates(true);
                                uihandler.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
                            }
                            uihandler.playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
                            break;
                    }
                }
            }
            
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("__@@@_____" + other.name);
    //    if (other.name == api.nickName)
    //    {
    //        switch (transform.name)
    //        {
    //            case "Npc1"://NPC_OnboardingObj
    //                uihandler.characterDialogPanelObj.SetActive(true);
    //                break;
    //            case "Npc2":
    //                uihandler.descriptionTextNpcIntroductionObj.text = "Would you like to do a quiz to win 50 tennis ball?";
    //                Npc_Question_PopUp.transform.parent.parent.gameObject.SetActive(true);
    //                Npc_Question_PopUp.transform.parent.GetChild(0).gameObject.SetActive(true);
    //                uihandler.playerNameValueTextNpcIntroducationObj.gameObject.SetActive(true);
    //                uihandler.descriptionTextNpcIntroductionObj.gameObject.SetActive(true);
    //                uihandler.playButtonNpcIntroducationObj.gameObject.SetActive(true);
    //                break;
    //            case "Npc3":
    //                Npc_Game_PopUp.SetActive(true);
    //                break;
    //        }
    //    }
    //}
}           