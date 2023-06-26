using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Collider Objects")]
    public List<GameObject> gameColliderObj;
    [Header("Main Game Object")]
    public GameObject mainGameObj;
    [Header("Video Display Object")]
    public GameObject videoDisplayObj;

    [Header("Game Controls For Mobile")]
    public GameObject movementJoysticFormobileObj;
    public GameObject sliceBtnForMobileObj;
    public GameObject topSpinBtnForMobileObj;

    public static GameManager instance;

    [Serializable]
    public struct PlayerPosition
    {
        public Transform playerPosition;
        public Transform aiPlayerPosition;
        [Header("Ai Player Service Point")]
        public Transform aiPlayerServicePoint1;
        public Transform aiPlayerServicePoint2;
        [Header("Player Service Point")]
        public Transform playerServicePoint1;
        public Transform playerServicePoint2;
    }
    [Space(20)]
    public List<PlayerPosition> playerPositions;

    //[SerializeField] GameObject playerRacket;
    [SerializeField] GameObject ballObj;
    [SerializeField] public WhoHit whoHit;
    [SerializeField] public WhoHit currentService;
    public WhoHit penaltyPlayer;
    public Vector3 servDirection;
    [HideInInspector] public int selectedFields;
    public RectTransform scoreText;
    [SerializeField] private Text playerRound;
    [SerializeField] private Text aiRound;
    [SerializeField] private Text playerScore;
    [SerializeField] private Text aiScore;
    public Text speedText;
    public Text rpmText;
    public Text hitTypeText;

    public bool isServe;

    [Serializable]
    public struct UIScore
    {
        public GameObject finalScoreObj;
        public Text finalResult;
        public Text finalResult_New;
        public ScoreFields playerScoreFields;
        public ScoreFields aiScoreFields;
    }
    public UIScore gameOverScore;
    [Serializable]
    public struct ScoreFields
    {
        public Text aces;
        public Text breakPointsWon;
        public Text receivingPointsWon;
        public Text Winners;
        public Text UnforcedErrors;
        public Text totalPointsWon;
        public Text fastestServe;
    }

    [Header("Serve Panel")]
    public GameObject serverPanelObj;
    public GameObject serverPanelObjForMobile;

    [SerializeField] private bool isStartService;

    [Header("Audio Source Handler")]
    public AudioSource publicCheerAudioSource;
    public AudioSource ballHitAudioSource;
    public AudioSource scoreAudioSource;
    public AudioSource clapAudioSource;

    [HideInInspector] private bool isMute;

    [Serializable]
    public class AllAudio
    {
        public AudioClip crowdClip;
        public AudioClip ballOutClip;
        public AudioClip ballBounceFlorClip;
        public BallHitAudio ballHitAudio;

        public List<AudioClip> claps;
        [Serializable]
        public class BallHitAudio
        {
            public AudioClip aiBallShotClip;
            public AudioClip playerBallShotClip;
        }
    }

    [Header("All audio Clip")]
    public AllAudio allAudio;

    [Header("Canvas How To Play Game")]
    public GameObject howToPlayScreenObj;

    [Header("All Trees Object")]
    public GameObject allTreeParentObj;

    bool isMobile;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        //currentService = WhoHit.AI;
        isMobile = UIhandler.instance.IsMobileCheck();
        if (!isMobile)
        {
            allTreeParentObj.SetActive(true);
        }
        Events.instance.OnBallOutSide += Instance_OnBallOutSide;
        Events.instance.OnServBallhit += Instance_OnServBallhit;
        Events.instance.OnResetRound += Instance_OnResetRound;
        //SetFields();
        if (currentService == WhoHit.Player)
        {
            EnableServePanel(true);
            //isStartService = true;
            //serverPanelObj.SetActive(true);
        }        
    }
    void Update()
    {
        if (!isMobile && isStartService && currentService == WhoHit.Player)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Start Service");
                EnableServePanel(false);
                TennisPlayer.instance.DoService();
            }
        }
    }

    public void OnClickDoServiceButton()
    {
        if (isStartService && currentService == WhoHit.Player)
        {
            EnableServePanel(false);
            TennisPlayer.instance.DoService();
            TennisPlayer.instance.playerHitButtonObj.onClick.RemoveListener(OnClickDoServiceButton);
        }
    }

    public void StartGame()
    {
        videoDisplayObj.SetActive(false);
        playerScore.text = "0";
        aiScore.text = "0";
        playerRound.text = "0";
        aiRound.text = "0";
        if (isMobile)
        {
            UIhandler.instance.GamePlayCanvasForJoystick.SetActive(false);
            movementJoysticFormobileObj.SetActive(true);
            sliceBtnForMobileObj.SetActive(true);
            topSpinBtnForMobileObj.SetActive(true);
        }

        for (int i=0;i<gameColliderObj.Count;i++)
        {
            gameColliderObj[i].SetActive(true);
        }
        for (int i = 0; i < mainGameObj.transform.childCount; i++)
        {
            mainGameObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        publicCheerAudioSource.Play();
        SetFields();
        ballObj.SetActive(true);
    }

    public void EndGame()
    {
        Debug.Log("********* EndGame *********"+ Time.timeScale);
        videoDisplayObj.SetActive(true);
        for (int i = 0; i < gameColliderObj.Count; i++)
        {
            gameColliderObj[i].SetActive(false);
        }
        for (int i = 0; i < mainGameObj.transform.childCount; i++)
        {
            mainGameObj.transform.GetChild(i).gameObject.SetActive(false);
        }
        publicCheerAudioSource.Stop();
        gameOverScore.finalScoreObj.SetActive(false);
        UIhandler uiHandlerInst = GameObject.Find("Uihandler").GetComponent<UIhandler>();
        uiHandlerInst.inGameMenuCanvasObj.gameObject.SetActive(true);
        uiHandlerInst.playerArmatureObj.SetActive(true);
        uiHandlerInst.npcCourtyardObj.SetActive(true);

        multiplayerhandler.instance.UnHidePlayer();
        multiplayerhandler.instance.isTennisGameStarted = false;
        ballObj.SetActive(false);
        Physics.autoSimulation = true;
        if (isMobile)
        {
            UIhandler.instance.GamePlayCanvasForJoystick.SetActive(true);
        }
    }

    private void Instance_OnResetRound()
    {
        if(currentService == WhoHit.AI) { currentService = WhoHit.Player; }
        else if(currentService == WhoHit.Player) { currentService = WhoHit.AI; }

        AudioClip clip = Resources.Load<AudioClip>("ScoreAudio/game");
        scoreAudioSource.PlayOneShot(clip);
    }

    private void Instance_OnServBallhit()
    {
        //Debug.Log("Ball hit from animation");
        ballObj.SetActive(true);
        //playerRacket.SetActive(true);
    }
  
    private void SetFields()
    {
        selectedFields = selectedFields == 0 ? 1 : 0;
        AiPlayer.instance.transform.position = playerPositions[selectedFields].aiPlayerPosition.position;
        TennisPlayer.instance.transform.position = playerPositions[selectedFields].playerPosition.position;

        TennisBall.instance.SetBeforeGameStart();
        isServe = true;
        if (currentService == WhoHit.Player)
        {
            TennisBall.instance.SetWeight((int)WhoHit.Player, 1);
            TennisBall.instance.SetWeight((int)WhoHit.AI, 0);
            float zPosition = Random.Range(playerPositions[selectedFields].aiPlayerServicePoint1.position.z, playerPositions[selectedFields].aiPlayerServicePoint2.position.z);
            Vector3 targetPosition = playerPositions[selectedFields].aiPlayerServicePoint2.position;
            targetPosition.z = zPosition;
            Servicedirection(playerPositions[selectedFields].playerPosition.position, targetPosition);
            //TennisPlayer.instance.DoService();
        }
        else if (currentService == WhoHit.AI)
        {
            TennisBall.instance.SetWeight((int)WhoHit.Player, 0);
            TennisBall.instance.SetWeight((int)WhoHit.AI, 1);

            float zPosition = Random.Range(playerPositions[selectedFields].playerServicePoint1.position.z, playerPositions[selectedFields].playerServicePoint2.position.z);
            Vector3 targetPosition = playerPositions[selectedFields].playerServicePoint2.position;
            targetPosition.z = zPosition;
            Servicedirection(playerPositions[selectedFields].aiPlayerPosition.position, targetPosition);

            AiPlayer.instance.DoService();
        }
    }

    private void Instance_OnBallOutSide()
    {
        if (penaltyPlayer != WhoHit.Player)
        {
            Debug.Log("Player Got Point");
            TennisPlayer.instance.SetScore();
        }
        else if(penaltyPlayer != WhoHit.AI)
        {
            Debug.Log("AI Got Point");
            AiPlayer.instance.SetScore();
        }
        rpmText.text = speedText.text = hitTypeText.text = "-" ;
        StartCoroutine(ScoreTextAnimation());
        StartCoroutine(StartAgainGame());
    }
    public IEnumerator ScoreTextAnimation()
    {
        //-575.5f
        scoreText.transform.DOLocalMoveZ(-575.5f, 0.2f);
        yield return null;
        //yield return new WaitForSeconds(0.5f);
        if(TennisPlayer.instance.scoreManagement.advantageCount != 0)
        {
            scoreText.GetComponent<Text>().text = string.Format("{0}:{1}", "AD", AiPlayer.instance.scoreManagement.score);
            playerScore.text = "AD";//TennisPlayer.instance.scoreManagement.score.ToString();
            aiScore.text = AiPlayer.instance.scoreManagement.score.ToString();

            //AudioClip clip = Resources.Load<AudioClip>("ScoreAudio/game");
           // GameManager.instance.scoreAudioSource.PlayOneShot(clip);
        }
        else if(AiPlayer.instance.scoreManagement.advantageCount != 0)
        {
            scoreText.GetComponent<Text>().text = string.Format("{0}:{1}", TennisPlayer.instance.scoreManagement.score, "AD");
            playerScore.text = TennisPlayer.instance.scoreManagement.score.ToString();
            aiScore.text = "AD";//AiPlayer.instance.scoreManagement.score.ToString();

           // AudioClip clip = Resources.Load<AudioClip>("ScoreAudio/game");
           // GameManager.instance.scoreAudioSource.PlayOneShot(clip);
        }
        else
        {
            PlayScoreAudio(); 

            scoreText.GetComponent<Text>().text = string.Format("{0}:{1}", TennisPlayer.instance.scoreManagement.score, AiPlayer.instance.scoreManagement.score);
            playerScore.text = TennisPlayer.instance.scoreManagement.score.ToString();
            aiScore.text = AiPlayer.instance.scoreManagement.score.ToString();
        }
        playerRound.text = TennisPlayer.instance.scoreManagement.totalWin.ToString();
        aiRound.text = AiPlayer.instance.scoreManagement.totalWin.ToString();
        //yield return new WaitForSeconds(1.5f);
        scoreText.transform.DOLocalMoveZ(-644.7f,0.2f);

        clapAudioSource.PlayOneShot(allAudio.claps[Random.Range(0, allAudio.claps.Count)]);
    }

    private void PlayScoreAudio()
    {
        if (currentService == WhoHit.AI)
        {
            //if ((TennisPlayer.instance.scoreManagement.score == 40 && AiPlayer.instance.scoreManagement.score != 40) || (TennisPlayer.instance.scoreManagement.score != 40 && AiPlayer.instance.scoreManagement.score == 40))
            //{
            //    AudioClip clip = Resources.Load<AudioClip>("ScoreAudio/game");
            //    GameManager.instance.scoreAudioSource.PlayOneShot(clip);
            //}
            //else
            //{
                AudioClip clip = Resources.Load<AudioClip>(string.Format("ScoreAudio/{0:00}{1:00}", AiPlayer.instance.scoreManagement.score, TennisPlayer.instance.scoreManagement.score));
                GameManager.instance.scoreAudioSource.PlayOneShot(clip);
           // }
        }
        else
        {
            //if ((TennisPlayer.instance.scoreManagement.score == 40 && AiPlayer.instance.scoreManagement.score != 40) || (TennisPlayer.instance.scoreManagement.score != 40 && AiPlayer.instance.scoreManagement.score == 40))
            //{
            //    AudioClip clip = Resources.Load<AudioClip>("ScoreAudio/game");
            //    GameManager.instance.scoreAudioSource.PlayOneShot(clip);
            //}
            //else
            //{
                AudioClip clip = Resources.Load<AudioClip>(string.Format("ScoreAudio/{0:00}{1:00}", TennisPlayer.instance.scoreManagement.score, AiPlayer.instance.scoreManagement.score));
                GameManager.instance.scoreAudioSource.PlayOneShot(clip);
            //}
        }
    }

    IEnumerator StartAgainGame()
    {
        TennisBall.instance.GetComponent<Rigidbody>().useGravity = false;
        if (TennisPlayer.instance.scoreManagement.totalWin == 2 || AiPlayer.instance.scoreManagement.totalWin == 2)
        {
            gameOverScore.finalScoreObj.SetActive(true);
           // gameOverScore.gameScoreObj.SetActive(false);
            Debug.Log("Game Complate No need to play next Round");

            gameOverScore.finalResult.text = string.Format("{0} - {1}", TennisPlayer.instance.scoreManagement.totalWin, AiPlayer.instance.scoreManagement.totalWin);

            gameOverScore.finalResult_New.text = string.Format("{0} - {1}", TennisPlayer.instance.scoreManagement.totalWin, AiPlayer.instance.scoreManagement.totalWin);
            

            ScoreManagement temp = TennisPlayer.instance.scoreManagement;
            //TODO - Set Player Data
            gameOverScore.playerScoreFields.aces.text = temp.aces.ToString();
            gameOverScore.playerScoreFields.breakPointsWon.text = temp.breakPointsWon.ToString();
            gameOverScore.playerScoreFields.receivingPointsWon.text = temp.receivingPointsWon.ToString();
            gameOverScore.playerScoreFields.Winners.text = temp.winners.ToString();
            gameOverScore.playerScoreFields.UnforcedErrors.text = temp.unforcesErrors.ToString();
            gameOverScore.playerScoreFields.totalPointsWon.text = temp.totalPointsWon.ToString();
            gameOverScore.playerScoreFields.fastestServe.text = temp.fastestServe.ToString();

            //TODO - Set ai Data
            temp = AiPlayer.instance.scoreManagement;
            gameOverScore.aiScoreFields.aces.text = temp.aces.ToString();
            gameOverScore.aiScoreFields.breakPointsWon.text = temp.breakPointsWon.ToString();
            gameOverScore.aiScoreFields.receivingPointsWon.text = temp.receivingPointsWon.ToString();
            gameOverScore.aiScoreFields.Winners.text = temp.winners.ToString();
            gameOverScore.aiScoreFields.UnforcedErrors.text = temp.unforcesErrors.ToString();
            gameOverScore.aiScoreFields.totalPointsWon.text = temp.totalPointsWon.ToString();
            gameOverScore.aiScoreFields.fastestServe.text = temp.fastestServe.ToString();

            playerRound.text = "0";//TennisPlayer.instance.scoreManagement.totalWin.ToString();
            aiRound.text = "0"; //AiPlayer.instance.scoreManagement.totalWin.ToString();
            playerScore.text = "0";//TennisPlayer.instance.scoreManagement.score.ToString();
            aiScore.text = "0";// AiPlayer.instance.scoreManagement.score.ToString();

            if(TennisPlayer.instance.scoreManagement.totalWin == 2)
            {
                api apiInstance = multiplayerhandler.instance.GetComponent<api>();
                if(apiInstance != null)
                    apiInstance.CallPointsTransaction(100, 1, false, 0, false, 0, false, 0);
            }

            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(3);
            if (currentService == WhoHit.Player)
            {
                EnableServePanel(true);
                AiPlayer.instance.aiServerBallObj.SetActive(false);
            }
            TennisPlayer.instance.playerHitButtonObj.onClick.AddListener(OnClickDoServiceButton);
            TennisBall.instance.GetComponent<Rigidbody>().useGravity = false;
            SetFields();
        }
    }
    private void EnableServePanel(bool tempVar)
    {
        isStartService = tempVar;
        if (isMobile)
        {
            serverPanelObjForMobile.SetActive(tempVar);
        }
        else
        {
            serverPanelObj.SetActive(tempVar);
        }
    }

    public void ResetGame()
    {
        playerRound.text = "0";
        aiRound.text = "0";
        gameOverScore.finalScoreObj.SetActive(false);
        SetFields();
    }

    private void Servicedirection(Vector3 servicePlayerPosition, Vector3 targetPosition)
    {
        servDirection = targetPosition - servicePlayerPosition;
    }
    #region [ Mute and unmute]
    public Sprite Mute, Unmute;
    public Image Mute_Button;

    public void MuteAudio()
    {
        isMute = !isMute;
        if (isMute)
        {
            Mute_Button.sprite = Mute;
            publicCheerAudioSource.volume =
                ballHitAudioSource.volume =
                scoreAudioSource.volume =
                clapAudioSource.volume = 0;
        }
        else
        {
            Mute_Button.sprite = Unmute;
            publicCheerAudioSource.volume =
                ballHitAudioSource.volume =
                scoreAudioSource.volume = 1;
            clapAudioSource.volume = 0.6f;
        }
    }
    #endregion
}
