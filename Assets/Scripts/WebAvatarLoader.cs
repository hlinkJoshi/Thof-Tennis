using System.Runtime.InteropServices;
using ReadyPlayerMe;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson.PunDemos;
using Photon.Pun;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using System;

public class WebAvatarLoader : MonoBehaviour
{
    private const string TAG = nameof(WebAvatarLoader);
    private GameObject avatar;
    private string avatarUrl = "";

    public UIhandler uiHandler;
    public GameObject Canvas_New;
    public GameObject loadingPanel;
    public Text loadingTextLoadingScreenObj;
    public static GameObject loadedAvatar;
    public GameObject staticCharacter;
    [Header("Joystick Canvas")]
    public GameObject GamePlayCanvasForJoystick;
    [Header("multiplayer Handler Obj")]
    public multiplayerhandler multiplayerHandlerInst;

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    [DllImport("__Internal")]
    private static extern bool IsMobileModified();

    [DllImport("__Internal")]
    private static extern void Releaseiframe();

    bool isMineAvatarLoaded = false;

    [HideInInspector]
    public bool isEditAvatar = false;

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

    private void Start()
    {
        //AvatarCache.Clear();
#if !UNITY_EDITOR && UNITY_WEBGL

        PartnerSO partner = Resources.Load<PartnerSO>("Partner");
        
        WebInterface.SetupRpmFrame(partner.Subdomain);
           //Debug.Log(" *** SetupRpmFrame   *** ");
#endif
    }

