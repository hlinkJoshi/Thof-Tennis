using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
//using SimpleJSON;
using com.pakhee.common;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Playables;
using static api;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using ReadyPlayerMe;
using System.Security.Policy;
using System.Security.Permissions;
using System.Xml;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Linq;

public class api : MonoBehaviour
{
    [Header("UIHandler Object")]
    public UIhandler uiHandler;
    [Header("WebAvatarLoader Object")]
    public WebAvatarLoader webAvatarLoaderObj;

    [HideInInspector]
    public static string nickName = "";
    [Space(20)]
    public GameObject CountryText, Country_Parents;

    public static string baseUrl = "http://52.21.127.189/ithof_admin/api/v1/";
    string loginUrl = baseUrl + "user/login";
    string registerUrl = baseUrl + "user/signup";
    string logoutUrl = baseUrl + "user/logout";
    string profileUrl = baseUrl + "user/profile";
    string forgotpasswrodUrl = baseUrl + "user/forgot-password";
    string verifyOtpUrl = baseUrl + "user/verify-otp";
    string resetPasswordUrl = baseUrl + "user/reset-password";
    string changePasswordUrl = baseUrl + "user/change-password";
    string getCoutnriesUrl = baseUrl + "user/get-countries";
    string avatarSubmitUrl = baseUrl + "user/update-ready-player-me-id";
    string sendOtpUrl = baseUrl + "user/send-otp";
    string artifactUrl = baseUrl + "artifact";
    string getfarmerlistUrl = baseUrl + "hall-of-famers/list";
    string famers_detailUrl = baseUrl + "hall-of-famers/details";
    string updateProfileUrl = baseUrl + "user/profile/update";
    string galleryUrl = baseUrl + "";
    string pointsTransactionUrl = baseUrl + "points/transaction-add";
    string tiersDetailsUrl = baseUrl + "tiers/user/details";
    string shopDetailsUrl = baseUrl + "shop/list";
    string shopItemDetailsUrl = baseUrl + "shop/details";
    string shopCollectionUrl = baseUrl + "collections";
    string InteractiveNpcQuestionUrl = baseUrl + "interactive-npc";
    string guestUserSignupUrl = baseUrl + "user/guest-signup";

    string key12 = "j4y8jonjNSHjp5A8AHEo2WbdsPz837b5";
    string iv12 = "j4y8jonjNSHjp5A8";
    string key1;


    public class playerinfo
    {
        public int code;
        public string message;
        public Data_login data;
    }

    [Serializable]
    public class Data_login
    {
        public int id;
        public string name;
        public string nick_name;
        public string country;
        public string user_type;
        public string signup_type;
        public string otp;
        public string token;
        public string email;
        public string ready_player_me_id;
        public int points;
        public int total_points;
        public int current_points;
        public int unlocked_tier;
    }

    public class countryList
    {
        public int code;
        public string message;
        public List<DataCountry> data;
    }

    [Serializable]
    public class DataCountry
    {
        public string name;
        public string code;
        public string short_code;
    }

    public class Artifact
    {
        public int code;
        public string message;
        public ArtifactData data;
    }
    [Serializable]
    public class ArtifactData
    {
        public int id;
        public int category_id;
        public string artifact_code;
        public string title;
        public string description;
        public List<GallaryImages> uploads;
    }
    [Serializable]
    public class GallaryImages
    {
        public int id;
        public string type;
        public string file_name;
    }

    public class Famer
    {
        public int code;
        public string message;
        public FamerDetails data;
    }
    [Serializable]
    public class FamerDetails
    {
        public int id;
        public string question;
        public int points;
        public string answer_a;
        public string answer_b;
        public string answer_c;
        public string correct_answer;
        public string status;

        public string name;
        public string full_name;
        public string biography_text;
        public string biography_description;
        public string career_highlights_left_description;
        public string career_highlights_right_description;
        public string career_timeline_description;
        public TrivaQuestion[] trivia;
        public string gs_australlian_open_text;
        public string gs_australlian_open_description;
        public string gs_french_open_text;
        public string gs_french_open_description;
        public string gs_wimbledon_open_text;
        public string gs_wimbledon_open_description;
        public string gs_us_open_text;
        public string gs_us_open_description;
        public List<FamerGallaryData> uploads;
    }
    [Serializable]
    public class FamerGallaryData
    {
        public string file_name;
    }

    [Serializable]
    public class TrivaQuestion
    {
        public string question;
        public string answer_a;
        public string answer_b;
        public string answer_c;
        public string correct_answer;
    }

    public class ShopItemData
    {
        public int code;
        public string message;
        public ShopItemDetailClass data;
    }
    public class ShopData
    {
        public int code;
        public string message;
        public List<ShopItemDetailClass> data;
    }
    public class ShopCollectionData
    {
        public int code;
        public string message;
        public List<ShopItemDetailClass> data;
    }
    [Serializable]
    public class ShopItemDetailClass
    {
        public List<ShopTiersItemsDetailsClass> shops_list;
        public int id;
        public int points;
        public int total_points;
        public int unlocked_tier;
        public string file_name;
        public string title;
        public string description;
        public bool is_purchased;
    }
    [Serializable]
    public class ShopTiersItemsDetailsClass
    {
        public int shop_id;
        public int tier_id;
        public int points;
        public string file_name;
    }

    string user = "USER";
    string ip = "123.34.76.09";
    string jsonData = "";
    string decryptString;

    Coroutine otpTimerCoroutine;

    [Header("BioGraphy")]
    public Text FamerName;
    public Text Famer_Header;
    public Text Famer_Description;
    public Image famerGallaryMainImageObj;
    public Button perviousButtonFamerGallaryObj;
    public Button nextButtonFamerGallaryObj;

    [Header("Highlights Career")]
    public Text career_highlights_left_description_txt;
    public Text career_highlights_right_description_txt;

    [Header("Highlights Career")]
    public Text career_timeline_description_txt;

    [Header("Trivia")]
    public Text Quesion_txt;
    public Text Quesion_No_Display;
    public Text Quesion_optionA, Quesion_optionB, Quesion_optionC;
    public string Answer;
    public int Quesion_No = 0;
    public Sprite Greenlight, RedLight, Normal;
    public Image Dot1, Dot2, Dot3;
    public Text playerNameTextTriviaWinScreenObj;

    [Header("Interactive-npc")]
    public GameObject Npc_Question_Panel;
    public Text Quesion_NPC_Display;
    public Text Quesion_NPC_optionA, Quesion_NPC_optionB, Quesion_NPC_optionC;
    public string NPC_Answer;
    public GameObject Npc_Win, Npc_Lose;

    //[Header("Loading Panel")]
    //public GameObject uiHandler.loadingPanelScreenObj;
    //public Text loadingTextLoadingScreenObj;

    [Header("All Details Panel")]
    public GameObject biographyPanelDetailsScreenObj;
    public GameObject careerHighlightsPanelDetailsScreenObj;
    public GameObject grandSlamPanelDetailsScreenObj;
    public GameObject careerTimelineDetailsScreenObj;
    public GameObject triviaPanelDetailsScreenObj;
    public GameObject winPanelTriviaScreenObj;
    public GameObject losePanelTriviaScreenObj;

    [Header("Shop Prefab")]
    public GameObject shopItemPrefabObj;

    [Header("Grand Slap")]
    public Text AUSTRALIAN_OPEN;
    public Text FRENCH_OPEN;
    public Text WIMBLEDON;
    public Text US_OPEN;

    public Text AUSTRALIAN_OPEN_Description;
    public Text FRENCH_OPEN_Description;
    public Text WIMBLEDON_Description;
    public Text US_OPEN_Description;

    string Tempfamer = "0";

    public GameObject win_Panel;
    public GameObject Lose_Panel;
    bool AllTrue = true;

    [Serializable]
    public class Trivi_QuestionList
    {
        public int id;
        public string question;
        public string answer_a;
        public string answer_b;
        public string answer_c;
        public string correct_answer;
    }
    public List<Trivi_QuestionList> All_Question = new List<Trivi_QuestionList>();

    XmlDocument ScoreDataXml;

    [DllImport("__Internal")]
    public static extern string GetURLFromPage();
    [DllImport("__Internal")]
    public static extern string OpenNewTab(string url);

    //public static string UrlFromPage() => GetURLFromPage();

