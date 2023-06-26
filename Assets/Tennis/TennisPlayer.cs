using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TennisPlayer : MonoBehaviour
{
    public static TennisPlayer instance;
    [Header("Player Movement for mobile")]
    public FixedJoystick playerMovementJoystickObj;
    public Button playerHitButtonObj;
   // float force = 13f;
    [SerializeField] Transform opponantPlayer;
    [SerializeField] Transform ball;
    TennisBall tennisBall;
    [SerializeField] Text hitStatusText;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator serviceAnimator;
    [SerializeField] ShotManager shotManager;
    [SerializeField] Transform racketPosition;
    [SerializeField] GameObject serviceBall;
    [SerializeField] GameObject serviceRacket;
    Shot currentShot;
    float ballDistance;
    bool isHit;
    Vector3 targetPosition = new Vector3(29.3f, 0.5f, -16.0f);
    Vector3 orignalRacketPosition;
    Vector3 racketOffsetPostion = new Vector3(-0.5f,0,-0.1f);
    bool isBollOutSide = true;
    bool isServing;

    [Header("Score Managemnet")]
    public ScoreManagement scoreManagement = new ScoreManagement { round = 0, score = 0, totalWin = 0};

    [Header("Trail")]
    [SerializeField] TrailRenderer ballTrail;

    public static string topSpin = "TopSpin";
    public static string slice = "Slice";

    [HideInInspector]
    public bool isMobile = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        Debug.Log("Enable Player ");
        ResetOnStart();
    }

    private void ResetOnStart()
    {
        scoreManagement.score =
           scoreManagement.round =
           scoreManagement.totalWin =
           scoreManagement.aces =
           scoreManagement.breakPointsWon =
           scoreManagement.receivingPointsWon =
           scoreManagement.winners =
           scoreManagement.unforcesErrors =
           scoreManagement.totalPointsWon =
           scoreManagement.fastestServe = 0;

        GameManager.instance.gameOverScore.playerScoreFields.aces.text = "0";
        GameManager.instance.gameOverScore.playerScoreFields.breakPointsWon.text = "0";
        GameManager.instance.gameOverScore.playerScoreFields.receivingPointsWon.text = "0";
        GameManager.instance.gameOverScore.playerScoreFields.Winners.text = "0";
        GameManager.instance.gameOverScore.playerScoreFields.UnforcedErrors.text = "0";
        GameManager.instance.gameOverScore.playerScoreFields.totalPointsWon.text = "0";
        GameManager.instance.gameOverScore.playerScoreFields.fastestServe.text = "0";
    }

    // Start is called before the first frame update
    void Start()
    {
        tennisBall = ball.GetComponent<TennisBall>();
        Invoke("CreatePhySceneObstical", 0.2f);
        Events.instance.OnBallOutSide += Instance_OnBallOutSide;
        Events.instance.OnServBallhit += Instance_OnServBallhit;
        Events.instance.OnResetRound += Instance_OnResetRound;
        isMobile = UIhandler.instance.IsMobileCheck();
        if (isMobile)
        {
            playerMovementJoystickObj.gameObject.SetActive(true);
            playerHitButtonObj.gameObject.SetActive(true);
        }
        else
        {
            playerMovementJoystickObj.gameObject.SetActive(false);
            playerHitButtonObj.gameObject.SetActive(false);
        }
    }

    private void Instance_OnResetRound()
    {
        scoreManagement.round = 0;
        scoreManagement.score = 0;
        scoreManagement.advantageCount = 0;
    }

    private void Instance_OnServBallhit()
    {
        isServing = false;
        GameManager.instance.isServe = true;
        isBollOutSide = false;
        playerAnimator.gameObject.SetActive(true);
        opponantPlayer.GetComponent<AiPlayer>().SetAiPlayerTurn();
    }

    public void DoService()
    {
        GameManager.instance.whoHit = WhoHit.Player;
        isServing = true;
        serviceBall.SetActive(true);
        serviceRacket.SetActive(true);
        playerAnimator.gameObject.SetActive(false);
        serviceAnimator.Play("Player Services");
    }
    private void Instance_OnBallOutSide()
    {
        //Debug.Log("Ball Out Side", this);
        isBollOutSide = true;
    }

    void CreatePhySceneObstical()
    {
        PredictionManager.instance.copyAllObstacles();
    }
    void Update()
    {
        MovePlayer();
        if (isBollOutSide) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(slice);
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Hit(topSpin);
        }
    }

    public void Hit(string hitAnimationName)
    {
        if (serviceRacket.activeSelf)
            serviceRacket.SetActive(false);
        isHit = true;
        ballDistance = Vector3.Distance(transform.position, ball.position);
        racketPosition.gameObject.SetActive(true);
        Vector3 direction = transform.position - ball.position;
        if (direction.z > 0)
        {
            racketPosition.localScale = Vector3.one;
        }
        else
        {
            racketPosition.localScale = new Vector3(-1, 1, 1);
        }
        playerAnimator.Play(hitAnimationName);
        GameManager.instance.hitTypeText.text = hitAnimationName;
        StartCoroutine(HideRacket());
        //Debug.Log("key Press - " + ballDistance);
    }

    IEnumerator HideRacket()
    {
        yield return new WaitForSeconds(0.3f);
        racketPosition.gameObject.SetActive(false);
    }

    float horizontal = 0f;
    float vertical = 0f;
    void MovePlayer()
    {
        if (isMobile)
        {
            horizontal = playerMovementJoystickObj.Horizontal;
            vertical = playerMovementJoystickObj.Vertical;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        if(isServing || isBollOutSide)
        {
            vertical = 0;
            if(GameManager.instance.selectedFields == 0)
            {
                if(transform.position.z >= -14)
                {
                    if (horizontal > 0)
                        horizontal = 0;
                }
                else if(transform.position.z <= -17)
                {
                    if (horizontal < 0)
                        horizontal = 0;
                }
            }
            else if(GameManager.instance.selectedFields == 1)
            {
                if(transform.position.z <= -12)
                {
                    if (horizontal < 0)
                        horizontal = 0;
                }
                else if(transform.position.z >= -7)
                {
                    if(horizontal > 0)
                        horizontal = 0;
                }
            }
        }
        else if(!isServing)
        {
            if (transform.position.z > -3 && horizontal > 0)
                horizontal = 0;
            else if (transform.position.z < -22 && horizontal < 0)
                horizontal = 0;
            if (transform.position.x > 60 && vertical < 0)
                vertical = 0;
            else if (transform.position.x < 43 && vertical > 0)
                vertical = 0;
        }
        transform.Translate(new Vector3(vertical, 0, horizontal * -1) * Time.deltaTime * 7); // previous speed 5
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball" && isHit)
        {
            //Debug.Log("hit - " + transform.InverseTransformPoint(other.transform.position));
            if (GameManager.instance.currentService == WhoHit.AI) { GameManager.instance.isServe = false; }
            racketPosition.position = other.transform.position + racketOffsetPostion;
            if(ballDistance > 3 && ballDistance < 10)
            {
                //Debug.Log("Early");
                //force = 10;
                hitStatusText.text = "Early";
                hitStatusText.color = Color.red;
                SetBallTrailColor(new Color(255.0f / 255.0f, 191.0f / 255.0f, 0.0f / 255.0f));
                currentShot = shotManager.topSpin;
            }
            else if(ballDistance > 2 && ballDistance < 3)
            {
                //Debug.Log("Perfect");
                hitStatusText.text = "Perfect";
                hitStatusText.color = Color.green;
                SetBallTrailColor(Color.green);
                //force = 13;
                currentShot = shotManager.flat;
            }
            else if(ballDistance > 0 && ballDistance < 2)
            {
                //Debug.Log("Late");
                hitStatusText.text = "Late";
                hitStatusText.color = Color.red;
                SetBallTrailColor(Color.red);
                //force = 11;
                currentShot = shotManager.topSpin;
            }
            targetPosition.z = Random.Range(-16.0f, -9.0f);//(-17.0f, -8.0f);
            Vector3 dir = targetPosition - transform.position;

            GameManager.instance.ballHitAudioSource.PlayOneShot(GameManager.instance.allAudio.ballHitAudio.playerBallShotClip);
            other.GetComponent<Rigidbody>().velocity = dir.normalized * /*force*/currentShot.hitForce + new Vector3(0, /*6*/currentShot.upForce, 0);
            TennisBall.instance.CalculateBallSpeed();
            GameManager.instance.penaltyPlayer = GameManager.instance.whoHit = WhoHit.Player;
            isHit = false;
            PredictionManager.instance.predict(ball.gameObject, other.transform.position, other.GetComponent<Rigidbody>().velocity);
            opponantPlayer.GetComponent<AiPlayer>().SetAiPlayerTurn();
            Invoke("RemoveStatus", 1.5f);
        }
    }
    private void SetBallTrailColor(Color color)
    {
        ballTrail.material.color = color;
    }
    private void RemoveStatus()
    {
        hitStatusText.text = "";
    }
    public void SetScore()
    {

        if (scoreManagement.round == 3 && AiPlayer.instance.scoreManagement.round == 3)
        {
            if (AiPlayer.instance.scoreManagement.advantageCount != 0)
            {
                AiPlayer.instance.scoreManagement.advantageCount = 0;
                //Clear advanteg From Polayer
            }
            else
            {
                scoreManagement.advantageCount += 1;
            }
            if (scoreManagement.advantageCount == 2)
            {
                //Ai Win the math
                PointCount();
            }
            Debug.Log("Player Got Advantage - " + scoreManagement.advantageCount);
        }
        else
        {
            PointCount();
        }
    }
    private void PointCount()
    {
        if (GameManager.instance.isServe)
        {
            Debug.Log("Set aces");
            GameManager.instance.isServe = false;
            scoreManagement.aces += 1;
        }
        scoreManagement.winners += 1;
        scoreManagement.round += 1;

        if (GameManager.instance.currentService == WhoHit.AI)
        {
            scoreManagement.receivingPointsWon += 1;
        }

        if (scoreManagement.round == 1)
        {
            scoreManagement.score = 15;
        }
        else if (scoreManagement.round == 2)
        {
            scoreManagement.score = 30;
        }
        else if (scoreManagement.round == 3)
        {
            scoreManagement.score = 40;
        }
        else if (scoreManagement.round == 4)
        {
            scoreManagement.round = 0;
            scoreManagement.score = 0;
            scoreManagement.totalWin += 1;
            if (GameManager.instance.currentService == WhoHit.AI)
            {
                scoreManagement.breakPointsWon += 1;
            }
            Debug.Log("First Game Complate Change Service");
            Events.instance.ResetRound();
        }
    }
}
