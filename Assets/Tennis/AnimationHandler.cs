using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public void HitBallFromAnimation()
    {
        Events.instance.ServBallHit();
    }
}
