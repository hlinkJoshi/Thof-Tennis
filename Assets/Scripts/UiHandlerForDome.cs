using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using com.pakhee.common;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using static api;

public class UiHandlerForDome : MonoBehaviour
{
    public class FrameClass
    {
        public int code;
        public string message;
        //public List<FrameDetailClass> data;
        public FrameDetailClass data;
    }
    [Serializable]
    public class FrameDetailClass
    {
        public int id;
        public string file_name;
        public int famer_id;
        public int statue_id;
        public List<FramerDetails> frames;
        public List<statuesDetails> statues;
    }

    [Serializable]
    public class FramerDetails
    {
        public int id;
        public string file_name;
        public int famer_id;
        public int statue_id;
    }

    [Serializable]
    public class statuesDetails
    {
        public int id;
        public int famer_id;
    }

    public class FrameList
    {
        public int code;
        public string message;
        public List<FrameListDetails> data;
    }
    [Serializable]
    public class FrameListDetails
    {
        public string name;
        public string full_name;
        public string nationality;
        public string class_of;
        public string overall_record;
        public string type;
    }

    [Header("Camera of Demo")]
    public Camera CameraForDemo;

    [Header("Canvas For Dome")]
    public Canvas CanvasForDome;

    [Header("Home Screen")]
    public GameObject homeScreenObj;
    public Text ballAmountTexthHomeScreenObj;

    [Header("Artifact Screen")]
    public GameObject ArtifactScreenObj;
    public GameObject ArtifactPanelMainParentObj;
    public GameObject ArtifactParentObj;
    public Text ArtifactNameArtifactScreenTextObj;
    public TextMeshProUGUI ArtifactDescriptionArtifactScreenTextObj;

    [Header("Gallery Screen")]
    public GameObject artifactGallaryPanel;
    public Image artifactImageObjArtifactGallaryScreenObj;
    public Button previousButtonArtifactGallaryScreenObj;
    public Button nextButtonartifactGalleryScreenObj;

    [Header("Dome Frame Objects")]
    public List<GameObject> frameOfFamerListInDomeObj;

    [Header("Dome Frame Objects")]
    public GameObject famerRecordPrefabObj;
    public GameObject FamerRecordScreenObj;
    public Transform famerRecordContainerParentObj;

    [Header("Score")]
    public Text Score_text;

    //static string baseUrl = "http://52.21.127.189/ithof_admin/api/v1/";    
    //string artifactUrl = baseUrl + "artifact";
    //string galleryUrl = baseUrl + "";
    //string frameListUrl = baseUrl + "frames";
    //string getfarmerlistUrl = baseUrl + "hall-of-famers/list";

    string jsonData = "";
    string key12 = "j4y8jonjNSHjp5A8AHEo2WbdsPz837b5";
    string iv12 = "j4y8jonjNSHjp5A8";
    string key1;

    public bool isDetailOpen = false;

    GameObject artifactToDisplayObj;
    float artifactMaxScale = 0;
    float artifactMinScale = 0;
    float artifactScaleAmount = 15;
    UIhandler uiHandlerInst;

    multiplayerhandler  multiplayerInst;
    Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    Task t;

    [DllImport("__Internal")]
    private static extern bool IsMobile();
    [DllImport("__Internal")]
    private static extern bool IsMobileModified();

    public GameObject listplayer;
    public Texture spp;

    public bool isMobile()
    {
    #if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
    #endif
        return false;
    }