    public void SetHeader(UnityWebRequest request)
    {
        request.SetRequestHeader("api-key", "RjN08fAefrzO5DQKz3Kvjg==");
        request.SetRequestHeader("Content-Type", "text/plain");
        if (PlayerPrefs.GetString("token") != "")
        {
            request.SetRequestHeader("token", Encryption(PlayerPrefs.GetString("token")));
        }
    }
    public void SetHeaderaa(UnityWebRequest request)
    {
        request.SetRequestHeader("api-key", "RjN08fAefrzO5DQKz3Kvjg==");
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Content-Type", "text/plain");
        if (PlayerPrefs.GetString("token") != "")
        {
            request.SetRequestHeader("token", PlayerPrefs.GetString("token"));
            //Debug.Log(PlayerPrefs.GetString("token"));
        }
    }

    string url;
    void Awake()
    {
        //string urlXML = "http://52.21.127.189/Unity/index.htmlStreamingAssets/BaseUrl.xml";
        //string[] splitString = urlXML.Split('/');
        //if (splitString[splitString.Length - 1].Contains("html"))
        //{
        //    Debug.Log("this is inside forloop " + splitString.Length);
        //    urlXML = "";
        //    for (int i = 0; i < (splitString.Length - 1); i++)
        //    {
        //        urlXML += splitString[i];
        //    }
        //}
        //Debug.Log("urlXml " + urlXML);

#if UNITY_WEBGL && !UNITY_EDITOR
        url = GetURLFromPage();
        //Debug.Log("this is url returned fro myplugin " + url);
        StartCoroutine(Downloadxml());
#endif
    }

