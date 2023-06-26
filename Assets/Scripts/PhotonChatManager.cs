using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    #region Setup
    ChatClient chatClient;
    bool isConnected;
    [HideInInspector]
    public string nickName = "";

    [Header("Uihandler")]
    public UIhandler uiHandler;
    UIhandler ui_hanscript;
    public static PhotonChatManager PhotonChatManagerInst;

    bool isSubscribed = false;

    void Awake()
    {
        PhotonChatManagerInst = this;
        ui_hanscript = uiHandler.transform.GetComponent<UIhandler>();
    }

    public void ChatConnectOnClick(string nickname)
    {
        nickName = nickname;
        isConnected = true;
        chatClient = new ChatClient(this);
        //chatClient.ChatRegion = "asia";
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(nickName));
        //Debug.Log("Connenting");
    }
    #endregion Setup

    #region General

    [SerializeField] GameObject chatPanel;
    string privateReceiver = "";
    string currentChat;
    [SerializeField] InputField chatField;

    [SerializeField] Text chatDisplay;
    [SerializeField] Text chatDisplayPrivate;

    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

        if (chatField.text != "" && Input.GetKey(KeyCode.Return))
        {
            SubmitPublicChatOnClick();
            SubmitPrivateChatOnClick();
        }
    }

    #endregion General

    #region PublicChat

    public void SubmitPublicChatOnClick()
    {
        if (privateReceiver == "")
        {
            if(currentChat == "" || currentChat.Trim(' ') == "")
            {
                return;
            }
            //Debug.Log("Text " + currentChat);
            currentChat = ProfanityFilter.instance.CheckInput(currentChat);
            chatClient.PublishMessage("RegionChannel", currentChat);
            chatField.text = "";
            currentChat = "";
        }
    }

    public void Calling()
    {
        //OnPrivateMessage(nickName, chatField.text, "dhvanil");
        chatClient.SendPrivateMessage("anu3", chatField.text);
    }

    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    #endregion PublicChat

    // #region PrivateChat
    //public void ReceiverOnValueChange(string valueIn)
    // {
    //     privateReceiver = valueIn;
    // }

    // public void SubmitPrivateChatOnClick()
    // {
    //     if (chatField.text != "")
    //     {
    //         chatClient.SendPrivateMessage("anu3", chatField.text);
    //         chatField.text = "";
    //         currentChat = "";
    //     }
    // }

    // #endregion PrivateChat

    #region PrivateChat
    public GameObject oneToOneChatDialog;
    public GameObject privateChatPnl;
    public GameObject publicChatPnl;
    public Text playerNameTxtOnPrvtChat;

    public void ReceiverOnValueChange(string valueIn)
    {
        privateReceiver = valueIn;
    }
    public string currentPlayerName;
    public void SubmitPrivateChatOnClick(InputField inp = null)
    {
        if (currentChat != "")
        {
            string temp = currentChat;
            //Debug.LogError("currentPlayerName:" + currentPlayerName);
            chatClient.SendPrivateMessage(currentPlayerName, temp);
            currentChat = "";
        }
    }
    public void YesPrivateChat()
    {
        privateChatPnl.SetActive(true);
        publicChatPnl.SetActive(false);
    }
    #endregion PrivateChat

    #region Callbacks

    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        if(state == ChatState.Uninitialized)
        {
            isConnected = false;
            //joinChatButton.SetActive(true);
            //chatPanel.SetActive(false);
        }
        //throw new System.NotImplementedException();
        //Debug.Log("Connected");
        //isConnected = true;
        //joinChatButton.SetActive(false);
    }

    public void OnConnected()
    {
        //Debug.Log("Connected");
        //joinChatButton.SetActive(false);
        chatClient.Subscribe(new string[] { "RegionChannel" },100);
        isConnected = true;
        //Dictionary<string, ChatChannel> channels = chatClient.PublicChannels;
        //Debug.Log("channgel count :- "+ channels.Count);
        //foreach(KeyValuePair<string , ChatChannel> t in channels)
        //{
        //    Debug.Log(t.Key + " -- "+t.Value.Name);
        //}
        //Dictionary<string, ChatChannel> channelss = chatClient.PrivateChannels;
        //Debug.Log("channgel count :- " + channelss.Count);
        //foreach (KeyValuePair<string, ChatChannel> t in channelss)
        //{
        //    Debug.Log(t.Key + " -- " + t.Value.Name);
        //}
        //if (channels.Count > 0)
        //{
        //    if (chatClient.TryGetChannel("RegionChannel", out ChatChannel publicChannelObj))
        //    {
        //        string msgs = "";
        //        List<object> messages = publicChannelObj.Messages;
        //        List<string> senders = publicChannelObj.Senders;
        //        for (int i = 0; i < messages.Count; i++)
        //        {
        //            if (senders[i] == nickName)
        //                nameyello = "<color=\"yellow\">" + senders[i] + "</color>";
        //            else
        //                nameyello = senders[i];

        //            msgs = string.Format("{0}: {1}", nameyello, messages[i]);
        //            chatDisplay.text += msgs + "\n";
        //            LayoutRebuilder.ForceRebuildLayoutImmediate(chatDisplay.GetComponent<RectTransform>());
        //            Debug.Log(msgs);
        //        }
        //        Debug.Log("Getted Channel");
        //    }
        //    else
        //    {
        //        Debug.Log("in else of trygettingchannel");
        //    }
        //}
        //Debug.Log("in connected end");
    }

    public void OnDisconnected()
    {
        isConnected = false;
        //joinChatButton.SetActive(true);
        //chatPanel.SetActive(false);
    }
    string nameyello;
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";

        Debug.Log("onGetMessage");

        for (int i = 0; i < senders.Length; i++)
        {
            if (senders[i] == nickName)
                nameyello = "<color=\"yellow\">" + senders[i] + "</color>";
            else
                nameyello = senders[i];
            
            msgs = string.Format("{0}: {1}", nameyello, messages[i]);
            chatDisplay.text +=  msgs+ "\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatDisplay.GetComponent<RectTransform>());
            //Debug.Log(msgs);
        }
        if (!isSubscribed)
        {
            isSubscribed = true;
            if (!uiHandler.IsMobileCheck())
            {
                UIhandler.instance.chatInGameScreenObj.SetActive(true);
            }
            return;
        }
        uiHandler.OnMessageReceivedChat();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string msgs = "";
        msgs = string.Format("{0}: {1}", sender, message);
        chatDisplayPrivate.text += msgs + "\n";
       // Debug.Log(msgs);
    }

    public void DoSomething()
    {
        chatClient.UseBackgroundWorkerForSending = true;
        chatClient.SendAcksOnly();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //Debug.Log("onSubscribed Count is " + channels.Length + " --- " + channels[0]);
        //if (channels.Length > 0)
        //{
        //    if (chatClient.TryGetChannel(channels[0], out ChatChannel publicChannelObj))
        //    {
        //        string msgs = "";
        //        List<object> messages = publicChannelObj.Messages;
        //        List<string> senders = publicChannelObj.Senders;
        //        for (int i = 0; i < messages.Count; i++)
        //        {
        //            if (senders[i] == nickName)
        //                nameyello = "<color=\"yellow\">" + senders[i] + "</color>";
        //            else
        //                nameyello = senders[i];

        //            msgs = string.Format("{0}: {1}", nameyello, messages[i]);
        //            chatDisplay.text += msgs + "\n";
        //            LayoutRebuilder.ForceRebuildLayoutImmediate(chatDisplay.GetComponent<RectTransform>());
        //            //Debug.Log(msgs);
        //        }
        //        //Debug.Log("Getted Channel " +messages.Count + " -- " + senders.Count + " -- " + publicChannelObj.Messages.Count);
        //    }
        //    else
        //    {
        //        //Debug.Log("in else of trygettingchannel");
        //    }
        //}
        //Debug.Log("in end onSubscribed");
        //isSubscribed = true;
        // Debug.Log("in connected end");
        //chatPanel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
        isSubscribed = false;
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }
    #endregion Callbacks


}