    public bool IsMobileCheck()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobileModified();
        #endif
        return false;
    }

    public static UiHandlerForDome instance;
    void Awake()
    {
        instance = this;        
    }

    void Start()
    {
        if (GameObject.Find("Uihandler"))
        {
            uiHandlerInst = GameObject.Find("Uihandler").GetComponent<UIhandler>();
            multiplayerInst= GameObject.Find("multiplayerHandler").GetComponent<multiplayerhandler>();
        }

        multiplayerInst.lookat1.SetActive(true);
        multiplayerInst.lookat2.SetActive(true);
        multiplayerInst.lookat3.SetActive(true);
        multiplayerInst.lookat4.SetActive(true);
        multiplayerInst.lookat5.SetActive(true);

        uiHandlerInst.npcDomeObj.SetActive(true);
        uiHandlerInst.npcCourtyardObj.SetActive(true);

        mainCamera = Camera.main;
        if (!IsMobileCheck())
        {
            ArtifactPanelMainParentObj.transform.localScale = Vector3.one * 0.7f;
            artifactGallaryPanel.transform.GetChild(0).localScale = Vector3.one * 0.7f;
            FamerRecordScreenObj.transform.GetChild(0).localScale = Vector3.one * 0.7f;
        }
        StartCoroutine(CallFamerListDetails());
        listplayer.transform.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap",spp);
    }

    // Ff Tag is Frame
    // artifact Tag is 3d Artifact Model

    float distance;
    void Update()
    {
        ray = ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (!isDetailOpen && ((hit.transform.CompareTag("artifact") || hit.transform.CompareTag("Ff") || hit.transform.CompareTag("IDot") || hit.transform.CompareTag("FDot"))))
            {
                distance = Vector3.Distance(uiHandlerInst.playerArmatureObj.transform.position, hit.transform.position);
                if (distance >= 6)
                    return;

                uiHandlerInst.overCursur();        
                if (hit.transform.CompareTag("artifact"))
                {
                    if (!isDetailOpen && Input.GetMouseButtonDown(0))
                    {
                        OnClickArtifact(hit.transform.gameObject);
                    }
                }
                else if(hit.transform.CompareTag("IDot"))
                {
                    if(!isDetailOpen && Input.GetMouseButtonDown(0))
                    {
                        if(hit.transform.GetComponent<AlphaChanger>())
                        {
                            GameObject interactableObj = hit.transform.GetComponent<AlphaChanger>().interactableObj;
                            OnClickArtifact(interactableObj);
                        }
                    }
                }
                else if(hit.transform.CompareTag("FDot"))
                {
                    if(!isDetailOpen && Input.GetMouseButtonDown(0))
                    {
                        isDetailOpen = true;
                        uiHandlerInst.OnClickBiographyInGameMenuScreen(hit.transform.parent.name, hit.transform.parent.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture);
                    }
                }
                else if (hit.transform.CompareTag("Ff"))
                {
                   // hit.transform.GetComponent<Outline>().enabled = true;
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (hit.transform.TryGetComponent<MeshRenderer>(out MeshRenderer component))
                        {
                            isDetailOpen = true;
                            uiHandlerInst.OnClickBiographyInGameMenuScreen(hit.transform.name,hit.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture);
                        }
                        else
                        {
                            if (int.TryParse(hit.transform.name, out int aa))
                            {
                                if (aa < (frameOfFamerListInDomeObj.Count - 1))
                                {
                                    uiHandlerInst.OnClickBiographyInGameMenuScreen(hit.transform.name, frameOfFamerListInDomeObj[aa - 1].transform.GetComponent<MeshRenderer>().material.mainTexture);
                                    isDetailOpen = true;
                                }
                                else
                                {
                                    uiHandlerInst.OnClickBiographyInGameMenuScreen(hit.transform.name, null);
                                    isDetailOpen = true;
                                }
                            }
                            else
                            {
                                SSTools.ShowMessage("Statue is not assign", SSTools.Position.top, SSTools.Time.oneSecond);
                            }
                        }
                    }
                }
            }
            else if (!isDetailOpen && hit.transform.name == "AllFamer")
            {
                distance = Vector3.Distance(uiHandlerInst.playerArmatureObj.transform.position, hit.transform.position);
                if (distance >= 6)
                    return;

                uiHandlerInst.overCursur();
                if (Input.GetMouseButtonDown(0))
                {
                    OnClickAllFamerFrame();
                }
            }
            else
            {
                uiHandlerInst.leftCursur();
            }
        }
    }

    public void OnClickGalleyArtifactDetailScreen()
    {
        if (artifactGallerySprites.Count > 0)
        {
            artifactImageObjArtifactGallaryScreenObj.sprite = artifactGallerySprites[0];
        }
        previousButtonArtifactGallaryScreenObj.interactable = false;
        //nextButtonartifactGalleryScreenObj.interactable = true;
        artifactGallaryPanel.SetActive(true);
        ArtifactScreenObj.SetActive(false);
    }

    public void OnClickArtifactArtifactDetailScreen()
    {
        ArtifactScreenObj.SetActive(true);
        artifactGallaryPanel.SetActive(false);
    }

    public void OnClickNextButtonGalleryScreen()
    {
        artifactGallaryImagesCounter++;
        Debug.Log("coutner :- " + artifactGallaryImagesCounter + "   " + artifactGallerySprites.Count);
        artifactImageObjArtifactGallaryScreenObj.sprite = artifactGallerySprites[artifactGallaryImagesCounter];
        if(artifactGallaryImagesCounter >= (artifactGallerySprites.Count-1))
        {
            nextButtonartifactGalleryScreenObj.interactable = false;
        }
        if(!previousButtonArtifactGallaryScreenObj.interactable)
        {
            previousButtonArtifactGallaryScreenObj.interactable = true;
        }
        
    }

    public void OnClickPreviousButtonGalleryScreen()
    {
        artifactGallaryImagesCounter--;
        Debug.Log("coutner :- " + artifactGallaryImagesCounter);
        artifactImageObjArtifactGallaryScreenObj.sprite = artifactGallerySprites[artifactGallaryImagesCounter];
        if (artifactGallaryImagesCounter <= 0)
        {
            previousButtonArtifactGallaryScreenObj.interactable = false;
        }
        if (!nextButtonartifactGalleryScreenObj.interactable)
        {
            nextButtonartifactGalleryScreenObj.interactable = true;
        }
    }

    public void OnClickArtifact(GameObject artifactObj)
    {
        Score_text.text= uiHandlerInst.ballBalanceValueTextHomeScreenOnGameObj.text;
        if (uiHandlerInst != null)
        {
            if (IsMobileCheck())
            {
                uiHandlerInst.GamePlayCanvasForJoystick.SetActive(false);
            }
            uiHandlerInst.inGameMenuCanvasObj.gameObject.SetActive(false);
        }
        isDetailOpen = true;
        CameraForDemo.gameObject.SetActive(true);
        CanvasForDome.gameObject.SetActive(true);
        homeScreenObj.gameObject.SetActive(true);
        ArtifactScreenObj.SetActive(true);
        artifactGallaryPanel.SetActive(false);
        artifactToDisplayObj = null;
 
        foreach (Transform t in ArtifactParentObj.transform)
            Destroy(t.gameObject);
        
        Debug.Log("clik artifact " + artifactObj.name);
        uiHandlerInst.playerArmatureObj.GetComponent<ThirdPersonController>().resetPlayerStates(true);
        uiHandlerInst.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        uiHandlerInst.playerFollowCameraObj.GetComponent<CameraController>().enabled = false;

        GameObject gm = Instantiate(artifactObj, ArtifactParentObj.transform) as GameObject;
        gm.transform.localPosition = Vector3.zero;
        gm.transform.localScale = Vector3.one * (600);
        gm.transform.rotation = Quaternion.Euler(0,180,0);
        string[] artifactNameArray = artifactObj.name.Split('_');

        //if (artifactNameArray[3] == "28" || artifactNameArray[3] == "31")
        //{
        //    gm.transform.rotation = Quaternion.Euler(0, 0, 0);
        //}

        switch (artifactNameArray[3])
        {
            case "28":
                gm.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "31":
                gm.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "18":
                gm.transform.rotation = Quaternion.Euler(-270, 0, 0);
                break;
            case "21":
                gm.transform.rotation = Quaternion.Euler(0, 90, -90);
                break;
        }

        int ScaleSize = 0;
        if (artifactNameArray.Length > 0)
        {
            bool isParsed = int.TryParse(artifactNameArray[artifactNameArray.Length - 3], out ScaleSize);
            if (isParsed)
            {
                gm.transform.localScale = Vector3.one * ScaleSize;
                artifactMinScale = ScaleSize;
            }
            if (artifactNameArray.Length > 1)
            {
                isParsed = int.TryParse(artifactNameArray[artifactNameArray.Length - 2], out ScaleSize);
                if (isParsed)
                {
                    artifactMaxScale = ScaleSize;
                }
            }
        }
        artifactScaleAmount = (float)(artifactMaxScale - artifactMinScale) / 5f;
        if (gm.GetComponent<Animation>())
            Destroy(gm.transform.GetComponent<Animation>());
        
        if (gm.GetComponent<BoxCollider>())
            Destroy(gm.transform.GetComponent<BoxCollider>());
        
        if (gm.GetComponent<Rigidbody>())
            Destroy(gm.transform.GetComponent<Rigidbody>());

        if (gm.GetComponent<Interactable>())
            Destroy(gm.GetComponent<Interactable>());

        if(gm.GetComponent<Outline>())
        {
            gm.GetComponent<Outline>().enabled = false;
            gm.GetComponent<Outline>().OnDisable();
            gm.GetComponent<Outline>().OnDisable();
        }
        
        artifactToDisplayObj = gm;
        int artifactCode = 40;
        int.TryParse(artifactNameArray[artifactNameArray.Length - 1], out artifactCode);
        StartCoroutine(CallArtifactDetails(artifactCode));

        ballAmountTexthHomeScreenObj.text = uiHandlerInst.ballBalanceValueTextHomeScreenOnGameObj.text;
    }

    public void OnClickCloseArtifaceScreen()
    {
        if (uiHandlerInst != null)
        {
            if (IsMobileCheck())
            {
                uiHandlerInst.GamePlayCanvasForJoystick.SetActive(true);
            }
            uiHandlerInst.inGameMenuCanvasObj.gameObject.SetActive(true);
        }
        foreach (Transform t in ArtifactParentObj.transform)
        {
            Destroy(t.gameObject);
        }
        uiHandlerInst.playerArmatureObj.GetComponent<StarterAssetsInputs>().jump = false;
        uiHandlerInst.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        uiHandlerInst.playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        artifactToDisplayObj = null;
        CanvasForDome.gameObject.SetActive(false);
        artifactGallaryPanel.SetActive(false);
        CameraForDemo.gameObject.SetActive(false);
        isDetailOpen = false;
    }

    public void OnClickZoominButtonArtifactDetailsScreen()
    {
        if (artifactToDisplayObj == null)
            return;

        float nextSize = artifactToDisplayObj.transform.localScale.x + artifactScaleAmount;
        if (nextSize > artifactMaxScale)
            artifactToDisplayObj.transform.localScale = Vector3.one * artifactMaxScale;
        else
            artifactToDisplayObj.transform.localScale += Vector3.one * (artifactScaleAmount);
    }

    public void OnClickZoomOutbuttonArtifactDetailsScreen()
    {
        if (artifactToDisplayObj == null)
            return;

        float nextSize = artifactToDisplayObj.transform.localScale.x - artifactScaleAmount;
        if (nextSize < artifactMinScale)
            artifactToDisplayObj.transform.localScale = Vector3.one * artifactMinScale;
        else
            artifactToDisplayObj.transform.localScale -= Vector3.one * (artifactScaleAmount);
    }

    string replaceString(string description)
    {
        string des = description;
        des = des.Replace("&nbsp;", " ");
        des = des.Replace("&rsquo;", "'");
        return des;
    }

    public IEnumerator CallArtifactDetails(int artifactCode)
    {
        string artifactUrl = api.baseUrl + "artifact";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(artifactUrl, "post");
        request.SetRequestHeader("api-key", "RjN08fAefrzO5DQKz3Kvjg==");
        request.SetRequestHeader("Content-Type", "text/plain");
        if (PlayerPrefs.GetString("token") != "")
        {
            request.SetRequestHeader("token", Encryption(PlayerPrefs.GetString("token")));
        }
        jsonData = "{\"artifact_code\":" + artifactCode + "}";
        if (jsonData != "")
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            string decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            Debug.Log(decryptString);
            Artifact response = JsonUtility.FromJson<Artifact>(decryptString);
            if (response.code == 1)
            {

                ArtifactNameArtifactScreenTextObj.text = response.data.title;
                //.Replace("&nbsp;", " ");
                //discription = discription.Replace("&amp;", "&");
                //discription = discription.Replace("<p>", "");
                //discription = discription.Replace("</p>", "");art`
                //discription = discription.Replace("<strong>", "<b>");
                //discription = discription.Replace("</strong>", "</b>");
                ArtifactDescriptionArtifactScreenTextObj.text = replaceString(response.data.description);
                artifactGallerySprites.Clear();
                artifactGallaryImagesCounter = 0;
                artifactImageObjArtifactGallaryScreenObj.sprite = null;
                previousButtonArtifactGallaryScreenObj.interactable = false;
                nextButtonartifactGalleryScreenObj.interactable = false;

                for (int i = 0; i < response.data.uploads.Count; i++)
                {
                    //t = new Task(DownloadProfile_pic(response.data.uploads[i].file_name));
                    imageDownloaded = false;
                    Debug.Log("file extension : " + System.IO.Path.GetExtension(response.data.uploads[i].file_name));
                    if (System.IO.Path.GetExtension(response.data.uploads[i].file_name) == ".mp4")
                    {
                        continue;
                    }
                    StartCoroutine(DownloadProfile_pic(response.data.uploads[i].file_name, 0,(isDone, num,sprite) => { }));
                    while (!imageDownloaded)
                    {
                        yield return null;
                    }
                    artifactGallerySprites.Add(gameSprite);
                }
                if(artifactGallerySprites.Count > 0)
                {
                    artifactImageObjArtifactGallaryScreenObj.sprite = artifactGallerySprites[0];
                }

                if (artifactGallerySprites.Count > 1)
                {
                    nextButtonartifactGalleryScreenObj.interactable = true;
                }
            }
        }
    }

    List<Sprite> artifactGallerySprites = new List<Sprite>();
    int artifactGallaryImagesCounter = 0;

    #region FamerFrame Api
    public IEnumerator CallFamerListDetails()
    {
        string frameListUrl = api.baseUrl + "frames";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(frameListUrl, "post");
        request.SetRequestHeader("api-key", "RjN08fAefrzO5DQKz3Kvjg==");
        request.SetRequestHeader("Content-Type", "text/plain");
        if (PlayerPrefs.GetString("token") != "")
        {
            request.SetRequestHeader("token", Encryption(PlayerPrefs.GetString("token")));
        }

        yield return request.SendWebRequest();
        if(request.isDone)
        {
            FrameClass response = JsonUtility.FromJson<FrameClass>(Decryption(request.downloadHandler.text.Trim('"')));
            if(response.code == 1)
            {
                //Debug.Log("data count of frame " + Decryption(request.downloadHandler.text.Trim('"')));
                //for (int i = 0; i < response.data.Count; i++)
                //{
                //    frameOfFamerListInDomeObj[i].transform.parent.name = response.data[i].famer_id.ToString();
                //        StartCoroutine(DownloadProfile_pic(response.data[i].file_name, i, (isDone, frameNum, sprite) =>
                //        {
                //            if (frameNum < frameOfFamerListInDomeObj.Count)
                //            {
                //                frameOfFamerListInDomeObj[frameNum].GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", sprite);
                //            }
                //        }));

                //    string tempname;
                //    if (response.data[i].statue_id != 0)
                //    {
                //       tempname = response.data[i].famer_id.ToString();
                //       switch (response.data[i].statue_id)
                //        {
                //            case 1:
                //                multiplayerInst.lookat1.name = tempname;
                //                break;
                //            case 2:
                //                multiplayerInst.lookat2.name = tempname;
                //                break;
                //            case 3:
                //                multiplayerInst.lookat3.name = tempname;
                //                break;
                //            case 4:
                //                multiplayerInst.lookat4.name = tempname;
                //                break;
                //            case 5:
                //                multiplayerInst.lookat5.name = tempname;
                //                break;
                //        }
                //    }
                //}

                Debug.Log("data count of frame " + Decryption(request.downloadHandler.text.Trim('"')));
                Debug.Log("_____" + response.data.frames.Count);

                for (int i = 0; i < response.data.frames.Count; i++)
                {
                    frameOfFamerListInDomeObj[i].transform.parent.name = response.data.frames[i].famer_id.ToString();
                    StartCoroutine(DownloadProfile_pic(response.data.frames[i].file_name, i, (isDone, frameNum, sprite) =>
                    {
                        if (frameNum < frameOfFamerListInDomeObj.Count)
                        {
                            frameOfFamerListInDomeObj[frameNum].GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", sprite);
                        }
                    }));
                }
                for (int i = 0; i < response.data.statues.Count; i++)
                {
                    switch (response.data.statues[i].id)
                    {
                        case 1:
                            multiplayerInst.lookat1.name = response.data.statues[i].famer_id.ToString();
                            break;
                        case 2:
                            multiplayerInst.lookat2.name = response.data.statues[i].famer_id.ToString();
                            break;
                        case 3:
                            multiplayerInst.lookat3.name = response.data.statues[i].famer_id.ToString();
                            break;
                        case 4:
                            multiplayerInst.lookat4.name = response.data.statues[i].famer_id.ToString();
                            break;
                        case 5:
                            multiplayerInst.lookat5.name = response.data.statues[i].famer_id.ToString();
                            break;

                            //case 0:
                            //    multiplayerInst.lookat1.name = response.data.statues[i].famer_id.ToString();
                            //    break;
                            //case 1:
                            //    multiplayerInst.lookat2.name = response.data.statues[i].famer_id.ToString();
                            //    break;
                            //case 2:
                            //    multiplayerInst.lookat3.name = response.data.statues[i].famer_id.ToString();
                            //    break;
                            //case 3:
                            //    multiplayerInst.lookat4.name = response.data.statues[i].famer_id.ToString();
                            //    break;
                            //case 4:
                            //    multiplayerInst.lookat5.name = response.data.statues[i].famer_id.ToString();
                            //    break;
                    }
                }
            }
        }
    }
    #endregion

    #region image Download
    Sprite gameSprite;
    bool imageDownloaded = false;
    IEnumerator DownloadProfile_pic(string urllink, int number,Action<bool, int ,Texture> response)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(urllink);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            gameSprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero, 10f, 0, SpriteMeshType.Tight);
            imageDownloaded = true;
            response(true, number, myTexture);
            Debug.Log("imag downloaded");
        }
    }
    #endregion

    public string Encryption(string inputData)
    {
        CryptLib _crypt = new CryptLib();
        key1 = CryptLib.getHashSha256(key12, 32);
        String cypherText = _crypt.encrypt(inputData, key1, iv12);
        //Debug.Log("cypher text is " + cypherText);
        return cypherText;
    }
    public string Decryption(string inputData)
    {
        CryptLib _crypt = new CryptLib();
        key1 = CryptLib.getHashSha256(key12, 32);
        String cypherText = _crypt.decrypt(inputData, key1, iv12);
        return cypherText;
    }
    #region
    public void OnClickAllFamerFrame()
    {
        Score_text.text = uiHandlerInst.ballBalanceValueTextHomeScreenOnGameObj.text;
        // Stop Control on Webl WASD 
        uiHandlerInst.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        uiHandlerInst.playerFollowCameraObj.GetComponent<CameraController>().enabled = false;

        if (uiHandlerInst != null)
        {
            if (IsMobileCheck())
            {
                uiHandlerInst.GamePlayCanvasForJoystick.SetActive(false);
            }
            uiHandlerInst.inGameMenuCanvasObj.gameObject.SetActive(false);
        }
        isDetailOpen = true;
        CameraForDemo.gameObject.SetActive(true);
        CanvasForDome.gameObject.SetActive(true);
        homeScreenObj.gameObject.SetActive(true);
        FamerRecordScreenObj.SetActive(true);
        foreach (Transform t in famerRecordContainerParentObj)
        {
            Destroy(t.gameObject);
        }
        StartCoroutine(Famerlist());
    }

    public void OnClickCloseAllFamerScreen()
    {
        if (uiHandlerInst != null)
        {
            if (IsMobileCheck())
            {
                uiHandlerInst.GamePlayCanvasForJoystick.SetActive(true);
            }
            uiHandlerInst.inGameMenuCanvasObj.gameObject.SetActive(true);
            // Start Control on Webl WASD 
            uiHandlerInst.playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
            uiHandlerInst.playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        }
        isDetailOpen = false;
        CameraForDemo.gameObject.SetActive(false);
        CanvasForDome.gameObject.SetActive(false);
        homeScreenObj.gameObject.SetActive(false);
        FamerRecordScreenObj.SetActive(false);
    }

    public IEnumerator Famerlist()
    {
        string getfarmerlistUrl = api.baseUrl + "hall-of-famers/list";
        UnityWebRequest request = UnityWebRequest.Get(getfarmerlistUrl);
        request.SetRequestHeader("api-key", "RjN08fAefrzO5DQKz3Kvjg==");
        request.SetRequestHeader("Content-Type", "text/plain");
        if (PlayerPrefs.GetString("token") != "")
        {
            request.SetRequestHeader("token", Encryption(PlayerPrefs.GetString("token")));
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            Debug.Log("AllfamerResponse :   " + Decryption(request.downloadHandler.text.Trim('"')));
            FrameList response = JsonUtility.FromJson<FrameList>(Decryption(request.downloadHandler.text.Trim('"')));
            if (response.code == 1)
            {
                //Debug.Log("total count  : " + response.data.Count);
                for (int i = 0; i < response.data.Count; i++)
                {
                    Debug.Log("setting famer record " + i);
                    GameObject gm = Instantiate(famerRecordPrefabObj) as GameObject;
                    gm.transform.SetParent(famerRecordContainerParentObj, false);
                    gm.transform.GetChild(0).GetComponent<Text>().text = response.data[i].full_name;
                    gm.transform.GetChild(1).GetComponent<Text>().text = response.data[i].nationality;
                    gm.transform.GetChild(2).GetComponent<Text>().text = response.data[i].class_of;
                    gm.transform.GetChild(3).GetComponent<Text>().text = response.data[i].type;
                }
            }
        }
    }
    #endregion

}