using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGround : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Ball")
        {
            if(GameManager.instance.penaltyPlayer == WhoHit.AI)
            {
                Debug.Log("Distance - " + Vector3.Distance(AiPlayer.instance.transform.position, collision.collider.transform.position));
                if (Vector3.Distance(AiPlayer.instance.transform.position, collision.collider.transform.position) < 3.0f/*1.6f*/)
                {
                    AiPlayer.instance.isNeedExtraHeight = true;
                    Debug.Log("Give Extra Height");
                }
            }
            else
            {
                //if (Vector3.Distance(TennisPlayer.instance.transform.position, collision.collider.transform.position) < 1.6f)
                //{
                //    Debug.Log("Give Extra Height");
                //}
            }
        }
    }
}