    public static void SetupRpmFrame()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        PartnerSO partner = Resources.Load<PartnerSO>("Partner");
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif
    }

    public void OnWebViewAvatarGenerated(string generatedUrl)
    {
        if (isEditAvatar)
        {
            //AvatarCache.Clear();
            Debug.Log("this is edit avatar");
            StartCoroutine(uiHandler.apiHandler.CallReadyPlayerMeAvatarUrlSubmit(generatedUrl));
            isEditAvatar = false;
            StartCoroutine(OnAvatarGeneratedLoadingForEditAvatar(generatedUrl));
            return;
        }
        if (!isMineAvatarLoaded)
        {
            loadingPanel.SetActive(true);
        }
        Canvas_New.SetActive(false);
        loadingTextLoadingScreenObj.text = "Please wait while avatar is loading ...";
        loadingPanel.SetActive(true);
        uiHandler.showInGameCanvas(generatedUrl);
        // var avatarLoader = new AvatarLoader();
        // avatarUrl = generatedUrl;
        // avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        // avatarLoader.OnFailed += OnAvatarLoadFailed;
        // avatarLoader.LoadAvatar(avatarUrl);
        multiplayerHandlerInst.Started(generatedUrl, api.nickName);
        //Debug.Log("onAvatarGenerated");
    }


    public void events(object sender, ProgressChangeEventArgs args)
    {
        //Debug.Log("avatar Loading Progress " + args.Progress);
        int progress = (int)(args.Progress * 100);
        loadingTextLoadingScreenObj.text = "Please wait while avatar is loading ... " + progress + "%";
    }

    public void OnAvatarGeneratedLoading(string generatedUrl)
    {
        var avatarLoader = new AvatarLoader();
        avatarUrl = generatedUrl;
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        avatarLoader.OnFailed += OnAvatarLoadFailed;
        avatarLoader.OnProgressChanged += events;
        avatarLoader.LoadAvatar(avatarUrl);
    }

    public GameObject PlayerFollowCamera;
    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        loadedAvatar = null;
        avatar = args.Avatar;
        AvatarAnimatorHelper.SetupAnimator(args.Metadata.BodyType, avatar);
        //Canvas_New.SetActive(false);
        loadingPanel.SetActive(false);

        uiHandler.npcOnboardingObj.SetActive(true);
       // uiHandler.npcDomeObj.SetActive(true);
      //  uiHandler.npcCourtyardObj.SetActive(true);

        loadedAvatar = ReadyPlayerMe.AvatarLoader.loadedAvatarInGame;
        if (!IsMobileCheck())
            GamePlayCanvasForJoystick.SetActive(false);
        else
            GamePlayCanvasForJoystick.SetActive(true);
        
        if (loadedAvatar != null)
        {
            GameObject mainParent = staticCharacter.transform.parent.gameObject;
            Transform loadedAvtrTrans = loadedAvatar.transform;

            loadedAvtrTrans.SetParent(staticCharacter.transform);
            loadedAvtrTrans.localPosition = new Vector3(0f, 0f, 0f);
            loadedAvtrTrans.localRotation = Quaternion.Euler(Vector3.zero);
            if (!mainParent.GetComponent<PhotonAnimatorView>())
            {
                mainParent.AddComponent<PhotonAnimatorView>();
            }
            mainParent.GetComponent<Animator>().enabled = false;
            mainParent.GetComponent<Animator>().runtimeAnimatorController = avatar.GetComponent<Animator>().runtimeAnimatorController; 
            if (loadedAvtrTrans.root.GetComponent<PhotonView>().IsMine && (mainParent.name == api.nickName))
            {
                isMineAvatarLoaded = true;
                uiHandler.playerNameTextObjCharacterDialogObj.text ="<b>Hi " + mainParent.name + " !</b>";
                uiHandler.Trivia_UserNameValueTextObj.text = "<b>Hi " + mainParent.name + " !</b>";

                if(mainParent.GetComponent<ThirdPersonController>())
                {
                    mainParent.GetComponent<ThirdPersonController>().enabled = false;
                }
                if(PlayerFollowCamera.GetComponent<CameraController>())
                {
                    PlayerFollowCamera.GetComponent<CameraController>().enabled = false;
                }
                uiHandler.characterDialogPanelObj.SetActive(true);
                PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().Follow = mainParent.transform.GetChild(0);
                PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().LookAt = loadedAvtrTrans;
            }
            else
            {
                staticCharacter.transform.parent.GetComponent<ThirdPersonController>().istomove = false;
                staticCharacter.transform.parent.GetComponent<PlayerInput>().enabled = false;
                if(staticCharacter.transform.parent.GetComponent<Tracking>())
                {
                    Destroy(staticCharacter.transform.parent.GetComponent<Tracking>());
                }
            }
            loadedAvtrTrans.SetParent(staticCharacter.transform);
            loadedAvtrTrans.localPosition = new Vector3(0f, 0f, 0f);
            loadedAvtrTrans.localRotation = Quaternion.Euler(Vector3.zero);
            staticCharacter.transform.parent.GetComponent<PhotonAnimatorView>().SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
            staticCharacter.transform.parent.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("Speed", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            //loadedAvatar.transform.GetChild(2).SetParent(staticCharacter.transform.parent);
            if (mainParent.transform.GetChild(mainParent.transform.childCount - 1).name == "Armature")
            {
                Destroy(mainParent.transform.GetChild(mainParent.transform.childCount - 1).gameObject);
            }
            loadedAvatar.transform.Find("Armature").SetParent(staticCharacter.transform.parent);
            
            StartCoroutine(EnableCharacter(mainParent.GetComponent<Animator>()));
        }
        //Debug.Log(" *** OnAvatarLoadCompleted   *** ");
        //uiHandler.showInGameCanvas(avatarUrl);
        multiplayerhandler.isdownloadStarted = false;
    }

    public void AvatarLoadingProgress(ProgressChangeEventArgs args)
    {

    }

    IEnumerator EnableCharacter(Animator mainParent)
    {
        mainParent.enabled = false;
        mainParent.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        mainParent.enabled = true;
        mainParent.gameObject.SetActive(true);

        if (multiplayerhandler.instance.isTennisGameStarted)
        {
            multiplayerhandler.instance.HidePlayer();
        }
    }

    private void OnAvatarLoadFailed(object sender, FailureEventArgs args)
    {
        loadingPanel.SetActive(false);
        SDKLogger.Log(TAG,$"Avatar Load failed with error: {args.Message}");
        //Debug.Log(" *** OnAvatarLoadFailed   *** ");
#if !UNITY_EDITOR && UNITY_WEBGL
        WebInterface.SetIFrameVisibility(true);
#endif
    }


    #region Edit Avatar

    public IEnumerator OnAvatarGeneratedLoadingForEditAvatar(string generatedUrl)
    {
        //while(!AvatarCache.IsCacheEmpty())
        //{
        //    AvatarCache.Clear();
        //    yield return null;
        //}
        yield return null;
        SSTools.ShowMessage("Loading new Avatar, Please wait", SSTools.Position.top, SSTools.Time.threeSecond);
        var avatarLoader = new AvatarLoader();
        avatarUrl = generatedUrl;
        avatarLoader.OnCompleted += OnAvatarLoadCompletedForEditAvatar;
        avatarLoader.OnFailed += OnAvatarLoadFailed;
        avatarLoader.LoadAvatar(avatarUrl);
    }

    private void OnAvatarLoadCompletedForEditAvatar(object sender, CompletionEventArgs args)
    {
        loadedAvatar = null;
        avatar = args.Avatar;
        loadingPanel.SetActive(false);
        loadedAvatar = ReadyPlayerMe.AvatarLoader.loadedAvatarInGame;
        if (!IsMobileCheck())
            GamePlayCanvasForJoystick.SetActive(false);
        else
            GamePlayCanvasForJoystick.SetActive(true);

        if (loadedAvatar != null)
        {
            GameObject mainParent = uiHandler.playerArmatureObj;
            staticCharacter = mainParent.transform.GetChild(1).gameObject;
            Transform loadedAvtrTrans = loadedAvatar.transform;
            loadedAvtrTrans.SetParent(staticCharacter.transform);
            loadedAvtrTrans.localPosition = new Vector3(0f, 0f, 0f);
            loadedAvtrTrans.localRotation = Quaternion.Euler(Vector3.zero);
            PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().LookAt = loadedAvtrTrans;
            loadedAvtrTrans.SetParent(staticCharacter.transform);
            loadedAvtrTrans.localPosition = new Vector3(0f, 0f, 0f);
            loadedAvtrTrans.localRotation = Quaternion.Euler(Vector3.zero);
            if (mainParent.transform.GetChild(mainParent.transform.childCount - 1).name == "Armature")
            {
                Destroy(mainParent.transform.GetChild(mainParent.transform.childCount - 1).gameObject);
            }
            loadedAvatar.transform.Find("Armature").SetParent(staticCharacter.transform.parent);
            StartCoroutine(EnableCharacter(mainParent.GetComponent<Animator>()));
        }
        multiplayerhandler.isdownloadStarted = false;
    }

    #endregion
}
