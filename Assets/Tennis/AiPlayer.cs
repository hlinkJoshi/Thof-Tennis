using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlayer : MonoBehaviour
{
    public static AiPlayer instance;
    public enum AIAnimationState
    {
        idle,
        Run,
        High,
        Low,
        Center
    }


    public bool isStartBallTrack;

    float force = 18f;//13f; // force subtract by distacne
    float upForce = 20;//3; upforce devide by distance
    public Transform nearToNet, FarFromNet;
    Vector3 targetPosition = new Vector3(53.42f, 0.5f, -16.0f);
    [SerializeField] Transform opponantPlayer;
    [SerializeField] Transform ballObj;
    [SerializeField] Transform aiPlayerMesh;
    [SerializeField] Collider aiPlayerCollider;
    public GameObject aiServerBallObj;
    TennisBall tennisBall;
    float aiPlayerMovementspeed = 5f;
    Vector3 ballTargetPosition;
    Vector3 ballLandPoint;
    string selectedAnimation;
    public bool isAnimationStart; //This is only for hit animation
    bool isRight;
    bool isBallOutSide;
    [HideInInspector] public bool isNeedExtraHeight;

    [Header("Score Managemnet")]
    public ScoreManagement scoreManagement = new ScoreManagement { round = 0, score = 0, totalWin = 0 };

    [SerializeField] Animator aiAnimator;
    private AIAnimationState aIAnimationState;

    (float, float) GetUpForceAndForce;

    [SerializeField] private int totalShot;
    [SerializeField] private int currentShot;

    private void Awake()
    {
        Debug.Log("Calling ________");
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
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

        GameManager.instance.gameOverScore.aiScoreFields.aces.text = "0";
        GameManager.instance.gameOverScore.aiScoreFields.breakPointsWon.text = "0";
        GameManager.instance.gameOverScore.aiScoreFields.receivingPointsWon.text = "0";
        GameManager.instance.gameOverScore.aiScoreFields.Winners.text = "0";
        GameManager.instance.gameOverScore.aiScoreFields.UnforcedErrors.text = "0";
        GameManager.instance.gameOverScore.aiScoreFields.totalPointsWon.text = "0";
        GameManager.instance.gameOverScore.aiScoreFields.fastestServe.text = "0";
    }

    private void Start()
    {
        tennisBall = ballObj.GetComponent<TennisBall>();
        ballTargetPosition = transform.position;
        aIAnimationState = AIAnimationState.idle;
        aiPlayerCollider.enabled = false;
        Events.instance.OnBallOutSide += Instance_OnBallOutSide;
        Events.instance.OnServBallhit += Instance_OnServBallhit;
        Events.instance.OnResetRound += Instance_OnResetRound;

      //  DoService();
    }

    private void Instance_OnResetRound()
    {
        scoreManagement.round = 0;
        scoreManagement.score = 0;
        scoreManagement.advantageCount = 0;
    }

    public void SetScore()
    {
        if(scoreManagement.round == 3 && TennisPlayer.instance.scoreManagement.round == 3)
        {
            if(TennisPlayer.instance.scoreManagement.advantageCount != 0)
            {
                TennisPlayer.instance.scoreManagement.advantageCount = 0;
                //Clear advanteg From Polayer
            }
            else
            {
                scoreManagement.advantageCount += 1;
            }
            if(scoreManagement.advantageCount == 2)
            {
                //Ai Win the math
                PointCount();
            }
            Debug.Log("AI Got Advantage - " + scoreManagement.advantageCount);
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
            GameManager.instance.isServe = false;
            scoreManagement.aces += 1;
        }

        scoreManagement.winners += 1;
        scoreManagement.round += 1;

        if (GameManager.instance.currentService == WhoHit.Player)
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
            if (GameManager.instance.currentService == WhoHit.Player)
            {
                scoreManagement.breakPointsWon += 1;
            }
            Debug.Log("First Game Complate Change Service");
            Events.instance.ResetRound();
        }
    }

    public void DoService()
    {
        GameManager.instance.whoHit = WhoHit.AI;
        //isBallOutSide = false;
        aiAnimator.Play("Services");
    }

    private void Instance_OnBallOutSide()
    {
        isBallOutSide = true;
        isStartBallTrack = false;
        aiPlayerCollider.enabled = false;
        ballLandPoint = Vector3.zero;
        aiPlayerMesh.rotation = Quaternion.Euler(0, 90, 0);
        UpdateAnimationstate(AIAnimationState.idle);
        currentShot = 0;
    }
    private void Instance_OnServBallhit()
    {
        GameManager.instance.isServe = true;
        isBallOutSide = false;
        StartCoroutine(StartCollider());
        GetRandomShot();
    }

    private IEnumerator StartCollider()
    {
        yield return new WaitForSeconds(0.2f);
        aiPlayerCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.currentService == WhoHit.AI && GameManager.instance.isServe)
        {
            return;
        }

        if (other.tag == "Ball")
        {
            currentShot += 1;
            //CheckHitPoint(other.transform.position);
            //Debug.Log("Ai Hit");
            if (GameManager.instance.currentService == WhoHit.Player) { GameManager.instance.isServe = false; }
            isStartBallTrack = false;
            targetPosition.z = Random.Range(-18.0f, -7.0f);//(-17.0f, -8.0f);
            int shotPrediction = Random.Range(0, 101);
            Vector3 dir = targetPosition - transform.position;
            if (shotPrediction < 81)
            {
                dir = TennisPlayer.instance.transform.position - transform.position;
                dir.z += Random.Range(-3, 3);
            }
            GetUpForceAndForce = CheckDistance();

            GameManager.instance.ballHitAudioSource.PlayOneShot(GameManager.instance.allAudio.ballHitAudio.aiBallShotClip);
            other.GetComponent<Rigidbody>().velocity = dir.normalized * (Mathf.Abs(force - GetUpForceAndForce.Item2)) + new Vector3(0, (upForce / GetUpForceAndForce.Item1), 0);
            GameManager.instance.penaltyPlayer = GameManager.instance.whoHit = WhoHit.AI;
            aiPlayerMesh.rotation = Quaternion.Euler(0, 90, 0);
            //Debug.Log("height - " + Vector3.Angle(new Vector3(tempVelocity.x, 0, tempVelocity.z), tempVelocity));
        }
        //Debug.Log(other.name);
    }
    private void Update()
    {
        ballTargetPosition.z = ballObj.position.z;
        if (!isBallOutSide && isStartBallTrack && currentShot <= totalShot)
        {
            MoveToLandPoint();
        }
    }
    public void SetAiPlayerTurn()
    {
        if(Vector3.Distance(transform.position, ballLandPoint) <= 0.5f && aiPlayerMesh.rotation.eulerAngles.y == 90){}
        else
        {
            if (currentShot <= totalShot)
            {
                UpdateAnimationstate(AIAnimationState.Run);
            }
        }
        isStartBallTrack = true;
        isAnimationStart = false;
    }
    void RotateToWardLandPoint()
    {
        Vector3 direction = (ballLandPoint - transform.position).normalized;
        Vector3 cross = Vector3.Cross(direction, new Vector3(1,0,0));
        if (cross.y < 0)
        {
            //Debug.Log("Ball is right side");
            aiPlayerMesh.rotation = Quaternion.Euler(0, 180, 0);
            isRight = true;
            aiAnimator.SetBool("isRight", isRight);
        }
        else if (cross.y > 0)
        {
            //Debug.Log("Ball is left side");
            aiPlayerMesh.rotation = Quaternion.identity;
            isRight = false;
            aiAnimator.SetBool("isRight", isRight);
        }
    }
    void MoveToLandPoint()
    {
        transform.position = /*Vector3.MoveTowards(transform.position, ballLandPoint, aiPlayerMovementspeed * Time.deltaTime);//*/Vector3.MoveTowards(transform.position, ballTargetPosition, aiPlayerMovementspeed * Time.deltaTime);
        if(Mathf.Abs(transform.position.z - ballTargetPosition.z) <= 0.5f && aiPlayerMesh.rotation.eulerAngles.y != 90 && !isAnimationStart)//if(Vector3.Distance(transform.position, ballLandPoint) <= 0.5f && aiPlayerMesh.rotation.eulerAngles.y != 90 && !isAnimationStart)
        {
            RotateToWardLandPoint();
            UpdateAnimationstate(AIAnimationState.idle);
            aiPlayerMesh.rotation = Quaternion.Euler(0, 90, 0);
        }

        
        //transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, ballTargetPosition, aiPlayerMovementspeed * Time.deltaTime), (transform.position.z - ballObj.position.z) > 0.5f ?
        //Quaternion.Euler(0, 180, 0) :
        //Quaternion.identity);

        if (Vector3.Distance(transform.position, ballObj.position) <= 5 && !isAnimationStart)
        {
            //Debug.Log("Update animation");
            RotateToWardLandPoint();
            isAnimationStart = true;
            if(isRight)
            {
                aiPlayerMesh.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                aiPlayerMesh.rotation = Quaternion.identity;
            }
            UpdateAnimationstate(aIAnimationState);
        }
    }
    (float devideUpForce, float subtractForce) CheckDistance()
    {
        float upForce = (Mathf.Abs(transform.position.x - nearToNet.position.x) >= 5 ? 5 : Mathf.Abs(transform.position.x - nearToNet.position.x));
        if (isNeedExtraHeight)
        {
            //Debug.Log("old force - " + upForce);
            upForce += 2;
            //Debug.Log("New force - " + upForce);
            isNeedExtraHeight = false;
        }
        return (upForce, Mathf.Abs(transform.position.x - FarFromNet.position.x));
    }
    public void CheckHitPoint(Vector3 ballHitPosition)
    {
        ballLandPoint = Vector3.ProjectOnPlane(ballHitPosition, Vector3.up);
        ballLandPoint.y = 0.5f;
        ballLandPoint.x = ballLandPoint.x < 30.0f ? 30.0f : ballLandPoint.x;
        ballLandPoint.x = ballLandPoint.x > 40.0f ? 40.0f : ballLandPoint.x;
        RotateToWardLandPoint();

        Vector3 hitPoint = transform.InverseTransformPoint(ballHitPosition);
        if (hitPoint.y > 0.3f)//0
        {
            //Debug.Log("Top Hit");
            //selectedAnimation = "High";
            aIAnimationState = AIAnimationState.High;
        }
        else if (hitPoint.y <= 0.3f && hitPoint.y > -0.15f)
        {
            //Debug.Log("Center Hit");
            //selectedAnimation = "Center";
            aIAnimationState = AIAnimationState.Center;
        }
        else if (hitPoint.y <= -0.15)
        {
            //Debug.Log("Lower hit");
            aIAnimationState = AIAnimationState.Low;//"Low";
        }
    }

    public void UpdateAnimationstate(AIAnimationState animationState)
    {
        aiAnimator.Play(animationState.ToString());
    }

    //private void OnDrawGizmos()
    //{
    //    ballLandPoint.x = ballLandPoint.x < 30.0f ? 30.0f : ballLandPoint.x;
    //    Gizmos.DrawSphere(ballLandPoint + new Vector3(0, .5f, 0), 1);
    //}

    private void GetRandomShot()
    {
        totalShot = Random.Range(7, 20);
    }
}
