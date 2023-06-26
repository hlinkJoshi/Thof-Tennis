using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using TMPro;
using StarterAssets;
using ExitGames.Client.Photon;
using UnityEngine.Animations;
using System.Security.Policy;
using static api;
using UnityEngine.EventSystems;

public class multiplayerhandler : MonoBehaviourPunCallbacks
{
    public UIhandler uiHandlerInst;

    public string PlayerName;
    string ImageUrl;
    byte Max_player = 20;
    private api apihander;

    public GameObject PlayerFollowCamera;

    [Header("Joystic for mobile")]
    public FixedJoystick movementJoystick;
    public FixedJoystick cameraMovementJoystic;
    [Space(20)]
    public GameObject T2, T3;

    string avatarurl;
    private WebAvatarLoader webavatarload;
    bool wasConnected = false;
    public int photonViewId = 0;

    public GameObject lookat1, lookat2, lookat3, lookat4, lookat5;
   
    [SerializeField] private List<GameObject> playersList = new List<GameObject>();
    public bool isTennisGameStarted;

    public static multiplayerhandler instance;
    public void Awake()
    {
        if (instance == null) instance = this;

        PhotonNetwork.EnableCloseConnection = true;
    }

    public void Started(string avatarUrl, string playerNamestr)
    {
      
        PlayerName = playerNamestr;
        avatarurl = avatarUrl;
        webavatarload = GameObject.Find("WebAvatarLoader").transform.GetComponent<WebAvatarLoader>();
        apihander = transform.GetComponent<api>();
        //Debug.Log("now calling connect");
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else if(!PhotonNetwork.InLobby && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinLobby();
        }
        else if(!PhotonNetwork.InRoom)
        {
            JoinRandomRoom();
        }
    }
    public override void OnConnectedToMaster()
    {
        //Debug.Log(" On connect to Master ");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        //Debug.Log(" OnJoined Lobby ");
        JoinRandomRoom();
        //CreateRoom_pp();
        //JoinRoom("Room_No_0302");
    }

    public override void OnJoinedRoom()
    {
        if (!wasConnected)
        {
            //Debug.Log("Calling start Game");
            StartGame();
        }
        else
        {
            //Debug.Log("Calling initiatePlayer");
            StartCoroutine(initiatePlayer());
        }
        //Debug.Log("Hello Join Room"+PhotonNetwork.CurrentRoom.Name);
    }