    IEnumerator Downloadxml()
    {
        string urlXML = url + "StreamingAssets/BaseUrl.xml";
        string[] splitString = url.Split('/');
        Debug.Log("splitlLength " + splitString[splitString.Length - 1]);
        if (splitString[splitString.Length - 1].Contains("html"))
        {
            Debug.Log("this is inside forloop " + splitString.Length);
            urlXML = "";
            for(int i=0; i<(splitString.Length - 1); i++)
            {
                urlXML += splitString[i] + "/";
            }
        }
        urlXML = urlXML + "StreamingAssets/BaseUrl.xml";
        Debug.Log("urlXml " + urlXML);
        UnityWebRequest request = UnityWebRequest.Get(urlXML);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(request.downloadHandler.text);
            AssignBasePath(xmlDoc.FirstChild.InnerText);
            Debug.Log("returnedPathFromXml " + xmlDoc.FirstChild.InnerText);
        }
    }

    void AssignBasePath(string url)
    {
        baseUrl = url;
        loginUrl = baseUrl + "user/login";
        registerUrl = baseUrl + "user/signup";
        logoutUrl = baseUrl + "user/logout";
        profileUrl = baseUrl + "user/profile";
        forgotpasswrodUrl = baseUrl + "user/forgot-password";
        verifyOtpUrl = baseUrl + "user/verify-otp";
        resetPasswordUrl = baseUrl + "user/reset-password";
        changePasswordUrl = baseUrl + "user/change-password";
        getCoutnriesUrl = baseUrl + "user/get-countries";
        avatarSubmitUrl = baseUrl + "user/update-ready-player-me-id";
        sendOtpUrl = baseUrl + "user/send-otp";
        artifactUrl = baseUrl + "artifact";
        getfarmerlistUrl = baseUrl + "hall-of-famers/list";
        famers_detailUrl = baseUrl + "hall-of-famers/details";
        updateProfileUrl = baseUrl + "user/profile/update";
        galleryUrl = baseUrl + "";
        pointsTransactionUrl = baseUrl + "points/transaction-add";
        tiersDetailsUrl = baseUrl + "tiers/user/details";
        shopDetailsUrl = baseUrl + "shop/list";
        shopItemDetailsUrl = baseUrl + "shop/details";
        shopCollectionUrl = baseUrl + "collections";
        InteractiveNpcQuestionUrl = baseUrl + "interactive-npc";
        guestUserSignupUrl = baseUrl + "user/guest-signup";
    }

    #region Login
    public IEnumerator CallLogin()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(loginUrl, "post");
        SetHeaderaa(request);

        jsonData = "{\"email\":\"" + uiHandler.Email_Login_Inp.text + "\",\"password\":\"" + uiHandler.Password_Login_Inp.text + "\",\"user_type\":\"" + user + "\",\"ip_address\":\"" + ip + "\"}";

        //Debug.Log("jsondata is " + jsonData);

        if (jsonData != null)
        {
            //byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            byte[] data = Encoding.UTF8.GetBytes((jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            decryptString = (request.downloadHandler.text.Trim('"'));
            //Debug.Log(decryptString);
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                Debug.Log("successfully Login Done   " + decryptString);
                PlayerPrefs.SetString("token", response.data.token);
                PlayerPrefs.SetString("email", response.data.email);
                uiHandler.playerBallBalance = response.data.points;
                uiHandler.playerTotalBallCollect = response.data.total_points;
                nickName = response.data.nick_name;
                transform.GetComponent<PhotonChatManager>().ChatConnectOnClick(nickName);
                uiHandler.displayNameTextProfileScreenObj.text = response.data.nick_name;
                uiHandler.fullNameTextProfileScreenObj.text = response.data.name;
                uiHandler.originTextProfileScreenObj.text = response.data.country;
                uiHandler.emailTextProfileScreenObj.text = response.data.email;
                uiHandler.currentBalanceTextProfileScreenObj.text = "0";
                uiHandler.totalBallsCollectedTextProfileScreenObj.text = "0";
                uiHandler.tierUnlockedTextProfileScreenObj.text = "0/3";

                uiHandler.changeAccountDetailsButtonProfileScreenObj.interactable = true;
                uiHandler.changePasswordButtonProfileScreenObj.interactable = true;

                uiHandler.displayNameTextEditProfileScreenObj.text = response.data.nick_name;
                uiHandler.originTextEditProfileScreenObj.text = response.data.country;
                uiHandler.emailTextEditProfileScreenObj.text = response.data.email;
                webAvatarLoaderObj.OnWebViewAvatarGenerated(response.data.ready_player_me_id);
                if (response.data.user_type == "GUEST")
                {
                    Debug.Log("in setting false buttton");
                    uiHandler.changeAccountDetailsButtonProfileScreenObj.interactable = false;
                    uiHandler.changePasswordButtonProfileScreenObj.interactable = false;
                }
                //uiHandler.characterDialogPanelObj.SetActive(true);
                //#if !UNITY_EDITOR && UNITY_WEBGL
                //    WebInterface.SetIFrameVisibility(true);
                //#endif
            }
            else
            {
                //Debug.Log("Error :- " + response.message);
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
                //UIhandler.showMessage(response.message);
            }

            uiHandler.signUpSigninScreenBtnObj.interactable = true;
            uiHandler.forgotPasswordSigninScreenBtnObj.interactable = true;
            uiHandler.joinGuestSigninScreenBtnObj.interactable = true;

        }
    }
    #endregion

    #region Register
    public IEnumerator CallRegister()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(registerUrl, "post");
        SetHeaderaa(request);
        jsonData = "{\"name\":\"" + uiHandler.Name_SignUp_Inp.text + "\",\"nick_name\":\"" + uiHandler.NickName_SignUp_Inp.text + "\",\"country\":\"" + uiHandler.countryShortCode + "\",\"email\":\"" + uiHandler.Email_SignUp_Inp.text + "\",\"password\":\"" + uiHandler.Passwword_SignUp_Inp.text + "\",\"c_password\":\"" + uiHandler.C_Passwword_SignUp_Inp.text + "\",\"profile_image\":\"\",\"user_type\":\"USER\",\"latitude\":\"\",\"longitude\":\"\",\"ip_address\":\"1.0.0.1\",\"device_type\":\"\",\"signup_type\":\"S\"}";
        //Debug.Log("jsonDataForRegister " + jsonData);
        if (jsonData != null)
        {
            //byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            byte[] data = Encoding.UTF8.GetBytes((jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {

            //decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            decryptString = (request.downloadHandler.text.Trim('"'));
            //Debug.Log(decryptString);
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                nickName = response.data.nick_name;
                transform.GetComponent<PhotonChatManager>().ChatConnectOnClick(nickName);
                //Debug.Log("registered Successfully token  " + response.data.token);
                PlayerPrefs.SetString("token", response.data.token);
                PlayerPrefs.SetString("email", response.data.email);
                string email = response.data.email;
                string[] emailList = email.Split('@');
                if (emailList.Length > 0)
                {
                    if (emailList[0].Length > 0)
                        email = emailList[0][0] + "***" + emailList[0][emailList[0].Length - 1] + "@";
                    if (emailList.Length > 1)
                        email += emailList[1];
                }
                uiHandler.descriptionVerifyScreenTextObj.text = "Security code sent to " + email;
                uiHandler.registerOtp = response.data.otp;
                uiHandler.Sign_Up_Pnl.SetActive(false);
                uiHandler.Verify_Pnl.SetActive(true);
                uiHandler.playerBallBalance = response.data.points;
                uiHandler.playerTotalBallCollect = response.data.total_points;
                uiHandler.continuteButtonVerifyScreenObj.interactable = true;
                uiHandler.mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
                uiHandler.firstInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.secondInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.threeInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.continuteButtonVerifyScreenObj.onClick.RemoveAllListeners();
                uiHandler.continuteButtonVerifyScreenObj.onClick.AddListener(uiHandler.OnClickContinueVerifyOtpScreen);
                uiHandler.resendButtonVerifyScreenObj.onClick.RemoveAllListeners();
                uiHandler.resendButtonVerifyScreenObj.onClick.AddListener(uiHandler.OnClickResendOtpVerifyOtpScreenForRegister);
                nickName = response.data.nick_name;
                transform.GetComponent<PhotonChatManager>().ChatConnectOnClick(nickName);
                uiHandler.changeAccountDetailsButtonProfileScreenObj.interactable = true;
                uiHandler.changePasswordButtonProfileScreenObj.interactable = true;
                uiHandler.displayNameTextProfileScreenObj.text = response.data.nick_name;
                uiHandler.fullNameTextProfileScreenObj.text = response.data.name;
                uiHandler.originTextProfileScreenObj.text = response.data.country;
                uiHandler.emailTextProfileScreenObj.text = response.data.email;
                uiHandler.currentBalanceTextProfileScreenObj.text = "0";
                uiHandler.totalBallsCollectedTextProfileScreenObj.text = "0";
                uiHandler.tierUnlockedTextProfileScreenObj.text = "0/3";

                uiHandler.displayNameTextEditProfileScreenObj.text = response.data.nick_name;
                uiHandler.originTextEditProfileScreenObj.text = response.data.country;
                uiHandler.emailTextEditProfileScreenObj.text = response.data.email;
                if (otpTimerCoroutine != null)
                {
                    StopCoroutine(otpTimerCoroutine);
                }
                otpTimerCoroutine = StartCoroutine(timerCoroutine(uiHandler.resendCountdownTextObj, 60));
            }
            else
            {
                //Debug.Log("Error : " + response.message);
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
        uiHandler.signinSignupScreenBtnObj.interactable = true;
    }

    public IEnumerator CallGetCountry(Transform countryParents, InputField inputfieldToSet)
    {
        UnityWebRequest request = UnityWebRequest.Get(getCoutnriesUrl);
        SetHeaderaa(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            decryptString = (request.downloadHandler.text.Trim('"'));
            countryList response = JsonUtility.FromJson<countryList>(decryptString);

            for (int i = 0; i < response.data.Count; i++)
            {
                //Debug.Log("inCountry");
                GameObject gm = Instantiate(CountryText, countryParents) as GameObject;
                gm.transform.GetChild(0).GetComponent<Text>().text = response.data[i].code;
                gm.transform.GetChild(1).GetComponent<Text>().text = response.data[i].name;
                gm.name = response.data[i].short_code;
                gm.transform.GetComponent<Button>().onClick.AddListener(() => countryfill(gm, inputfieldToSet));
            }
        }
    }
    #endregion

    public void countryfill(GameObject gm , InputField inputFieldToSet)
    {
        gm.transform.parent.parent.gameObject.SetActive(false);
        uiHandler.countryShortCode = gm.name;
        inputFieldToSet.name = gm.name;
        inputFieldToSet.text = gm.transform.GetChild(1).GetComponent<Text>().text;
    }

    #region Send Otp
    public IEnumerator CallSendOtp(string email, int verifySignup)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(sendOtpUrl, "post");
        SetHeader(request);
        jsonData = "{\"email\":\"" + email + "\",\"is_verify_signup\":\"" + verifySignup + "\"}";
        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();

        if (request.isDone)
        {
            string decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                SSTools.ShowMessage("Otp Resend Successfull", SSTools.Position.top, SSTools.Time.twoSecond);
                uiHandler.resendButtonVerifyScreenObj.interactable = false;
                if (uiHandler.apiCoroutine != null)
                {
                    StopCoroutine(uiHandler.apiCoroutine);
                }
                otpTimerCoroutine = StartCoroutine(timerCoroutine(uiHandler.resendCountdownTextObj, 60));
                uiHandler.mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
                uiHandler.firstInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.secondInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.threeInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.sixInputFieldVerifyOtpScreenObj.text = string.Empty;
            }
        }
        uiHandler.continuteButtonVerifyScreenObj.interactable = true;
    }
    #endregion

    #region Verify Otp
    public IEnumerator CallVerifyOtp(string email, string otp, bool isRegister)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(verifyOtpUrl, "post");
        SetHeaderaa(request);
        jsonData = "{\"email\":\"" + email + "\",\"otp\":\"" + otp + "\",\"is_verify_signup\":\"1\"}";
        if (jsonData != null)
        {
            //byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));

            byte[] data = Encoding.UTF8.GetBytes((jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //decryptString = Decryption(request.downloadHandler.text.Trim('"'));

            decryptString = (request.downloadHandler.text.Trim('"'));
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                uiHandler.mainOtpTextMeshInputFieldVerifyOtpScreenObj.text = "";
                uiHandler.firstInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.secondInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.threeInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.fourInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.fiveInputFieldVerifyOtpScreenObj.text = string.Empty;
                uiHandler.sixInputFieldVerifyOtpScreenObj.text = string.Empty;
                PlayerPrefs.SetString("token", response.data.token);

                nickName = response.data.nick_name;
                transform.GetComponent<PhotonChatManager>().ChatConnectOnClick(nickName);
                uiHandler.displayNameTextProfileScreenObj.text = response.data.nick_name;
                uiHandler.fullNameTextProfileScreenObj.text = response.data.name;
                uiHandler.originTextProfileScreenObj.text = response.data.country;
                uiHandler.emailTextProfileScreenObj.text = response.data.email;
                uiHandler.currentBalanceTextProfileScreenObj.text = "0";
                uiHandler.totalBallsCollectedTextProfileScreenObj.text = "0";
                uiHandler.tierUnlockedTextProfileScreenObj.text = "0/3";

                uiHandler.displayNameTextEditProfileScreenObj.text = response.data.nick_name;
                uiHandler.originTextEditProfileScreenObj.text = response.data.country;
                uiHandler.emailTextEditProfileScreenObj.text = response.data.email;
                if (otpTimerCoroutine != null)
                {
                    StopCoroutine(otpTimerCoroutine);
                }
                if (isRegister)
                {
#if !UNITY_EDITOR && UNITY_WEBGL
                        WebInterface.SetIFrameVisibility(true);
#endif
                }
                else
                {
                    uiHandler.confirmPasswordResetPasswordScreenInputFieldObj.text = "";
                    uiHandler.newPasswordResetPasswordScreenInputFieldObj.text = "";
                    uiHandler.Reset_Pnl.SetActive(true);
                    uiHandler.verifyPanelObj.SetActive(false);
                }
            }
            else
            {
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
    }
    #endregion

    #region Logout
    public IEnumerator CallLogout()
    {
        yield return new WaitForSeconds(2f);
        UnityWebRequest request = UnityWebRequest.Get(logoutUrl);
        SetHeaderaa(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //decryptString = Decryption(request.downloadHandler.text);
            decryptString = (request.downloadHandler.text);
            PlayerPrefs.SetString("token", "");
            uiHandler.Email_Login_Inp.text = "";
            uiHandler.Password_Login_Inp.text = "";
            uiHandler.Sign_Up_Pnl.SetActive(false);
            uiHandler.Verify_Pnl.SetActive(false);
            uiHandler.Login_Pnl.SetActive(true);
            uiHandler.CanvasforUI.gameObject.SetActive(true);
            uiHandler.inGameMenuCanvasObj.gameObject.SetActive(false);
            uiHandler.menuPanelInGameScreenObj.SetActive(false);
            multiplayerhandler.instance.OnLogout();

            yield return new WaitForSeconds(0.2f);
            OpenNewTab("http://52.21.127.189/react/");

            //for (int i = 0; i < SceneManager.sceneCount; ++i)
            //{
            //    Scene scene = SceneManager.GetSceneAt(i);
            //    if (scene.name != "Authencation")
            //    {
            //        SceneManager.UnloadSceneAsync(scene);
            //    }
            //}
        }
    }
    #endregion

    #region Profile
    public IEnumerator CallProfile()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(profileUrl, "post");
        SetHeader(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            Debug.Log(decryptString);
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                uiHandler.Login_Pnl.SetActive(false);
                uiHandler.playerBallBalance = response.data.points;
                uiHandler.playerTotalBallCollect = response.data.total_points;

                nickName = response.data.nick_name;
                transform.GetComponent<PhotonChatManager>().ChatConnectOnClick(nickName);
                uiHandler.displayNameTextProfileScreenObj.text = response.data.nick_name;
                uiHandler.fullNameTextProfileScreenObj.text = response.data.name;
                uiHandler.originTextProfileScreenObj.text = response.data.country;
                uiHandler.emailTextProfileScreenObj.text = response.data.email;
                uiHandler.currentBalanceTextProfileScreenObj.text = response.data.points.ToString();
                uiHandler.totalBallsCollectedTextProfileScreenObj.text = response.data.total_points.ToString();
                uiHandler.tierUnlockedTextProfileScreenObj.text = response.data.unlocked_tier + " / 3";
                uiHandler.ballBalanceValueTextShopItemDetailsScreenObj.text = response.data.points.ToString();
                uiHandler.currentBalanceValueTextShopTiersScreenObj.text = response.data.points.ToString();
                uiHandler.totalBallsCollectedValueTextShopTiersScreenObj.text = response.data.total_points.ToString();
                uiHandler.tiersUnlockedValueTextShopTiersScreenObj.text = response.data.unlocked_tier + " / 3";

                for (int i = 0; i < response.data.unlocked_tier; i++)
                {
                    if (i < uiHandler.tiersIndicatorsImagesParentShopTiersScreenObj.transform.childCount)
                    {
                        //Debug.LogError("unlocked");
                        uiHandler.tiersIndicatorsImagesParentShopTiersScreenObj.transform.GetChild(i).GetComponent<Image>().sprite = uiHandler.tierUnlockedLineSprite;
                    }
                }

                uiHandler.displayNameTextEditProfileScreenObj.text = response.data.nick_name;
                uiHandler.originTextEditProfileScreenObj.text = response.data.country;
                uiHandler.emailTextEditProfileScreenObj.text = response.data.email;
                //uiHandler.characterDialogPanelObj.SetActive(true);
                if (response.data.user_type == "GUEST")
                {
                    Debug.Log("in setting false buttton");
                    uiHandler.changeAccountDetailsButtonProfileScreenObj.interactable = false;
                    uiHandler.changePasswordButtonProfileScreenObj.interactable = false;
                }
                if (response.data.ready_player_me_id != "")
                {
                    webAvatarLoaderObj.OnWebViewAvatarGenerated(response.data.ready_player_me_id);
                }
                else
                {
#if !UNITY_EDITOR && UNITY_WEBGL
                            WebInterface.SetIFrameVisibility(true);
#endif
                }
            }
            else
            {
                //UIhandler.showMessage("User details not found, please login again");
                //UIhandler.showMessage(response.message);
                uiHandler.Login_Pnl.SetActive(true);
            }
            uiHandler.Login_Pnl.SetActive(true);
        }
    }
    #endregion

    #region update profile
    public IEnumerator CallUpdateProfile()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(updateProfileUrl, "post");
        SetHeader(request);
        jsonData = "{\"name\":\"" + uiHandler.fullNameInputFieldEditAccountScreenObj.text + "\",\"country\":\"" + uiHandler.originTextEditProfileScreenObj.name + "\",\"nick_name\":\"" + uiHandler.displayNameTextEditProfileScreenObj.text + "\",\"email\":\"" + uiHandler.emailTextEditProfileScreenObj.text + "\"}";
        Debug.Log(jsonData);
        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();


        if (request.isDone)
        {
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                //Debug.Log(decryptString);
                uiHandler.fullNameTextProfileScreenObj.text = uiHandler.fullNameInputFieldEditAccountScreenObj.text;
                uiHandler.editAccountDetailsPanelObj.SetActive(false);
                uiHandler.profilePanelObj.SetActive(true);
            }
            else
            {
                //Debug.Log("Error : " + response.message);
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
        uiHandler.closeBtnEditProfileScreenObj.interactable = true;
    }
    #endregion

    #region ForgotPassword
    public IEnumerator CallForgotPassword()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(forgotpasswrodUrl, "post");
        SetHeader(request);
        jsonData = "{\"email\":\"" + uiHandler.passwordForgotPasswordInputFieldObj.text + "\"}";

        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        //Debug.Log("jsonData " + jsonData);
        yield return request.SendWebRequest();

        if (request.isDone)
        {
            //Debug.Log("response " + request.downloadHandler.text);
            SSTools.ShowMessage("OTP sent successfully to E-mail", SSTools.Position.top, SSTools.Time.twoSecond);
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            //Debug.Log(decryptString);
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                string email = response.data.email;
                string[] emailList = email.Split('@');
                if (emailList.Length > 0)
                {
                    if (emailList[0].Length > 0)
                        email = emailList[0][0] + "***" + emailList[0][emailList[0].Length - 1] + "@";
                    if (emailList.Length > 1)
                        email += emailList[1];
                }
                uiHandler.descriptionVerifyScreenTextObj.text = "Security code sent to " + email;
                uiHandler.forgotPasswordEmail = response.data.email;
                uiHandler.forgotPasswordOtp = response.data.otp;
                uiHandler.verifyPanelObj.SetActive(true);
                uiHandler.Forgot_Password_Pnl.SetActive(false);
                uiHandler.continuteButtonVerifyScreenObj.interactable = true;
                uiHandler.continuteButtonVerifyScreenObj.onClick.RemoveAllListeners();
                uiHandler.continuteButtonVerifyScreenObj.onClick.AddListener(uiHandler.OnClickContinueVerifyScreenForForgotPassword);
                uiHandler.resendButtonVerifyScreenObj.onClick.RemoveAllListeners();
                uiHandler.resendButtonVerifyScreenObj.onClick.AddListener(uiHandler.OnClickResendOtpVerifyOtpScreenForForgotPassword);
                if (otpTimerCoroutine != null)
                {
                    StopCoroutine(otpTimerCoroutine);
                }
                otpTimerCoroutine = StartCoroutine(timerCoroutine(uiHandler.resendCountdownTextObj, 60));
            }
            else
            {
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
        uiHandler.backButtonForgotPasswordScreenBtnObj.interactable = true;
    }

    public IEnumerator CallResetPassword()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(resetPasswordUrl, "post");
        SetHeader(request);
        jsonData = "{\"email\":\"" + uiHandler.forgotPasswordEmail + "\",\"password\":\"" + uiHandler.newPasswordResetPasswordScreenInputFieldObj.text + "\",\"c_password\":\"" + uiHandler.confirmPasswordResetPasswordScreenInputFieldObj.text + "\"}";
        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                //Debug.Log(decryptString);
                //Debug.Log("Password Reseted successfully");
                uiHandler.Email_Login_Inp.text = "";
                uiHandler.Password_Login_Inp.text = "";
                uiHandler.Login_Pnl.SetActive(true);
                uiHandler.resetPasswordScreenObj.SetActive(false);
            }
            else
            {
                //.Log("Error : " + response.message);
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
    }
    #endregion

    #region ChangePassword
    public IEnumerator CallChangePassword()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(changePasswordUrl, "post");
        SetHeader(request);
        jsonData = "{\"old_password\":\"" + uiHandler.currentPasswordInputFieldChangePasswordScreenObj.text + "\",\"new_password\":\"" + uiHandler.newPasswordInputFieldChangePasswordScreenObj.text + "\",\"c_new_password\":\"" + uiHandler.confirmPasswordInputFieldChangePasswordScreenObj.text + "\"}";
        //Debug.Log(jsonData);
        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            if (response.code == 1)
            {
                //Debug.Log("changePassword Successfull");
                SSTools.ShowMessage("Password changed successfully", SSTools.Position.top, SSTools.Time.twoSecond);
                uiHandler.currentPasswordInputFieldChangePasswordScreenObj.text = "";
                uiHandler.newPasswordInputFieldChangePasswordScreenObj.text = "";
                uiHandler.confirmPasswordInputFieldChangePasswordScreenObj.text = "";
                uiHandler.changePasswordPanelObj.SetActive(false);
                uiHandler.profilePanelObj.SetActive(true);
            }
            else
            {
                //Debug.Log("Error : " + response.message);
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }
        uiHandler.closeBtnChangePasswordScreenObj.interactable = true;
    }
    #endregion

    #region ReadyPlayerMe Avatar URL

    public IEnumerator CallReadyPlayerMeAvatarUrlSubmit(string avatarUrl)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(avatarSubmitUrl, "post");
        SetHeader(request);
        jsonData = "{\"ready_player_me_id\":\"" + avatarUrl + "\"}";
        //Debug.Log(jsonData);
        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //Debug.Log("readyPlayerid   " + request.downloadHandler.text);
            string decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            playerinfo response = JsonUtility.FromJson<playerinfo>(decryptString);
            //Debug.Log(decryptString);
            if (response.code == 1)
            {
                //Debug.Log("avatar url submited");
                //SSTools.ShowMessage("Avatar updated successfully", SSTools.Position.top, SSTools.Time.twoSecond);
            }
            else
            {
                //Debug.Log("Error : " + response.message);
            }
        }
    }

    #endregion

    #region artifact Detail
    public IEnumerator CallArtifactDetails(int artifactCode)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(artifactUrl, "post");
        SetHeader(request);
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
            //Debug.Log(decryptString);
            Artifact response = JsonUtility.FromJson<Artifact>(decryptString);
            if (response.code == 1)
            {

                uiHandler.ArtifactNameTextObj.text = response.data.title;
                string discription = response.data.description;//.Replace("&nbsp;", " ");
                //discription = discription.Replace("&amp;", "&");
                //discription = discription.Replace("<p>", "");
                //discription = discription.Replace("</p>", "");
                //discription = discription.Replace("<strong>", "<b>");
                //discription = discription.Replace("</strong>", "</b>");
                uiHandler.ArtifactDescriptionTextObj.text = discription;
            }
        }
    }
    #endregion

    #region //Get farmer List
    public IEnumerator Farmerlist()
    {
        UnityWebRequest request = UnityWebRequest.Get(getfarmerlistUrl);
        SetHeader(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            countryList response = JsonUtility.FromJson<countryList>(decryptString);
            //Debug.Log("Country " + decryptString);
            for (int i = 0; i < response.data.Count; i++)
            {
                //Debug.Log("inCountry");
            }
        }
    }
    #endregion

    #region famer details
    public void OnClickHeaderBtn(int value)
    {
        
        uiHandler.apiCoroutine = StartCoroutine(Famer_Section_Details(uiHandler.bioGraphyPanelInGameScreenObj.name, value));
        uiHandler.loadingTextLoadingScreenObj.text = "Please wait while loading ...";
        uiHandler.loadingPanelScreenObj.SetActive(true);
    }

    void disableAll()
    {
        biographyPanelDetailsScreenObj.SetActive(false);
        careerHighlightsPanelDetailsScreenObj.SetActive(false);
        grandSlamPanelDetailsScreenObj.SetActive(false);
        careerTimelineDetailsScreenObj.SetActive(false);
        triviaPanelDetailsScreenObj.SetActive(false);
        winPanelTriviaScreenObj.SetActive(false);
        losePanelTriviaScreenObj.SetActive(false);
    }

    string replaceString(string description)    {
        string des = description;
        des = des.Replace("&nbsp;", " ");
        return des;
    }

    //[HideInInspector]
    public List<Sprite> famerGallarySpriteList = new List<Sprite>();
    [HideInInspector]
    public int famerGallaryCounter = 0;
    public void OnClickPreviousFamerGallaryBiography()
    {
        famerGallaryCounter--;
        if (famerGallaryCounter >= 0 && famerGallarySpriteList.Count > 0)
        {
            famerGallaryMainImageObj.sprite = famerGallarySpriteList[famerGallaryCounter];
        }
        if (famerGallaryCounter <= 0)
        {
            perviousButtonFamerGallaryObj.interactable = false;
        }
        if (!nextButtonFamerGallaryObj.interactable)
        {
            nextButtonFamerGallaryObj.interactable = true;
        }

    }
    public void OnClickNextFamerGallaryBiography()
    {
        famerGallaryCounter++;
        if (famerGallaryCounter < famerGallarySpriteList.Count)
        {
            famerGallaryMainImageObj.sprite = famerGallarySpriteList[famerGallaryCounter];
        }
        if (famerGallaryCounter >= famerGallarySpriteList.Count - 1)
        {
            nextButtonFamerGallaryObj.interactable = false;
        }
        if (!perviousButtonFamerGallaryObj.interactable)
        {
            perviousButtonFamerGallaryObj.interactable = true;
        }

    }

    #region image Download
    bool imageDownloaded = false;
    IEnumerator DownloadProfile_pic(string urllink, bool isSetToObject, Image objectToSetImage)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(urllink);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite createdSprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero, 10f, 0, SpriteMeshType.Tight);
            createdSprite.name = System.IO.Path.GetFileName(urllink);
            if(isSetToObject)
            {
                objectToSetImage.sprite = createdSprite;
            }
            famerGallarySpriteList.Add(createdSprite);
            
            imageDownloaded = true;
        }
    }
    #endregion

    public IEnumerator famerGallaryImagePopup(Famer response)
    {
        for (int i=0; i < response.data.uploads.Count; i ++)
        {
            imageDownloaded = false;
            Debug.Log("image url  " + response.data.uploads[i].file_name);
            StartCoroutine(DownloadProfile_pic(response.data.uploads[i].file_name, false, null));
            while(!imageDownloaded)
            {
                yield return null;
            }
            if (i == 0)
            {
                famerGallaryMainImageObj.sprite = famerGallarySpriteList[0];
            }
            if (i > 0 && !nextButtonFamerGallaryObj.interactable)
            {
                nextButtonFamerGallaryObj.interactable = true;
            }
        }
    }

    public IEnumerator Famer_Section_Details(string famerId,int value)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(famers_detailUrl, "post");
        SetHeader(request);        
        jsonData = "{\"famer_id\":" + famerId + ",\"code\":" + value + "}";
        Tempfamer = famerId;
        if (jsonData != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //Debug.Log(request.downloadHandler.text);
            string decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            Debug.Log(decryptString);
            Famer response = JsonUtility.FromJson<Famer>(decryptString);

            if (response.code == 1)
            {
                //1 Biography
                disableAll();
                switch (value)
                {
                    case 1:
                        biographyPanelDetailsScreenObj.SetActive(true);
                        FamerName.text = response.data.full_name;
                        Famer_Header.text = replaceString(response.data.biography_text);
                        Famer_Description.text = replaceString(response.data.biography_description);
                        StartCoroutine(famerGallaryImagePopup(response));
                        break;
                    case 2:
                        careerHighlightsPanelDetailsScreenObj.SetActive(true);
                        career_highlights_left_description_txt.text = replaceString(response.data.career_highlights_left_description);
                        career_highlights_right_description_txt.text = replaceString(response.data.career_highlights_right_description);
                        break;
                    case 3:
                        //grandSlamPanelDetailsScreenObj.SetActive(true);
                        grandSlamPanelDetailsScreenObj.SetActive(true);
                        AUSTRALIAN_OPEN.text = replaceString(response.data.gs_australlian_open_text);
                        FRENCH_OPEN.text = replaceString(response.data.gs_french_open_text);
                        WIMBLEDON.text = replaceString(response.data.gs_wimbledon_open_text);
                        US_OPEN.text = replaceString(response.data.gs_us_open_text);

                        AUSTRALIAN_OPEN_Description.text = replaceString(response.data.gs_australlian_open_description);
                        FRENCH_OPEN_Description.text = replaceString(response.data.gs_french_open_description);
                        WIMBLEDON_Description.text = replaceString(response.data.gs_wimbledon_open_description);
                        US_OPEN_Description.text = replaceString(response.data.gs_us_open_description);
                        break;
                    case 4:
                        careerTimelineDetailsScreenObj.SetActive(true);
                        career_timeline_description_txt.text = replaceString(response.data.career_timeline_description);
                        break;
                    case 5:
                        triviaPanelDetailsScreenObj.SetActive(true);
                        AllTrue = true;
                        Quesion_No = 0;
                        Dot1.color = Color.white;
                        Dot2.color = Color.white;
                        Dot3.color = Color.white;
                        Quesion_optionA.transform.parent.GetComponent<Image>().sprite = Normal;
                        Quesion_optionB.transform.parent.GetComponent<Image>().sprite = Normal;
                        Quesion_optionC.transform.parent.GetComponent<Image>().sprite = Normal;
                        Quesion_No_Display.text = "Question 1 Of 3";
                        for (int i = 0; i < response.data.trivia.Length; i++)
                        {
                            All_Question[i].question = response.data.trivia[i].question;
                            All_Question[i].answer_a = response.data.trivia[i].answer_a;
                            All_Question[i].answer_b = response.data.trivia[i].answer_b;
                            All_Question[i].answer_c = response.data.trivia[i].answer_c;
                            All_Question[i].correct_answer = response.data.trivia[i].correct_answer;
                        }
                        Quesion_txt.text = All_Question[0].question;
                        Quesion_optionA.text = All_Question[0].answer_a;
                        Quesion_optionB.text = All_Question[0].answer_b;
                        Quesion_optionC.text = All_Question[0].answer_c;
                        Answer = All_Question[0].correct_answer;
                        break;
                }
                //Debug.Log("**** "+response.data.career_highlights_left_description);
                //Debug.Log(decryptString);
            }
            else if(response.code == 2)
            {
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.threeSecond);
                if (value == 5)
                {
                    playerNameTextTriviaWinScreenObj.text = "WELL DONE " + nickName + "! ALL YOUR ANSWERS WERE CORRECT!";
                    winPanelTriviaScreenObj.SetActive(true);
                }
            }
            else
            {
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.threeSecond);
            }
            uiHandler.loadingPanelScreenObj.SetActive(false);
        }
    }
    #endregion
    bool isQuestionAnswered = true;
    #region Trivia Input
    public void TriviaAnswer(GameObject gm)
    {
        if(!isQuestionAnswered)
        {
            return;
        }
        isQuestionAnswered = false;

        Image dot;
        if (Quesion_No == 0)
            dot = Dot1;
        else if (Quesion_No == 1)
            dot = Dot2;
        else
            dot = Dot3;

        if (Answer == gm.name)
        {
            dot.color = Color.green;
            gm.transform.GetComponent<Image>().sprite = Greenlight;
        }
        else
        {
            dot.color = Color.red;
            AllTrue = false;
            gm.transform.GetComponent<Image>().sprite = RedLight;
        }
        StartCoroutine(waitforsecond(gm));
    }
    #endregion

    #region  PlayAgain Trivia
    public void PlayAgain()
    {
        AllTrue = true;
        Quesion_No = 0;

        Dot1.color = Color.white;
        Dot2.color = Color.white;
        Dot3.color = Color.white;
        Quesion_optionA.transform.parent.GetComponent<Image>().sprite = Normal;
        Quesion_optionB.transform.parent.GetComponent<Image>().sprite = Normal;
        Quesion_optionC.transform.parent.GetComponent<Image>().sprite = Normal;
        Quesion_No_Display.text = "Question 1 Of 3";
        Quesion_txt.text = All_Question[Quesion_No].question;
        Quesion_optionA.text = All_Question[Quesion_No].answer_a;
        Quesion_optionB.text = All_Question[Quesion_No].answer_b;
        Quesion_optionC.text = All_Question[Quesion_No].answer_c;
        Answer = All_Question[Quesion_No].correct_answer;

        Lose_Panel.SetActive(false);
        triviaPanelDetailsScreenObj.SetActive(true);
    }
    #endregion

    #region Gallery Images
    IEnumerator Wait_GetGallery()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(galleryUrl, "post");
        SetHeader(request);

        yield return request.SendWebRequest();
        if (request.isDone)
        {
            string decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            //Debug.Log(decryptString);
            //GameData ss = JsonUtility.FromJson<GameData>(decryptString);
            //for (int i = 0; i < ss.data.Length; i++)
            //{
            //    GameObject gm = Instantiate(gallery_Prefeb) as GameObject;
            //    gm.transform.SetParent(parent_gallery);
            //    gm.transform.localPosition = Vector3.zero;
            //    gm.transform.localScale = Vector3.one;
            //    gm.name = i.ToString();
            //    //gm.transform.GetComponent<Button>().onClick.AddListener(() => LoadGame(int.Parse(gm.name)));
            //    // gm.transform.GetComponent<Button>().interactable = false;
            //}

            //for (int i = 0; i < parent_gallery.childCount; i++)
            //{
            //    t = new Task(DownloadProfile_pic(ss.data[i].name, parent_gallery.transform.GetChild(i).transform.gameObject));
            //    yield return new WaitWhile(() => t.Running);
            //}
        }

    }
    #endregion

    #region image Download
    Sprite gameSprite;
    IEnumerator DownloadProfile_pic(string urllink, Image gm)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(urllink);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite createdSprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero, 10f, 0, SpriteMeshType.Tight);
            createdSprite.name = System.IO.Path.GetFileName(urllink);
            gm.sprite = createdSprite;
        }
    }
    #endregion

    IEnumerator waitforsecond(GameObject gm)
    {
        //Debug.Log(Quesion_No + "______" + gm.name);
        uiHandler.loadingPanelScreenObj.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        isQuestionAnswered = true;
        if (Quesion_No < 2)
        {
            uiHandler.loadingPanelScreenObj.SetActive(false);
            gm.transform.GetComponent<Image>().sprite = Normal;
            Quesion_No++;
            Quesion_No_Display.text = "Question " + (Quesion_No + 1) + " Of 3";
            Quesion_txt.text = All_Question[Quesion_No].question;
            Quesion_optionA.text = All_Question[Quesion_No].answer_a;
            Quesion_optionB.text = All_Question[Quesion_No].answer_b;
            Quesion_optionC.text = All_Question[Quesion_No].answer_c;
            Answer = All_Question[Quesion_No].correct_answer;
        }
        else
        {
            uiHandler.loadingPanelScreenObj.SetActive(false);
            if (AllTrue)
            {
                playerNameTextTriviaWinScreenObj.text = "WELL DONE " + nickName + "! ALL YOUR ANSWERS WERE CORRECT!";
                win_Panel.SetActive(true);
                StartCoroutine(CallPointsTransaction(50, 3, true, int.Parse(Tempfamer), false, 0, false, 0));
            }
            else
            {
                Lose_Panel.SetActive(true);
            }
        }
    }

    public IEnumerator CallPointsTransaction(int points, int type, bool isFamer, int famerId, bool isNpc, int NpcId, bool isShop, int shopId)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(pointsTransactionUrl, "post");
        SetHeader(request);
        jsonData = "{\"points\":\"" + points + "\",\"type\":\"" + type + "\",\"famer_id\":\"" + famerId + "\",\"npc_id\":\"" + NpcId + "\",\"shop_id\":\"" + shopId + "\"}";
        Debug.Log(jsonData);
        if(jsonData != "")
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            playerinfo response = JsonUtility.FromJson<playerinfo>(Decryption(request.downloadHandler.text.Trim('"')));
            //.Log(Decryption(request.downloadHandler.text.Trim('"')));
            if(response.code == 1)
            {
                //Debug.Log("Successfull Transaction   " + response.message);
                uiHandler.playerBallBalance = response.data.current_points;
                uiHandler.playerTotalBallCollect = response.data.total_points;
                if (isShop)
                {
                    SSTools.ShowMessage("Item purchase successfull", SSTools.Position.top, SSTools.Time.twoSecond);
                    uiHandler.purchaseBtnShopItemDetailsScreenObj.gameObject.SetActive(false);
                    uiHandler.itemBallCostTextShopItemDetailsScreenObj.gameObject.SetActive(false);
                    uiHandler.inYourCollectionTextShopItemDetailsScreenObj.SetActive(true);
                }
            }
            else
            {
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.twoSecond);
            }
        }

    }

    public IEnumerator CallTiersDetails()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(tiersDetailsUrl, "post");
        SetHeader(request);
        yield return request.SendWebRequest();
        if(request.isDone)
        {
            //Debug.Log(request.downloadHandler.text);
            //Debug.Log("tiers response " + Decryption(request.downloadHandler.text.Trim('"')));

            ShopItemData response = JsonUtility.FromJson<ShopItemData>(Decryption(request.downloadHandler.text.Trim('"')));
            uiHandler.currentBalanceValueTextShopTiersScreenObj.text = response.data.points.ToString();
            uiHandler.totalBallsCollectedValueTextShopTiersScreenObj.text = response.data.total_points.ToString();
            uiHandler.tiersUnlockedValueTextShopTiersScreenObj.text = response.data.unlocked_tier + " / 3";

            uiHandler.tier1LockUnlockTextShopTiersScreenObj.text = "Tier 1: Locked";
            uiHandler.tier2LockUnlockTextShopTiersScreenObj.text = "Tier 2: Locked";
            uiHandler.tier3LockUnlockTextShopTiersScreenObj.text = "Tier 3: Locked";
            uiHandler.tier1LockStatusImageShopTiersScreenObj.sprite = uiHandler.tierLockedSprite;
            uiHandler.tier2LockStatusImageShopTiersScreenObj.sprite = uiHandler.tierLockedSprite;
            uiHandler.tier3LockStatusImageShopTiersScreenObj.sprite = uiHandler.tierLockedSprite;


            for (int i = 0; i < response.data.unlocked_tier; i++)
            {
                if (i < uiHandler.tiersIndicatorsImagesParentShopTiersScreenObj.transform.childCount)
                {
                    //Debug.LogError("unlocked");
                    uiHandler.tiersIndicatorsImagesParentShopTiersScreenObj.transform.GetChild(i).GetComponent<Image>().sprite = uiHandler.tierUnlockedLineSprite;
                    if (i == 0)
                    {
                        uiHandler.tier1LockUnlockTextShopTiersScreenObj.text = "Tier 1: Unlocked";
                        uiHandler.tier1LockUnlockTextShopTiersScreenObj.transform.parent.GetComponent<Image>().sprite = null;
                        uiHandler.tier1LockUnlockTextShopTiersScreenObj.transform.parent.GetComponent<Image>().color = new Color(0,0.38823529f, 0.2823529412f);
                        if(uiHandler.tier1LockUnlockTextShopTiersScreenObj.transform.parent.parent.childCount > 2)
                        {
                            uiHandler.tier1LockUnlockTextShopTiersScreenObj.transform.parent.parent.GetChild(2).GetComponent<Image>().sprite = null;
                            uiHandler.tier1LockUnlockTextShopTiersScreenObj.transform.parent.parent.GetChild(2).GetComponent<Image>().color = new Color(0, 0.38823529f, 0.2823529412f);
                        }
                        uiHandler.tier1LockStatusImageShopTiersScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                    }
                    if (i == 1)
                    {
                        uiHandler.tier2LockUnlockTextShopTiersScreenObj.text = "Tier 2: Unlocked";
                        uiHandler.tier2LockUnlockTextShopTiersScreenObj.transform.parent.GetComponent<Image>().sprite = null;
                        uiHandler.tier2LockUnlockTextShopTiersScreenObj.transform.parent.GetComponent<Image>().color = new Color(0, 0.38823529f, 0.2823529412f);
                        if (uiHandler.tier2LockUnlockTextShopTiersScreenObj.transform.parent.parent.childCount > 2)
                        {
                            uiHandler.tier2LockUnlockTextShopTiersScreenObj.transform.parent.parent.GetChild(2).GetComponent<Image>().sprite = null;
                            uiHandler.tier2LockUnlockTextShopTiersScreenObj.transform.parent.parent.GetChild(2).GetComponent<Image>().color = new Color(0, 0.38823529f, 0.2823529412f);
                        }
                        uiHandler.tier2LockStatusImageShopTiersScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                    }
                    if (i == 2)
                    {
                        uiHandler.tier3LockUnlockTextShopTiersScreenObj.text = "Tier 3: Unlocked";
                        uiHandler.tier3LockUnlockTextShopTiersScreenObj.transform.parent.GetComponent<Image>().sprite = null;
                        uiHandler.tier3LockUnlockTextShopTiersScreenObj.transform.parent.GetComponent<Image>().color = new Color(0, 0.38823529f, 0.2823529412f);
                        if (uiHandler.tier3LockUnlockTextShopTiersScreenObj.transform.parent.parent.childCount > 2)
                        {
                            uiHandler.tier3LockUnlockTextShopTiersScreenObj.transform.parent.parent.GetChild(2).GetComponent<Image>().sprite = null;
                            uiHandler.tier3LockUnlockTextShopTiersScreenObj.transform.parent.parent.GetChild(2).GetComponent<Image>().color = new Color(0, 0.38823529f, 0.2823529412f);
                        }
                        uiHandler.tier3LockStatusImageShopTiersScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                    }
                }
            }
        }
    }

    public IEnumerator CallShopDetails()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(shopDetailsUrl, "post");
        SetHeader(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            Debug.Log(Decryption(request.downloadHandler.text.Trim('"')));
            ShopData response = JsonUtility.FromJson<ShopData>(Decryption(request.downloadHandler.text.Trim('"')));
            ///Debug.Log("shopTiersDetails : " + response.data.Count);
            if (response.code == 1)
            {
                foreach (Transform t in uiHandler.tier1ItemsParentShopContainerScreenObj)
                {
                    Destroy(t.gameObject);
                }
                foreach (Transform t in uiHandler.tier2ItemsParentShopContainerScreenObj)
                {
                    Destroy(t.gameObject);
                }
                foreach (Transform t in uiHandler.tier3ItemsParentShopContainerScreenObj)
                {
                    Destroy(t.gameObject);
                }
                //Debug.Log(" - " + response.data.Count + " - " + response.data[0].shops_list.Count);
                uiHandler.tier1LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierLockedSprite;
                uiHandler.tier2LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierLockedSprite;
                uiHandler.tier3LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierLockedSprite;
                if (response.data.Count >= 0)
                {
                    if(response.data[0].unlocked_tier == 3)
                    {
                        uiHandler.tier1LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                        uiHandler.tier2LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                        uiHandler.tier3LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                    }
                    else if (response.data[0].unlocked_tier == 2)
                    {
                        uiHandler.tier1LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                        uiHandler.tier2LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                    }
                    else if (response.data[0].unlocked_tier == 1)
                    {
                        uiHandler.tier1LockStatusImageShopContainerScreenObj.sprite = uiHandler.tierUnlcokedSprite;
                    }
                }
                foreach (ShopItemDetailClass tierDetails in response.data)
                {
                    foreach (ShopTiersItemsDetailsClass itemDetails in tierDetails.shops_list)
                    {
                        GameObject item = Instantiate(shopItemPrefabObj);
                        item.name = itemDetails.shop_id.ToString();
                        item.transform.GetChild(0).name = itemDetails.tier_id.ToString();
                        item.transform.GetChild(1).GetComponent<Text>().text = itemDetails.points.ToString();
                        item.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            uiHandler.OnClickShopItemShopContainerScreen(item, response.data[0].unlocked_tier);
                        });
                        StartCoroutine(DownloadProfile_pic(itemDetails.file_name, item.transform.GetChild(0).GetComponent<Image>()));
                        if (itemDetails.tier_id == 1)
                        {
                            item.transform.SetParent(uiHandler.tier1ItemsParentShopContainerScreenObj, false);
                            uiHandler.tier1LockStatusImageShopContainerScreenObj.transform.parent.gameObject.SetActive(true);
                        }
                        else if (itemDetails.tier_id == 2)
                        {
                            item.transform.SetParent(uiHandler.tier2ItemsParentShopContainerScreenObj, false);
                            uiHandler.tier2LockStatusImageShopContainerScreenObj.transform.parent.gameObject.SetActive(true);
                        }
                        else if (itemDetails.tier_id == 3)
                        {
                            item.transform.SetParent(uiHandler.tier3ItemsParentShopContainerScreenObj, false);
                            uiHandler.tier3LockStatusImageShopContainerScreenObj.transform.parent.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public IEnumerator CallShopItemDetails(int shopItemId)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(shopItemDetailsUrl, "post");
        SetHeader(request);
        jsonData = "{\"shop_id\":\"" + shopItemId + "\"}";
        //Debug.Log(jsonData);
        if (jsonData != "")
        {
            byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //Debug.Log("tiers response " + Decryption(request.downloadHandler.text.Trim('"')));
            ShopItemData response = JsonUtility.FromJson<ShopItemData>(Decryption(request.downloadHandler.text.Trim('"')));
            uiHandler.itemNameTextShopItemDetailsScreenObj.text = response.data.title;
            uiHandler.itemBallCostTextShopItemDetailsScreenObj.text = response.data.points.ToString();
            uiHandler.itemDescriptionTextShopItemDetailsScreenObj.text = response.data.description;
            if(response.data.is_purchased)
            {
                uiHandler.purchaseBtnShopItemDetailsScreenObj.gameObject.SetActive(false);
                uiHandler.itemBallCostTextShopItemDetailsScreenObj.gameObject.SetActive(false);
                uiHandler.inYourCollectionTextShopItemDetailsScreenObj.SetActive(true);
            }
            else
            {
                uiHandler.purchaseBtnShopItemDetailsScreenObj.gameObject.SetActive(true);
                uiHandler.itemBallCostTextShopItemDetailsScreenObj.gameObject.SetActive(true);
                uiHandler.inYourCollectionTextShopItemDetailsScreenObj.SetActive(false);
            }
        }
    }

    public IEnumerator CallShopCollection()
    {
        UnityWebRequest request = UnityWebRequest.Get(shopCollectionUrl);
        SetHeader(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //Debug.Log("CollectionResponse : " + Decryption(request.downloadHandler.text.Trim('"')));
            ShopCollectionData response = JsonUtility.FromJson<ShopCollectionData>(Decryption(request.downloadHandler.text.Trim('"')));
            foreach (Transform t in uiHandler.collectionItemsParentShopCollectionPanelObj.transform)
            {
                Destroy(t.gameObject);
            }
            foreach (ShopItemDetailClass data in response.data)
            {
                GameObject item = Instantiate(shopItemPrefabObj);
                item.name = data.id.ToString();
                item.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                {
                    uiHandler.OnClickShopItemShopCollectionScreen(item);
                });
                StartCoroutine(DownloadProfile_pic(data.file_name, item.transform.GetChild(0).GetComponent<Image>()));
                item.transform.SetParent(uiHandler.collectionItemsParentShopCollectionPanelObj.transform, false);
            }
        }
    }

    public IEnumerator CallGuestSignup()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(guestUserSignupUrl, "post");
        SetHeaderaa(request);
        jsonData = "{\"nick_name\":\"" + uiHandler.nickNameInputFieldGuestSignupScreenObj.text + "\",\"ready_player_me_id\":\"https://readyplayer.me/avatarurl123.glb\"}";
        if(jsonData != "")
        {
            Debug.Log(jsonData);
            //byte[] data = Encoding.UTF8.GetBytes(Encryption(jsonData));
            byte[] data = System.Text.Encoding.UTF8.GetBytes((jsonData));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            request.uploadHandler = upHandler;
        }
        yield return request.SendWebRequest();
        if(request.isDone)
        {
            //playerinfo response = JsonUtility.FromJson<playerinfo>(Decryption(request.downloadHandler.text.Trim('"')));
            playerinfo response = JsonUtility.FromJson<playerinfo>((request.downloadHandler.text.Trim('"')));
            Debug.Log((request.downloadHandler.text.Trim('"')));
            if(response.code == 1)
            {
                PlayerPrefs.SetString("token", response.data.token);
                uiHandler.playerBallBalance = response.data.points;
                uiHandler.playerTotalBallCollect = response.data.total_points;
                nickName = response.data.nick_name;
                uiHandler.displayNameTextProfileScreenObj.text = nickName;
                uiHandler.fullNameTextProfileScreenObj.text = "";
                uiHandler.emailTextEditProfileScreenObj.text = "";
                uiHandler.originTextEditProfileScreenObj.text = "";
                transform.GetComponent<PhotonChatManager>().ChatConnectOnClick(nickName);
                uiHandler.currentBalanceTextProfileScreenObj.text = "0";
                uiHandler.totalBallsCollectedTextProfileScreenObj.text = "0";
                uiHandler.tierUnlockedTextProfileScreenObj.text = "0/3";
                uiHandler.changeAccountDetailsButtonProfileScreenObj.interactable = false;
                uiHandler.changePasswordButtonProfileScreenObj.interactable = false;
#if UNITY_WEBGL && !UNITY_EDITOR
                    WebInterface.SetIFrameVisibility(true);
#endif
            }
            else
            {
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.threeSecond);
            }
        }
    }

    #region ****  Npc Question    ****
    public void Get_Npc_Question(GameObject gm)
    {
        Quesion_NPC_optionA.transform.parent.GetComponent<Image>().sprite = Normal;
        Quesion_NPC_optionB.transform.parent.GetComponent<Image>().sprite = Normal;
        Quesion_NPC_optionC.transform.parent.GetComponent<Image>().sprite = Normal;
        uiHandler.loadingPanelScreenObj.SetActive(true);
        gm.SetActive(false);
        StartCoroutine(Npc_Question());
    }
    IEnumerator Npc_Question()
    {
        UnityWebRequest request = UnityWebRequest.Get(InteractiveNpcQuestionUrl);
        SetHeader(request);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            decryptString = Decryption(request.downloadHandler.text.Trim('"'));
            Famer response = JsonUtility.FromJson<Famer>(decryptString);
            if (response.code == 1)
            {
                uiHandler.loadingPanelScreenObj.SetActive(false);
                Npc_Question_Panel.SetActive(true);
                Quesion_NPC_Display.name = response.data.id.ToString();
                Quesion_NPC_Display.text = response.data.question;
                Quesion_NPC_optionA.text = response.data.answer_a;
                Quesion_NPC_optionB.text = response.data.answer_b;
                Quesion_NPC_optionC.text = response.data.answer_c;
                NPC_Answer = response.data.correct_answer;
                isNpcAnswered = false;
                //Debug.Log(response.data.question);
            }
            else
            {
                uiHandler.loadingPanelScreenObj.SetActive(false);
                Npc_Question_Panel.transform.parent.parent.gameObject.SetActive(true);
                Npc_Question_Panel.transform.parent.GetChild(0).gameObject.SetActive(true);
                uiHandler.playerNameValueTextNpcIntroducationObj.gameObject.SetActive(false);
                uiHandler.descriptionTextNpcIntroductionObj.gameObject.SetActive(true);
                uiHandler.descriptionTextNpcIntroductionObj.text = "You completed all questions.";
                uiHandler.playButtonNpcIntroducationObj.gameObject.SetActive(false);
                SSTools.ShowMessage(response.message, SSTools.Position.top, SSTools.Time.threeSecond);
            }
        }
    }

    bool isNpcAnswered = false;
    public void NpcAnswer(GameObject gm)
    {
        if(isNpcAnswered)
        {
            return;
        }
        isNpcAnswered = true;
        if (NPC_Answer == gm.name)
        {
            gm.transform.GetComponent<Image>().sprite = Greenlight;
            uiHandler.playerNameTextNpcWinScreenObj.text = "WELL DONE " + nickName + "! ALL YOUR ANSWERS WERE CORRECT!";
            StartCoroutine(Npc_Questioncall(Npc_Win));
            StartCoroutine(CallPointsTransaction(50, 4, false, 0, true, int.Parse(Quesion_NPC_Display.name), false, 0));
            //Debug.Log("_____True Answer_______");
        }
        else
        {
            gm.transform.GetComponent<Image>().sprite = RedLight;
            StartCoroutine(Npc_Questioncall(Npc_Lose));
            //Debug.Log("_____Wrong Answer_______");
        }
    }
    IEnumerator Npc_Questioncall(GameObject gm)
    {
        yield return new WaitForSeconds(2.0f);
        gm.SetActive(true);
    }
    #endregion

    TimeSpan t1 = TimeSpan.FromSeconds(1f);
    public IEnumerator timerCoroutine(Text timerText, int timeInSecond)
    {
        TimeSpan t2 = TimeSpan.FromSeconds(timeInSecond);
        timerText.gameObject.SetActive(true);
        uiHandler.resendCountdownTextObj.gameObject.SetActive(true);
        while (true)
        {
            t2 = t2 - t1;
            timerText.text = string.Format("{0:00}:{1:00}", t2.Minutes, t2.Seconds);
            if (t2.TotalSeconds <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        uiHandler.resendButtonVerifyScreenObj.interactable = true;
        timerText.gameObject.SetActive(false);
        yield return null;
    }

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
}

