using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Photon.Pun;
using ReadyPlayerMe;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIhandler : MonoBehaviour
{
    public const string MatchEmailPattern =
@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
  + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    [Header("Sprites")]
    public Sprite tierUnlcokedSprite;
    public Sprite tierLockedSprite;
    public Sprite tierUnlockedLineSprite;
    public Sprite tierLockedLineSprite;
    public Sprite chatNormalSprite;
    public Sprite chatMessageReceivedSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    [Header("ApiHandler Object")]
    public api apiHandler;
    [Header("WebAvatarLoader Object")]
    public WebAvatarLoader webAvatarLoaderInst;

    [Header("Panel")]
    public Canvas CanvasforUI;
    public GameObject Login_Pnl;
    public GameObject Sign_Up_Pnl;
    public GameObject Forgot_Password_Pnl;
    public GameObject Verify_Pnl;
    public GameObject Reset_Pnl;
    public GameObject Account_Pnl;

    [Header("Login Panel")]
    public InputField Email_Login_Inp;
    public InputField Password_Login_Inp;
    public Button signUpSigninScreenBtnObj;
    public Button forgotPasswordSigninScreenBtnObj;
    public Button joinGuestSigninScreenBtnObj;

    [Header("Sign Up")]
    public InputField Name_SignUp_Inp;
    public InputField NickName_SignUp_Inp;
    public InputField Country_SignUp_Inp;
    public InputField Email_SignUp_Inp;
    public InputField Passwword_SignUp_Inp;
    public InputField C_Passwword_SignUp_Inp;
    public GameObject CountryListingContainerSignupScreenObj;
    public Button signinSignupScreenBtnObj;

    [Header("Guest Signup")]
    public GameObject signupGuestPanel;
    public InputField nickNameInputFieldGuestSignupScreenObj;

    [Header("ForgotPassword Screen")]
    public GameObject forgotPasswordPanelObj;
    public InputField passwordForgotPasswordInputFieldObj;
    public Button backButtonForgotPasswordScreenBtnObj;

    [Header("Verify Screen")]
    public GameObject verifyPanelObj;
    public Text descriptionVerifyScreenTextObj;
    public Button continuteButtonVerifyScreenObj;
    public Button resendButtonVerifyScreenObj;
    public Text resendCountdownTextObj;
    public TMP_InputField mainOtpTextMeshInputFieldVerifyOtpScreenObj;
    public InputField firstInputFieldVerifyOtpScreenObj;
    public InputField secondInputFieldVerifyOtpScreenObj;
    public InputField threeInputFieldVerifyOtpScreenObj;
    public InputField fourInputFieldVerifyOtpScreenObj;
    public InputField fiveInputFieldVerifyOtpScreenObj;
    public InputField sixInputFieldVerifyOtpScreenObj;

    [Header("ResetPassword Panel")]
    public GameObject resetPasswordScreenObj;
    public InputField newPasswordResetPasswordScreenInputFieldObj;
    public InputField confirmPasswordResetPasswordScreenInputFieldObj;

    [Header("Joystick Canvas")]
    public GameObject GamePlayCanvasForJoystick;

    [Header("PlayerArmature Object")]
    public GameObject playerArmatureObj;
    public GameObject playerFollowCameraObj;

    [Header("InGame Panels")]
    public GameObject openmenuPanelInGameScreenObj;
    public GameObject menuPanelInGameScreenObj;
    public GameObject chatInGameScreenObj;
    public Canvas inGameMenuCanvasObj;

    [Header("Home Screen In Game")]
    public GameObject chatButtonHomeScreenInGameObj;
    public Text ballBalanceValueTextHomeScreenOnGameObj;
    public Image soundIconImageHomeScreenObj;

    [Header("Chat Panel")]
    public InputField chatInputFieldObj;

    [Header("BioGraphy Panel")]
    public GameObject bioGraphyPanelInGameScreenObj;

    [Header("Artifact Panel")]
    public Text ArtifactNameTextObj;
    public TextMeshProUGUI ArtifactDescriptionTextObj;
    public GameObject ArtifactImageObj;

    [Header("Character Dialog")]
    public GameObject characterDialogPanelObj;
    public GameObject walkContainerDesktopObj;
    public GameObject lookContainerDesktopObj;
    public GameObject interactContainerDesktopObj;
    public GameObject runContainerDesktopObj;
    public GameObject jumpContainerDesktopObj;
    public GameObject titleContainerDesktopObj;
    public GameObject titleContainerMobileObj;
    public GameObject walkContainerMobileObj;
    public GameObject lookContainerMobileObj;
    public GameObject interactContainerMobileObj;
    public Text playerNameTextObjCharacterDialogObj;
    public Text Trivia_UserNameValueTextObj;  // Trivia Panel Name of User display

   [Header("Profile Screen")]
    public GameObject profilePanelObj;
    public Text displayNameTextProfileScreenObj;
    public Text fullNameTextProfileScreenObj;
    public Text originTextProfileScreenObj;
    public Text emailTextProfileScreenObj;
    public Text currentBalanceTextProfileScreenObj;
    public Text totalBallsCollectedTextProfileScreenObj;
    public Text tierUnlockedTextProfileScreenObj;
    public Button changeAccountDetailsButtonProfileScreenObj;
    public Button changePasswordButtonProfileScreenObj;

    [Header("Change Password")]
    public GameObject changePasswordPanelObj;
    public InputField currentPasswordInputFieldChangePasswordScreenObj;
    public InputField newPasswordInputFieldChangePasswordScreenObj;
    public InputField confirmPasswordInputFieldChangePasswordScreenObj;
    public Button saveChangesBtnChangePasswordScreenObj;
    public Button closeBtnChangePasswordScreenObj;

    [Header("Edit Account Details")]
    public GameObject editAccountDetailsPanelObj;
    public Text displayNameTextEditProfileScreenObj;
    public Text originTextEditProfileScreenObj;
    public Text emailTextEditProfileScreenObj;
    public InputField displayNameInputFieldEditAccountScreenObj;
    public InputField fullNameInputFieldEditAccountScreenObj;
    public InputField originInputFieldEditAccountScreenObj;
    public InputField emailInputFieldEditAccountScreenObj;
    public Button saveChangesBtnEditProfileScreenObj;
    public Button closeBtnEditProfileScreenObj;
    public GameObject countryListObjectParentsEditAccountDetailsScreenObj;
    public GameObject countryListContainerChangeAccountScreenObj;

    [Header("Map Panel")]
    public GameObject mapPanelObj;
    public GameObject horseshoeCourtMapScreenObj;
    public GameObject hallOfFameMapScreenObj;
    public GameObject artifactRoomMapScreenObj;
    public GameObject entraceMapScreenObj;

    [Header("Invite Panel")]
    public GameObject invitePanelObj;

    [Header("Control Panel")]
    public GameObject ControlPanelObj;
    public GameObject mobileControlPanelObj;
    public GameObject desktopControlPanelObj;

    [Header("Shop Panel")]
    public GameObject shopMainPanelObj;
    public GameObject shopContainerPanelObj;
    public GameObject shopCollectionPanelObj;
    public GameObject shopTiersPanelObj;
    public GameObject shopItemDetailsPanelObj;

    [Header("shop Container panel")]
    public TextMeshProUGUI topBallBalanceValueTextShopContainerScreenObj;
    public Image tier1LockStatusImageShopContainerScreenObj;
    public Transform tier1ItemsParentShopContainerScreenObj;
    public Image tier2LockStatusImageShopContainerScreenObj;
    public Transform tier2ItemsParentShopContainerScreenObj;
    public Image tier3LockStatusImageShopContainerScreenObj;
    public Transform tier3ItemsParentShopContainerScreenObj;

    [Header("shop Collection panel")]
    public GameObject collectionItemsParentShopCollectionPanelObj;

    [Header("shop tiers Panel")]
    public Text currentBalanceValueTextShopTiersScreenObj;
    public Text totalBallsCollectedValueTextShopTiersScreenObj;
    public Text tiersUnlockedValueTextShopTiersScreenObj;
    public Transform tiersIndicatorsImagesParentShopTiersScreenObj;
    public Image tier1LockStatusImageShopTiersScreenObj;
    public Text tier1LockUnlockTextShopTiersScreenObj;
    public Image tier2LockStatusImageShopTiersScreenObj;
    public Text tier2LockUnlockTextShopTiersScreenObj;
    public Image tier3LockStatusImageShopTiersScreenObj;
    public Text tier3LockUnlockTextShopTiersScreenObj;

    [Header("shop item details Panel")]
    public TextMeshProUGUI ballBalanceValueTextShopItemDetailsScreenObj;
    public TextMeshProUGUI itemDescriptionTextShopItemDetailsScreenObj;
    public Text itemNameTextShopItemDetailsScreenObj;
    public Text itemBallCostTextShopItemDetailsScreenObj;
    public Image itemImageShopItemDetailsScreenObj;
    public Button purchaseBtnShopItemDetailsScreenObj;
    public GameObject insuficientFundsContainerShopItemDetailsScreenObj;
    public GameObject topBallBalanceCotainerShopItemDetailsScreenObj;
    public GameObject ballAmountContainerShopItemDetailsScreenObj;
    public GameObject inYourCollectionTextShopItemDetailsScreenObj;

    [Header("NPC Panel")]
    public GameObject npcMainPanelObj;
    public GameObject npcIntroductionPanelObj;
    public GameObject npcQuestionPanelObj;
    public GameObject npcWinPanelObj;
    public GameObject npcLosePanelObj;
    public Text playerNameTextNpcIntroducationObj;
    public Text descriptionTextNpcIntroductionObj;
    public Text playerNameValueTextNpcIntroducationObj;
    public Button playButtonNpcIntroducationObj;

    [Header("npc question panel")]
    public Text questionTextNpcQuestionScreenObj;
    public Text optionANpcQuestionScreenObj;
    public Text optionBNpcQuestionScreenObj;
    public Text optionCNpcQuestionScreenObj;

    [Header("npc win game panel")]
    public Text ballWinAmountTextNpcWinScreenObj;
    public Text playerNameTextNpcWinScreenObj;

    [Header("NPC Characters")]
    public GameObject npcOnboardingObj;
    public GameObject npcDomeObj;
    public GameObject npcCourtyardObj;

    [Header("Npc Game Popup")]
    public GameObject npcGamePopupPanel;
    public GameObject npcPopupGameControlPanel;
    public GameObject movementIconImageObj;
    public GameObject movementTitleTextObj;
    public GameObject movementDescriptionTextObj;
    public GameObject shiftIconImageObj;
    public GameObject shiftTitleTextObj;
    public GameObject shiftDescriptionTextObj;
    public GameObject sliceIconImageObj;
    public GameObject sliceTitleTextObj;
    public GameObject sliceDescriptionTextObj;
    public GameObject movementContainermobileNpcGamePopupObj;
    public GameObject topspinContainerMobileNpcGamePopupObj;
    public GameObject sliceContainerMobileNpcGamePopupObj;

    [Header(" Map Location ")]
    public int Map_Location;

    [Header("Player interactions")]
    public Button playerInteractionButton;
    public GameObject PlayerInteractionButtonsContainerObj;

    [Header("Loading Panel")]
    public GameObject loadingPanelScreenObj;
    public Text loadingTextLoadingScreenObj;

    [Space(30)]
    public Texture tex;
    public Vector2 hoverCursorHotSpot;
    public Vector2 NormalCursorHotSpot;

    [HideInInspector]
    public string registerOtp = "";
    [HideInInspector]
    public string registerEmailVerify = "";
    [HideInInspector]
    public string forgotPasswordEmail = "";
    [HideInInspector]
    public string forgotPasswordOtp = "";
    [HideInInspector]
    public string countryShortCode = "";
    [HideInInspector]
    public int Selectinput = -1;
    int numOfInputField = 1;
    delegate void Mydelegate();
    Mydelegate evetForTab;

    [Header("Scale for Biography")]
    public Transform Header_scale;
    public Transform Legand_Image;
    public Transform Legand_Details;

    [Header("Button For Player Animations")]
    public Button danceBtn, laughBtn, waveBtn, backflipBtn;

    public Coroutine apiCoroutine;
    Camera mainCamera;

    [Header("URP Pipeline Asset")]
    public UniversalRenderPipelineAsset urpAssets;
    public UniversalRenderPipelineAsset lowQualityUrpAsset;

    [Header("ReeadyPlayerMe Avatar Setting")]
    public AvatarLoaderSettings avatarLoaderSetting;
    public AvatarConfig highAvatarLoaderConfig;
    public AvatarConfig mediumAvatarLoaderConfig;
    public AvatarConfig lowAvtarLoaderConfig;


    public void overCursur()
    {
        Cursor.SetCursor((Texture2D)tex, hoverCursorHotSpot, CursorMode.Auto);
    }

    public void leftCursur()
    {
        Cursor.SetCursor(null, NormalCursorHotSpot, CursorMode.Auto);
    }

    [DllImport("__Internal")]
    private static extern bool IsMobile();
    [DllImport("__Internal")]
    private static extern bool IsMobileModified();

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

    public void onClickCourtYard()
    {
        CanvasforUI.gameObject.SetActive(false);
    }

    public static UIhandler instance;

    void Awake()
    {
        instance = this;

        #if !UNITY_EDITOR && UNITY_WEBGL
        string[] gettoken = api.GetURLFromPage().Split('?');

        if (gettoken.Length >= 2)
        {
            PlayerPrefs.SetString("token", gettoken[1]);
            Debug.Log("__ Get Url Token __" + gettoken[1]);
        }
        #endif

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        PhotonNetwork.KeepAliveInBackground = 600;
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = true;
#endif
        if(!IsMobileCheck())
        {
            avatarLoaderSetting.AvatarConfig = highAvatarLoaderConfig;
        }
    }

    public int playerBallBalance
    {
        set
        {
            ballBalanceValueTextHomeScreenOnGameObj.text = value.ToString();
            ballBalanceValueTextShopItemDetailsScreenObj.text = value.ToString();
            currentBalanceTextProfileScreenObj.text = value.ToString();
            topBallBalanceValueTextShopContainerScreenObj.text = value.ToString();
        }
    }

    public int playerTotalBallCollect
    {
        set
        {
            totalBallsCollectedTextProfileScreenObj.text = value.ToString();
            totalBallsCollectedValueTextShopTiersScreenObj.text = value.ToString();
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        //Application.runInBackground = true;
        IsMobileCheck();
        if (!IsMobileCheck())
        {
            Vector3 scaleVector = Vector3.one * 0.7f;
            Login_Pnl.transform.GetChild(0).transform.localScale = scaleVector;
            Login_Pnl.transform.GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0.25f, 0.5f);
            Login_Pnl.transform.GetChild(0).GetComponent<AspectRatioFitter>().enabled = true;
            menuPanelInGameScreenObj.transform.GetChild(0).localScale = scaleVector;
            Sign_Up_Pnl.transform.GetChild(0).transform.localScale = scaleVector;
            Sign_Up_Pnl.transform.GetChild(0).transform.localPosition = new Vector3(-140, 0, 0);
            Forgot_Password_Pnl.transform.GetChild(0).transform.localScale = scaleVector;
            Verify_Pnl.transform.GetChild(0).transform.localScale = scaleVector;
            Reset_Pnl.transform.GetChild(0).transform.localScale = scaleVector;

            characterDialogPanelObj.transform.GetChild(0).localScale = scaleVector;
            bioGraphyPanelInGameScreenObj.transform.GetChild(0).localScale = scaleVector;
            profilePanelObj.transform.GetChild(0).localScale = scaleVector;
            changePasswordPanelObj.transform.GetChild(0).localScale = scaleVector;
            editAccountDetailsPanelObj.transform.GetChild(0).localScale = scaleVector;
            invitePanelObj.transform.GetChild(0).localScale = scaleVector;
            mapPanelObj.transform.GetChild(0).localScale = scaleVector;
            ControlPanelObj.transform.GetChild(0).localScale = scaleVector;
            shopMainPanelObj.transform.GetChild(0).localScale = scaleVector;
            npcMainPanelObj.transform.GetChild(0).localScale = scaleVector;
            signupGuestPanel.transform.GetChild(0).localScale = scaleVector;
            npcGamePopupPanel.transform.GetChild(0).localScale = scaleVector;
            npcPopupGameControlPanel.transform.GetChild(0).localScale = scaleVector;
            walkContainerDesktopObj.SetActive(true);
            lookContainerDesktopObj.SetActive(true);
            interactContainerDesktopObj.SetActive(true);
            runContainerDesktopObj.SetActive(true);
            jumpContainerDesktopObj.SetActive(true);
            titleContainerDesktopObj.SetActive(true);
            titleContainerMobileObj.SetActive(false);
            walkContainerMobileObj.SetActive(false);
            lookContainerMobileObj.SetActive(false);
            interactContainerMobileObj.SetActive(false);
            movementIconImageObj.SetActive(true); 
            movementTitleTextObj.SetActive(true);
            movementDescriptionTextObj.SetActive(true);
            shiftIconImageObj.SetActive(true);
            shiftTitleTextObj.SetActive(true);
            shiftDescriptionTextObj.SetActive(true);
            sliceIconImageObj.SetActive(true);
            sliceTitleTextObj.SetActive(true);
            sliceDescriptionTextObj.SetActive(true);
            movementContainermobileNpcGamePopupObj.SetActive(false);
            topspinContainerMobileNpcGamePopupObj.SetActive(false);
            sliceContainerMobileNpcGamePopupObj.SetActive(false);

            mobileControlPanelObj.SetActive(false);
            desktopControlPanelObj.SetActive(true);

            if (urpAssets != null)
            {
                urpAssets.msaaSampleCount = 4;
            }
        }
        else
        {
            QualitySettings.SetQualityLevel(0, false);
            QualitySettings.renderPipeline = lowQualityUrpAsset;
            //QualitySettings.red
            Login_Pnl.transform.GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
            Login_Pnl.transform.GetChild(0).GetComponent<AspectRatioFitter>().enabled = true;
        }
        evetForTab = selectedInputfieldForLoginScreen;
        if (PlayerPrefs.GetString("token") != "")
        {
            apiCoroutine = StartCoroutine(apiHandler.CallProfile());
        }
        else
        {
            Login_Pnl.SetActive(true);
        }
        //StartCoroutine(apiHandler.CallGuestSignup());
        //farmerList();
        //Famer_Section(1);
    }

    Ray ray;
    RaycastHit hit;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && UnityEngine.Input.GetKey(KeyCode.LeftShift))
        {
            Selectinput--;
            if (Selectinput < 0)
                Selectinput = numOfInputField;

            //selectedInputfield();
            if (evetForTab != null) evetForTab();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectinput++;

            if (Selectinput > numOfInputField)
                Selectinput = 0;

            //selectedInputfield();
            if (evetForTab != null) evetForTab();
        }

        //string orientation = Input.deviceOrientation.ToString();
        //SSTools.ShowMessage("Orientation : " + orientation, SSTools.Position.top, SSTools.Time.oneSecond);
    }

    #region skip inputfield using tab button
    public void SelectInput(int number)
    {
        //Debug.Log("in selectinput");
        Selectinput = number;
        CountryListingContainerSignupScreenObj.SetActive(false);
    }

    void selectedInputfieldForSignupScreen()
    {
        CountryListingContainerSignupScreenObj.SetActive(false);
        switch (Selectinput)
        {
            case 0:
                Name_SignUp_Inp.Select();
                break;
            case 1:
                NickName_SignUp_Inp.Select();
                break;
            case 2:
                Country_SignUp_Inp.Select();
                OnClickCountryListArrowSignupScreen();
                break;
            case 3:
                Email_SignUp_Inp.Select();
                break;
            case 4:
                Passwword_SignUp_Inp.Select();
                break;
            case 5:
                C_Passwword_SignUp_Inp.Select();
                break;
        }
    }

    public void SelectInputForLogin(int number)
    {
        //Debug.Log("selectinput");
        Selectinput = number;
        CountryListingContainerSignupScreenObj.SetActive(false);
    }

    void selectedInputfieldForLoginScreen()
    {
        switch (Selectinput)
        {
            case 0:
                Email_Login_Inp.Select();
                break;
            case 1:
                Password_Login_Inp.Select();
                break;
        }
    }

    void selectedInputFieldForForgotPassword()
    {
        switch (Selectinput)
        {
            case 0:
                passwordForgotPasswordInputFieldObj.Select();
                break;
        }
    }

    void selectInputFieldForChangePassword()
    {
        Debug.Log("selectinput : " + Selectinput);
        switch (Selectinput)
        {
            case 0:
                currentPasswordInputFieldChangePasswordScreenObj.Select();
                break;
            case 1:
                newPasswordInputFieldChangePasswordScreenObj.Select();
                break;
            case 2:
                confirmPasswordInputFieldChangePasswordScreenObj.Select();
                break;
        }

    }

    void selectInputFieldForChangeAccountDetails()
    {
        switch (Selectinput)
        {
            case 0:
                displayNameInputFieldEditAccountScreenObj.Select();
                break;
            case 1:
                fullNameInputFieldEditAccountScreenObj.Select();
                break;
            case 2:
                originInputFieldEditAccountScreenObj.Select();
                OnClickCountryListArrowChangeAccountDetailScreen();
                break;
            case 3:
                emailInputFieldEditAccountScreenObj.Select();
                break;
        }
    }
    #endregion

    #region Login Screen
    public void OnClickForgotPasswordLoginScreen()
    {
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
        firstInputFieldVerifyOtpScreenObj.text = string.Empty;
        secondInputFieldVerifyOtpScreenObj.text = string.Empty;
        threeInputFieldVerifyOtpScreenObj.text = string.Empty;
        fourInputFieldVerifyOtpScreenObj.text = string.Empty;
        fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
        sixInputFieldVerifyOtpScreenObj.text = string.Empty;
        Selectinput = -1;
        numOfInputField = 0;
        evetForTab = selectedInputFieldForForgotPassword;
        Login_Pnl.SetActive(false);
        Forgot_Password_Pnl.SetActive(true);
        passwordForgotPasswordInputFieldObj.text = "";
    }

    public void OnClickSignupLoginScreenBtnObj()
    {
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        Selectinput = -1;
        numOfInputField = 5;
        evetForTab = selectedInputfieldForSignupScreen;
        Name_SignUp_Inp.text = "";
        NickName_SignUp_Inp.text = "";
        Country_SignUp_Inp.text = "";
        Email_SignUp_Inp.text = "";
        Passwword_SignUp_Inp.text = "";
        C_Passwword_SignUp_Inp.text = "";
        CountryListingContainerSignupScreenObj.SetActive(false);
        apiCoroutine = StartCoroutine(apiHandler.CallGetCountry(apiHandler.Country_Parents.transform, Country_SignUp_Inp));
        Sign_Up_Pnl.SetActive(true);
        Login_Pnl.SetActive(false);
    }

    public void Sign_In_Fun()
    {
        if (Email_Login_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter emaild", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (Password_Login_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter password", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }

        if (IsEmail(Email_Login_Inp.text))
        {
            //SSTools.ShowMessage("SuccssFully Login !", SSTools.Position.top, SSTools.Time.twoSecond);
            //demoCanvasObj.OnCreateAvatar();
            if (apiCoroutine != null)
            {
                StopCoroutine(apiCoroutine);
            }
            signUpSigninScreenBtnObj.interactable = false;
            forgotPasswordSigninScreenBtnObj.interactable = false;
            joinGuestSigninScreenBtnObj.interactable = false;
            apiCoroutine = StartCoroutine(apiHandler.CallLogin());
        }
        else
        {
            SSTools.ShowMessage("Please enter valid email", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }

    public static bool IsEmail(string email)
    {
        if (email != null)
            return

        Regex.IsMatch(email, MatchEmailPattern);
        return false;
    }

    #endregion

    #region SignUp
    public void OnClickSigninSignupScreen()
    {
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        Selectinput = -1;
        numOfInputField = 1;
        evetForTab = selectedInputfieldForLoginScreen;
        Email_Login_Inp.text = "";
        Password_Login_Inp.text = "";
        Sign_Up_Pnl.SetActive(false);
        Login_Pnl.SetActive(true);
    }

    public void Sign_Up_Detail()
    {
        if (Name_SignUp_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter name", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (NickName_SignUp_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter nickname", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (Country_SignUp_Inp.text == "" && countryShortCode != "")
        {
            SSTools.ShowMessage("Please enter country", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (Email_SignUp_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter emaild", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (Passwword_SignUp_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter password", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (C_Passwword_SignUp_Inp.text == "")
        {
            SSTools.ShowMessage("Please enter Confirm password", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }
        if (Passwword_SignUp_Inp.text != C_Passwword_SignUp_Inp.text)
        {
            SSTools.ShowMessage("Passwords do not match", SSTools.Position.top, SSTools.Time.twoSecond);
            return;
        }

        if (IsEmail(Email_SignUp_Inp.text))
        {
            registerEmailVerify = Email_SignUp_Inp.text;
            //SSTools.ShowMessage("SuccssFully Login !", SSTools.Position.top, SSTools.Time.twoSecond);
            if (apiCoroutine != null)
            {
                StopCoroutine(apiCoroutine);
            }
            signinSignupScreenBtnObj.interactable = false;
            apiCoroutine = StartCoroutine(apiHandler.CallRegister());
            //demoCanvasObj.OnCreateAvatar();
        }
        else
        {
            SSTools.ShowMessage("Please enter valid email", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }

    public void OnClickCountryListArrowSignupScreen()
    {
        CountryListingContainerSignupScreenObj.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(apiHandler.Country_Parents.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(apiHandler.Country_Parents.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(apiHandler.Country_Parents.GetComponent<RectTransform>());
    }

    public void OnClickCountryListArrowChangeAccountDetailScreen()
    {
        countryListContainerChangeAccountScreenObj.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(countryListObjectParentsEditAccountDetailsScreenObj.transform.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(countryListObjectParentsEditAccountDetailsScreenObj.transform.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(countryListObjectParentsEditAccountDetailsScreenObj.transform.GetComponent<RectTransform>());
    }

    public void OnClickResendOtpVerifyOtpScreenForRegister()
    {
        mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        continuteButtonVerifyScreenObj.interactable = false;
        apiCoroutine = StartCoroutine(apiHandler.CallSendOtp(registerEmailVerify, 1));
    }
    #endregion

    #region Change Password
    public void OnClickChangePasswordChangePasswordScreen()
    {
        if (currentPasswordInputFieldChangePasswordScreenObj.text != "" && newPasswordInputFieldChangePasswordScreenObj.text != "" && confirmPasswordInputFieldChangePasswordScreenObj.text != "")
        {
            if (newPasswordInputFieldChangePasswordScreenObj.text == confirmPasswordInputFieldChangePasswordScreenObj.text)
            {
                if (apiCoroutine != null)
                {
                    StopCoroutine(apiCoroutine);
                }
                closeBtnChangePasswordScreenObj.interactable = false;
                apiCoroutine = StartCoroutine(apiHandler.CallChangePassword());
            }
            else
            {
                SSTools.ShowMessage("New password and Confirm password do not match", SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
        else
        {
            SSTools.ShowMessage("Please enter all fields", SSTools.Position.top, SSTools.Time.twoSecond);
        }

    }
    #endregion

    #region VerifyOtp


    public void OnClickContinueVerifyOtpScreen()
    {
        //Debug.Log("call verify pass true");
        if (firstInputFieldVerifyOtpScreenObj.text != "" && secondInputFieldVerifyOtpScreenObj.text != "" && threeInputFieldVerifyOtpScreenObj.text != "" && fourInputFieldVerifyOtpScreenObj.text != "" && fiveInputFieldVerifyOtpScreenObj.text != "" && sixInputFieldVerifyOtpScreenObj.text != "")
        {
            string enteredOtp = firstInputFieldVerifyOtpScreenObj.text + secondInputFieldVerifyOtpScreenObj.text + threeInputFieldVerifyOtpScreenObj.text + fourInputFieldVerifyOtpScreenObj.text + fiveInputFieldVerifyOtpScreenObj.text + sixInputFieldVerifyOtpScreenObj.text;
            if (apiCoroutine != null)
            {
                StopCoroutine(apiCoroutine);
            }
            resendButtonVerifyScreenObj.interactable = false;
            apiCoroutine = StartCoroutine(apiHandler.CallVerifyOtp(registerEmailVerify, enteredOtp, true));
            //if(enteredOtp == registerOtp)
            //{
            //    showMessage("Verify Successfully");
            //    demoCanvasObj.OnCreateAvatar();
            //}
            //else
            //{
            //    showMessage("Otp you entered is incorrect");
            //}
        }
        else
        {
            SSTools.ShowMessage("Enter all six digits", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }
    #endregion

    #region Forget Password

    public void OnClickResendOtpVerifyOtpScreenForForgotPassword()
    {
        mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        continuteButtonVerifyScreenObj.interactable = false;
        apiCoroutine = StartCoroutine(apiHandler.CallSendOtp(forgotPasswordEmail, 0));
    }

    public void OnClickBackForgotPasswordScreen()
    {
        Selectinput = -1;
        numOfInputField = 1;
        evetForTab = selectedInputfieldForLoginScreen;
        Email_Login_Inp.text = "";
        Password_Login_Inp.text = "";
        Login_Pnl.SetActive(true);
        Forgot_Password_Pnl.SetActive(false);
    }

    public void ForgetPassword_In_Fun()
    {
        mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
        if (passwordForgotPasswordInputFieldObj.text != "")
        {
            if (apiCoroutine != null)
            {
                StopCoroutine(apiCoroutine);
            }
            backButtonForgotPasswordScreenBtnObj.interactable = false;
            apiCoroutine = StartCoroutine(apiHandler.CallForgotPassword());
        }
        else
        {
            SSTools.ShowMessage("Please enter email", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }

    public void OnClickContinueVerifyScreenForForgotPassword()
    {
        // Debug.Log("call verify otp pass false");
        if (firstInputFieldVerifyOtpScreenObj.text != "" && secondInputFieldVerifyOtpScreenObj.text != "" && threeInputFieldVerifyOtpScreenObj.text != "" && fourInputFieldVerifyOtpScreenObj.text != "" && fiveInputFieldVerifyOtpScreenObj.text != "" && sixInputFieldVerifyOtpScreenObj.text != "")
        {
            string enteredOtp = firstInputFieldVerifyOtpScreenObj.text + secondInputFieldVerifyOtpScreenObj.text + threeInputFieldVerifyOtpScreenObj.text + fourInputFieldVerifyOtpScreenObj.text + fiveInputFieldVerifyOtpScreenObj.text + sixInputFieldVerifyOtpScreenObj.text;
            if (apiCoroutine != null)
            {
                StopCoroutine(apiCoroutine);
            }
            //resendButtonVerifyScreenObj.interactable = false;
            apiCoroutine = StartCoroutine(apiHandler.CallVerifyOtp(forgotPasswordEmail, enteredOtp, false));
        }
        else
        {
            SSTools.ShowMessage("Enter all six digits", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }

    public void OnClickResetPasswordResetPassowrdScreen()
    {
        if (newPasswordResetPasswordScreenInputFieldObj.text != "" && confirmPasswordResetPasswordScreenInputFieldObj.text != "")
        {
            if (newPasswordResetPasswordScreenInputFieldObj.text == confirmPasswordResetPasswordScreenInputFieldObj.text)
            {
                if (apiCoroutine != null)
                {
                    StopCoroutine(apiCoroutine);
                }
                apiCoroutine = StartCoroutine(apiHandler.CallResetPassword());
            }
            else
            {
                SSTools.ShowMessage("password and Confirm password do not match", SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
        else
        {
            SSTools.ShowMessage("Please enter password and Confirm password", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }
    #endregion

    #region In Game Menu Operatations

    public void showInGameCanvas(string avatarUrl)
    {
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        apiCoroutine = StartCoroutine(apiHandler.CallReadyPlayerMeAvatarUrlSubmit(avatarUrl));
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(true);
        }
        inGameMenuCanvasObj.gameObject.SetActive(true);
        openmenuPanelInGameScreenObj.SetActive(true);
    }

    public void OnClickMenuBtn()
    {
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(false);
        }
        playerArmatureObj.GetComponent<StarterAssetsInputs>().jump = false;
        //ThirdPersonController playerController = playerArmatureObj.GetComponent<ThirdPersonController>();
        //playerController._animator.SetBool("Grounded", true);
        //playerController._animator.SetBool("Jump", false);
        //playerController._animator.SetBool("FreeFall", false);
        //playerController._animator.SetFloat("Speed", 0);
        //playerController._input.jump = false;
        //playerController.enabled = false;
        playerArmatureObj.GetComponent<ThirdPersonController>().resetPlayerStates(true);
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
        openmenuPanelInGameScreenObj.SetActive(false);
        menuPanelInGameScreenObj.SetActive(true);
    }

    public void OnClickCloseMenuBtn()
    {
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(true);
        }
        if (playerArmatureObj != null)
        {
            playerArmatureObj.GetComponent<StarterAssetsInputs>().jump = false;
            playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
            playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        }
        openmenuPanelInGameScreenObj.SetActive(true);
        menuPanelInGameScreenObj.SetActive(false);
    }

    public void OnClickLogoutInGameMenuScreen()
    {
        if (apiCoroutine != null)
        {
            StopCoroutine(apiCoroutine);
        }
        apiCoroutine = StartCoroutine(apiHandler.CallLogout());
    }

    public void OnClickBiographyInGameMenuScreen(string famerId, Texture imageTexture)
    {
        playerArmatureObj.GetComponent<ThirdPersonController>().resetPlayerStates(true);
        playerArmatureObj.transform.position = new Vector3(playerArmatureObj.transform.position.x, 0f, playerArmatureObj.transform.position.z);
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        playerFollowCameraObj.GetComponent<CameraController>().enabled = false;

        apiHandler.famerGallaryCounter = 0;
        apiHandler.perviousButtonFamerGallaryObj.interactable = false;
        apiHandler.nextButtonFamerGallaryObj.interactable = false;
        //if (imageTexture != null)
        //{
        //    Texture2D nexText = (Texture2D)imageTexture;
        //    Sprite createdSprite = Sprite.Create(nexText, new Rect(0f, 0f, imageTexture.width, imageTexture.height), Vector2.zero, 10f, 0, SpriteMeshType.Tight);
        //    apiHandler.famerGallarySpriteList.Add(createdSprite);
        //    apiHandler.famerGallaryMainImageObj.sprite = createdSprite;
        //}
        //else
        //{
        apiHandler.famerGallaryMainImageObj.sprite = null;
        //}
        apiHandler.famerGallarySpriteList.Clear();
        bioGraphyPanelInGameScreenObj.name = famerId;
        apiHandler.OnClickHeaderBtn(1);
        bioGraphyPanelInGameScreenObj.SetActive(true);
        menuPanelInGameScreenObj.SetActive(false);
    }

    public void OnClickCloseBtnBioGraphyScreen()
    {
        //menuPanelInGameScreenObj.SetActive(true);
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        bioGraphyPanelInGameScreenObj.SetActive(false);
        GameObject.Find("Dome").transform.GetComponent<UiHandlerForDome>().isDetailOpen = false;
    }

    public void OnClickChatBtnHomeScreenInGameMenu()
    {
        chatInputFieldObj.text = "";
        chatButtonHomeScreenInGameObj.GetComponent<Image>().sprite = chatNormalSprite;
        //playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        //playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
        //apiHandler.GetComponent<PhotonChatManager>().ChatConnectOnClick(api.nickName);
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(false);
        }
        chatInGameScreenObj.SetActive(true);
    }

    public void OnClickCloseInGameChatPanel()
    {
        //playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        //playerArmatureObj.GetComponent<StarterAssetsInputs>().jump = false;
        //playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;

        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(true);
        }
        chatInGameScreenObj.SetActive(false);
    }

    public void OnClickCloseArtifactDetailScreen()
    {
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(true);
        }
        foreach (Transform t in ArtifactImageObj.transform)
        {
            Destroy(t.gameObject);
        }
        playerArmatureObj.GetComponent<StarterAssetsInputs>().jump = false;
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
    }

    #endregion

    #region OtpFill Method
    public void Otpfill()
    {
        char[] code = mainOtpTextMeshInputFieldVerifyOtpScreenObj.text.ToCharArray();
        switch (code.Length)
        {
            case 0:
                firstInputFieldVerifyOtpScreenObj.text = string.Empty;
                secondInputFieldVerifyOtpScreenObj.text = string.Empty;
                threeInputFieldVerifyOtpScreenObj.text = string.Empty;
                fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                break;
            case 1:
                firstInputFieldVerifyOtpScreenObj.text = code[0].ToString();
                secondInputFieldVerifyOtpScreenObj.text = string.Empty;
                threeInputFieldVerifyOtpScreenObj.text = string.Empty;
                fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                break;
            case 2:
                firstInputFieldVerifyOtpScreenObj.text = code[0].ToString();
                secondInputFieldVerifyOtpScreenObj.text = code[1].ToString();
                threeInputFieldVerifyOtpScreenObj.text = string.Empty;
                fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                break;
            case 3:
                firstInputFieldVerifyOtpScreenObj.text = code[0].ToString();
                secondInputFieldVerifyOtpScreenObj.text = code[1].ToString();
                threeInputFieldVerifyOtpScreenObj.text = code[2].ToString();
                fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                break;
            case 4:
                firstInputFieldVerifyOtpScreenObj.text = code[0].ToString();
                secondInputFieldVerifyOtpScreenObj.text = code[1].ToString();
                threeInputFieldVerifyOtpScreenObj.text = code[2].ToString();
                fourInputFieldVerifyOtpScreenObj.text = code[3].ToString();
                fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                break;
            case 5:
                firstInputFieldVerifyOtpScreenObj.text = code[0].ToString();
                secondInputFieldVerifyOtpScreenObj.text = code[1].ToString();
                threeInputFieldVerifyOtpScreenObj.text = code[2].ToString();
                fourInputFieldVerifyOtpScreenObj.text = code[3].ToString();
                fiveInputFieldVerifyOtpScreenObj.text = code[4].ToString();
                sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                break;
            case 6:
                firstInputFieldVerifyOtpScreenObj.text = code[0].ToString();
                secondInputFieldVerifyOtpScreenObj.text = code[1].ToString();
                threeInputFieldVerifyOtpScreenObj.text = code[2].ToString();
                fourInputFieldVerifyOtpScreenObj.text = code[3].ToString();
                fiveInputFieldVerifyOtpScreenObj.text = code[4].ToString();
                sixInputFieldVerifyOtpScreenObj.text = code[5].ToString();
                break;
        }
    }
    #endregion

    public void OnClickProfileMenuScreen()
    {
        profilePanelObj.SetActive(true);
        menuPanelInGameScreenObj.SetActive(false);
    }

    public void OnClickCloseProfileScreen()
    {
        menuPanelInGameScreenObj.SetActive(true);
        profilePanelObj.SetActive(false);
    }

    public void OnClickChangePasswordProfileScreen()
    {
        currentPasswordInputFieldChangePasswordScreenObj.text = "";
        newPasswordInputFieldChangePasswordScreenObj.text = "";
        confirmPasswordInputFieldChangePasswordScreenObj.text = "";
        Selectinput = -1;
        numOfInputField = 2;
        evetForTab = selectInputFieldForChangePassword;
        changePasswordPanelObj.SetActive(true);
        profilePanelObj.SetActive(false);
    }

    public void OnClickChangeAvatarProfileScreen()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebInterface.SetIFrameVisibility(true);
#endif
        webAvatarLoaderInst.isEditAvatar = true;
    }

    public void OnClickCloseChangePasswordScreen()
    {
        profilePanelObj.SetActive(true);
        changePasswordPanelObj.SetActive(false);
    }

    public void OnClickInviteBtnMenuScreen()
    {
        invitePanelObj.SetActive(true);
        menuPanelInGameScreenObj.SetActive(false);
    }

    public void OnClickCloseInviteScreen()
    {
        menuPanelInGameScreenObj.SetActive(true);
        invitePanelObj.SetActive(false);
    }

    public void OnClickChangeAccountDetailsProfileScreen()
    {
        displayNameInputFieldEditAccountScreenObj.text = displayNameTextProfileScreenObj.text;
        fullNameInputFieldEditAccountScreenObj.text = fullNameTextProfileScreenObj.text;
        originInputFieldEditAccountScreenObj.text = originTextProfileScreenObj.text;
        originInputFieldEditAccountScreenObj.name = originTextProfileScreenObj.text;
        emailInputFieldEditAccountScreenObj.text = emailTextProfileScreenObj.text;
        apiCoroutine = StartCoroutine(apiHandler.CallGetCountry(countryListObjectParentsEditAccountDetailsScreenObj.transform, originInputFieldEditAccountScreenObj));
        editAccountDetailsPanelObj.SetActive(true);
        profilePanelObj.SetActive(false);
    }

    public void OnClickCloseChangeAccountDetailsScreen()
    {
        profilePanelObj.SetActive(true);
        editAccountDetailsPanelObj.SetActive(false);
    }

    public void OnClickCloseCharacterDialogScreen()
    {
        characterDialogPanelObj.SetActive(false);
        if(playerArmatureObj != null && playerArmatureObj.GetComponent<ThirdPersonController>())
        {
            playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        }
        if (playerFollowCameraObj.GetComponent<CameraController>())
        {
            playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        }
        //if (!IsMobileCheck())
        //{
        //    chatInGameScreenObj.SetActive(true);
        //}
    }

    public void OnClickCloseNpcTriviaPanel()
    {
        if (UiHandlerForDome.instance != null)
        {
            UiHandlerForDome.instance.isDetailOpen = false;
        }
        if(playerArmatureObj != null)
        {
            playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        }
        playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        apiHandler.Npc_Question_Panel.transform.parent.parent.gameObject.SetActive(false);
        apiHandler.Npc_Question_Panel.transform.parent.GetChild(0).gameObject.SetActive(true);
        apiHandler.Npc_Question_Panel.SetActive(false);
        apiHandler.win_Panel.SetActive(false);
        apiHandler.Lose_Panel.SetActive(false);
    }

    public void OnClickMapMenuScreenObj()
    {
        //OnClickHorseshoeCourtMapScreen();

        disableAllMapScreen();

        if (Map_Location == 1)
            entraceMapScreenObj.SetActive(true);
        else if (Map_Location == 2)
            hallOfFameMapScreenObj.SetActive(true);
        else if (Map_Location == 3)
            horseshoeCourtMapScreenObj.SetActive(true);
        else if (Map_Location == 4)
            artifactRoomMapScreenObj.SetActive(true);

        mapPanelObj.SetActive(true);
        menuPanelInGameScreenObj.SetActive(false);
    }

    public void OnClickCloseMapScreenObj()
    {
        menuPanelInGameScreenObj.SetActive(true);
        mapPanelObj.SetActive(false);
    }

    public void disableAllMapScreen()
    {
        horseshoeCourtMapScreenObj.SetActive(false);
        hallOfFameMapScreenObj.SetActive(false);
        artifactRoomMapScreenObj.SetActive(false);
        entraceMapScreenObj.SetActive(false);
    }

    public void OnClickHorseshoeCourtMapScreen()
    {
        disableAllMapScreen();
        horseshoeCourtMapScreenObj.SetActive(true);
    }

    public void OnClickHallOfFameMapScreen()
    {
        disableAllMapScreen();
        hallOfFameMapScreenObj.SetActive(true);
    }

    public void OnClickArtifactMapScreen()
    {
        disableAllMapScreen();
        artifactRoomMapScreenObj.SetActive(true);
    }

    public void OnClickEntraceMapScreen()
    {
        disableAllMapScreen();
        entraceMapScreenObj.SetActive(true);
    }

    public void OnClickControlBtnMenuScreen()
    {
        ControlPanelObj.SetActive(true);
        menuPanelInGameScreenObj.SetActive(false);
    }

    public void OnClickCloseBtnControlScreen()
    {
        menuPanelInGameScreenObj.SetActive(true);
        ControlPanelObj.SetActive(false);
    }

    public void OnClickDesktopMobileControlScreen()
    {
        desktopControlPanelObj.SetActive(true);
        mobileControlPanelObj.SetActive(false);
    }

    public void OnClickMobileDesktopControlScreen()
    {
        mobileControlPanelObj.SetActive(true);
        desktopControlPanelObj.SetActive(false);
    }

    public void OnClickShopMenuScreen()
    {
        apiCoroutine = StartCoroutine(apiHandler.CallShopDetails());
        shopMainPanelObj.SetActive(true);
        shopContainerPanelObj.SetActive(true);
    }

    public void OnClickCloseShopPanel()
    {
        shopMainPanelObj.SetActive(false);
        shopContainerPanelObj.SetActive(true);
        shopCollectionPanelObj.SetActive(false);
        shopTiersPanelObj.SetActive(false);
        shopItemDetailsPanelObj.SetActive(false);
    }

    public void OnClickShopShopPanelScreen()
    {
        shopContainerPanelObj.SetActive(true);
        shopCollectionPanelObj.SetActive(false);
        shopTiersPanelObj.SetActive(false);
    }

    public void OnClickCollectionShopPanelScreen()
    {
        shopCollectionPanelObj.SetActive(true);
        shopContainerPanelObj.SetActive(false);
        shopTiersPanelObj.SetActive(false);
        StartCoroutine(apiHandler.CallShopCollection());
    }

    public void OnClickTiersShopPanelScreen()
    {
        apiCoroutine = StartCoroutine(apiHandler.CallTiersDetails());
        shopTiersPanelObj.SetActive(true);
        shopCollectionPanelObj.SetActive(false);
        shopContainerPanelObj.SetActive(false);
    }

    public void OnClickShopItemShopContainerScreen(GameObject itemImageObj, int unlockedTier)
    {
        if (unlockedTier == 0)
        {
            SSTools.ShowMessage("This Tier is not unlocked", SSTools.Position.top, SSTools.Time.threeSecond);
        }
        else
        {
            if (int.Parse(itemImageObj.transform.GetChild(0).name) > unlockedTier)
            {
                SSTools.ShowMessage("This Tier is not unlocked", SSTools.Position.top, SSTools.Time.threeSecond);
            }
            else
            {
                itemImageShopItemDetailsScreenObj.sprite = itemImageObj.transform.GetChild(0).GetComponent<Image>().sprite;
                purchaseBtnShopItemDetailsScreenObj.name = itemImageObj.name;
                purchaseBtnShopItemDetailsScreenObj.gameObject.SetActive(false);
                itemNameTextShopItemDetailsScreenObj.text = "";
                itemBallCostTextShopItemDetailsScreenObj.text = "";
                itemDescriptionTextShopItemDetailsScreenObj.text = "";
                itemImageShopItemDetailsScreenObj.preserveAspect = true;
                itemImageShopItemDetailsScreenObj.gameObject.SetActive(true);
                purchaseBtnShopItemDetailsScreenObj.gameObject.SetActive(false);
                insuficientFundsContainerShopItemDetailsScreenObj.SetActive(false);
                ballAmountContainerShopItemDetailsScreenObj.SetActive(true);
                topBallBalanceCotainerShopItemDetailsScreenObj.SetActive(true);
                itemBallCostTextShopItemDetailsScreenObj.gameObject.SetActive(true);
                inYourCollectionTextShopItemDetailsScreenObj.SetActive(false);
                apiCoroutine = StartCoroutine(apiHandler.CallShopItemDetails(int.Parse(itemImageObj.name)));
                shopItemDetailsPanelObj.SetActive(true);
                shopContainerPanelObj.SetActive(false);
            }
        }

    }

    public void OnClickShopItemShopCollectionScreen(GameObject itemImageObj)
    {
        itemImageShopItemDetailsScreenObj.sprite = itemImageObj.transform.GetChild(0).GetComponent<Image>().sprite;
        apiCoroutine = StartCoroutine(apiHandler.CallShopItemDetails(int.Parse(itemImageObj.name)));
        itemNameTextShopItemDetailsScreenObj.text = "";
        itemBallCostTextShopItemDetailsScreenObj.text = "";
        itemDescriptionTextShopItemDetailsScreenObj.text = "";
        itemImageShopItemDetailsScreenObj.preserveAspect = true;
        itemImageShopItemDetailsScreenObj.gameObject.SetActive(true);
        purchaseBtnShopItemDetailsScreenObj.gameObject.SetActive(false);
        insuficientFundsContainerShopItemDetailsScreenObj.SetActive(false);
        ballAmountContainerShopItemDetailsScreenObj.SetActive(true);
        itemBallCostTextShopItemDetailsScreenObj.gameObject.SetActive(false);
        inYourCollectionTextShopItemDetailsScreenObj.SetActive(true);
        topBallBalanceCotainerShopItemDetailsScreenObj.SetActive(false);
        shopItemDetailsPanelObj.SetActive(true);
        shopContainerPanelObj.SetActive(false);
    }

    public void OnClickCloseShopItemDetailsScreen()
    {
        shopContainerPanelObj.SetActive(true);
        shopItemDetailsPanelObj.SetActive(false);
    }

    public void OnClickJoinAsGuestLoginScreen()
    {
        nickNameInputFieldGuestSignupScreenObj.text = "";
        signupGuestPanel.SetActive(true);
        Login_Pnl.SetActive(false);
    }

    public void OnClickBackJoinAsGuestScreen()
    {
        Email_Login_Inp.text = "";
        Password_Login_Inp.text = "";
        Login_Pnl.SetActive(true);
        signupGuestPanel.SetActive(false);
    }

    public void OnClickSubmitGuestSignupScreen()
    {
        if(nickNameInputFieldGuestSignupScreenObj.text == "")
        {
            SSTools.ShowMessage("Please enter nickname", SSTools.Position.top, SSTools.Time.threeSecond);
        }
        apiCoroutine = StartCoroutine(apiHandler.CallGuestSignup());
    }

    public void OnClickSaveChangesEditProfileScreen()
    {
        if (fullNameInputFieldEditAccountScreenObj.text != "")
        {
            if (apiCoroutine != null)
            {
                StopCoroutine(apiCoroutine);
            }
            closeBtnEditProfileScreenObj.interactable = false;
            apiCoroutine = StartCoroutine(apiHandler.CallUpdateProfile());
        }
    }

    public void farmerList()
    {
        StartCoroutine(apiHandler.Farmerlist());
    }

    public void OnClickArtifact(GameObject artifactObj)
    {
        foreach (Transform t in ArtifactImageObj.transform)
        {
            Destroy(t.gameObject);
        }
        //Debug.Log("clik artifact " + artifactObj.name);
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
        GamePlayCanvasForJoystick.SetActive(false);
        apiCoroutine = StartCoroutine(apiHandler.CallArtifactDetails(40));
        GameObject gm = Instantiate(artifactObj) as GameObject;
        gm.transform.SetParent(ArtifactImageObj.transform);
        gm.transform.localPosition = Vector3.zero;
        gm.transform.localScale = Vector3.one * (1800);
        gm.transform.rotation = Quaternion.identity;
        Destroy(gm.transform.GetComponent<Animation>());
        Destroy(gm.transform.GetComponent<BoxCollider>());
        Destroy(gm.transform.GetComponent<Rigidbody>());
    }

    public void OnSelectChatPanel()
    {
        //Debug.Log("OnSelectChat Panel Called");
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = false;
        playerFollowCameraObj.GetComponent<CameraController>().enabled = false;
        //apiHandler.GetComponent<PhotonChatManager>().ChatConnectOnClick(api.nickName);
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(false);
        }
    }

    public void OnDeselectChatPanel()
    {
        //Debug.Log("OnDeselectChat Panel Called");
        playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        playerArmatureObj.GetComponent<StarterAssetsInputs>().jump = false;
        playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        if (IsMobileCheck())
        {
            GamePlayCanvasForJoystick.SetActive(true);
        }
    }

    public void OnMessageReceivedChat()
    {
        //Debug.Log("onmessagereceivedChat");
        if (!chatInGameScreenObj.activeSelf)
        {
            //Debug.Log("onmessagereceivedChat changing color");
            chatButtonHomeScreenInGameObj.GetComponent<Image>().sprite = chatMessageReceivedSprite;
        }
    }

    public void onClickOtpInputField()
    {
        //Debug.Log("inOnClickOtp");
        mainOtpTextMeshInputFieldVerifyOtpScreenObj.ActivateInputField();
    }

    public void OnClickPlayerInteractionButtonHomeScreen()
    {
        PlayerInteractionButtonsContainerObj.SetActive(!PlayerInteractionButtonsContainerObj.activeSelf);
    }

    public void OnClickCloseNpcGamePopup()
    {
        if(playerArmatureObj != null)
        {
            playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        }
        playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        npcGamePopupPanel.SetActive(false);
    }
    public void OnClickCloseNpcGameControlsPopup()
    {
        if (playerArmatureObj != null)
        {
            playerArmatureObj.GetComponent<ThirdPersonController>().enabled = true;
        }
        playerFollowCameraObj.GetComponent<CameraController>().enabled = true;
        npcPopupGameControlPanel.SetActive(false);
    }

    [HideInInspector] public List<UnityEvent> soundEvents;
    bool IsMute = false;
    public void OnClickSoundBtnHomeScreen()
    {
        if (IsMute)
        {
            soundIconImageHomeScreenObj.sprite = soundOnSprite;
            IsMute = false;
        }
        else
        {
            soundIconImageHomeScreenObj.sprite = soundOffSprite;
            IsMute = true;
        }
        if(GameManager.instance != null)
        {
            GameManager.instance.MuteAudio();
        }
        if(webglVideoPlayer.instance != null)
        {
            webglVideoPlayer.instance.muteVideo();
        }
        
    }

    public void OnClickPurchaseBtnShopItemDetailsPanel()
    {
        int points = 0;
        int shopId = 0;
        bool parsed = int.TryParse(itemBallCostTextShopItemDetailsScreenObj.text, out points);
        if (parsed)
        {
            parsed = int.TryParse(purchaseBtnShopItemDetailsScreenObj.name, out shopId);
            if (parsed)
            {
                StartCoroutine(apiHandler.CallPointsTransaction(points, 5, false, 0, false, 0, true, shopId));
            }
        }
    }

    public void OnClickPlayNpcPopUp()
    {
        Debug.Log("in onclickplay");
        if (GameManager.instance == null)
        {
            Debug.Log("return gamemanager got null");
            return;
        }
        for (int i = 0; i < GameManager.instance.transform.childCount; i++)
        {
            GameManager.instance.transform.GetChild(i).gameObject.SetActive(true);
        }
        multiplayerhandler.instance.HidePlayer();
        multiplayerhandler.instance.isTennisGameStarted = true;

        inGameMenuCanvasObj.gameObject.SetActive(false);
        GameManager.instance.StartGame();
    }
}