    public IEnumerator initiatePlayer()
    {
        //Debug.Log("inInitiating player after rejoin");
        yield return new WaitForSeconds(4f);
        foreach (PhotonView view in PhotonNetwork.PhotonViews)
        {
            if (!view.IsMine)
            {
                view.gameObject.SetActive(false);
            }
        }
        foreach (PhotonView view in PhotonNetwork.PhotonViews)
        {
            if (view.IsMine)
            {
                GameObject gm = view.gameObject;
                gm.SetActive(false);
                gm.name = PlayerName;
                gm.transform.localPosition = Vector3.zero;
                gm.SetActive(true);
                gm.transform.localPosition = new Vector3(-7.67f, 0.248f, 32.52f);
                PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().Follow = gm.transform.GetChild(0).gameObject.transform;
                PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().LookAt = gm.transform.GetChild(0).gameObject.transform;
                gm.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 0));
                gm.transform.GetComponent<ThirdPersonController>().movementJoystick = movementJoystick;
                gm.transform.GetComponent<ThirdPersonController>().cameraMovementJoystic = cameraMovementJoystic;
                webavatarload.staticCharacter = gm.transform.GetChild(1).transform.gameObject;
                //webavatarload.OnWebViewAvatarGenerated(avatarurl);
                isdownloadStarted = true;
                webavatarload.OnAvatarGeneratedLoading(avatarurl);
                
                break;
            }
        }
        foreach (PhotonView view in PhotonNetwork.PhotonViews)
        {
            if (!view.IsMine)
            {
                view.gameObject.SetActive(true);
            }
        }
        StartCoroutine(GetPlayerList());
    }

    public override void OnLeftLobby()
    {
        //Debug.Log(" Left Lobby calling reconnect and rejoin");
        PhotonNetwork.ReconnectAndRejoin();
    }
    public Transform gm;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.LogError(newPlayer.NickName + "URl Add" + newPlayer.CustomProperties["AvatarUrl"].ToString());
        StartCoroutine(WaitAndPrint(newPlayer));

     //   StartCoroutine(GetPlayerList());
    }

    IEnumerator WaitAndPrint(Player newPlayer)
    {
        yield return new WaitUntil(() => isdownloadStarted == false);
        webavatarload.staticCharacter = null;
        yield return new WaitForSeconds(5.0f);
        //Debug.LogError("_____________" + newPlayer.NickName);
        gm = GameObject.Find(newPlayer.NickName).transform;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name != newPlayer.NickName)
            {
                continue;
            }
            if (players[i].GetComponent<PhotonView>() && !players[i].GetComponent<PhotonView>().IsMine)
            {
                gm = players[i].GetComponent<Transform>();
                break;
            }
        }

        GameObject findObj;
        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i] == null )
            {
                playersList.RemoveAt(i);
            }
        }

        findObj = playersList.Find(i => i.name == gm.name);
        if (findObj == null) { playersList.Add(gm.gameObject); }
        if (isTennisGameStarted)
        {
            HidePlayer();
        }

        webavatarload.staticCharacter = gm.GetChild(1).transform.gameObject;
        webavatarload.OnAvatarGeneratedLoading(newPlayer.CustomProperties["AvatarUrl"].ToString());
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //Debug.Log("join room failed    " + message);
        CreateRoom_pp();
    }

    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            //Debug.Log("-----  Join  ----- " + PlayerName);
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();

            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom(false);

            PhotonNetwork.LocalPlayer.NickName = PlayerName;
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Userid", PlayerName);
            hash.Add("AvatarUrl", avatarurl);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //Debug.Log("______JoinRandomRoomCalling_____________");
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                        {"EntryFee", "100"}
                };
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, (byte)Max_player);
        }
    }

    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            //Debug.Log("-----  Join  ----- " + PlayerName);
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();

            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom(false);

            PhotonNetwork.LocalPlayer.NickName = PlayerName;
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Userid", PlayerName);
            hash.Add("AvatarUrl", avatarurl);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //Debug.Log("______JoinRandomRoomCalling_____________");
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                        {"EntryFee", "100"}
                };
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void CreateRoom_pp()
    {
        string roomName = "Room_No_" + UnityEngine.Random.Range(0, 9999);
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable() { { "EntryFee", "100" } };
        PhotonNetwork.LocalPlayer.NickName = PlayerName;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("Userid", PlayerName);
        hash.Add("AvatarUrl", avatarurl);
        //Debug.Log("______CreateRoom_____________");
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        string[] roomPropsInLobby = { "EntryFee" };
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = roomProps;
        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)Max_player;
        roomOptions.PlayerTtl = 900000;
        //roomOptions.EmptyRoomTtl = 300000;
        //Debug.Log("___________" + roomName);
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void Back_LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom(false);
    }
    GameObject gmm;
    public void StartGame()
    {
        //Debug.Log("in StartGame");
        isdownloadStarted = true;
        GameObject gm = PhotonNetwork.Instantiate("PlayerArmature", Vector3.zero, Quaternion.Euler(0, -180, 0)) as GameObject;
        playersList.Add(gm);
        gm.name = PlayerName;
        gm.transform.localPosition = new Vector3(Random.Range(-17,3), 0.248f, Random.Range(30, 45));//7,20  //28,45
        PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().Follow = gm.transform.GetChild(0).gameObject.transform;
        PlayerFollowCamera.transform.GetComponent<CinemachineVirtualCamera>().LookAt = gm.transform.GetChild(0).gameObject.transform;
       // gm.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 0));
        gm.transform.GetComponent<ThirdPersonController>().movementJoystick = movementJoystick;
        gm.transform.GetComponent<ThirdPersonController>().cameraMovementJoystic = cameraMovementJoystic;
        webavatarload.isEditAvatar = false;
        webavatarload.staticCharacter = gm.transform.GetChild(1).transform.gameObject;
        webavatarload.OnAvatarGeneratedLoading(avatarurl);
        uiHandlerInst.playerArmatureObj = gm;
        Invoke("activecollider", 3.0f);
        GetComponent<PhotonChatManager>().ChatConnectOnClick(PlayerName);

        StartCoroutine( GetPlayerList());
        gmm = gm.transform.GetChild(2).GetChild(0).gameObject;
        gmm.GetComponent<TextMeshProUGUI>().text = PlayerName;

        ConstraintSource cn = new ConstraintSource();
        cn.sourceTransform = Camera.main.transform;
        cn.weight = 1;
        gmm.GetComponent<LookAtConstraint>().AddSource(cn);
        gmm.GetComponent<LookAtConstraint>().constraintActive = true;
        wasConnected = true;

        ConstraintSource cn1 = new ConstraintSource();
        cn1.sourceTransform = Camera.main.transform;//gm.transform;
        cn1.weight = 1;
        lookat1.transform.GetComponent<AimConstraint>().AddSource(cn1);
        lookat1.transform.GetComponent<AimConstraint>().constraintActive = true;
        lookat2.transform.GetComponent<AimConstraint>().AddSource(cn1);
        lookat2.transform.GetComponent<AimConstraint>().constraintActive = true;
        lookat3.transform.GetComponent<AimConstraint>().AddSource(cn1);
        lookat3.transform.GetComponent<AimConstraint>().constraintActive = true;

        lookat4.transform.GetComponent<AimConstraint>().AddSource(cn1);
        lookat4.transform.GetComponent<AimConstraint>().constraintActive = true;

        lookat5.transform.GetComponent<AimConstraint>().AddSource(cn1);
        lookat5.transform.GetComponent<AimConstraint>().constraintActive = true;
        //uiHandlerInst.npcOnboardingObj.SetActive(true);
        StartCoroutine(RemoveInactivePlayer());
        
    }

    [PunRPC]
    void whaturl(string avatarurl)
    {
        webavatarload.OnAvatarGeneratedLoading(avatarurl);
    }

    public static bool isdownloadStarted = false;
    IEnumerator GetPlayerList()
    {
        yield return new WaitUntil(() => isdownloadStarted == false);
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            if (playerInfo.Value.GetPlayerNumber() != PhotonNetwork.LocalPlayer.GetPlayerNumber() && (!playerInfo.Value.IsInactive))
            {
                gm = GameObject.Find(playerInfo.Value.NickName).transform;
                if (gm != null)
                {
                    playersList.Add(gm.gameObject);
                    if (isTennisGameStarted)
                    {
                        HidePlayer();
                    }

                    if (gm.GetChild(1).transform.childCount == 0)
                    {
                        yield return new WaitUntil(() => isdownloadStarted == false);
                        isdownloadStarted = true;
                        webavatarload.staticCharacter = gm.GetChild(1).transform.gameObject;
                        webavatarload.OnAvatarGeneratedLoading(playerInfo.Value.CustomProperties["AvatarUrl"].ToString());
                    }
                }
            }
        }
    }

    IEnumerator RemoveInactivePlayer()
    {
        yield return null;
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            if (playerInfo.Value.IsInactive)
            {
                if (playerInfo.Value.NickName == api.nickName)
                    photonView.RPC("KickPlayer", RpcTarget.MasterClient, playerInfo.Value);
            }
        }

        //Photon.Realtime.Player player = Photon.Realtime.Player.Find(playerID);
        //photonView.RPC("KickPlayer", player);
    }

    [PunRPC]
    public void KickPlayer(Player player)
    {
        PhotonNetwork.CloseConnection(player);
        string nickName = player.NickName;
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].name == nickName)
            {
                if (playerList[i].GetComponent<PhotonView>() && playerList[i].GetComponent<PhotonView>().IsMine)
                {
                    //Debug.Log("Destroynig players photon instantiate");
                    PhotonNetwork.Destroy(playerList[i]);
                }
            }
        }
    }

    void activecollider()
    {
        T2.SetActive(true);
        T3.SetActive(true);
    }

    public void KeppNetworkAlive()
    {
        Debug.Log("in KeepNetworkAlive function");
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendAcksOnly();
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DispatchIncomingCommands();
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
        //photonView.RPC("rpcCalled", RpcTarget.Others);
    }

    //[PunRPC]
    //public void rpcCalled()
    //{
    //    Debug.Log("in other player rpc");
    //}

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log("OnDisconnected Called   " + cause.ToString());
        if (CanRecoverFromDisconnect(cause))
        {
            //Debug.Log("Calling Recover from OnDisconnected");
            //Recover();
        }
    }

    public bool CanRecoverFromDisconnect(DisconnectCause cause)
    {
        switch (cause)
        {
            case DisconnectCause.Exception:
            case DisconnectCause.ServerTimeout:
            case DisconnectCause.ClientTimeout:
            case DisconnectCause.DisconnectByServerLogic:
            case DisconnectCause.DisconnectByServerReasonUnknown:
                return true;
        }
        return false;
    }

    public void Recover()
    {
        //Debug.Log("Recover Called");
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("Returning From InRoom");
                return;
            }
        }
        //Debug.Log("After Checking the first Condition");
        if (!PhotonNetwork.ReconnectAndRejoin())
        {
            //Debug.Log("ReconnectAndRejoin Failed, trying Reconnect");
            if (!PhotonNetwork.Reconnect())
            {
               // Debug.Log("Reconnect Failed, try ConnectUsingSetting");
                if (!PhotonNetwork.ConnectUsingSettings())
                {
                    //Debug.Log("ConnectUsingSetting failed");
                }
                else
                {
                    //Debug.Log("Called ConnectUsingSetting");
                }
            }
            else
            {
                //Debug.Log("Called Reconnect");
            }
        }
        else
        {
            Debug.Log("Called ReconnectAndRejoin");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("JoinRommFailed " + message);
        wasConnected = false;
        JoinRandomRoom();
    }

    public void HidePlayer()
    {
        foreach (GameObject playerObj in playersList)
        {
            if (playerObj != null) { playerObj.SetActive(false); }
        }
    }
    public void UnHidePlayer()
    {
        foreach (GameObject playerObj in playersList)
        {
            if (playerObj != null) { playerObj.SetActive(true); }
        }
    }

    
    public void OnApplicationPause(bool pause)
    {
        Debug.Log("application pause " + pause);
    }

    public void OnLogout()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.Disconnect();
        }
        T2.SetActive(false);
        T3.SetActive(false);
        wasConnected = false;
    }
}
