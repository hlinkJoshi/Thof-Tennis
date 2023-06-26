using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public event Action OnBallOutSide;
    public void BallOutSide() => OnBallOutSide?.Invoke();

    public event Action OnServBallhit;
    public void ServBallHit() => OnServBallhit?.Invoke();

    public event Action OnResetRound;
    public void ResetRound() => OnResetRound?.Invoke();
}
