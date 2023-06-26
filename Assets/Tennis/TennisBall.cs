using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class TennisBall : MonoBehaviour
{
    public static TennisBall instance;
    [SerializeField] Rigidbody ballBody;
    [SerializeField] PositionConstraint positionConstraint;
    [SerializeField] Collider ballCollider;
    [SerializeField] float force;
    Vector3 pointVector;
    public bool isGhost;
    bool isBallOutSide;
    public Vector3 positionAtRest { get; private set; }

    private void Awake()
    {
        if(instance == null) { instance = this; }
    }
    private void Start()
    {
        Events.instance.OnServBallhit += Instance_OnServBallhit;
        Events.instance.OnBallOutSide += Instance_OnBallOutSide;
        positionAtRest = positionConstraint.translationAtRest;
        //if(GameManager.instance.currentService == WhoHit.Player)
        //{
        //    SetWeight((int)WhoHit.Player, 1);
        //    SetWeight((int)WhoHit.AI, 0);
        //}
        //else if(GameManager.instance.currentService == WhoHit.AI)
        //{
        //    SetWeight((int)WhoHit.Player, 0);
        //    SetWeight((int)WhoHit.AI, 1);
        //}
        //ballBody.velocity = Vector3.up * 7;
    }

    

    public void SetBeforeGameStart()
    {
        //isBallOutSide = false;
        ballBody.drag = 0;
        transform.position = instance.positionAtRest;
        positionConstraint.constraintActive = true;
        if (GameManager.instance.currentService == WhoHit.Player)
        {
            SetWeight((int)WhoHit.Player, 1);
            SetWeight((int)WhoHit.AI, 0);
        }
        else if (GameManager.instance.currentService == WhoHit.AI)
        {
            SetWeight((int)WhoHit.Player, 0);
            SetWeight((int)WhoHit.AI, 1);
        }
    }
    public void SetWeight(int index, int weight)
    {
        ConstraintSource constraintSource = positionConstraint.GetSource(index);
        constraintSource.weight = weight;
        positionConstraint.SetSource(index, constraintSource);
    }

    private void Instance_OnServBallhit()
    {
        ballCollider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        isBallOutSide = false;
        positionConstraint.constraintActive = false;
        transform.parent = null;
        ballBody.isKinematic = false;
        ballBody.useGravity = true;
        //ballBody.GetComponent<SphereCollider>().enabled = true;
        //ballBody.transform.GetChild(0).gameObject.SetActive(true);
        if(GameManager.instance.currentService == WhoHit.AI)
        {
            force = 15;
        }
        else
        {
            force = 18;
        }
        ballBody.velocity = GameManager.instance.servDirection.normalized * force + new Vector3(0, /*6*/3, 0);
        GameManager.instance.hitTypeText.text = "Serve";
        CalculateBallSpeed();
        PredictionManager.instance.predict(this.gameObject, transform.position, ballBody.velocity);
        if(GameManager.instance.currentService == WhoHit.AI)
        {
            GameManager.instance.ballHitAudioSource.PlayOneShot(GameManager.instance.allAudio.ballHitAudio.aiBallShotClip);
            if (AiPlayer.instance.scoreManagement.fastestServe < ballBody.velocity.magnitude)
            {
                AiPlayer.instance.scoreManagement.fastestServe = (int)ballBody.velocity.magnitude * Mathf.RoundToInt(3.6f);
            }
        }
        else if(GameManager.instance.currentService == WhoHit.Player)
        {
            GameManager.instance.ballHitAudioSource.PlayOneShot(GameManager.instance.allAudio.ballHitAudio.playerBallShotClip);
            if (TennisPlayer.instance.scoreManagement.fastestServe < ballBody.velocity.magnitude)
            {
                TennisPlayer.instance.scoreManagement.fastestServe = (int)ballBody.velocity.magnitude * Mathf.RoundToInt(3.6f);
            }
        }
        //AiPlayer.instance.SetAiPlayerTurn();
    }
    public void CalculateBallSpeed()
    {
        //ballBody.angularVelocity = Vector3.forward * Random.Range(60f, 80f);
        // GameManager.instance.rpmText.text = string.Format("{0:00'}", (ballBody.angularVelocity * Mathf.Rad2Deg).z) + " Rpm";
        // GameManager.instance.speedText.text = string.Format("{0:00'}", ballBody.velocity.magnitude) +" Km/h";

         GameManager.instance.rpmText.text = string.Format("{0:00'}", Random.Range(2800, 3800) + " Rpm");
         GameManager.instance.speedText.text = string.Format("{0:00'}", Random.Range(190,250) + " Km/h");
    }
    private void Instance_OnBallOutSide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        ballCollider.enabled = false;
    }
    public void Init(Vector3 velocity, bool isGhost = true)
    {
        ballBody.AddForce(velocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGhost || isBallOutSide) return;
        if(collision.collider.name == "inGround")
        {
            GameManager.instance.ballHitAudioSource.PlayOneShot(GameManager.instance.allAudio.ballBounceFlorClip);
            Debug.Log("In Ground",this);
            if(GameManager.instance.whoHit == WhoHit.AI)
            {
                GameManager.instance.penaltyPlayer = WhoHit.Player;
            }
            else if(GameManager.instance.whoHit == WhoHit.Player)
            {
                GameManager.instance.penaltyPlayer = WhoHit.AI;
            }
        }
        if (collision.collider.name == "outside" && !isBallOutSide)
        {
            GameManager.instance.ballHitAudioSource.PlayOneShot(GameManager.instance.allAudio.ballOutClip);
            isBallOutSide = true;
            Events.instance.BallOutSide();
        }
        if (collision.collider.name == "net")
        {
            if(GameManager.instance.whoHit == WhoHit.Player)
            {
                TennisPlayer.instance.scoreManagement.unforcesErrors += 1;
            }
            else if(GameManager.instance.whoHit == WhoHit.AI)
            {
                AiPlayer.instance.scoreManagement.unforcesErrors += 1;
            }
            isBallOutSide = true;
            ballBody.drag = 5;
            Events.instance.BallOutSide();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //private void Update()
    //{
    //    pointVector = Vector3.ProjectOnPlane(transform.position, Vector3.up);
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(pointVector, 3);
    //}
}
